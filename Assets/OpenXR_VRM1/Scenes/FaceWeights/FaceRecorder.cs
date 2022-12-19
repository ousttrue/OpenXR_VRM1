using System.Runtime.InteropServices;
using OxrExtraFeatures;
using UnityEngine;
using static OxrExtraFeatures.FaceTrackingFeature;

namespace Vrm10XR
{
    public class FaceRecorder : MonoBehaviour, FaceTracker.IReceiver
    {
        [SerializeField]
        int RecordFrames = 1024;

        [SerializeField]
        string WritePath = "FaceJoints.dat";

        FrameWriter sink_;

        int count_;

        public static readonly int ItemSize = Marshal.SizeOf<float>() * (int)XrFaceExpressionFB.XR_FACE_EXPRESSION_COUNT_FB;

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

        public void OnReceived(long time, float[] expressions)
        {
            if (count_++ == 0)
            {
                // skip first frame
                return;
            }

            if (!sink_.PushArray(time, expressions))
            {
                enabled = false;
            }
        }
    }
}
