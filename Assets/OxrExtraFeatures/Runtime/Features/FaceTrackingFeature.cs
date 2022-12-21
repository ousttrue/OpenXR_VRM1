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
        DocumentationLink = "https://developer.oculus.com/documentation/native/android/move-face-tracking/",
        OpenxrExtensionStrings = XR_EXTENSION,
        Version = Constants.VERSION,
        FeatureId = FEATURE_ID)]
#endif
    public class FaceTrackingFeature : OpenXRFeature
    {
        public const string FEATURE_ID = Constants.AUTHOR_ID + ".face_tracking";
        public const string XR_EXTENSION = "XR_FB_face_tracking";

        public enum XrFaceExpressionFB
        {
            XR_FACE_EXPRESSION_BROW_LOWERER_L_FB = 0,
            XR_FACE_EXPRESSION_BROW_LOWERER_R_FB = 1,

            XR_FACE_EXPRESSION_CHEEK_PUFF_L_FB = 2,
            XR_FACE_EXPRESSION_CHEEK_PUFF_R_FB = 3,
            XR_FACE_EXPRESSION_CHEEK_RAISER_L_FB = 4,
            XR_FACE_EXPRESSION_CHEEK_RAISER_R_FB = 5,
            XR_FACE_EXPRESSION_CHEEK_SUCK_L_FB = 6,
            XR_FACE_EXPRESSION_CHEEK_SUCK_R_FB = 7,

            XR_FACE_EXPRESSION_CHIN_RAISER_B_FB = 8,
            XR_FACE_EXPRESSION_CHIN_RAISER_T_FB = 9,

            XR_FACE_EXPRESSION_DIMPLER_L_FB = 10,
            XR_FACE_EXPRESSION_DIMPLER_R_FB = 11,

            XR_FACE_EXPRESSION_EYES_CLOSED_L_FB = 12,
            XR_FACE_EXPRESSION_EYES_CLOSED_R_FB = 13,
            XR_FACE_EXPRESSION_EYES_LOOK_DOWN_L_FB = 14,
            XR_FACE_EXPRESSION_EYES_LOOK_DOWN_R_FB = 15,
            XR_FACE_EXPRESSION_EYES_LOOK_LEFT_L_FB = 16,
            XR_FACE_EXPRESSION_EYES_LOOK_LEFT_R_FB = 17,
            XR_FACE_EXPRESSION_EYES_LOOK_RIGHT_L_FB = 18,
            XR_FACE_EXPRESSION_EYES_LOOK_RIGHT_R_FB = 19,
            XR_FACE_EXPRESSION_EYES_LOOK_UP_L_FB = 20,
            XR_FACE_EXPRESSION_EYES_LOOK_UP_R_FB = 21,

            XR_FACE_EXPRESSION_INNER_BROW_RAISER_L_FB = 22,
            XR_FACE_EXPRESSION_INNER_BROW_RAISER_R_FB = 23,

            XR_FACE_EXPRESSION_JAW_DROP_FB = 24,
            XR_FACE_EXPRESSION_JAW_SIDEWAYS_LEFT_FB = 25,
            XR_FACE_EXPRESSION_JAW_SIDEWAYS_RIGHT_FB = 26,
            XR_FACE_EXPRESSION_JAW_THRUST_FB = 27,

            XR_FACE_EXPRESSION_LID_TIGHTENER_L_FB = 28,
            XR_FACE_EXPRESSION_LID_TIGHTENER_R_FB = 29,

            XR_FACE_EXPRESSION_LIP_CORNER_DEPRESSOR_L_FB = 30,
            XR_FACE_EXPRESSION_LIP_CORNER_DEPRESSOR_R_FB = 31,
            XR_FACE_EXPRESSION_LIP_CORNER_PULLER_L_FB = 32,
            XR_FACE_EXPRESSION_LIP_CORNER_PULLER_R_FB = 33,
            XR_FACE_EXPRESSION_LIP_FUNNELER_LB_FB = 34,
            XR_FACE_EXPRESSION_LIP_FUNNELER_LT_FB = 35,
            XR_FACE_EXPRESSION_LIP_FUNNELER_RB_FB = 36,
            XR_FACE_EXPRESSION_LIP_FUNNELER_RT_FB = 37,
            XR_FACE_EXPRESSION_LIP_PRESSOR_L_FB = 38,
            XR_FACE_EXPRESSION_LIP_PRESSOR_R_FB = 39,
            XR_FACE_EXPRESSION_LIP_PUCKER_L_FB = 40,
            XR_FACE_EXPRESSION_LIP_PUCKER_R_FB = 41,
            XR_FACE_EXPRESSION_LIP_STRETCHER_L_FB = 42,
            XR_FACE_EXPRESSION_LIP_STRETCHER_R_FB = 43,
            XR_FACE_EXPRESSION_LIP_SUCK_LB_FB = 44,
            XR_FACE_EXPRESSION_LIP_SUCK_LT_FB = 45,
            XR_FACE_EXPRESSION_LIP_SUCK_RB_FB = 46,
            XR_FACE_EXPRESSION_LIP_SUCK_RT_FB = 47,
            XR_FACE_EXPRESSION_LIP_TIGHTENER_L_FB = 48,
            XR_FACE_EXPRESSION_LIP_TIGHTENER_R_FB = 49,
            XR_FACE_EXPRESSION_LIPS_TOWARD_FB = 50,
            XR_FACE_EXPRESSION_LOWER_LIP_DEPRESSOR_L_FB = 51,
            XR_FACE_EXPRESSION_LOWER_LIP_DEPRESSOR_R_FB = 52,

            XR_FACE_EXPRESSION_MOUTH_LEFT_FB = 53,
            XR_FACE_EXPRESSION_MOUTH_RIGHT_FB = 54,

            XR_FACE_EXPRESSION_NOSE_WRINKLER_L_FB = 55,
            XR_FACE_EXPRESSION_NOSE_WRINKLER_R_FB = 56,

            XR_FACE_EXPRESSION_OUTER_BROW_RAISER_L_FB = 57,
            XR_FACE_EXPRESSION_OUTER_BROW_RAISER_R_FB = 58,

            XR_FACE_EXPRESSION_UPPER_LID_RAISER_L_FB = 59,
            XR_FACE_EXPRESSION_UPPER_LID_RAISER_R_FB = 60,
            XR_FACE_EXPRESSION_UPPER_LIP_RAISER_L_FB = 61,
            XR_FACE_EXPRESSION_UPPER_LIP_RAISER_R_FB = 62,

            XR_FACE_EXPRESSION_COUNT_FB = 63,
            XR_FACE_EXPRESSION_NONE_FB = -1,
            XR_FACE_EXPRESSION_MAX_ENUM_FB = 0x7FFFFFFF
        }

        public enum XrFaceExpressionSetFB
        {
            XR_FACE_EXPRESSSION_SET_DEFAULT_FB = 0,
            XR_FACE_EXPRESSSION_SET_MAX_ENUM_FB = 0x7FFFFFFF
        };

        public enum XrFaceConfidenceFB
        {
            XR_FACE_CONFIDENCE_LOWER_FACE_FB = 0,
            XR_FACE_CONFIDENCE_UPPER_FACE_FB = 1,

            XR_FACE_CONFIDENCE_COUNT_FB = 2,
            XR_FACE_CONFIDENCE_NONE_FB = -1,
            XR_FACE_CONFIDENCE_MAX_ENUM_FB = 0x7FFFFFFF
        };

        [StructLayout(LayoutKind.Sequential)]
        public struct XrFaceTrackerCreateInfoV2FB
        {
            public XrStructureType type;
            public IntPtr next;
            public XrFaceExpressionSetFB faceExpressionSet;
        };

        [StructLayout(LayoutKind.Sequential)]
        public struct XrFaceExpressionInfoFB
        {
            public XrStructureType type;
            public IntPtr next;
            public long time;
        };

        [StructLayout(LayoutKind.Sequential)]
        public struct XrFaceExpressionStatusFB
        {
            public bool isValid;
            public bool isEyeFollowingBlendshapesValid;
        };

        /*
        typedef struct XrFaceExpressionWeightsV2FB {
            XrStructureType type;
            void* XR_MAY_ALIAS next;
            uint32_t weightCount;
            float* weights;
            uint32_t confidenceCount;
            float* confidences;
            XrFaceExpressionStatusFB status;
            XrTime time;
        } XrFaceExpressionWeightsV2FB;
        */
        [StructLayout(LayoutKind.Sequential)]
        public struct XrFaceExpressionWeightsV2FB
        {
            public XrStructureType type;
            public IntPtr next;
            public UInt32 weightCount;
            public IntPtr weights;
            public UInt32 confidenceCount;
            public IntPtr confidences;
            public XrFaceExpressionStatusFB status;
            public long time;
        };

        public delegate XrResult PFN_xrCreateFaceTrackerFB(
            ulong session,
            in XrFaceTrackerCreateInfoV2FB createInfo,
            out ulong faceTracker);
        PFN_xrCreateFaceTrackerFB xrCreateFaceTrackerFB_;
        public PFN_xrCreateFaceTrackerFB XrCreateFaceTrackerFB => xrCreateFaceTrackerFB_;

        public delegate XrResult PFN_xrDestroyFaceTrackerFB(ulong faceTracker);
        PFN_xrDestroyFaceTrackerFB xrDestroyFaceTrackerFB_;
        public PFN_xrDestroyFaceTrackerFB XrDestroyFaceTrackerFB => xrDestroyFaceTrackerFB_;

        public delegate XrResult PFN_xrGetFaceExpressionWeightsFB(
            ulong faceTracker,
            in XrFaceExpressionInfoFB expressionInfo,
            ref XrFaceExpressionWeightsV2FB expressionWeights);
        PFN_xrGetFaceExpressionWeightsFB xrGetFaceExpressionWeightsFB_;
        public PFN_xrGetFaceExpressionWeightsFB XrGetFaceExpressionWeightsFB => xrGetFaceExpressionWeightsFB_;

        ulong instance_;
        ulong session_;

        public event Action<FaceTrackingFeature, ulong> SessionBegin;
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
            xrCreateFaceTrackerFB_ = Marshal.GetDelegateForFunctionPointer<PFN_xrCreateFaceTrackerFB>(getAddr("xrCreateFaceTrackerFB"));
            xrDestroyFaceTrackerFB_ = Marshal.GetDelegateForFunctionPointer<PFN_xrDestroyFaceTrackerFB>(getAddr("xrDestroyFaceTrackerFB"));
            xrGetFaceExpressionWeightsFB_ = Marshal.GetDelegateForFunctionPointer<PFN_xrGetFaceExpressionWeightsFB>(getAddr("xrGetFaceExpressionWeightsFB"));

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
