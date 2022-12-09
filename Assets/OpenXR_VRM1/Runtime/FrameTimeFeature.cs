using System.Collections.Generic;
using UnityEngine.XR.OpenXR.Features;
using System.Runtime.InteropServices;
using System;
using AOT;


namespace openxr
{
    /// <summary>
    /// Intercept xrWaitFrame and get frame time
    /// </summary>
    /// <value></value>
#if UNITY_EDITOR
    [UnityEditor.XR.OpenXR.Features.OpenXRFeature(UiName = "Frame time",
        Company = "VRMC",
        FeatureId = featureId,
        Version = "0.1.0",
        Desc = "get xrFrameState from xrWaitFrame hook",
        DocumentationLink = "https://docs.unity3d.com/Packages/com.unity.xr.openxr@1.5/manual/index.html"
        )]
#endif
    public class FrameTimeFeature : OpenXRFeature
    {
        public const string featureId = "com.vrmc.frame_time";

        PFN_xrGetInstanceProcAddr mOldProc;

        /*
        typedef struct XrFrameWaitInfo {
            XrStructureType    type;
            const void*        next;
        } XrFrameWaitInfo;
        */
        [StructLayout(LayoutKind.Sequential)]
        internal struct XrFrameWaitInfo
        {
            int stype;
            IntPtr next;
        };

        /*typedef struct XrFrameState {
            XrStructureType    type;
            void*              next;
            XrTime             predictedDisplayTime;
            XrDuration         predictedDisplayPeriod;
            XrBool32           shouldRender;
        } XrFrameState;*/
        [StructLayout(LayoutKind.Sequential)]
        internal struct XrFrameState
        {
            int stype;
            IntPtr next;
            public long predictedDisplayTime;
            public long predictedDisplayPeriod;
            public int shouldRender;
        }

        /*XrResult xrWaitFrame(
            XrSession                                   session,
            const XrFrameWaitInfo*                      frameWaitInfo,
            XrFrameState*                               frameState);*/
        internal delegate int Type_xrWaitFrame(ulong session, in XrFrameWaitInfo waitInfo, ref XrFrameState state);
        Type_xrWaitFrame mOldWaitFrame;

        long frame_time = 0;
        public long FrameTime => frame_time;
        List<Delegate> callbacks = new List<Delegate>();

        [MonoPInvokeCallback(typeof(PFN_xrGetInstanceProcAddr))]
        int xrGetInstanceProcAddr_HOOK_STATIC(ulong instance, string name, out IntPtr function)
        {
            return xrGetInstanceProcAddr_HOOK(instance, name, out function);
        }

        protected IntPtr GetCallback<T>(T functionAddr) where T : System.Delegate
        {
            callbacks.Add(functionAddr); // store it so it doesn't get garbage collected
            IntPtr fp = Marshal.GetFunctionPointerForDelegate(functionAddr);
            return fp;
        }

        int xrGetInstanceProcAddr_HOOK(ulong instance, string name, out IntPtr function)
        {
            // Debug.Log($"HookGetInstanceProcAddr: {name}");
            if (name == "xrWaitFrame")
            {
                IntPtr fp;
                int retVal = mOldProc(instance, name, out fp);
                mOldWaitFrame = (Type_xrWaitFrame)Marshal.GetDelegateForFunctionPointer(fp, typeof(Type_xrWaitFrame));
                function = GetCallback<Type_xrWaitFrame>(new Type_xrWaitFrame(xrWaitFrame_HOOK_STATIC));
                return retVal;
            }
            else
            {
                return mOldProc(instance, name, out function);
            }
        }

        [MonoPInvokeCallback(typeof(Type_xrWaitFrame))]
        int xrWaitFrame_HOOK_STATIC(ulong session, in XrFrameWaitInfo waitInfo, ref XrFrameState state)
        {
            return xrWaitFrame_HOOK(session, waitInfo, ref state);
        }

        int xrWaitFrame_HOOK(ulong session, in XrFrameWaitInfo waitInfo, ref XrFrameState state)
        {
            int retVal = mOldWaitFrame(session, waitInfo, ref state);
            frame_time = state.predictedDisplayTime;
            return retVal;
        }

        protected override IntPtr HookGetInstanceProcAddr(IntPtr func)
        {
            mOldProc = Marshal.GetDelegateForFunctionPointer<PFN_xrGetInstanceProcAddr>(
                xrGetInstanceProcAddr);
            return GetCallback(new PFN_xrGetInstanceProcAddr(xrGetInstanceProcAddr_HOOK_STATIC));
        }

        public ulong CurrentAppSpace => OpenXRFeature.GetCurrentAppSpace();
    }
}