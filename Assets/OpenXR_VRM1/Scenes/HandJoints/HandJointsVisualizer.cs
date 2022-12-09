using openxr;
using UnityEngine;
using UnityEngine.XR.OpenXR;


[DisallowMultipleComponent]
public class HandJointsVisualizer : MonoBehaviour
{
    FrameTimeFeature frame_;

    HandTrackingFeature handTracking_;
    HandTracker leftHandTracker_;
    HandTracker rightHandTracker_;

    HandJoints leftHandJoints_;
    HandJoints rightHandJoints_;

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
        leftHandJoints_ = new HandJoints(transform, "left");
        rightHandJoints_ = new HandJoints(transform, "right");

        if (!TryGetFeature(out frame_))
        {
            this.enabled = false;
            Debug.LogError("fail to get frameState_");
            return;
        }

        if (!TryGetFeature(out handTracking_))
        {
            this.enabled = false;
            Debug.LogError("fail to get handTracking_");
            return;
        }

        handTracking_.SessionBegin += HandBegin;
        handTracking_.SessionEnd += HandEnd;
    }

    void HandBegin(HandTrackingFeature feature, ulong session)
    {
        Debug.Log("HandBegin");
        leftHandTracker_ = HandTracker.CreateTracker(feature, session, HandTrackingFeature.XrHandEXT.XR_HAND_LEFT_EXT);
        rightHandTracker_ = HandTracker.CreateTracker(feature, session, HandTrackingFeature.XrHandEXT.XR_HAND_RIGHT_EXT);
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
            leftHandJoints_.Update(time, space, leftHandTracker_);
        }
        if (rightHandTracker_ != null)
        {
            rightHandJoints_.Update(time, space, rightHandTracker_);
        }
    }
}
