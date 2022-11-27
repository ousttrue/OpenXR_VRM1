
using System;
using System.Collections.Generic;
using UnityEngine;
using UniVRM10;
using static openxr.HandTrackingFeature;

class VRM1HandUpdater : IDisposable
{
    static (int, XrHandJointEXT, HumanBodyBones)[] JointToBone = new (int, XrHandJointEXT, HumanBodyBones)[]
        {
            // left
            // {(0, XrHandJointEXT.XR_HAND_JOINT_PALM_EXT), HumanBodyBones.LeftHand},
            (0, XrHandJointEXT.XR_HAND_JOINT_WRIST_EXT, HumanBodyBones.LeftHand),
            (0, XrHandJointEXT.XR_HAND_JOINT_THUMB_METACARPAL_EXT, HumanBodyBones.LeftThumbProximal),
            (0, XrHandJointEXT.XR_HAND_JOINT_THUMB_PROXIMAL_EXT, HumanBodyBones.LeftThumbIntermediate),
            (0, XrHandJointEXT.XR_HAND_JOINT_THUMB_DISTAL_EXT, HumanBodyBones.LeftThumbDistal),
            // (0, XrHandJointEXT.XR_HAND_JOINT_THUMB_TIP_EXT, HumanBodyBones.),
            // (0, XrHandJointEXT.XR_HAND_JOINT_INDEX_METACARPAL_EXT, HumanBodyBones.),
            (0, XrHandJointEXT.XR_HAND_JOINT_INDEX_PROXIMAL_EXT, HumanBodyBones.LeftIndexProximal),
            (0, XrHandJointEXT.XR_HAND_JOINT_INDEX_INTERMEDIATE_EXT, HumanBodyBones.LeftIndexIntermediate),
            (0, XrHandJointEXT.XR_HAND_JOINT_INDEX_DISTAL_EXT, HumanBodyBones.LeftIndexDistal),
            // (0, XrHandJointEXT.XR_HAND_JOINT_INDEX_TIP_EXT, HumanBodyBones.),
            // (0, XrHandJointEXT.XR_HAND_JOINT_MIDDLE_METACARPAL_EXT, HumanBodyBones.),
            (0, XrHandJointEXT.XR_HAND_JOINT_MIDDLE_PROXIMAL_EXT, HumanBodyBones.LeftMiddleProximal),
            (0, XrHandJointEXT.XR_HAND_JOINT_MIDDLE_INTERMEDIATE_EXT, HumanBodyBones.LeftIndexIntermediate),
            (0, XrHandJointEXT.XR_HAND_JOINT_MIDDLE_DISTAL_EXT, HumanBodyBones.LeftMiddleDistal),
            // (0, XrHandJointEXT.XR_HAND_JOINT_MIDDLE_TIP_EXT, HumanBodyBones.),
            // (0, XrHandJointEXT.XR_HAND_JOINT_RING_METACARPAL_EXT, HumanBodyBones.),
            (0, XrHandJointEXT.XR_HAND_JOINT_RING_PROXIMAL_EXT, HumanBodyBones.LeftRingProximal),
            (0, XrHandJointEXT.XR_HAND_JOINT_RING_INTERMEDIATE_EXT, HumanBodyBones.LeftRingIntermediate),
            (0, XrHandJointEXT.XR_HAND_JOINT_RING_DISTAL_EXT, HumanBodyBones.LeftRingDistal),
            // (0, XrHandJointEXT.XR_HAND_JOINT_RING_TIP_EXT, HumanBodyBones.),
            // (0, XrHandJointEXT.XR_HAND_JOINT_LITTLE_METACARPAL_EXT, HumanBodyBones.),
            (0, XrHandJointEXT.XR_HAND_JOINT_LITTLE_PROXIMAL_EXT, HumanBodyBones.LeftLittleProximal),
            (0, XrHandJointEXT.XR_HAND_JOINT_LITTLE_INTERMEDIATE_EXT, HumanBodyBones.LeftLittleIntermediate),
            (0, XrHandJointEXT.XR_HAND_JOINT_LITTLE_DISTAL_EXT, HumanBodyBones.LeftLittleDistal),
            // (0, XrHandJointEXT.XR_HAND_JOINT_LITTLE_TIP_EXT, HumanBodyBones.),
            // right
            (1, XrHandJointEXT.XR_HAND_JOINT_WRIST_EXT, HumanBodyBones.RightHand),
            (1, XrHandJointEXT.XR_HAND_JOINT_THUMB_METACARPAL_EXT, HumanBodyBones.RightThumbProximal),
            (1, XrHandJointEXT.XR_HAND_JOINT_THUMB_PROXIMAL_EXT, HumanBodyBones.RightThumbIntermediate),
            (1, XrHandJointEXT.XR_HAND_JOINT_THUMB_DISTAL_EXT, HumanBodyBones.RightThumbDistal),
            (1, XrHandJointEXT.XR_HAND_JOINT_INDEX_PROXIMAL_EXT, HumanBodyBones.RightIndexProximal),
            (1, XrHandJointEXT.XR_HAND_JOINT_INDEX_INTERMEDIATE_EXT, HumanBodyBones.RightIndexIntermediate),
            (1, XrHandJointEXT.XR_HAND_JOINT_INDEX_DISTAL_EXT, HumanBodyBones.RightIndexDistal),
            (1, XrHandJointEXT.XR_HAND_JOINT_MIDDLE_PROXIMAL_EXT, HumanBodyBones.RightMiddleProximal),
            (1, XrHandJointEXT.XR_HAND_JOINT_MIDDLE_INTERMEDIATE_EXT, HumanBodyBones.RightIndexIntermediate),
            (1, XrHandJointEXT.XR_HAND_JOINT_MIDDLE_DISTAL_EXT, HumanBodyBones.RightMiddleDistal),
            (1, XrHandJointEXT.XR_HAND_JOINT_RING_PROXIMAL_EXT, HumanBodyBones.RightRingProximal),
            (1, XrHandJointEXT.XR_HAND_JOINT_RING_INTERMEDIATE_EXT, HumanBodyBones.RightRingIntermediate),
            (1, XrHandJointEXT.XR_HAND_JOINT_RING_DISTAL_EXT, HumanBodyBones.RightRingDistal),
            (1, XrHandJointEXT.XR_HAND_JOINT_LITTLE_PROXIMAL_EXT, HumanBodyBones.RightLittleProximal),
            (1, XrHandJointEXT.XR_HAND_JOINT_LITTLE_INTERMEDIATE_EXT, HumanBodyBones.RightLittleIntermediate),
            (1, XrHandJointEXT.XR_HAND_JOINT_LITTLE_DISTAL_EXT, HumanBodyBones.RightLittleDistal),
        };

    Vrm10Instance vrm_;
    bool isLeft_;

    public VRM1HandUpdater(Vrm10Instance vrm, bool isLeft)
    {
        vrm_ = vrm;
        isLeft_ = isLeft;
    }

    public void Dispose()
    {
    }

    public void Update(long frameTime, openxr.HandTrackingTracker tracker)
    {
        if (tracker == null)
        {
            return;
        }

        XrHandJointLocationEXT[] joints = default;
        if (tracker.TryGetJoints(frameTime, out joints))
        {
            Update(joints, isLeft_ ? 0 : 1);
        }
    }

    void Update(XrHandJointLocationEXT[] joints, int leftRight)
    {
        foreach (var (i, j, b) in JointToBone)
        {
            if (i != leftRight)
            {
                continue;
            }
            var joint = joints[(int)j];

            var t = vrm_.Runtime.ControlRig.GetBoneTransform(b);
            if (t != null)
            {
                if (j == XrHandJointEXT.XR_HAND_JOINT_WRIST_EXT)
                {
                    if (vrm_.TryGetBoneTransform(b, out var handTransform))
                    {
                        // hand is root. directory assign position
                        handTransform.position = joint.pose.position.ToUnity();
                    }
                }
                t.rotation = joint.pose.orientation.ToUnity();
            }
        }
    }
}
