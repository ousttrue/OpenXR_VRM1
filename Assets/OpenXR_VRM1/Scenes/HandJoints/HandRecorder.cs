using System.IO;
using System.Linq;
using OxrExtraFeatures;
using UnityEngine;
using static OxrExtraFeatures.HandTrackingFeature;

namespace Vrm10XR
{
    public class HandRecorder : MonoBehaviour, OxrExtraFeatures.HandTracker.IReceiver
    {
        [SerializeField]
        int RecordFrames = 1024;

        [SerializeField]
        string WritePath = "HandJoints.dat";

        public struct Frame
        {
            public long Time;
            public HandTrackingFeature.XrHandJointLocationEXT[] Joints;
        }
        public class Reader
        {
            byte[] data_;
            int frameCount_;
            int current_ = 0;
            public Reader(byte[] data)
            {
                data_ = data;
                frameCount_ = System.BitConverter.ToInt32(data_);
            }

            public static Reader Open(string path)
            {
                if (!File.Exists(path))
                {
                    return null;
                }
                return new Reader(File.ReadAllBytes(path));
            }

            const int JointSize = 4 + 7 * 4 + 4;
            const int FrameSize = 4 + HandTrackingFeature.XR_HAND_JOINT_COUNT_EXT * JointSize;

            void ReadJoint(ref int pos, ref HandTrackingFeature.XrHandJointLocationEXT joint)
            {
                joint.locationFlags = (XrSpaceLocationFlags)System.BitConverter.ToInt32(data_, pos);
                pos += 4;
                joint.pose.position.x = System.BitConverter.ToSingle(data_, pos);
                pos += 4;
                joint.pose.position.y = System.BitConverter.ToSingle(data_, pos);
                pos += 4;
                joint.pose.position.z = System.BitConverter.ToSingle(data_, pos);
                pos += 4;
                joint.pose.orientation.x = System.BitConverter.ToSingle(data_, pos);
                pos += 4;
                joint.pose.orientation.y = System.BitConverter.ToSingle(data_, pos);
                pos += 4;
                joint.pose.orientation.z = System.BitConverter.ToSingle(data_, pos);
                pos += 4;
                joint.pose.orientation.w = System.BitConverter.ToSingle(data_, pos);
                pos += 4;
                joint.radius = System.BitConverter.ToSingle(data_, pos);
                pos += 4;
            }

            public bool TryGetJoints(long time, out XrHandJointLocationEXT[] joints)
            {
                if (current_ >= frameCount_)
                {
                    joints = default;
                    return false;
                }
                var pos = 4 + FrameSize * current_;
                ++current_;

                // time
                pos += 4;

                // joints              
                joints = new HandTrackingFeature.XrHandJointLocationEXT[HandTrackingFeature.XR_HAND_JOINT_COUNT_EXT];
                for (int i = 0; i < HandTrackingFeature.XR_HAND_JOINT_COUNT_EXT; ++i)
                {
                    ReadJoint(ref pos, ref joints[i]);
                }
                return true;
            }
        }
        Frame[] Data;
        int Current = 0;

        void Start()
        {
            Data = new Frame[RecordFrames];
        }

        void OnDisable()
        {
            using (var w = new System.IO.FileStream(WritePath, FileMode.Create))
            using (var b = new BinaryWriter(w))
            {
                b.Write(Current);
                for (int i = 0; i < Current; ++i)
                {
                    b.Write(Data[i].Time);
                    foreach (var joint in Data[i].Joints)
                    {
                        b.Write((int)joint.locationFlags);
                        b.Write(joint.radius);
                        b.Write(joint.pose.position.x);
                        b.Write(joint.pose.position.y);
                        b.Write(joint.pose.position.z);
                        b.Write(joint.pose.orientation.x);
                        b.Write(joint.pose.orientation.y);
                        b.Write(joint.pose.orientation.z);
                        b.Write(joint.pose.orientation.w);
                    }
                }
            }
        }

        public void OnReceived(long time, HandTrackingFeature.XrHandJointLocationEXT[] joints)
        {
            if (Current < Data.Length)
            {
                Data[Current++] = new Frame
                {
                    Time = time,
                    Joints = joints.Select(x => x).ToArray(), // copy
                };
            }
        }
    }
}
