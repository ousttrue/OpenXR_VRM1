using System;
using System.IO;
using System.Runtime.InteropServices;
using OxrExtraFeatures;
using UnityEngine;

namespace Vrm10XR
{
    public class FrameReader : IDisposable
    {
        const long NANOSECONDS = 1000000000;
        const int HEADER_SIZE = 8 + 8 + 4;

        FrameHeader header_;
        byte[] data_;
        int itemSize_;
        ArrayPin pin_;
        float start_;
        byte[] item_;

        FrameReader(byte[] data, float start, int itemSize)
        {
            var mod = (data.Length - HEADER_SIZE) % (itemSize + 8);
            if (mod != 0)
            {
                // header
                // (long + item) * count
                throw new ArgumentException();
            }

            data_ = data;
            itemSize_ = itemSize;
            pin_ = new ArrayPin(data_);
            header_.StartTime = Marshal.ReadInt64(pin_.Ptr, 0);
            header_.EndTime = Marshal.ReadInt64(pin_.Ptr, 8);
            header_.FrameCount = Marshal.ReadInt32(pin_.Ptr, 16);
            start_ = start;
            item_ = new byte[itemSize];

            // for (int i = 0; i < 100; ++i)
            // {
            //     GetFrame(i, out var time);
            //     Debug.Log($"{i}: {time}");
            // }
        }

        public void Dispose()
        {
            pin_.Dispose();
            pin_ = null;
        }

        public static FrameReader Open(string path, float now, int itemSize)
        {
            if (!File.Exists(path))
            {
                Debug.LogError($"{path} not exists");
                return null;
            }
            return new FrameReader(File.ReadAllBytes(path), now, itemSize);
        }

        byte[] GetFrame(int i, out long time)
        {
            var offset = HEADER_SIZE + i * (itemSize_ + 8);
            var ptr = IntPtr.Add(pin_.Ptr, offset);
            time = Marshal.ReadInt64(ptr);
            Marshal.Copy(IntPtr.Add(pin_.Ptr, offset + 8), item_, 0, itemSize_);
            return item_;
        }

        public bool TryGetJoints(float now, out long time, out byte[] item)
        {
            var elapsedMS = (int)((now - start_) * 1000);
            // milli, micro, nano
            var elapsed = (long)elapsedMS * 1000000;

            var target = header_.StartTime + elapsed;

            var start = GetFrame(0, out var startTime);
            for (int i = 1; i < header_.FrameCount; ++i)
            {
                var end = GetFrame(i, out var endTime);
                if (target >= startTime && target < endTime)
                {
                    time = startTime;
                    item = start;
                    return true;
                }
                start = end;
                startTime = endTime;
            }

            time = default;
            item = default;
            return false;
        }
    }
}
