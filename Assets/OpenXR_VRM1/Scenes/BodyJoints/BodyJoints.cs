
using System;
using openxr;
using UnityEngine;


class BodyJoints : IDisposable
{
    Transform[] objects_ = new Transform[openxr.BodyTrackingFeature.XR_BODY_JOINT_COUNT_FB];

    public BodyJoints(Transform parent, string prefix)
    {
        for (int i = 0; i < BodyTrackingFeature.XR_BODY_JOINT_COUNT_FB; ++i)
        {
            var value = (BodyTrackingFeature.XrBodyJointFB)i;
            var t = GameObject.CreatePrimitive(PrimitiveType.Cube).transform;
            t.localScale = new Vector3(0.01f, 0.01f, 0.01f);
            t.name = $"{prefix}.{value}";
            t.SetParent(parent);
            objects_[i] = t;
        }
    }

    public void Dispose()
    {
        foreach (var joint in objects_)
        {
            GameObject.Destroy(joint);
        }
    }

    public void Update(long frameTime, ulong space, openxr.BodyTracker tracker)
    {
        if (tracker == null)
        {
            return;
        }

        BodyTrackingFeature.XrBodyJointLocationFB[] joints = default;
        if (tracker.TryGetJoints(frameTime, space, out joints))
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
