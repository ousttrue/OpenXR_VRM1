
using System;
using openxr;
using UnityEngine;


class HandJoints : IDisposable
{
    Transform[] objects_ = new Transform[openxr.HandTrackingFeature.XR_HAND_JOINT_COUNT_EXT];
    HandTrackingFeature.XrHandJointLocationEXT[] joints_;

    public HandJoints(Transform parent, string prefix)
    {
        for (int i = 0; i < HandTrackingFeature.XR_HAND_JOINT_COUNT_EXT; ++i)
        {
            var value = (HandTrackingFeature.XrHandJointEXT)i;
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

    public void Draw(long frameTime, HandTrackingFeature.Tracker tracker)
    {
        if (tracker == null)
        {
            return;
        }
        if (tracker.TryGetJoints(frameTime, out joints_))
        {
            for (int i = 0; i < joints_.Length; ++i)
            {
                var joint = joints_[i];
                // objects_[i].localScale = new Vector3(joint.radius, joint.radius, joint.radius);

                // convert OpenXR right handed to unity left handed !
                objects_[i].position = joint.pose.position.ToUnity();
                objects_[i].rotation = joint.pose.orientation.ToUnity();
            }
        }
    }
}