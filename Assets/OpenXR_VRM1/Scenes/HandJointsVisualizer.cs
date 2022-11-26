using openxr;
using UnityEngine;
using UnityEngine.XR.OpenXR;


[DisallowMultipleComponent]
public class HandJointsVisualizer : MonoBehaviour
{
    public Transform[] leftHandJoints_;
    public Transform[] rightHandJoints_;

    FrameStateFeature frameState_;

    HandTrackingFeature handTracking_;
    HandTrackingFeature.Tracker leftHandTracker_;
    HandTrackingFeature.Tracker rightHandTracker_;

    HandJoints drawLeftHand_;
    HandJoints drawRightHand_;

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
        drawLeftHand_ = new HandJoints(transform, "left");
        drawRightHand_ = new HandJoints(transform, "right");

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

    void HandBegin(HandTrackingFeature.Tracker left, HandTrackingFeature.Tracker right)
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
        drawLeftHand_.Update(time, leftHandTracker_);
        drawRightHand_.Update(time, rightHandTracker_);
    }
}
