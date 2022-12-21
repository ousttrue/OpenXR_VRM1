using System;
using UnityEngine;

namespace OxrExtraFeatures
{
    using static HandTrackingFeature;

    public class HandTracker : IDisposable
    {
        // UnityEngine.Events.UnityEvent<long, HandTrackingFeature.XrHandJointLocationEXT[]>
        public interface IReceiver
        {
            void OnReceived(long time, XrHandJointLocationEXT[] joints);
        }

        HandTrackingFeature feature_;
        ulong handle_ = 0;

        XrHandJointLocationEXT[] allJoints_;
        ArrayPin pin_;
        XrHandJointsLocateInfoEXT jli_;
        XrHandJointLocationsEXT joints_;

        public static HandTracker Create(HandTrackingFeature feature, ulong session, XrHandEXT hand)
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

            Debug.Log($"tracker: {handle}");

            var allJoints = new XrHandJointLocationEXT[XR_HAND_JOINT_COUNT_EXT];
            return new HandTracker
            {
                feature_ = feature,
                handle_ = handle,
                allJoints_ = allJoints,
                pin_ = new ArrayPin(allJoints),
                jli_ = new XrHandJointsLocateInfoEXT
                {
                    stype = XrStructureType.XR_TYPE_HAND_JOINTS_LOCATE_INFO_EXT,
                },
                joints_ = new XrHandJointLocationsEXT
                {
                    stype = XrStructureType.XR_TYPE_HAND_JOINT_LOCATIONS_EXT,
                    jointCount = XR_HAND_JOINT_COUNT_EXT,
                },
            };
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
