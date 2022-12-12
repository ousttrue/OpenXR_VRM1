
using System;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.XR.OpenXR;
using UnityEngine.XR.OpenXR.Features;


namespace openxr
{
#if UNITY_EDITOR
    [UnityEditor.XR.OpenXR.Features.OpenXRFeature(UiName = xr_extension,
        BuildTargetGroups = new[] {
            UnityEditor.BuildTargetGroup.Standalone, UnityEditor.BuildTargetGroup.Android },
        Company = "VRMC",
        Desc = "Enable body tracking in unity",
        DocumentationLink = "https://developer.oculus.com/documentation/native/android/move-body-tracking/",
        OpenxrExtensionStrings = xr_extension,
        Version = "0.0.1",
        FeatureId = featureId)]
#endif
    public class BodyTrackingFeature : OpenXRFeature
    {
        public const string featureId = "com.ousttrue.bodytracking";
        public const string xr_extension = "XR_FB_body_tracking";

        public enum XrBodyJointSetFB
        {
            XR_BODY_JOINT_SET_DEFAULT_FB = 0,
            XR_BODY_JOINT_SET_MAX_ENUM_FB = 0x7FFFFFFF
        };

        public enum XrBodyJointFB
        {
            XR_BODY_JOINT_ROOT_FB = 0,
            XR_BODY_JOINT_HIPS_FB = 1,
            XR_BODY_JOINT_SPINE_LOWER_FB = 2,
            XR_BODY_JOINT_SPINE_MIDDLE_FB = 3,
            XR_BODY_JOINT_SPINE_UPPER_FB = 4,
            XR_BODY_JOINT_CHEST_FB = 5,
            XR_BODY_JOINT_NECK_FB = 6,
            XR_BODY_JOINT_HEAD_FB = 7,
            XR_BODY_JOINT_LEFT_SHOULDER_FB = 8,
            XR_BODY_JOINT_LEFT_SCAPULA_FB = 9,
            XR_BODY_JOINT_LEFT_ARM_UPPER_FB = 10,
            XR_BODY_JOINT_LEFT_ARM_LOWER_FB = 11,
            XR_BODY_JOINT_LEFT_HAND_WRIST_TWIST_FB = 12,
            XR_BODY_JOINT_RIGHT_SHOULDER_FB = 13,
            XR_BODY_JOINT_RIGHT_SCAPULA_FB = 14,
            XR_BODY_JOINT_RIGHT_ARM_UPPER_FB = 15,
            XR_BODY_JOINT_RIGHT_ARM_LOWER_FB = 16,
            XR_BODY_JOINT_RIGHT_HAND_WRIST_TWIST_FB = 17,
            XR_BODY_JOINT_LEFT_HAND_PALM_FB = 18,
            XR_BODY_JOINT_LEFT_HAND_WRIST_FB = 19,
            XR_BODY_JOINT_LEFT_HAND_THUMB_METACARPAL_FB = 20,
            XR_BODY_JOINT_LEFT_HAND_THUMB_PROXIMAL_FB = 21,
            XR_BODY_JOINT_LEFT_HAND_THUMB_DISTAL_FB = 22,
            XR_BODY_JOINT_LEFT_HAND_THUMB_TIP_FB = 23,
            XR_BODY_JOINT_LEFT_HAND_INDEX_METACARPAL_FB = 24,
            XR_BODY_JOINT_LEFT_HAND_INDEX_PROXIMAL_FB = 25,
            XR_BODY_JOINT_LEFT_HAND_INDEX_INTERMEDIATE_FB = 26,
            XR_BODY_JOINT_LEFT_HAND_INDEX_DISTAL_FB = 27,
            XR_BODY_JOINT_LEFT_HAND_INDEX_TIP_FB = 28,
            XR_BODY_JOINT_LEFT_HAND_MIDDLE_METACARPAL_FB = 29,
            XR_BODY_JOINT_LEFT_HAND_MIDDLE_PROXIMAL_FB = 30,
            XR_BODY_JOINT_LEFT_HAND_MIDDLE_INTERMEDIATE_FB = 31,
            XR_BODY_JOINT_LEFT_HAND_MIDDLE_DISTAL_FB = 32,
            XR_BODY_JOINT_LEFT_HAND_MIDDLE_TIP_FB = 33,
            XR_BODY_JOINT_LEFT_HAND_RING_METACARPAL_FB = 34,
            XR_BODY_JOINT_LEFT_HAND_RING_PROXIMAL_FB = 35,
            XR_BODY_JOINT_LEFT_HAND_RING_INTERMEDIATE_FB = 36,
            XR_BODY_JOINT_LEFT_HAND_RING_DISTAL_FB = 37,
            XR_BODY_JOINT_LEFT_HAND_RING_TIP_FB = 38,
            XR_BODY_JOINT_LEFT_HAND_LITTLE_METACARPAL_FB = 39,
            XR_BODY_JOINT_LEFT_HAND_LITTLE_PROXIMAL_FB = 40,
            XR_BODY_JOINT_LEFT_HAND_LITTLE_INTERMEDIATE_FB = 41,
            XR_BODY_JOINT_LEFT_HAND_LITTLE_DISTAL_FB = 42,
            XR_BODY_JOINT_LEFT_HAND_LITTLE_TIP_FB = 43,
            XR_BODY_JOINT_RIGHT_HAND_PALM_FB = 44,
            XR_BODY_JOINT_RIGHT_HAND_WRIST_FB = 45,
            XR_BODY_JOINT_RIGHT_HAND_THUMB_METACARPAL_FB = 46,
            XR_BODY_JOINT_RIGHT_HAND_THUMB_PROXIMAL_FB = 47,
            XR_BODY_JOINT_RIGHT_HAND_THUMB_DISTAL_FB = 48,
            XR_BODY_JOINT_RIGHT_HAND_THUMB_TIP_FB = 49,
            XR_BODY_JOINT_RIGHT_HAND_INDEX_METACARPAL_FB = 50,
            XR_BODY_JOINT_RIGHT_HAND_INDEX_PROXIMAL_FB = 51,
            XR_BODY_JOINT_RIGHT_HAND_INDEX_INTERMEDIATE_FB = 52,
            XR_BODY_JOINT_RIGHT_HAND_INDEX_DISTAL_FB = 53,
            XR_BODY_JOINT_RIGHT_HAND_INDEX_TIP_FB = 54,
            XR_BODY_JOINT_RIGHT_HAND_MIDDLE_METACARPAL_FB = 55,
            XR_BODY_JOINT_RIGHT_HAND_MIDDLE_PROXIMAL_FB = 56,
            XR_BODY_JOINT_RIGHT_HAND_MIDDLE_INTERMEDIATE_FB = 57,
            XR_BODY_JOINT_RIGHT_HAND_MIDDLE_DISTAL_FB = 58,
            XR_BODY_JOINT_RIGHT_HAND_MIDDLE_TIP_FB = 59,
            XR_BODY_JOINT_RIGHT_HAND_RING_METACARPAL_FB = 60,
            XR_BODY_JOINT_RIGHT_HAND_RING_PROXIMAL_FB = 61,
            XR_BODY_JOINT_RIGHT_HAND_RING_INTERMEDIATE_FB = 62,
            XR_BODY_JOINT_RIGHT_HAND_RING_DISTAL_FB = 63,
            XR_BODY_JOINT_RIGHT_HAND_RING_TIP_FB = 64,
            XR_BODY_JOINT_RIGHT_HAND_LITTLE_METACARPAL_FB = 65,
            XR_BODY_JOINT_RIGHT_HAND_LITTLE_PROXIMAL_FB = 66,
            XR_BODY_JOINT_RIGHT_HAND_LITTLE_INTERMEDIATE_FB = 67,
            XR_BODY_JOINT_RIGHT_HAND_LITTLE_DISTAL_FB = 68,
            XR_BODY_JOINT_RIGHT_HAND_LITTLE_TIP_FB = 69,
            XR_BODY_JOINT_COUNT_FB = 70,
            XR_BODY_JOINT_NONE_FB = -1,
            XR_BODY_JOINT_MAX_ENUM_FB = 0x7FFFFFFF
        };


        /*
        typedef struct XrBodyTrackerCreateInfoFB {
            XrStructureType type;
            const void* XR_MAY_ALIAS next;
            XrBodyJointSetFB bodyJointSet;
        } XrBodyTrackerCreateInfoFB;
        */
        [StructLayout(LayoutKind.Sequential)]
        public struct XrBodyTrackerCreateInfoFB
        {
            public XrStructureType type;
            public IntPtr next;
            public XrBodyJointSetFB bodyJointSet;
        }

        /*
        XRAPI_ATTR XrResult XRAPI_CALL xrCreateBodyTrackerFB(
            XrSession session,
            const XrBodyTrackerCreateInfoFB* createInfo,
            XrBodyTrackerFB* bodyTracker);
        */
        public delegate XrResult PFN_xrCreateBodyTrackerFB(ulong session,
            in XrBodyTrackerCreateInfoFB createInfo,
            out ulong bodyTracker);
        PFN_xrCreateBodyTrackerFB xrCreateBodyTrackerFB_;
        public PFN_xrCreateBodyTrackerFB XrCreateBodyTrackerFB => xrCreateBodyTrackerFB_;

        /*
        XRAPI_ATTR XrResult XRAPI_CALL xrDestroyBodyTrackerFB(XrBodyTrackerFB bodyTracker);
        */
        public delegate XrResult PFN_xrDestroyBodyTrackerFB(ulong bodyTracker);
        PFN_xrDestroyBodyTrackerFB xrDestroyBodyTrackerFB_;
        public PFN_xrDestroyBodyTrackerFB XrDestroyBodyTrackerFB => xrDestroyBodyTrackerFB_;

        /*
        typedef struct XrBodyJointsLocateInfoFB {
            XrStructureType type;
            const void* XR_MAY_ALIAS next;
            XrSpace baseSpace;
            XrTime time;
        } XrBodyJointsLocateInfoFB;
        */
        [StructLayout(LayoutKind.Sequential)]
        public struct XrBodyJointsLocateInfoFB
        {
            public XrStructureType type;
            public IntPtr next;
            public ulong baseSpace;
            public long time;
        }

        /*
        typedef struct XrBodyJointLocationFB {
            XrSpaceLocationFlags locationFlags;
            XrPosef pose;
        } XrBodyJointLocationFB;
        */
        [StructLayout(LayoutKind.Sequential)]
        public struct XrBodyJointLocationFB
        {
            public XrSpaceLocationFlags locationFlags;
            public XrPosef pose;
        }

        /*
        typedef struct XrBodyJointLocationsFB {
            XrStructureType type;
            void* XR_MAY_ALIAS next;
            XrBool32 isActive;
            float confidence;
            uint32_t jointCount;
            XrBodyJointLocationFB* jointLocations;
            uint32_t skeletonChangedCount;
            XrTime time;
        } XrBodyJointLocationsFB;
        */
        [StructLayout(LayoutKind.Sequential)]
        public struct XrBodyJointLocationsFB
        {
            public XrStructureType type;
            public IntPtr next;
            public int isActive;
            public float confidence;
            public uint jointCount;
            public IntPtr jointLocations;
            public uint skeletonChangedCount;
            public long time;
        }

        /*
        XRAPI_ATTR XrResult XRAPI_CALL xrLocateBodyJointsFB(
            XrBodyTrackerFB bodyTracker,
            const XrBodyJointsLocateInfoFB* locateInfo,
            XrBodyJointLocationsFB* locations);
        */
        public delegate XrResult PFN_xrLocateBodyJointsFB(
            ulong bodyTracker,
            in XrBodyJointsLocateInfoFB locateInfo,
            ref XrBodyJointLocationsFB locations);
        PFN_xrLocateBodyJointsFB xrLocateBodyJointsFB_;
        public PFN_xrLocateBodyJointsFB XrLocateBodyJointsFB => xrLocateBodyJointsFB_;

        /*
        typedef struct XrBodySkeletonJointFB {
            int32_t joint;
            int32_t parentJoint;
            XrPosef pose;
        } XrBodySkeletonJointFB;
        */
        [StructLayout(LayoutKind.Sequential)]
        public struct XrBodySkeletonJointFB
        {
            public int joint;
            public int parentJoint;
            public XrPosef pose;
        };

        /*
        typedef struct XrBodySkeletonFB {
            XrStructureType type;
            void* XR_MAY_ALIAS next;
            uint32_t jointCount;
            XrBodySkeletonJointFB* joints;
        } XrBodySkeletonFB;
        */
        [StructLayout(LayoutKind.Sequential)]
        public struct XrBodySkeletonFB
        {
            public XrStructureType type;
            public IntPtr next;
            public uint jointCount;
            public IntPtr joints;
        };

        public delegate XrResult PFN_xrGetBodySkeletonFB(ulong bodyTracker, ref XrBodySkeletonFB skeleton);
        PFN_xrGetBodySkeletonFB xrGetBodySkeletonFB_;
        public PFN_xrGetBodySkeletonFB XrGetBodySkeletonFB => xrGetBodySkeletonFB_;

        Type_xrCreateReferenceSpace xrCreateReferenceSpace_;
        public Type_xrCreateReferenceSpace XrCreateReferenceSpace => xrCreateReferenceSpace_;

        ulong instance_;
        ulong session_;

        public event Action<BodyTrackingFeature, ulong> SessionBegin;
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
            xrCreateBodyTrackerFB_ = Marshal.GetDelegateForFunctionPointer<PFN_xrCreateBodyTrackerFB>(getAddr("xrCreateBodyTrackerFB"));
            xrDestroyBodyTrackerFB_ = Marshal.GetDelegateForFunctionPointer<PFN_xrDestroyBodyTrackerFB>(getAddr("xrDestroyBodyTrackerFB"));
            xrLocateBodyJointsFB_ = Marshal.GetDelegateForFunctionPointer<PFN_xrLocateBodyJointsFB>(getAddr("xrLocateBodyJointsFB"));
            xrGetBodySkeletonFB_ = Marshal.GetDelegateForFunctionPointer<PFN_xrGetBodySkeletonFB>(getAddr("xrGetBodySkeletonFB"));

            xrCreateReferenceSpace_ = Marshal.GetDelegateForFunctionPointer<Type_xrCreateReferenceSpace>(getAddr("xrCreateReferenceSpace"));

            if (SessionBegin != null)
            {
                SessionBegin(this, session_);
            }
        }

        override protected void OnSessionEnd(ulong session)
        {
            Debug.Log($"{featureId}: OnSessionEnd: {instance_}.{session_}");
            if (SessionEnd != null)
            {
                SessionEnd();
            }
            session_ = 0;
        }

        override protected void OnSessionDestroy(ulong xrSession)
        {
        }
    }
}