using UnityEngine.XR.OpenXR.Features;
using UnityEngine;
using System.Runtime.InteropServices;
using System;
using UnityEngine.XR.OpenXR;


namespace OxrExtraFeatures
{
#if UNITY_EDITOR
    [UnityEditor.XR.OpenXR.Features.OpenXRFeature(UiName = xr_extension,
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
        public const string featureId = "com.vrmc.hand_tracking";
        public const string xr_extension = "XR_EXT_hand_tracking";
        public const int XR_HAND_JOINT_COUNT_EXT = 26;

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

        // Provided by XR_EXT_hand_tracking
        public enum XrHandJointSetEXT
        {
            XR_HAND_JOINT_SET_DEFAULT_EXT = 0,
            // Provided by XR_ULTRALEAP_hand_tracking_forearm
            XR_HAND_JOINT_SET_HAND_WITH_FOREARM_ULTRALEAP = 1000149000,
            XR_HAND_JOINT_SET_MAX_ENUM_EXT = 0x7FFFFFFF
        };

        // get the address of the hand tracking functions using: OpenXRFeature.xrGetInstanceProcAddr
        /*typedef struct XrHandTrackerCreateInfoEXT {
            XrStructureType      type;
            const void*          next;
            XrHandEXT            hand;
            XrHandJointSetEXT    handJointSet;
        } XrHandTrackerCreateInfoEXT;*/
        [StructLayout(LayoutKind.Sequential)]
        public struct XrHandTrackerCreateInfoEXT
        {
            public XrStructureType stype;
            public IntPtr next;
            public XrHandEXT hand;
            public XrHandJointSetEXT handJointSet;
        }

        /*XrResult xrCreateHandTrackerEXT(
            XrSession                                   session,
            const XrHandTrackerCreateInfoEXT*           createInfo,
            XrHandTrackerEXT*                           handTracker);*/
        public delegate XrResult Type_xrCreateHandTrackerEXT(ulong session, in XrHandTrackerCreateInfoEXT createInfo, out ulong tracker);
        Type_xrCreateHandTrackerEXT xrCreateHandTrackerEXT_;
        public Type_xrCreateHandTrackerEXT XrCreateHandTrackerEXT => xrCreateHandTrackerEXT_;

        /*XrResult xrDestroyHandTrackerEXT(XrHandTrackerEXT handTracker);*/
        public delegate int Type_xrDestroyHandTrackerEXT(ulong tracker);
        Type_xrDestroyHandTrackerEXT xrDestroyHandTrackerEXT_;
        public Type_xrDestroyHandTrackerEXT XrDestroyHandTrackerEXT => xrDestroyHandTrackerEXT_;

        /*typedef struct XrHandJointsLocateInfoEXT {
            XrStructureType    type;
            const void*        next;
            XrSpace            baseSpace;
            XrTime             time;
        } XrHandJointsLocateInfoEXT;*/
        [StructLayout(LayoutKind.Sequential)]
        public struct XrHandJointsLocateInfoEXT
        {
            public XrStructureType stype;
            public IntPtr next;
            public ulong space;
            public long time;
        };

        /*typedef struct XrHandJointLocationsEXT {
            XrStructureType            type;
            void*                      next;
            XrBool32                   isActive;
            uint32_t                   jointCount;
            XrHandJointLocationEXT*    jointLocations;
        } XrHandJointLocationsEXT;*/
        [StructLayout(LayoutKind.Sequential)]
        public struct XrHandJointLocationsEXT
        {
            public XrStructureType stype;
            public IntPtr next;
            public int isActive;
            public uint jointCount;
            public IntPtr jointLocations;
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
            public XrSpaceLocationFlags locationFlags;
            public XrPosef pose;
            public float radius; // joint radius
        }

        /*XrResult xrLocateHandJointsEXT(
            XrHandTrackerEXT                            handTracker,
            const XrHandJointsLocateInfoEXT*            locateInfo,
            XrHandJointLocationsEXT*                    locations);*/
        public delegate XrResult Type_xrLocateHandJointsEXT(ulong tracker, in XrHandJointsLocateInfoEXT locateInfoEXT, ref XrHandJointLocationsEXT locations);
        Type_xrLocateHandJointsEXT xrLocateHandJointsEXT_;
        public Type_xrLocateHandJointsEXT XrLocateHandJointsEXT => xrLocateHandJointsEXT_;

        ulong instance_;
        ulong session_;

        public event Action<HandTrackingFeature, ulong> SessionBegin;
        public event Action SessionEnd;

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

            if (SessionBegin != null)
            {
                SessionBegin(this, session_);
            }
        }

        override protected void OnSessionEnd(ulong session)
        {
            Debug.Log($"OnSessionEnd: {instance_}.{session_}");
            if (SessionEnd != null)
            {
                SessionEnd();
            }
            session_ = 0;
        }

        override protected void OnSessionDestroy(ulong xrSession)
        {
            Debug.Log("OnSessionDestroy");
        }
    }
}