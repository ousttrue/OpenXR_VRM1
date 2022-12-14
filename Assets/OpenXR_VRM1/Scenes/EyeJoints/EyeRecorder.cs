using System.Runtime.InteropServices;
using OxrExtraFeatures;
using UnityEngine;
using static OxrExtraFeatures.EyeTrackingFeature;

namespace Vrm10XR
{
    public class EyeRecorder : MonoBehaviour, EyeTracker.IReceiver
    {
        [SerializeField]
        int RecordFrames = 1024;

        [SerializeField]
        string WritePath = "EyeJoints.dat";

        FrameWriter sink_;

        int count_;

        public static readonly int ItemSize = Marshal.SizeOf<XrEyeGazeV2FB>() * 2;

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

        public void OnReceived(long time, XrEyeGazeV2FB[] joints)
        {
            if (sink_ == null)
            {
                return;
            }

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
