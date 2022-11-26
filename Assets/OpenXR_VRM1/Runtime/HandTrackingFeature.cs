using UnityEngine.XR.OpenXR.Features;
using UnityEngine;
using System.Runtime.InteropServices;
using System;
using UnityEngine.XR.OpenXR;

namespace openxr
{
#if UNITY_EDITOR
    [UnityEditor.XR.OpenXR.Features.OpenXRFeature(UiName = "ext_hand_tracking",
        BuildTargetGroups = new[] {
            UnityEditor.BuildTargetGroup.Standalone, UnityEditor.BuildTargetGroup.WSA, UnityEditor.BuildTargetGroup.Android },
        Company = "VRMC",
        FeatureId = featureId,
        Version = "0.1.0",
        Desc = "XR_EXT_hand_tracking sample",
        DocumentationLink = "https://registry.khronos.org/OpenXR/specs/1.0/html/xrspec.html#XR_EXT_hand_tracking",
        OpenxrExtensionStrings = xr_extension
        )]
#endif
    public class HandTrackingFeature : OpenXRFeature
    {
        public const string featureId = "com.joemarshall.handtracking";
        public const string xr_extension = "XR_EXT_hand_tracking";
        public const int XR_HAND_JOINT_COUNT_EXT = 26;

        public const int XR_TYPE_HAND_TRACKER_CREATE_INFO_EXT = 1000051001;
        public const int XR_TYPE_HAND_JOINTS_LOCATE_INFO_EXT = 1000051002;
        public const int XR_TYPE_HAND_JOINT_LOCATIONS_EXT = 1000051003;

        public enum XrHandEXT
        {
            XR_HAND_LEFT_EXT = 1,
            XR_HAND_RIGHT_EXT = 2,
            XR_HAND_MAX_ENUM_EXT = 0x7FFFFFFF
        }

        public enum XrHandJointEXT
        {
            XR_HAND_JOINT_PALM_EXT = 0,
            XR_HAND_JOINT_WRIST_EXT = 1,
            XR_HAND_JOINT_THUMB_METACARPAL_EXT = 2,
            XR_HAND_JOINT_THUMB_PROXIMAL_EXT = 3,
            XR_HAND_JOINT_THUMB_DISTAL_EXT = 4,
            XR_HAND_JOINT_THUMB_TIP_EXT = 5,
            XR_HAND_JOINT_INDEX_METACARPAL_EXT = 6,
            XR_HAND_JOINT_INDEX_PROXIMAL_EXT = 7,
            XR_HAND_JOINT_INDEX_INTERMEDIATE_EXT = 8,
            XR_HAND_JOINT_INDEX_DISTAL_EXT = 9,
            XR_HAND_JOINT_INDEX_TIP_EXT = 10,
            XR_HAND_JOINT_MIDDLE_METACARPAL_EXT = 11,
            XR_HAND_JOINT_MIDDLE_PROXIMAL_EXT = 12,
            XR_HAND_JOINT_MIDDLE_INTERMEDIATE_EXT = 13,
            XR_HAND_JOINT_MIDDLE_DISTAL_EXT = 14,
            XR_HAND_JOINT_MIDDLE_TIP_EXT = 15,
            XR_HAND_JOINT_RING_METACARPAL_EXT = 16,
            XR_HAND_JOINT_RING_PROXIMAL_EXT = 17,
            XR_HAND_JOINT_RING_INTERMEDIATE_EXT = 18,
            XR_HAND_JOINT_RING_DISTAL_EXT = 19,
            XR_HAND_JOINT_RING_TIP_EXT = 20,
            XR_HAND_JOINT_LITTLE_METACARPAL_EXT = 21,
            XR_HAND_JOINT_LITTLE_PROXIMAL_EXT = 22,
            XR_HAND_JOINT_LITTLE_INTERMEDIATE_EXT = 23,
            XR_HAND_JOINT_LITTLE_DISTAL_EXT = 24,
            XR_HAND_JOINT_LITTLE_TIP_EXT = 25,
            XR_HAND_JOINT_MAX_ENUM_EXT = 0x7FFFFFFF
        }

        /*XrResult xrCreateHandTrackerEXT(
            XrSession                                   session,
            const XrHandTrackerCreateInfoEXT*           createInfo,
            XrHandTrackerEXT*                           handTracker);*/
        internal delegate int Type_xrCreateHandTrackerEXT(ulong session, in XrHandTrackerCreateInfoEXT createInfo, out ulong tracker);

        /*
            XrResult xrDestroyHandTrackerEXT(XrHandTrackerEXT handTracker);
        */
        internal delegate int Type_xrDestroyHandTrackerEXT(ulong tracker);

        /*XrResult xrLocateHandJointsEXT(
            XrHandTrackerEXT                            handTracker,
            const XrHandJointsLocateInfoEXT*            locateInfo,
            XrHandJointLocationsEXT*                    locations);*/
        internal delegate XrResult Type_xrLocateHandJointsEXT(ulong tracker, in XrHandJointsLocateInfoEXT locateInfoEXT, ref XrHandJointLocationsEXT locations);

        /*typedef struct XrHandJointsLocateInfoEXT {
            XrStructureType    type;
            const void*        next;
            XrSpace            baseSpace;
            XrTime             time;
        } XrHandJointsLocateInfoEXT;*/
        [StructLayout(LayoutKind.Sequential)]
        internal struct XrHandJointsLocateInfoEXT
        {
            public int stype;
            public IntPtr next;
            public ulong space;
            public long time;
        };

        /*
        typedef struct XrHandJointLocationEXT {
            XrSpaceLocationFlags    locationFlags;
            XrPosef                 pose;
            float                   radius;
        } XrHandJointLocationEXT;
        */
        [StructLayout(LayoutKind.Sequential)]
        public struct XrHandJointLocationEXT
        {
            // TODO check size of enums and types
            public ulong locationFlags;
            public XrPosef pose;
            public float radius; // joint radius
        }
        static GCHandle pinnedJointArray;
        /*typedef struct XrHandJointLocationsEXT {
            XrStructureType            type;
            void*                      next;
            XrBool32                   isActive;
            uint32_t                   jointCount;
            XrHandJointLocationEXT*    jointLocations;
        } XrHandJointLocationsEXT;*/
        [StructLayout(LayoutKind.Sequential)]
        internal struct XrHandJointLocationsEXT
        {
            public int stype;
            public IntPtr next;
            public int isActive;
            public uint jointCount;
            public IntPtr jointLocations;
        };

        /*typedef struct XrHandTrackerCreateInfoEXT {
            XrStructureType      type;
            const void*          next;
            XrHandEXT            hand;
            XrHandJointSetEXT    handJointSet;
        } XrHandTrackerCreateInfoEXT;*/
        [StructLayout(LayoutKind.Sequential)]
        internal struct XrHandTrackerCreateInfoEXT
        {
            public int stype;
            public IntPtr next;
            public XrHandEXT hand;
            public int handJointSet;
        }

        Type_xrCreateHandTrackerEXT xrCreateHandTrackerEXT_;
        Type_xrDestroyHandTrackerEXT xrDestroyHandTrackerEXT_;
        Type_xrLocateHandJointsEXT xrLocateHandJointsEXT_;

        ulong instance_;
        ulong session_;

        public event Action<HandTrackingTracker, HandTrackingTracker> SessionBegin;
        public event Action SessionEnd;

        HandTrackingTracker leftTracker_;
        HandTrackingTracker rightTracker_;

        bool TryCreateTracker(XrHandEXT hand, out HandTrackingTracker tracker)
        {
            var info = new XrHandTrackerCreateInfoEXT
            {
                stype = XR_TYPE_HAND_TRACKER_CREATE_INFO_EXT,
                hand = hand,
            };
            ulong handle;
            var retVal = xrCreateHandTrackerEXT_(session_, info, out handle);
            if (retVal != 0)
            {
                Debug.Log("Couldn't open hand tracker: Error " + retVal);
                tracker = default;
                return false;
            }

            tracker = new HandTrackingTracker(handle, () => xrDestroyHandTrackerEXT_(handle), GetJoints);
            return true;
        }

        bool GetJoints(ulong handle, XrHandJointsLocateInfoEXT info, ref XrHandJointLocationsEXT joints)
        {
            info.space = OpenXRFeature.GetCurrentAppSpace();
            var retVal = xrLocateHandJointsEXT_(handle, info, ref joints);
            if (retVal != 0)
            {
                Debug.LogWarning($"xrLocateHandJointsEXT: {handle}: {retVal}");
            }
            return retVal == 0;
        }

        override protected bool OnInstanceCreate(ulong xrInstance)
        {
            instance_ = xrInstance;
            if (!OpenXRRuntime.IsExtensionEnabled(xr_extension))
            {
                Debug.LogWarning($"{xr_extension} is not enabled.");
                // Return false here to indicate the system should disable your feature for this execution.  
                // Note that if a feature is marked required, returning false will cause the OpenXRLoader to abort and try another loader.
                return false;
            }

            return true;
        }

        override protected void OnInstanceDestroy(ulong xrInstance)
        {
            instance_ = 0;
        }

        override protected void OnSessionBegin(ulong session)
        {
            session_ = session;
            Debug.Log($"{featureId}: {instance_}.{session_}");

            var getInstanceProcAddr = Marshal.GetDelegateForFunctionPointer<PFN_xrGetInstanceProcAddr>(xrGetInstanceProcAddr);
            Func<string, IntPtr> getAddr = (string name) =>
            {
                IntPtr ptr;
                getInstanceProcAddr(instance_, name, out ptr);
                return ptr;
            };
            xrCreateHandTrackerEXT_ = Marshal.GetDelegateForFunctionPointer<Type_xrCreateHandTrackerEXT>(getAddr("xrCreateHandTrackerEXT"));
            xrDestroyHandTrackerEXT_ = Marshal.GetDelegateForFunctionPointer<Type_xrDestroyHandTrackerEXT>(getAddr("xrDestroyHandTrackerEXT"));
            xrLocateHandJointsEXT_ = Marshal.GetDelegateForFunctionPointer<Type_xrLocateHandJointsEXT>(getAddr("xrLocateHandJointsEXT"));

            if (!TryCreateTracker(XrHandEXT.XR_HAND_LEFT_EXT, out leftTracker_))
            {
                Debug.LogError("fail to create XrHandEXT.XR_HAND_LEFT_EXT");
            }
            if (!TryCreateTracker(XrHandEXT.XR_HAND_RIGHT_EXT, out rightTracker_))
            {
                Debug.LogError("fail to create XrHandEXT.XR_HAND_RIGHT_EXT");
            }
            if (SessionBegin != null)
            {
                SessionBegin(leftTracker_, rightTracker_);
            }
        }

        override protected void OnSessionEnd(ulong session)
        {
            Debug.Log($"OnSessionEnd: {instance_}.{session_}");
            if (SessionEnd != null)
            {
                SessionEnd();
            }
            leftTracker_.Dispose();
            leftTracker_ = null;
            rightTracker_.Dispose();
            rightTracker_ = null;
            session_ = 0;
        }

        override protected void OnSessionDestroy(ulong xrSession)
        {
            Debug.Log("OnSessionDestroy");
        }
    }
}