using OxrExtraFeatures;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.OpenXR;

namespace Vrm10XR
{
    [DisallowMultipleComponent]
    public class HandTracking : MonoBehaviour
    {
        FrameStateFeature frame_;

        HandTrackingFeature handTracking_;
        HandTracker leftHandTracker_;
        HandTracker rightHandTracker_;

        [SerializeField]
        UnityEvent<long, HandTrackingFeature.XrHandJointLocationEXT[]> OnLeftJointUpdated;

        [SerializeField]
        UnityEvent<long, HandTrackingFeature.XrHandJointLocationEXT[]> OnRightJointUpdated;

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

            if (!TryGetFeature(out handTracking_))
            {
                this.enabled = false;
                return;
            }

            handTracking_.SessionBegin += HandBegin;
            handTracking_.SessionEnd += HandEnd;
        }

        void HandBegin(HandTrackingFeature feature, ulong session)
        {
            Debug.Log("HandBegin");
            leftHandTracker_ = HandTracker.Create(feature, session, HandTrackingFeature.XrHandEXT.XR_HAND_LEFT_EXT);
            rightHandTracker_ = HandTracker.Create(feature, session, HandTrackingFeature.XrHandEXT.XR_HAND_RIGHT_EXT);
        }

        void HandEnd()
        {
            Debug.Log("HandEnd");
            leftHandTracker_.Dispose();
            leftHandTracker_ = null;
            rightHandTracker_.Dispose();
            rightHandTracker_ = null;
        }

        // Update is called once per frame
        void Update()
        {
            var time = frame_.FrameTime;
            var space = frame_.CurrentAppSpace;
            if (leftHandTracker_ != null)
            {
                HandTrackingFeature.XrHandJointLocationEXT[] joints = default;
                if (leftHandTracker_.TryGetJoints(time, space, out joints))
                {
                    OnLeftJointUpdated.Invoke(time, joints);
                }
            }
            if (rightHandTracker_ != null)
            {
                HandTrackingFeature.XrHandJointLocationEXT[] joints = default;
                if (rightHandTracker_.TryGetJoints(time, space, out joints))
                {
                    OnRightJointUpdated.Invoke(time, joints);
                }
            }
        }
    }
}