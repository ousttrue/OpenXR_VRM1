using openxr;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.OpenXR;

namespace Vrm10XR
{
    [DisallowMultipleComponent]
    public class EyeTracking : MonoBehaviour
    {
        FrameTimeFeature frame_;

        EyeTrackingFeature eyeTracking_;
        EyeTracker eyeTracker_;

        [SerializeField]
        UnityEvent<EyeTrackingFeature.XrEyeGazeV2FB[]> EyeUpdated;

        static bool TryGetFeature<T>(out T feature) where T : UnityEngine.XR.OpenXR.Features.OpenXRFeature
        {
            feature = OpenXRSettings.Instance.GetFeature<T>();
            if (feature == null || feature.enabled == false)
            {
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
                Debug.LogError("fail to get frameState_");
                return;
            }

            if (!TryGetFeature(out eyeTracking_))
            {
                this.enabled = false;
                Debug.LogError("fail to get bodyTracking_");
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
            var time = frame_.FrameTime;
            var space = frame_.CurrentAppSpace;
            if (eyeTracker_ != null)
            {
                if (eyeTracker_.TryGetGaze(time, space, out var gazes))
                {
                    EyeUpdated.Invoke(gazes);
                }
            }
        }
    }
}