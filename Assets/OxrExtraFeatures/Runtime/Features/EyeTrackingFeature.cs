using UnityEngine.XR.OpenXR.Features;
using UnityEngine;
using System.Runtime.InteropServices;
using System;
using UnityEngine.XR.OpenXR;


namespace OxrExtraFeatures
{
#if UNITY_EDITOR
    [UnityEditor.XR.OpenXR.Features.OpenXRFeature(UiName = XR_EXTENSION,
        BuildTargetGroups = new[] {
            UnityEditor.BuildTargetGroup.Standalone, UnityEditor.BuildTargetGroup.Android },
        Desc = XR_EXTENSION,
        DocumentationLink = "https://developer.oculus.com/documentation/native/android/move-eye-tracking/",
        OpenxrExtensionStrings = XR_EXTENSION,
        Version = Constants.VERSION,
        FeatureId = FEATURE_ID)]
#endif
    public class EyeTrackingFeature : OpenXRFeature
    {
        public const string FEATURE_ID = Constants.AUTHOR_ID + ".eye_tracking";
        public const string XR_EXTENSION = "XR_FB_eye_tracking_social";

        /*
        typedef struct XrEyeTrackerCreateInfoV2FB {
            XrStructureType type;
            const void* XR_MAY_ALIAS next;
        } XrEyeTrackerCreateInfoV2FB;
        */
        [StructLayout(LayoutKind.Sequential)]
        public struct XrEyeTrackerCreateInfoV2FB
        {
            public XrStructureType type;
            public IntPtr next;
        }

        /*
        typedef XrResult(XRAPI_PTR* PFN_xrCreateEyeTrackerFB)(
            XrSession session,
            const XrEyeTrackerCreateInfoV2FB* createInfo,
            XrEyeTrackerFB* eyeTracker);
        */
        public delegate int PFN_xrCreateEyeTrackerFB(
            ulong session,
            in XrEyeTrackerCreateInfoV2FB createInfo,
            out ulong eyeTracker);
        PFN_xrCreateEyeTrackerFB xrCreateEyeTrackerFB_ = null;
        public PFN_xrCreateEyeTrackerFB XrCreateEyeTrackerFB => xrCreateEyeTrackerFB_;

        /*
        typedef XrResult(XRAPI_PTR* PFN_xrDestroyEyeTrackerFB)(XrEyeTrackerFB eyeTracker);
        */
        public delegate int PFN_xrDestroyEyeTrackerFB(ulong eyeTracker);
        PFN_xrDestroyEyeTrackerFB xrDestroyEyeTrackerFB_ = null;
        public PFN_xrDestroyEyeTrackerFB XrDestroyEyeTrackerFB => xrDestroyEyeTrackerFB_;

        /*
        typedef struct XrEyeGazesInfoFB {
            XrStructureType type;
            const void* XR_MAY_ALIAS next;
            XrSpace baseSpace;
            XrTime time;
        } XrEyeGazesInfoFB;
        */
        [StructLayout(LayoutKind.Sequential)]
        public struct XrEyeGazesInfoFB
        {
            public XrStructureType type;
            public IntPtr next;
            public ulong baseSpace;
            public long time;
        }

        /*
        typedef struct XrEyeGazeV2FB {
            XrBool32 isValid;
            XrPosef gazePose;
            float gazeConfidence;
        } XrEyeGazeV2FB;
        */
        [StructLayout(LayoutKind.Sequential)]
        public struct XrEyeGazeV2FB
        {
            public bool isValid;
            public XrPosef gazePose;
            public float gazeConfidence;
        }

        /*
        typedef struct XrEyeGazesV2FB {
            XrStructureType type;
            void* XR_MAY_ALIAS next;
            XrEyeGazeV2FB gaze[2];
            XrTime time;
        } XrEyeGazesV2FB;
        */
        [StructLayout(LayoutKind.Sequential)]
        public struct XrEyeGazesV2FB
        {
            public XrStructureType type;
            public IntPtr next;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
            public XrEyeGazeV2FB[] gaze;
            public long time;
        }

        /*
        typedef XrResult(XRAPI_PTR* PFN_xrGetEyeGazesFB)(
            XrEyeTrackerFB eyeTracker,
            const XrEyeGazesInfoFB* gazeInfo,
            XrEyeGazesV2FB* eyeGazes);
        */
        public delegate int PFN_xrGetEyeGazesFB(
            ulong eyeTracker,
            in XrEyeGazesInfoFB gazeInfo,
            ref XrEyeGazesV2FB eyeGazes);
        PFN_xrGetEyeGazesFB xrGetEyeGazesFB_ = null;
        public PFN_xrGetEyeGazesFB XrGetEyeGazesFB => xrGetEyeGazesFB_;

        ulong instance_;
        ulong session_;

        public event Action<EyeTrackingFeature, ulong> SessionBegin;
        public event Action SessionEnd;

        override protected bool OnInstanceCreate(ulong xrInstance)
        {
            instance_ = xrInstance;
            if (!OpenXRRuntime.IsExtensionEnabled(XR_EXTENSION))
            {
                Debug.LogWarning($"{XR_EXTENSION} is not enabled.");
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
            Debug.Log($"{FEATURE_ID}: {instance_}.{session_}");

            var getInstanceProcAddr = Marshal.GetDelegateForFunctionPointer<PFN_xrGetInstanceProcAddr>(xrGetInstanceProcAddr);
            Func<string, IntPtr> getAddr = (string name) =>
            {
                IntPtr ptr;
                getInstanceProcAddr(instance_, name, out ptr);
                return ptr;
            };
            xrCreateEyeTrackerFB_ = Marshal.GetDelegateForFunctionPointer<PFN_xrCreateEyeTrackerFB>(getAddr("xrCreateEyeTrackerFB"));
            xrDestroyEyeTrackerFB_ = Marshal.GetDelegateForFunctionPointer<PFN_xrDestroyEyeTrackerFB>(getAddr("xrDestroyEyeTrackerFB"));
            xrGetEyeGazesFB_ = Marshal.GetDelegateForFunctionPointer<PFN_xrGetEyeGazesFB>(getAddr("xrGetEyeGazesFB"));

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
    }
}
