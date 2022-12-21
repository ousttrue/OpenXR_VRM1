using System;
using UnityEngine;

namespace OxrExtraFeatures
{
    public class EyeTracker : IDisposable
    {
        // UnityEngine.Events.UnityEvent<long, EyeTrackingFeature.XrEyeGazeV2FB[]>
        public interface IReceiver
        {
            void OnReceived(long time, EyeTrackingFeature.XrEyeGazeV2FB[] gazes);
        }

        EyeTrackingFeature feature_;
        ulong handle_;

        public static EyeTracker Create(EyeTrackingFeature feature, ulong session)
        {
            var create = new EyeTrackingFeature.XrEyeTrackerCreateInfoV2FB
            {
                type = XrStructureType.XR_TYPE_EYE_TRACKER_CREATE_INFO_FB,
            };

            var retVal = feature.XrCreateEyeTrackerFB(session, create, out var handle);
            if (retVal != 0)
            {
                Debug.LogWarning("XrCreateEyeTrackerFB: " + retVal);
                return null;
            }
            Debug.Log($"Create EyeTracker: {handle}");

            return new EyeTracker
            {
                feature_ = feature,
                handle_ = handle,
            };
        }

        public void Dispose()
        {
            feature_.XrDestroyEyeTrackerFB(handle_);
            handle_ = 0;
        }

        public bool TryGetGaze(long time, ulong space, out EyeTrackingFeature.XrEyeGazeV2FB[] gazes)
        {
            var gazesInfo = new EyeTrackingFeature.XrEyeGazesInfoFB
            {
                type = XrStructureType.XR_TYPE_EYE_GAZES_INFO_FB,
                time = time,
                baseSpace = space,
            };

            var eyeGazes = new EyeTrackingFeature.XrEyeGazesV2FB
            {
                type = XrStructureType.XR_TYPE_EYE_GAZES_FB,
                time = time,
            };

            var retVal = feature_.XrGetEyeGazesFB(handle_, in gazesInfo, ref eyeGazes);
            if (retVal != 0)
            {
                Debug.LogWarning($"XrGetEyeGazesFB: {handle_}: {retVal}");
                gazes = default;
                return false;
            }

            gazes = eyeGazes.gaze;
            return true;
        }
    }
}
