using System;
using UnityEngine;
using static openxr.HandTrackingFeature;

namespace openxr
{
    public class HandTracker : IDisposable
    {
        HandTrackingFeature feature_;
        internal ulong handle_ = 0;

        XrHandJointLocationEXT[] allJoints_;
        ArrayPin pin_;
        XrHandJointsLocateInfoEXT jli_;
        XrHandJointLocationsEXT joints_;

        HandTracker(HandTrackingFeature feature, ulong handle)
        {
            feature_ = feature;
            Debug.Log($"tracker: {handle}");
            handle_ = handle;
            allJoints_ = new XrHandJointLocationEXT[XR_HAND_JOINT_COUNT_EXT];
            pin_ = new ArrayPin(allJoints_);

            jli_ = new XrHandJointsLocateInfoEXT
            {
                stype = XrStructureType.XR_TYPE_HAND_JOINTS_LOCATE_INFO_EXT,
            };
            joints_ = new XrHandJointLocationsEXT
            {
                stype = XrStructureType.XR_TYPE_HAND_JOINT_LOCATIONS_EXT,
                jointCount = XR_HAND_JOINT_COUNT_EXT,
            };
        }

        public static HandTracker CreateTracker(HandTrackingFeature feature, ulong session, XrHandEXT hand)
        {
            var info = new XrHandTrackerCreateInfoEXT
            {
                stype = XrStructureType.XR_TYPE_HAND_TRACKER_CREATE_INFO_EXT,
                hand = hand,
            };
            ulong handle;
            var retVal = feature.XrCreateHandTrackerEXT(session, info, out handle);
            if (retVal != 0)
            {
                Debug.Log("Couldn't open hand tracker: Error " + retVal);
                return null;
            }

            return new HandTracker(feature, handle);
        }

        public void Dispose()
        {
            Debug.Log($"tracker.Dispose: {handle_}");
            pin_.Dispose();
            feature_.XrDestroyHandTrackerEXT(handle_);
            handle_ = 0;
        }

        public bool TryGetJoints(long frameTime, ulong currentAppSpace, out XrHandJointLocationEXT[] joints)
        {
            if (handle_ == 0)
            {
                joints = default;
                return false;
            }

            jli_.time = frameTime;
            jli_.space = currentAppSpace;
            joints_.jointLocations = pin_.Ptr;
            var retVal = feature_.XrLocateHandJointsEXT(handle_, jli_, ref joints_);
            if (retVal != XrResult.XR_SUCCESS)
            {
                Debug.LogWarning($"xrLocateHandJointsEXT: {handle_}: {retVal}");
                joints = default;
                return false;
            }

            joints = allJoints_;
            return true;
        }
    }
}
