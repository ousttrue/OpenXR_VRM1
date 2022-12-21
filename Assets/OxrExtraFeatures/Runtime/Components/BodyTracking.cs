using OxrExtraFeatures;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.OpenXR;

namespace Vrm10XR
{
    [DisallowMultipleComponent]
    public class BodyTracking : MonoBehaviour
    {
        FrameStateFeature frame_;

        BodyTrackingFeature bodyTracking_;
        BodyTracker bodyTracker_;

        [SerializeField]
        UnityEvent<long, BodyTrackingFeature.XrBodySkeletonJointFB[]> SkeletonUpdated;

        [SerializeField]
        UnityEvent<long, BodyTrackingFeature.XrBodyJointLocationFB[]> BodyUpdated;

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

            if (!TryGetFeature(out bodyTracking_))
            {
                this.enabled = false;
                return;
            }

            bodyTracking_.SessionBegin += BodyBegin;
            bodyTracking_.SessionEnd += BodyEnd;
        }

        void BodyBegin(BodyTrackingFeature feature, ulong session)
        {
            Debug.Log("BodyBegin");
            bodyTracker_ = BodyTracker.Create(feature, session);

            bodyTracker_.SkeletonUpdated += OnSkeleton;
        }

        void BodyEnd()
        {
            Debug.Log("BodyEnd");
            bodyTracker_.Dispose();
            bodyTracker_ = null;
        }

        void OnSkeleton(BodyTrackingFeature.XrBodySkeletonJointFB[] joints)
        {
            SkeletonUpdated.Invoke(frame_.State.predictedDisplayTime, joints);
        }

        void Update()
        {
            var time = frame_.State.predictedDisplayTime;
            var space = frame_.CurrentAppSpace;
            if (bodyTracker_ != null)
            {
                BodyTrackingFeature.XrBodyJointLocationFB[] joints = default;
                if (bodyTracker_.TryGetJoints(time, space, out joints))
                {
                    BodyUpdated.Invoke(time, joints);
                }
            }
        }
    }
}