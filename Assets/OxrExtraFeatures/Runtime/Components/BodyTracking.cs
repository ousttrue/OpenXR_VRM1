using OxrExtraFeatures;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.OpenXR;

namespace Vrm10XR
{
    [DisallowMultipleComponent]
    public class BodyTracking : MonoBehaviour
    {
        FrameTimeFeature frame_;

        BodyTrackingFeature bodyTracking_;
        BodyTracker bodyTracker_;

        [SerializeField]
        UnityEvent<BodyTrackingFeature.XrBodySkeletonJointFB[]> SkeletonUpdated;

        [SerializeField]
        UnityEvent<BodyTrackingFeature.XrBodyJointLocationFB[]> BodyUpdated;

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
            SkeletonUpdated.Invoke(joints);
            // if (skeletonJoints_ == null)
            // {
            //     skeletonJoints_ = new Transform[joints.Length];
            // }

            // for (int i = 0; i < skeletonJoints_.Length; ++i)
            // {
            //     if (skeletonJoints_[i] == null)
            //     {
            //         var go = GameObject.CreatePrimitive(PrimitiveType.Cube);
            //         go.name = $"{(BodyTrackingFeature.XrBodyJointFB)i}";
            //         skeletonJoints_[i] = go.transform;
            //         skeletonJoints_[i].SetParent(transform);
            //         skeletonJoints_[i].localScale = new Vector3(0.02f, 0.02f, 0.02f);
            //     }

            //     skeletonJoints_[i].localRotation = joints[i].pose.orientation.ToUnity();
            //     skeletonJoints_[i].localPosition = joints[i].pose.position.ToUnity();
            // }
        }

        // Update is called once per frame
        void Update()
        {
            var time = frame_.FrameTime;
            var space = frame_.CurrentAppSpace;
            if (bodyTracker_ != null)
            {
                BodyTrackingFeature.XrBodyJointLocationFB[] joints = default;
                if (bodyTracker_.TryGetJoints(time, space, out joints))
                {
                    BodyUpdated.Invoke(joints);
                }
            }
        }
    }
}