using openxr;
using UnityEngine;

namespace Vrm10XR
{
    class FaceWeights : MonoBehaviour
    {
        const float SIZE = 0.1f;
        const float START = -8 * SIZE;
        Transform[] objects_;

        void Start()
        {
            objects_ = new Transform[(int)FaceTrackingFeature.XrFaceExpressionFB.XR_FACE_EXPRESSION_COUNT_FB];
            var x = START;
            var y = START;
            for (int i = 0; i < objects_.Length; ++i, x += SIZE * 2)
            {
                if (i % 8 == 0)
                {
                    x = START;
                    y += SIZE * 2;
                }
                var value = (FaceTrackingFeature.XrFaceExpressionFB)i;
                var t = GameObject.CreatePrimitive(PrimitiveType.Cube).transform;
                t.name = $"{value}";
                t.SetParent(transform);
                t.transform.localPosition = new Vector3(x, y, 1);
                objects_[i] = t;
            }
        }

        public void OnWeightsUpdated(float[] weights)
        {
            // goto main camera forward
            transform.position = Camera.main.transform.position + Camera.main.transform.forward;
            transform.rotation = Camera.main.transform.rotation;

            // left and right
            for (int i = 0; i < weights.Length; ++i)
            {
                var w = Mathf.Lerp(0.01f, SIZE, weights[i]);
                objects_[i].localScale = new Vector3(w, w, w);
            }
        }
    }
}