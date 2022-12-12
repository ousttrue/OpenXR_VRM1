using System;
using UnityEngine;

namespace openxr
{
    public class BodyTracker : IDisposable
    {
        BodyTrackingFeature feature_;
        ulong handle_;
        BodyTrackingFeature.XrBodyJointLocationFB[] joints_;
        ArrayPin pin_;

        BodyTrackingFeature.XrBodySkeletonJointFB[] skeletonJoints_;
        ArrayPin skletonPin_;
        uint skeletonChangeCount_;

        public event Action<BodyTrackingFeature.XrBodySkeletonJointFB[]> SkeletonUpdated;

        public static BodyTracker Create(BodyTrackingFeature feature, ulong session)
        {
            var create = new BodyTrackingFeature.XrBodyTrackerCreateInfoFB
            {
                type = XrStructureType.XR_TYPE_BODY_TRACKER_CREATE_INFO_FB
            };

            ulong handle;
            var retVal = feature.XrCreateBodyTrackerFB(session, create, out handle);
            if (retVal != 0)
            {
                Debug.Log($"Couldn't open body hand tracker: {retVal}");
                return null;
            }

            var joints = new BodyTrackingFeature.XrBodyJointLocationFB[BodyTrackingFeature.XR_BODY_JOINT_COUNT_FB];
            var skeletonJoints = new BodyTrackingFeature.XrBodySkeletonJointFB[BodyTrackingFeature.XR_BODY_JOINT_COUNT_FB];
            return new BodyTracker
            {
                feature_ = feature,
                handle_ = handle,
                joints_ = joints,
                pin_ = new ArrayPin(joints),
                skeletonJoints_ = skeletonJoints,
                skletonPin_ = new ArrayPin(skeletonJoints),
            };
        }

        public void Dispose()
        {
            skletonPin_.Dispose();
            skletonPin_ = null;
            pin_.Dispose();
            pin_ = null;
            if (handle_ != 0)
            {
                feature_.XrDestroyBodyTrackerFB(handle_);
                handle_ = 0;
            }
        }

        public bool TryGetJoints(long frame_time, ulong space, out BodyTrackingFeature.XrBodyJointLocationFB[] values)
        {
            if (handle_ == 0)
            {
                Debug.LogWarning("zero");
                values = default;
                return false;
            }

            var jli = new BodyTrackingFeature.XrBodyJointsLocateInfoFB
            {
                type = XrStructureType.XR_TYPE_BODY_JOINTS_LOCATE_INFO_FB,
                baseSpace = space,
                time = frame_time,
            };
            var locations = new BodyTrackingFeature.XrBodyJointLocationsFB
            {
                type = XrStructureType.XR_TYPE_BODY_JOINT_LOCATIONS_FB,
                jointCount = (uint)joints_.Length,
                jointLocations = pin_.Ptr,
                time = frame_time,
            };
            var retVal = feature_.XrLocateBodyJointsFB(handle_, jli, ref locations);
            if (retVal != 0)
            {
                Debug.LogError($"XrLocateBodyJointsFB: {retVal}");
                values = default;
                return false;
            }

            if (locations.isActive != 0)
            {
                if (locations.skeletonChangedCount != skeletonChangeCount_)
                {
                    skeletonChangeCount_ = locations.skeletonChangedCount;

                    // retrieve the updated skeleton                    
                    var skeletonInfo = new BodyTrackingFeature.XrBodySkeletonFB
                    {
                        type = XrStructureType.XR_TYPE_BODY_SKELETON_FB,
                        jointCount = (uint)skeletonJoints_.Length,
                        joints = skletonPin_.Ptr,
                    };
                    retVal = feature_.XrGetBodySkeletonFB(handle_, ref skeletonInfo);
                    if (retVal == XrResult.XR_SUCCESS)
                    {
                        Debug.Log($"XrGetBodySkeletonFB: {retVal}");
                        if (SkeletonUpdated != null)
                        {
                            SkeletonUpdated(skeletonJoints_);
                        }
                    }
                    else
                    {
                        Debug.LogWarning($"XrGetBodySkeletonFB: {retVal}");
                    }
                }
            }

            values = joints_;
            return true;
        }
    }
}
