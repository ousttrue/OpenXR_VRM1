using openxr;
using UniGLTF;
using UnityEngine;
using UnityEngine.XR.OpenXR;
using UniVRM10;

public class VRM1Loader : MonoBehaviour
{
    // rename .vrm to .txt and set
    [SerializeField]
    TextAsset VRM1Binary;
    Vrm10Instance vrm_;

    // openxr
    FrameTimeFeature frame_;
    HandTrackingFeature handTracking_;
    HandTracker leftHandTracker_;
    HandTracker rightHandTracker_;

    HandJoints leftJoints_;
    VRM1HandUpdater leftUpdater_;
    HandJoints rightJoints_;
    VRM1HandUpdater rightUpdater_;

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

        if (!TryGetFeature(out handTracking_))
        {
            this.enabled = false;
            Debug.LogError("fail to get handTracking_");
            return;
        }

        handTracking_.SessionBegin += HandBegin;
        handTracking_.SessionEnd += HandEnd;

        leftJoints_ = new HandJoints(transform, "left.");
        rightJoints_ = new HandJoints(transform, ".right");
        LoadAsync();
    }

    async void LoadAsync()
    {
        // load vrm1
        var vrm10Instance = await Vrm10.LoadBytesAsync(VRM1Binary.bytes,
            controlRigGenerationOption: ControlRigGenerationOption.Vrm0XCompatibleWithXR_EXT_hand_tracking);
        if (vrm10Instance == null)
        {
            Debug.LogWarning("LoadPathAsync is null");
            return;
        }
        var instance = vrm10Instance.GetComponent<RuntimeGltfInstance>();
        instance.ShowMeshes();
        instance.EnableUpdateWhenOffscreen();

        instance.transform.SetParent(transform, false);

        vrm_ = instance.GetComponent<Vrm10Instance>();
        leftUpdater_ = new VRM1HandUpdater(vrm_, true);
        rightUpdater_ = new VRM1HandUpdater(vrm_, false);
    }

    void HandBegin(HandTrackingFeature feature, ulong session)
    {
        leftHandTracker_ = HandTracker.CreateTracker(feature, session, HandTrackingFeature.XrHandEXT.XR_HAND_LEFT_EXT);
        rightHandTracker_ = HandTracker.CreateTracker(feature, session, HandTrackingFeature.XrHandEXT.XR_HAND_RIGHT_EXT);
    }

    void HandEnd()
    {
        leftHandTracker_.Dispose();
        leftHandTracker_ = null;
        rightHandTracker_.Dispose();
        rightHandTracker_ = null;
    }

    // Update is called once per frame
    void Update()
    {
        if (leftUpdater_ == null || leftUpdater_ == null)
        {
            return;
        }
        var time = frame_.FrameTime;
        var space = frame_.CurrentAppSpace;
        if (leftHandTracker_ != null)
        {
            leftJoints_.Update(time, space, leftHandTracker_);
            leftUpdater_.Update(time, space, leftHandTracker_);
        }
        if (rightHandTracker_ != null)
        {
            rightJoints_.Update(time, space, rightHandTracker_);
            rightUpdater_.Update(time, space, rightHandTracker_);
        }
    }
}
