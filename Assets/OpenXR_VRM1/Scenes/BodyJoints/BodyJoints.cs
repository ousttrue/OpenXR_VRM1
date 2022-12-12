using openxr;
using UnityEngine;


namespace Vrm10XR
{
    class BodyJoints : MonoBehaviour
    {
        Transform[] objects_;

        public void Start()
        {
            objects_ = new Transform[(int)openxr.BodyTrackingFeature.XrBodyJointFB.XR_BODY_JOINT_COUNT_FB];
            for (int i = 0; i < objects_.Length; ++i)
            {
                var value = (BodyTrackingFeature.XrBodyJointFB)i;
                var t = GameObject.CreatePrimitive(PrimitiveType.Cube).transform;
                t.localScale = new Vector3(0.01f, 0.01f, 0.01f);
                t.name = $"{value}";
                t.SetParent(transform);
                objects_[i] = t;
            }
        }

        public void OnJointsUpdated(BodyTrackingFeature.XrBodyJointLocationFB[] joints)
        {
            for (int i = 0; i < joints.Length; ++i)
            {
                var joint = joints[i];
                var JOINT_SIZE = 0.02f;
                objects_[i].localScale = new Vector3(JOINT_SIZE, JOINT_SIZE, JOINT_SIZE);
                // convert OpenXR right handed to unity left handed !
                objects_[i].position = joint.pose.position.ToUnity();
                objects_[i].rotation = joint.pose.orientation.ToUnity();
            }
        }
    }
}