using System;
using UnityEngine;

namespace openxr
{
    public class FaceTracker : IDisposable
    {
        FaceTrackingFeature feature_;
        ulong handle_;
        float[] weights_;
        ArrayPin weightsPin_;
        float[] confidences_;
        ArrayPin confidencesPin_;

        public static FaceTracker Create(FaceTrackingFeature feature, ulong session)
        {
            var create = new FaceTrackingFeature.XrFaceTrackerCreateInfoV2FB
            {
                type = XrStructureType.XR_TYPE_FACE_TRACKER_CREATE_INFO_FB,
            };

            var retVal = feature.XrCreateFaceTrackerFB(session, create, out var handle);
            if (retVal != 0)
            {
                Debug.LogWarning("XrCreateFaceTrackerFB" + retVal);
                return null;
            }
            Debug.Log($"Create FaceTracker: {handle}");

            var weights = new float[(int)FaceTrackingFeature.XrFaceExpressionFB.XR_FACE_EXPRESSION_COUNT_FB];
            var confidences = new float[(int)FaceTrackingFeature.XrFaceConfidenceFB.XR_FACE_CONFIDENCE_COUNT_FB];
            return new FaceTracker
            {
                feature_ = feature,
                handle_ = handle,
                weights_ = weights,
                weightsPin_ = new ArrayPin(weights),
                confidences_ = confidences,
                confidencesPin_ = new ArrayPin(confidences),
            };
        }

        public void Dispose()
        {
            confidencesPin_.Dispose();
            confidencesPin_ = null;
            weightsPin_.Dispose();
            weightsPin_ = null;
            feature_.XrDestroyFaceTrackerFB(handle_);
            handle_ = 0;
        }

        public bool TryGetFaceExpressionWeights(long time, ulong space, out float[] weights, out float[] confidences)
        {
            var info = new FaceTrackingFeature.XrFaceExpressionInfoFB
            {
                type = XrStructureType.XR_TYPE_FACE_EXPRESSION_INFO_FB,
                time = time,
            };

            var data = new FaceTrackingFeature.XrFaceExpressionWeightsV2FB
            {
                type = XrStructureType.XR_TYPE_FACE_EXPRESSION_WEIGHTS_FB,
                weightCount = (uint)weights_.Length,
                weights = weightsPin_.Ptr,
                confidenceCount = (uint)confidences_.Length,
                confidences = confidencesPin_.Ptr,
                time = time,
            };

            var retVal = feature_.XrGetFaceExpressionWeightsFB(handle_, in info, ref data);
            if (retVal != 0)
            {
                Debug.LogWarning($"XrGetFaceExpressionWeightsFB: {handle_}: {retVal}");
                weights = default;
                confidences = default;
                return false;
            }

            weights = weights_;
            confidences = confidences_;
            return true;
        }
    }
}
