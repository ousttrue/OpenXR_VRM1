using UnityEngine;
using static OxrExtraFeatures.HandTrackingFeature;

namespace Vrm10XR
{
    class HandJoints : MonoBehaviour, OxrExtraFeatures.HandTracker.IReceiver
    {
        Transform[] objects_;

        void Start()
        {
            objects_ = new Transform[XR_HAND_JOINT_COUNT_EXT];
            for (int i = 0; i < XR_HAND_JOINT_COUNT_EXT; ++i)
            {
                var value = (XrHandJointEXT)i;
                var t = GameObject.CreatePrimitive(PrimitiveType.Cube).transform;
                t.localScale = new Vector3(0.01f, 0.01f, 0.01f);
                t.name = $"{value}";
                t.SetParent(transform);
                objects_[i] = t;
            }
        }

        public void OnReceived(long time, XrHandJointLocationEXT[] joints)
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