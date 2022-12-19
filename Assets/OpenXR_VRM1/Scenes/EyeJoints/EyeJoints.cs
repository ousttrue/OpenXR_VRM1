using OxrExtraFeatures;
using UnityEngine;
using static OxrExtraFeatures.EyeTrackingFeature;

namespace Vrm10XR
{
    class EyeJoints : MonoBehaviour, EyeTracker.IReceiver
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

        public void OnReceived(long time, XrEyeGazeV2FB[] gazes)
        {
            // goto main camera forward
            transform.position = Camera.main.transform.position + Camera.main.transform.forward;
            transform.rotation = Camera.main.transform.rotation;

            // left and right
            for (int i = 0; i < gazes.Length; ++i)
            {
                var gaze = gazes[i];

                if (gaze.isValid)
                {
                    // convert OpenXR right handed to unity left handed !
                    // var pos = gaze.gazePose.position.ToUnity();
                    // pos.y = 0;
                    // objects_[i].localPosition = pos;

                    var rotation = gaze.gazePose.orientation.ToUnity();
                    // to camera local
                    var headLocal = Quaternion.Inverse(Camera.main.transform.rotation) * rotation;
                    objects_[i].localRotation = headLocal;
                }
                else
                {
                    Debug.LogWarning($"gaze[{i}].isValid: false");
                }
            }
        }
    }
}