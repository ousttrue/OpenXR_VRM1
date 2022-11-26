using System;
using UnityEngine;
using UnityEngine.XR.OpenXR.Features;
using static openxr.HandTrackingFeature;

namespace openxr
{
    public class HandTrackingTracker : IDisposable
    {
        ulong handle_ = 0;
        Action disposer_;

        XrHandJointLocationEXT[] allJoints_;
        ArrayPin pin_;
        XrHandJointsLocateInfoEXT jli_;
        XrHandJointLocationsEXT joints_;

        internal delegate bool GetJoints(ulong handle, XrHandJointsLocateInfoEXT info, ref XrHandJointLocationsEXT joints);
        GetJoints getJoints_;

        internal HandTrackingTracker(ulong handle, Action disposer, GetJoints getJoints)
        {
            Debug.Log($"tracker: {handle}");
            handle_ = handle;
            disposer_ = disposer;
            getJoints_ = getJoints;
            allJoints_ = new XrHandJointLocationEXT[XR_HAND_JOINT_COUNT_EXT];
            pin_ = new ArrayPin(allJoints_);

            jli_ = new XrHandJointsLocateInfoEXT
            {
                stype = XR_TYPE_HAND_JOINTS_LOCATE_INFO_EXT,
            };
            joints_ = new XrHandJointLocationsEXT
            {
                stype = XR_TYPE_HAND_JOINT_LOCATIONS_EXT,
                jointCount = XR_HAND_JOINT_COUNT_EXT,
            };
        }

        public void Dispose()
        {
            pin_.Dispose();
            disposer_();
            handle_ = 0;
        }

        public bool TryGetJoints(long frameTime, out XrHandJointLocationEXT[] joints)
        {
            if (handle_ == 0)
            {
                joints = default;
                return false;
            }

            jli_.time = frameTime;
            joints_.jointLocations = pin_.Ptr;
            if (!getJoints_(handle_, jli_, ref joints_))
            {
                joints = default;
                return false;
            }

            joints = allJoints_;
            return true;
        }
    }
}
