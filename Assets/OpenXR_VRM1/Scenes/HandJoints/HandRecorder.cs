using System.Runtime.InteropServices;
using OxrExtraFeatures;
using UnityEngine;
using static OxrExtraFeatures.HandTrackingFeature;

namespace Vrm10XR
{
    public class HandRecorder : MonoBehaviour, HandTracker.IReceiver
    {
        [SerializeField]
        int RecordFrames = 1024;

        [SerializeField]
        string WritePath = "HandJoints.dat";

        FrameWriter sink_;

        int count_;

        public static readonly int ItemSize = Marshal.SizeOf<XrHandJointLocationEXT>() * HandTrackingFeature.XR_HAND_JOINT_COUNT_EXT;

        void Start()
        {
            sink_ = new FrameWriter(ItemSize, RecordFrames);
        }

        void OnDisable()
        {
            sink_.WriteTo(WritePath);
            sink_.Dispose();
            sink_ = null;
            Debug.LogWarning("Close");
        }

        public void OnReceived(long time, HandTrackingFeature.XrHandJointLocationEXT[] joints)
        {
            if (count_++ == 0)
            {
                // skip first frame
                return;
            }

            if (!sink_.PushArray(time, joints))
            {
                enabled = false;
            }
        }
    }
}
