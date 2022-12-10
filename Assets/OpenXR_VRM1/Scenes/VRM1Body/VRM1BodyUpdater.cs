using openxr;
using UnityEngine;
using UniVRM10;
using static openxr.BodyTrackingFeature;


namespace Vrm10XR
{
    class VRM1BodyUpdater : MonoBehaviour
    {
        static (XrBodyJointFB, HumanBodyBones)[] JointToBone = new (XrBodyJointFB, HumanBodyBones)[]
            {
            // (XrBodyJointFB.XR_BODY_JOINT_ROOT_FB),
            (XrBodyJointFB.XR_BODY_JOINT_HIPS_FB, HumanBodyBones.Hips),
            // (XrBodyJointFB.XR_BODY_JOINT_SPINE_LOWER_FB),
            (XrBodyJointFB.XR_BODY_JOINT_SPINE_MIDDLE_FB, HumanBodyBones.Spine),
            (XrBodyJointFB.XR_BODY_JOINT_SPINE_UPPER_FB, HumanBodyBones.Chest),
            (XrBodyJointFB.XR_BODY_JOINT_CHEST_FB, HumanBodyBones.UpperChest),
            (XrBodyJointFB.XR_BODY_JOINT_NECK_FB, HumanBodyBones.Neck),
            (XrBodyJointFB.XR_BODY_JOINT_HEAD_FB, HumanBodyBones.Head),
            (XrBodyJointFB.XR_BODY_JOINT_LEFT_SHOULDER_FB, HumanBodyBones.LeftShoulder),
            // (XrBodyJointFB.XR_BODY_JOINT_LEFT_SCAPULA_FB),
            (XrBodyJointFB.XR_BODY_JOINT_LEFT_ARM_UPPER_FB, HumanBodyBones.LeftUpperArm),
            (XrBodyJointFB.XR_BODY_JOINT_LEFT_ARM_LOWER_FB, HumanBodyBones.LeftLowerArm),
            // (XrBodyJointFB.XR_BODY_JOINT_LEFT_HAND_WRIST_TWIST_FB),
            (XrBodyJointFB.XR_BODY_JOINT_RIGHT_SHOULDER_FB, HumanBodyBones.RightShoulder),
            // (XrBodyJointFB.XR_BODY_JOINT_RIGHT_SCAPULA_FB),
            (XrBodyJointFB.XR_BODY_JOINT_RIGHT_ARM_UPPER_FB, HumanBodyBones.RightUpperArm),
            (XrBodyJointFB.XR_BODY_JOINT_RIGHT_ARM_LOWER_FB, HumanBodyBones.RightLowerArm),
            // (XrBodyJointFB.XR_BODY_JOINT_RIGHT_HAND_WRIST_TWIST_FB),
            // (XrBodyJointFB.XR_BODY_JOINT_LEFT_HAND_PALM_FB),
            (XrBodyJointFB.XR_BODY_JOINT_LEFT_HAND_WRIST_FB, HumanBodyBones.LeftHand),
            (XrBodyJointFB.XR_BODY_JOINT_LEFT_HAND_THUMB_METACARPAL_FB, HumanBodyBones.LeftThumbProximal),
            (XrBodyJointFB.XR_BODY_JOINT_LEFT_HAND_THUMB_PROXIMAL_FB, HumanBodyBones.LeftThumbIntermediate),
            (XrBodyJointFB.XR_BODY_JOINT_LEFT_HAND_THUMB_DISTAL_FB, HumanBodyBones.LeftThumbDistal),
            // (XrBodyJointFB.XR_BODY_JOINT_LEFT_HAND_THUMB_TIP_FB),
            // (XrBodyJointFB.XR_BODY_JOINT_LEFT_HAND_INDEX_METACARPAL_FB),
            (XrBodyJointFB.XR_BODY_JOINT_LEFT_HAND_INDEX_PROXIMAL_FB, HumanBodyBones.LeftIndexProximal),
            (XrBodyJointFB.XR_BODY_JOINT_LEFT_HAND_INDEX_INTERMEDIATE_FB, HumanBodyBones.LeftIndexIntermediate),
            (XrBodyJointFB.XR_BODY_JOINT_LEFT_HAND_INDEX_DISTAL_FB, HumanBodyBones.LeftIndexDistal),
            // (XrBodyJointFB.XR_BODY_JOINT_LEFT_HAND_INDEX_TIP_FB),
            // (XrBodyJointFB.XR_BODY_JOINT_LEFT_HAND_MIDDLE_METACARPAL_FB),
            (XrBodyJointFB.XR_BODY_JOINT_LEFT_HAND_MIDDLE_PROXIMAL_FB, HumanBodyBones.LeftMiddleProximal),
            (XrBodyJointFB.XR_BODY_JOINT_LEFT_HAND_MIDDLE_INTERMEDIATE_FB, HumanBodyBones.LeftMiddleIntermediate),
            (XrBodyJointFB.XR_BODY_JOINT_LEFT_HAND_MIDDLE_DISTAL_FB, HumanBodyBones.LeftMiddleDistal),
            // (XrBodyJointFB.XR_BODY_JOINT_LEFT_HAND_MIDDLE_TIP_FB),
            // (XrBodyJointFB.XR_BODY_JOINT_LEFT_HAND_RING_METACARPAL_FB),
            (XrBodyJointFB.XR_BODY_JOINT_LEFT_HAND_RING_PROXIMAL_FB, HumanBodyBones.LeftRingProximal),
            (XrBodyJointFB.XR_BODY_JOINT_LEFT_HAND_RING_INTERMEDIATE_FB, HumanBodyBones.LeftRingIntermediate),
            (XrBodyJointFB.XR_BODY_JOINT_LEFT_HAND_RING_DISTAL_FB, HumanBodyBones.LeftRingDistal),
            // (XrBodyJointFB.XR_BODY_JOINT_LEFT_HAND_RING_TIP_FB),
            // (XrBodyJointFB.XR_BODY_JOINT_LEFT_HAND_LITTLE_METACARPAL_FB),
            (XrBodyJointFB.XR_BODY_JOINT_LEFT_HAND_LITTLE_PROXIMAL_FB, HumanBodyBones.LeftLittleProximal),
            (XrBodyJointFB.XR_BODY_JOINT_LEFT_HAND_LITTLE_INTERMEDIATE_FB,HumanBodyBones.LeftLittleIntermediate),
            (XrBodyJointFB.XR_BODY_JOINT_LEFT_HAND_LITTLE_DISTAL_FB, HumanBodyBones.LeftLittleDistal),
            // (XrBodyJointFB.XR_BODY_JOINT_LEFT_HAND_LITTLE_TIP_FB),
            // (XrBodyJointFB.XR_BODY_JOINT_RIGHT_HAND_PALM_FB),
            (XrBodyJointFB.XR_BODY_JOINT_RIGHT_HAND_WRIST_FB, HumanBodyBones.RightHand),
            (XrBodyJointFB.XR_BODY_JOINT_RIGHT_HAND_THUMB_METACARPAL_FB, HumanBodyBones.RightThumbProximal),
            (XrBodyJointFB.XR_BODY_JOINT_RIGHT_HAND_THUMB_PROXIMAL_FB, HumanBodyBones.RightThumbIntermediate),
            (XrBodyJointFB.XR_BODY_JOINT_RIGHT_HAND_THUMB_DISTAL_FB, HumanBodyBones.RightThumbDistal),
            // (XrBodyJointFB.XR_BODY_JOINT_RIGHT_HAND_THUMB_TIP_FB),
            // (XrBodyJointFB.XR_BODY_JOINT_RIGHT_HAND_INDEX_METACARPAL_FB),
            (XrBodyJointFB.XR_BODY_JOINT_RIGHT_HAND_INDEX_PROXIMAL_FB, HumanBodyBones.RightIndexProximal),
            (XrBodyJointFB.XR_BODY_JOINT_RIGHT_HAND_INDEX_INTERMEDIATE_FB, HumanBodyBones.RightIndexIntermediate),
            (XrBodyJointFB.XR_BODY_JOINT_RIGHT_HAND_INDEX_DISTAL_FB, HumanBodyBones.RightIndexDistal),
            // (XrBodyJointFB.XR_BODY_JOINT_RIGHT_HAND_INDEX_TIP_FB),
            // (XrBodyJointFB.XR_BODY_JOINT_RIGHT_HAND_MIDDLE_METACARPAL_FB),
            (XrBodyJointFB.XR_BODY_JOINT_RIGHT_HAND_MIDDLE_PROXIMAL_FB, HumanBodyBones.RightMiddleProximal),
            (XrBodyJointFB.XR_BODY_JOINT_RIGHT_HAND_MIDDLE_INTERMEDIATE_FB, HumanBodyBones.RightMiddleIntermediate),
            (XrBodyJointFB.XR_BODY_JOINT_RIGHT_HAND_MIDDLE_DISTAL_FB, HumanBodyBones.RightMiddleDistal),
            // (XrBodyJointFB.XR_BODY_JOINT_RIGHT_HAND_MIDDLE_TIP_FB),
            // (XrBodyJointFB.XR_BODY_JOINT_RIGHT_HAND_RING_METACARPAL_FB),
            (XrBodyJointFB.XR_BODY_JOINT_RIGHT_HAND_RING_PROXIMAL_FB, HumanBodyBones.RightRingProximal),
            (XrBodyJointFB.XR_BODY_JOINT_RIGHT_HAND_RING_INTERMEDIATE_FB, HumanBodyBones.RightRingIntermediate),
            (XrBodyJointFB.XR_BODY_JOINT_RIGHT_HAND_RING_DISTAL_FB, HumanBodyBones.RightRingProximal),
            // (XrBodyJointFB.XR_BODY_JOINT_RIGHT_HAND_RING_TIP_FB),
            // (XrBodyJointFB.XR_BODY_JOINT_RIGHT_HAND_LITTLE_METACARPAL_FB),
            (XrBodyJointFB.XR_BODY_JOINT_RIGHT_HAND_LITTLE_PROXIMAL_FB, HumanBodyBones.RightLittleProximal),
            (XrBodyJointFB.XR_BODY_JOINT_RIGHT_HAND_LITTLE_INTERMEDIATE_FB, HumanBodyBones.RightLittleIntermediate),
            (XrBodyJointFB.XR_BODY_JOINT_RIGHT_HAND_LITTLE_DISTAL_FB, HumanBodyBones.RightLittleDistal),
            // (XrBodyJointFB.XR_BODY_JOINT_RIGHT_HAND_LITTLE_TIP_FB),
        };

        // rename .vrm to .txt and set
        [SerializeField]
        TextAsset VRM1Binary;
        Vrm10Instance vrm_;

        bool isLeft_;

        public VRM1BodyUpdater(Vrm10Instance vrm, bool isLeft)
        {
            vrm_ = vrm;
            isLeft_ = isLeft;
        }

        // Start is called before the first frame update
        async void Start()
        {
            vrm_ = await VRM1Loader.LoadAsync(VRM1Binary.bytes, ControlRigGenerationOption.Vrm0XCompatibleWithXR_FB_body_tracking);
            vrm_.transform.SetParent(transform, false);

            // VR用 FirstPerson 設定
            await vrm_.Vrm.FirstPerson.SetupAsync(vrm_.gameObject, new VRMShaders.RuntimeOnlyAwaitCaller());
        }

        public void OnJointsUpdated(BodyTrackingFeature.XrBodyJointLocationFB[] joints)
        {
            if (vrm_ == null)
            {
                return;
            }

            foreach (var (i, b) in JointToBone)
            {
                var joint = joints[(int)i];

                var t = vrm_.Runtime.ControlRig.GetBoneTransform(b);
                if (t != null)
                {
                    if (i == XrBodyJointFB.XR_BODY_JOINT_HIPS_FB)
                    {
                        if (vrm_.TryGetBoneTransform(b, out var bodyTransform))
                        {
                            // directory assign position
                            t.position = joint.pose.position.ToUnity();
                        }
                    }
                    t.rotation = joint.pose.orientation.ToUnity();
                }
                else
                {
                    Debug.LogWarning($"{b} is null");
                }
            }
        }
    }
}