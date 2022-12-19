using System;
using System.IO;
using System.Runtime.InteropServices;
using OxrExtraFeatures;
using UnityEngine;

namespace Vrm10XR
{
    public class FrameWriter : IDisposable
    {
        int maxCount_;
        int itemSize_;
        FrameHeader header_;
        byte[] data_;
        int pos_;
        ArrayPin pin_;

        public FrameWriter(int itemSize, int count)
        {
            if (count <= 0)
            {
                throw new ArgumentException();
            }
            maxCount_ = count;
            itemSize_ = itemSize;
            data_ = new byte[maxCount_ * (itemSize_ + 8)];
            pin_ = new ArrayPin(data_);
        }

        public void Dispose()
        {
            pin_.Dispose();
            pin_ = null;
        }

        public bool PushArray<T>(long time, T[] value) where T : struct
        {
            if (Marshal.SizeOf<T>() * value.Length != itemSize_)
            {
                throw new ArgumentException();
            }
            if (header_.FrameCount >= maxCount_)
            {
                return false;
            }
            header_.FrameCount++;

            if (pos_ == 0)
            {
                header_.StartTime = time;
            }
            header_.EndTime = time;

            // time
            Marshal.WriteInt64(pin_.Ptr, pos_, time);
            pos_ += 8;
            // value
            using (var pin = new ArrayPin(value))
            {
                Marshal.Copy(pin.Ptr, data_, pos_, itemSize_);
                pos_ += itemSize_;
            }

            return true;
        }

        public void WriteTo(string path)
        {
            if (header_.FrameCount == 0)
            {
                Debug.LogError("no data");
                return;
            }
            Debug.Log($"WriteTo: {path}...");

            using (var s = new FileStream(path, FileMode.Create))
            using (var w = new BinaryWriter(s))
            {
                // start
                w.Write(header_.StartTime);
                // end
                w.Write(header_.EndTime);
                // item count
                w.Write(header_.FrameCount);
                // items                
                w.Write(data_, 0, pos_);
            }
            Debug.Log($"WriteTo: {path} => {pos_}bytes");
        }
    }
}
