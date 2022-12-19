using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Events;
using static OxrExtraFeatures.FaceTrackingFeature;
using OxrExtraFeatures;

namespace Vrm10XR
{
    public class FacePlayer : MonoBehaviour
    {
        [SerializeField]
        string DataPath = "FaceJoints.dat";

        [SerializeField]
        UnityEvent<long, float[]> OnJointUpdated;

        FrameReader reader_;
        float[] weights_;
        ArrayPin pin_;

        // Start is called before the first frame update
        void Start()
        {
            reader_ = FrameReader.Open(DataPath, Time.time, FaceRecorder.ItemSize);
            if (reader_ == null)
            {
                gameObject.SetActive(false);
            }
            Debug.Log($"open: {DataPath}");
            weights_ = new float[(int)XrFaceExpressionFB.XR_FACE_EXPRESSION_COUNT_FB];
            pin_ = new ArrayPin(weights_);
        }

        // Update is called once per frame
        void Update()
        {
            if (reader_.TryGetJoints(Time.time, out var time, out var item))
            {
                Marshal.Copy(item, 0, pin_.Ptr, FaceRecorder.ItemSize);
                OnJointUpdated.Invoke(time, weights_);
            }
        }

        void OnDisable()
        {
            reader_.Dispose();
            reader_ = null;
        }
    }
}