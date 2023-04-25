using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UniVRM10;
using static OxrExtraFeatures.BodyTrackingFeature;
using static OxrExtraFeatures.EyeTrackingFeature;
using static OxrExtraFeatures.FaceTrackingFeature;


namespace Vrm10XR
{
    class VRM1BodyEyeFace : MonoBehaviour
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

        static (XrFaceExpressionFB, ExpressionPreset)[] ExpressionMap = new (XrFaceExpressionFB, ExpressionPreset)[]
        {
            (XrFaceExpressionFB.XR_FACE_EXPRESSION_EYES_CLOSED_R_FB, ExpressionPreset.blinkRight),
            (XrFaceExpressionFB.XR_FACE_EXPRESSION_EYES_CLOSED_L_FB, ExpressionPreset.blinkLeft),
            (XrFaceExpressionFB.XR_FACE_EXPRESSION_JAW_DROP_FB, ExpressionPreset.aa),
            //
            // (XrFaceExpressionFB.XR_FACE_EXPRESSION_UPPER_LID_RAISER_R_FB, ExpressionPreset.relaxed),
            // (XrFaceExpressionFB.XR_FACE_EXPRESSION_UPPER_LID_RAISER_L_FB, ExpressionPreset.relaxed),
            // (XrFaceExpressionFB.XR_FACE_EXPRESSION_LIP_CORNER_PULLER_L_FB, ExpressionPreset.happy),
            // (XrFaceExpressionFB.XR_FACE_EXPRESSION_LIP_CORNER_PULLER_R_FB, ExpressionPreset.happy),
            // (XrFaceExpressionFB.XR_FACE_EXPRESSION_CHIN_RAISER_B_FB, ExpressionPreset.angry),
            // (XrFaceExpressionFB.XR_FACE_EXPRESSION_CHEEK_PUFF_R_FB, ExpressionPreset.angry),
            // (XrFaceExpressionFB.XR_FACE_EXPRESSION_CHEEK_PUFF_L_FB, ExpressionPreset.angry),
            // (XrFaceExpressionFB.XR_FACE_EXPRESSION_INNER_BROW_RAISER_R_FB, ExpressionPreset.sad),
            // (XrFaceExpressionFB.XR_FACE_EXPRESSION_INNER_BROW_RAISER_L_FB, ExpressionPreset.sad),
            // (XrFaceExpressionFB.XR_FACE_EXPRESSION_BROW_LOWERER_R_FB, ExpressionPreset.surprised),
            // (XrFaceExpressionFB.XR_FACE_EXPRESSION_BROW_LOWERER_L_FB, ExpressionPreset.surprised),
            // (XrFaceExpressionFB.XR_FACE_EXPRESSION_OUTER_BROW_RAISER_R_FB, ExpressionPreset.surprised),
            // (XrFaceExpressionFB.XR_FACE_EXPRESSION_OUTER_BROW_RAISER_L_FB, ExpressionPreset.surprised),
        };

        // https://developer.oculus.com/documentation/native/android/move-ref-blendshapes/
        // https://hinzka.hatenablog.com/entry/2020/06/15/072929
        static Dictionary<XrFaceExpressionFB, string> XrTo52 = new Dictionary<XrFaceExpressionFB, string>
        {
            {XrFaceExpressionFB.XR_FACE_EXPRESSION_BROW_LOWERER_L_FB, "BrowDownLeft"}, // 2
            {XrFaceExpressionFB.XR_FACE_EXPRESSION_BROW_LOWERER_R_FB, "BrowDownRight"}, // 3

            // XR_FACE_EXPRESSION_CHEEK_PUFF_L_FB = 2,
            // XR_FACE_EXPRESSION_CHEEK_PUFF_R_FB = 3,
            // XR_FACE_EXPRESSION_CHEEK_RAISER_L_FB = 4,
            // XR_FACE_EXPRESSION_CHEEK_RAISER_R_FB = 5,
            // XR_FACE_EXPRESSION_CHEEK_SUCK_L_FB = 6,
            // XR_FACE_EXPRESSION_CHEEK_SUCK_R_FB = 7,

            {XrFaceExpressionFB.XR_FACE_EXPRESSION_CHIN_RAISER_B_FB, "CheekPuff"}, // 20
            {XrFaceExpressionFB.XR_FACE_EXPRESSION_CHIN_RAISER_T_FB, "MouthShrugUpper"}, // 35

            {XrFaceExpressionFB.XR_FACE_EXPRESSION_DIMPLER_L_FB, "MouthDimpleLeft"}, // 42
            {XrFaceExpressionFB.XR_FACE_EXPRESSION_DIMPLER_R_FB, "MouthDimpleRight"}, // 43

            {XrFaceExpressionFB.XR_FACE_EXPRESSION_EYES_CLOSED_L_FB, "EyeBlinkLeft"}, // 14
            {XrFaceExpressionFB.XR_FACE_EXPRESSION_EYES_CLOSED_R_FB, "EyeBlinkRight"}, // 15
            {XrFaceExpressionFB.XR_FACE_EXPRESSION_EYES_LOOK_DOWN_L_FB, "EyeLookDownLeft"}, // 8
            {XrFaceExpressionFB.XR_FACE_EXPRESSION_EYES_LOOK_DOWN_R_FB, "EyeLookDownRight"}, // 9
            {XrFaceExpressionFB.XR_FACE_EXPRESSION_EYES_LOOK_LEFT_L_FB, "EyeLookOutLeft"}, // 12
            {XrFaceExpressionFB.XR_FACE_EXPRESSION_EYES_LOOK_LEFT_R_FB, "EyeLookInRight"}, // 11
            {XrFaceExpressionFB.XR_FACE_EXPRESSION_EYES_LOOK_RIGHT_L_FB, "EyeLookInLeft"}, // 10
            {XrFaceExpressionFB.XR_FACE_EXPRESSION_EYES_LOOK_RIGHT_R_FB, "EyeLookOutRight"}, // 13
            {XrFaceExpressionFB.XR_FACE_EXPRESSION_EYES_LOOK_UP_L_FB, "EyeLookUpLeft"}, // 6
            {XrFaceExpressionFB.XR_FACE_EXPRESSION_EYES_LOOK_UP_R_FB, "EyeLookUpRight"}, // 7

            {XrFaceExpressionFB.XR_FACE_EXPRESSION_INNER_BROW_RAISER_L_FB, "BrowInnerUp"}, // 1
            {XrFaceExpressionFB.XR_FACE_EXPRESSION_INNER_BROW_RAISER_R_FB, "BrowInnerUp"}, // 1

            {XrFaceExpressionFB.XR_FACE_EXPRESSION_JAW_DROP_FB, "JawOpen"}, // 25
            {XrFaceExpressionFB.XR_FACE_EXPRESSION_JAW_SIDEWAYS_LEFT_FB, "JawLeft"}, // 27
            {XrFaceExpressionFB.XR_FACE_EXPRESSION_JAW_SIDEWAYS_RIGHT_FB, "JawRight"}, // 28
            {XrFaceExpressionFB.XR_FACE_EXPRESSION_JAW_THRUST_FB, "JawForward"}, // 26

            // XR_FACE_EXPRESSION_LID_TIGHTENER_L_FB = 28,
            // XR_FACE_EXPRESSION_LID_TIGHTENER_R_FB = 29,

            {XrFaceExpressionFB.XR_FACE_EXPRESSION_LIP_CORNER_DEPRESSOR_L_FB, "MouthFrownLeft"}, // 40
            {XrFaceExpressionFB.XR_FACE_EXPRESSION_LIP_CORNER_DEPRESSOR_R_FB, "MouthFrownRight"}, // 41
            {XrFaceExpressionFB.XR_FACE_EXPRESSION_LIP_CORNER_PULLER_L_FB, "MouthSmileLeft"}, // 38
            {XrFaceExpressionFB.XR_FACE_EXPRESSION_LIP_CORNER_PULLER_R_FB, "MouthSmileRight"}, // 39
            {XrFaceExpressionFB.XR_FACE_EXPRESSION_LIP_FUNNELER_LB_FB, "MouthFunnel"}, // 29
            {XrFaceExpressionFB.XR_FACE_EXPRESSION_LIP_FUNNELER_LT_FB, "MouthFunnel"}, // 29
            {XrFaceExpressionFB.XR_FACE_EXPRESSION_LIP_FUNNELER_RB_FB, "MouthFunnel"}, // 29
            {XrFaceExpressionFB.XR_FACE_EXPRESSION_LIP_FUNNELER_RT_FB, "MouthFunnel"}, // 29

            // XR_FACE_EXPRESSION_LIP_PRESSOR_L_FB = 38,
            // XR_FACE_EXPRESSION_LIP_PRESSOR_R_FB = 39,

            {XrFaceExpressionFB.XR_FACE_EXPRESSION_LIP_PUCKER_L_FB, "MouthPucker"}, // 30
            {XrFaceExpressionFB.XR_FACE_EXPRESSION_LIP_PUCKER_R_FB, "MouthPucker"}, // 30
            {XrFaceExpressionFB.XR_FACE_EXPRESSION_LIP_STRETCHER_L_FB, "MouthLeft"}, // 31
            {XrFaceExpressionFB.XR_FACE_EXPRESSION_LIP_STRETCHER_R_FB, "MouthRight"}, // 32 
            {XrFaceExpressionFB.XR_FACE_EXPRESSION_LIP_SUCK_LB_FB, "MouthRollLower"}, // 34
            {XrFaceExpressionFB.XR_FACE_EXPRESSION_LIP_SUCK_LT_FB, "MouthRollUpper"}, // 33
            {XrFaceExpressionFB.XR_FACE_EXPRESSION_LIP_SUCK_RB_FB, "MouthRollLower"}, // 34
            {XrFaceExpressionFB.XR_FACE_EXPRESSION_LIP_SUCK_RT_FB, "MouthRollUpper"}, // 33
            {XrFaceExpressionFB.XR_FACE_EXPRESSION_LIP_TIGHTENER_L_FB, "EyeSquintLeft"}, // 16
            {XrFaceExpressionFB.XR_FACE_EXPRESSION_LIP_TIGHTENER_R_FB, "EyeSquintRight"}, // 17
            {XrFaceExpressionFB.XR_FACE_EXPRESSION_LIPS_TOWARD_FB, "MouthClose"}, // 37
            {XrFaceExpressionFB.XR_FACE_EXPRESSION_LOWER_LIP_DEPRESSOR_L_FB, "MouthLowerDownLeft"}, // 46,
            {XrFaceExpressionFB.XR_FACE_EXPRESSION_LOWER_LIP_DEPRESSOR_R_FB, "MouthLowerDownRight"}, // 47

            {XrFaceExpressionFB.XR_FACE_EXPRESSION_MOUTH_LEFT_FB, "MouthPressLeft"}, // 48
            {XrFaceExpressionFB.XR_FACE_EXPRESSION_MOUTH_RIGHT_FB, "MouthPressRight"}, // 49

            {XrFaceExpressionFB.XR_FACE_EXPRESSION_NOSE_WRINKLER_L_FB, "NoseSneerLeft"}, // 23
            {XrFaceExpressionFB.XR_FACE_EXPRESSION_NOSE_WRINKLER_R_FB, "NoseSneerRight"}, // 24

            {XrFaceExpressionFB.XR_FACE_EXPRESSION_OUTER_BROW_RAISER_L_FB, "BrowOuterUpLeft"}, // 4 
            {XrFaceExpressionFB.XR_FACE_EXPRESSION_OUTER_BROW_RAISER_R_FB, "BrowOuterUpRight"}, // 5

            {XrFaceExpressionFB.XR_FACE_EXPRESSION_UPPER_LID_RAISER_L_FB, "EyeWideLeft"}, // 18
            {XrFaceExpressionFB.XR_FACE_EXPRESSION_UPPER_LID_RAISER_R_FB, "EyeWideRight"}, // 19
            {XrFaceExpressionFB.XR_FACE_EXPRESSION_UPPER_LIP_RAISER_L_FB, "MouthUpperUpLeft"}, // 44
            {XrFaceExpressionFB.XR_FACE_EXPRESSION_UPPER_LIP_RAISER_R_FB, "MouthUpperUpRight"}, // 45
        };

        [SerializeField]
        bool use52BlendShape;

        // rename .vrm to .txt and set
        [SerializeField]
        TextAsset VRM1Binary;

        Vrm10Instance vrm_;
        (INormalizedPoseApplicable, ITPoseProvider) sink_;

        bool isLeft_;

        Transform[] objects_;
        Transform skeletonRoot_;
        (INormalizedPoseProvider, ITPoseProvider) src_;
        UniHumanoid.Humanoid humanoid_;

        // Start is called before the first frame update
        async void Start()
        {
            vrm_ = await VRM1Loader.LoadAsync(VRM1Binary.bytes, ControlRigGenerationOption.Generate);
            vrm_.transform.SetParent(transform, false);

            // VR用 FirstPerson 設定
            await vrm_.Vrm.FirstPerson.SetupAsync(vrm_.gameObject, new VRMShaders.RuntimeOnlyAwaitCaller());
            // lookat
            vrm_.LookAtTargetType = VRM10ObjectLookAt.LookAtTargetTypes.YawPitchValue;

            sink_ = (vrm_.Runtime.ControlRig, vrm_.Runtime.ControlRig);

            // skeleton
            objects_ = new Transform[(int)XrBodyJointFB.XR_BODY_JOINT_COUNT_FB];
            for (int i = 0; i < objects_.Length; ++i)
            {
                var value = (XrBodyJointFB)i;
                var t = new GameObject($"{value}").transform;
                {
                    var cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    cube.transform.SetParent(t);
                    cube.transform.localScale = new Vector3(0.02f, 0.02f, 0.02f);
                }
                t.SetParent(transform);
                objects_[i] = t;
            }
            humanoid_ = objects_[0].gameObject.AddComponent<UniHumanoid.Humanoid>();
            humanoid_.AssignBones(JointToBone.Select((fb_unity) => (fb_unity.Item2, objects_[(int)fb_unity.Item1])));
        }

        public void OnSkeletonUpdated(long time, XrBodySkeletonJointFB[] joints)
        {
            for (int i = 0; i < joints.Length; ++i)
            {
                var joint = joints[i];
                if (joint.parentJoint >= 0 && joint.parentJoint < objects_.Length)
                {
                    objects_[i].SetParent(objects_[joint.parentJoint]);
                }
                // var JOINT_SIZE = 0.02f;
                // objects_[i].localScale = new Vector3(JOINT_SIZE, JOINT_SIZE, JOINT_SIZE);
                // convert OpenXR right handed to unity left handed !
                objects_[i].position = joint.pose.position.ToUnity();
                objects_[i].rotation = joint.pose.orientation.ToUnity();
            }

            var provider = new InitRotationPoseProvider(humanoid_.transform, humanoid_);
            src_ = (provider, provider);
        }

        public void OnBodyUpdated(long Time, XrBodyJointLocationFB[] joints)
        {
            if (vrm_ == null)
            {
                return;
            }

            for (int i = 0; i < joints.Length; ++i)
            {
                var t = objects_[i];
                var joint = joints[i];
                // t.
                t.rotation = joint.pose.orientation.ToUnity();
            }

            Retarget(src_, sink_);
        }

        static void Retarget(
            (INormalizedPoseProvider Pose, ITPoseProvider TPose) source,
            (INormalizedPoseApplicable Pose, ITPoseProvider TPose) sink)
        {
            foreach (var (head, parent) in sink.TPose.EnumerateBoneParentPairs())
            {
                var q = source.Pose.GetNormalizedLocalRotation(head, parent);
                sink.Pose.SetNormalizedLocalRotation(head, q);
            }

            // scaling hips position
            var scaling = sink.TPose.GetWorldTransform(HumanBodyBones.LeftUpperLeg).Value.Translation.y / source.TPose.GetWorldTransform(HumanBodyBones.LeftUpperLeg).Value.Translation.y;
            var delta = source.Pose.GetRawHipsPosition() - source.TPose.GetWorldTransform(HumanBodyBones.Hips).Value.Translation;
            sink.Pose.SetRawHipsPosition(sink.TPose.GetWorldTransform(HumanBodyBones.Hips).Value.Translation + delta * scaling);
        }

        static void EnforceTPose((INormalizedPoseApplicable Pose, ITPoseProvider TPose) sink)
        {
            foreach (var (bone, parent) in sink.TPose.EnumerateBoneParentPairs())
            {
                sink.Pose.SetNormalizedLocalRotation(bone, Quaternion.identity);
            }

            sink.Pose.SetRawHipsPosition(sink.TPose.GetWorldTransform(HumanBodyBones.Hips).Value.Translation);
        }

        static float ClampDegree(float deg)
        {
            while (deg > 180.0f)
            {
                deg -= 360.0f;
            }
            while (deg < -180.0f)
            {
                deg += 360.0f;
            }
            return deg;
        }

        const float LOOK_FACTOR = 4.0f;
        public void OnEyeUpdated(long time, XrEyeGazeV2FB[] gazes)
        {
            if (vrm_ == null)
            {
                return;
            }

            var leftRotation = gazes[0].gazePose.orientation.ToUnity();
            var leftLocal = Quaternion.Inverse(Camera.main.transform.rotation) * leftRotation;
            var leftEuler = leftLocal.eulerAngles;

            vrm_.Runtime.LookAt.SetYawPitchManually(
                ClampDegree(leftEuler.y) * LOOK_FACTOR,
                ClampDegree(-leftEuler.x) * LOOK_FACTOR);
        }

        public void OnFaceUpdated(long time, float[] weights)
        {
            if (vrm_ == null)
            {
                return;
            }

            if (use52BlendShape)
            {
                for (int i = 0; i < weights.Length; ++i)
                {
                    if (XrTo52.TryGetValue((XrFaceExpressionFB)i, out var key))
                    {
                        vrm_.Runtime.Expression.SetWeight(ExpressionKey.CreateCustom(key), weights[i]);
                    }
                }
            }
            else
            {
                foreach (var (xrExpression, vrmExpression) in ExpressionMap)
                {
                    vrm_.Runtime.Expression.SetWeight(ExpressionKey.CreateFromPreset(vrmExpression), weights[(int)xrExpression]);
                }
            }
        }
    }
}