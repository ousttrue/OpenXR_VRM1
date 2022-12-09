using openxr;
using UnityEngine;

namespace Vrm10XR
{
    class HandJoints : MonoBehaviour
    {
        Transform[] objects_;

        void Start()
        {
            objects_ = new Transform[openxr.HandTrackingFeature.XR_HAND_JOINT_COUNT_EXT];
            for (int i = 0; i < HandTrackingFeature.XR_HAND_JOINT_COUNT_EXT; ++i)
            {
                var value = (HandTrackingFeature.XrHandJointEXT)i;
                var t = GameObject.CreatePrimitive(PrimitiveType.Cube).transform;
                t.localScale = new Vector3(0.01f, 0.01f, 0.01f);
                t.name = $"{value}";
                t.SetParent(transform);
                objects_[i] = t;
            }
        }

        public void OnJointsUpdated(HandTrackingFeature.XrHandJointLocationEXT[] joints)
        {
            for (int i = 0; i < joints.Length; ++i)
            {
                var joint = joints[i];
                objects_[i].localScale = new Vector3(joint.radius, joint.radius, joint.radius);

                // convert OpenXR right handed to unity left handed !
                objects_[i].position = joint.pose.position.ToUnity();
                objects_[i].rotation = joint.pose.orientation.ToUnity();
            }
        }
    }
}