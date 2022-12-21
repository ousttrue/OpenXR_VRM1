using OxrExtraFeatures;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.OpenXR;

namespace Vrm10XR
{
    [DisallowMultipleComponent]
    public class FaceTracking : MonoBehaviour
    {
        FrameStateFeature frame_;

        FaceTrackingFeature faceTracking_;
        FaceTracker faceTracker_;

        [SerializeField]
        UnityEvent<long, float[]> FaceWeightsUpdated;

        static bool TryGetFeature<T>(out T feature) where T : UnityEngine.XR.OpenXR.Features.OpenXRFeature
        {
            feature = OpenXRSettings.Instance.GetFeature<T>();
            if (feature == null || feature.enabled == false)
            {
                Debug.LogError($"fail to get {typeof(T)}");
                return false;
            }
            return true;
        }

        // Start is called before the first frame update
        void Start()
        {
            if (!TryGetFeature(out frame_))
            {
                this.enabled = false;
                return;
            }

            if (!TryGetFeature(out faceTracking_))
            {
                this.enabled = false;
                return;
            }

            faceTracking_.SessionBegin += FaceBegin;
            faceTracking_.SessionEnd += FaceEnd;
        }

        void FaceBegin(FaceTrackingFeature feature, ulong session)
        {
            Debug.Log("FaceBegin");
            faceTracker_ = FaceTracker.Create(feature, session);
        }

        void FaceEnd()
        {
            Debug.Log("FaceEnd");
            faceTracker_.Dispose();
            faceTracker_ = null;
        }

        void Update()
        {
            var time = frame_.FrameTime;
            var space = frame_.CurrentAppSpace;
            if (faceTracker_ != null)
            {
                if (faceTracker_.TryGetFaceExpressionWeights(time, space, out var weights, out var confidences))
                {
                    FaceWeightsUpdated.Invoke(time, weights);
                }
            }
        }
    }
}