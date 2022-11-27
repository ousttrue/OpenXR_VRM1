using openxr;
using UnityEngine;
using UnityEngine.XR.OpenXR;


[DisallowMultipleComponent]
public class HandJointsVisualizer : MonoBehaviour
{
    FrameStateFeature frameState_;

    HandTrackingFeature handTracking_;
    HandTrackingTracker leftHandTracker_;
    HandTrackingTracker rightHandTracker_;

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

        if (!TryGetFeature(out frameState_))
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

    void HandBegin(HandTrackingTracker left, HandTrackingTracker right)
    {
        Debug.Log("HandBegin");
        leftHandTracker_ = left;
        rightHandTracker_ = right;
    }

    void HandEnd()
    {
        Debug.Log("HandEnd");
        leftHandTracker_ = null;
        rightHandTracker_ = null;
    }

    // Update is called once per frame
    void Update()
    {
        var frame = frameState_.FrameState;
        var time = frame.predictedDisplayTime;
        leftHandJoints_.Update(time, leftHandTracker_);
        rightHandJoints_.Update(time, rightHandTracker_);
    }
}
