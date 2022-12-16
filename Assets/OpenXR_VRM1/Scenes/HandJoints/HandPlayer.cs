using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Events;
using static OxrExtraFeatures.HandTrackingFeature;

namespace Vrm10XR
{
    public class HandPlayer : MonoBehaviour
    {
        [SerializeField]
        string DataPath = "HandJoint.dat";

        [SerializeField]
        UnityEvent<long, XrHandJointLocationEXT[]> OnJointUpdated;

        HandRecorder.Reader reader_;

        // Start is called before the first frame update
        void Start()
        {
            reader_ = HandRecorder.Reader.Open(DataPath);
            if (reader_ == null)
            {
                gameObject.SetActive(false);
            }
            Debug.Log($"open: {DataPath}");
        }

        // Update is called once per frame
        void Update()
        {
            var time = (long)((double)Time.time * 1000000);
            if (reader_.TryGetJoints(time, out var frame))
            {
                OnJointUpdated.Invoke(time, frame);
            }
        }
    }
}