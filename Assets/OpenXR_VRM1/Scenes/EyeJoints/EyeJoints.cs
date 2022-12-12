using openxr;
using UnityEngine;

namespace Vrm10XR
{
    class EyeJoints : MonoBehaviour
    {
        const float EYE_SIZE = 0.05f;

        Transform[] objects_;

        void Start()
        {
            objects_ = new Transform[2];
            for (int i = 0; i < objects_.Length; ++i)
            {
                var value = i;
                var t = GameObject.CreatePrimitive(PrimitiveType.Cube).transform;
                t.localScale = new Vector3(EYE_SIZE, EYE_SIZE, EYE_SIZE);
                t.name = $"{value}";
                t.SetParent(transform);
                objects_[i] = t;
            }
        }

        public void OnGazesUpdated(EyeTrackingFeature.XrEyeGazeV2FB[] gazes)
        {
            // goto main camera forward
            transform.position = Camera.main.transform.position + Camera.main.transform.forward;

            // left and right
            for (int i = 0; i < gazes.Length; ++i)
            {
                var gaze = gazes[i];

                if (gaze.isValid)
                {
                    // convert OpenXR right handed to unity left handed !
                    var pos = gaze.gazePose.position.ToUnity();
                    pos.y = 0;
                    objects_[i].localPosition = pos;

                    objects_[i].localRotation = gaze.gazePose.orientation.ToUnity();
                }
                else
                {
                    Debug.LogWarning($"gaze[{i}].isValid: false");
                }
            }
        }
    }
}