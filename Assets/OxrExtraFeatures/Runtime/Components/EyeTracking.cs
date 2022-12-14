using OxrExtraFeatures;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.OpenXR;

namespace Vrm10XR
{
    [DisallowMultipleComponent]
    public class EyeTracking : MonoBehaviour
    {
        FrameStateFeature frame_;

        EyeTrackingFeature eyeTracking_;
        EyeTracker eyeTracker_;

        [SerializeField]
        UnityEvent<long, EyeTrackingFeature.XrEyeGazeV2FB[]> EyeUpdated;

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

        void Start()
        {
            if (!TryGetFeature(out frame_))
            {
                this.enabled = false;
                return;
            }

            if (!TryGetFeature(out eyeTracking_))
            {
                this.enabled = false;
                return;
            }

            eyeTracking_.SessionBegin += EyeBegin;
            eyeTracking_.SessionEnd += EyeEnd;
        }

        void EyeBegin(EyeTrackingFeature feature, ulong session)
        {
            Debug.Log("EyeBegin");
            eyeTracker_ = EyeTracker.Create(feature, session);
        }

        void EyeEnd()
        {
            Debug.Log("EyeEnd");
            eyeTracker_.Dispose();
            eyeTracker_ = null;
        }

        void Update()
        {
            var time = frame_.State.predictedDisplayTime;
            var space = frame_.CurrentAppSpace;
            if (eyeTracker_ != null)
            {
                if (eyeTracker_.TryGetGaze(time, space, out var gazes))
                {
                    EyeUpdated.Invoke(time, gazes);
                }
            }
        }
    }
}