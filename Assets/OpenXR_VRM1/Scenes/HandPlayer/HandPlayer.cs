using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Events;
using static OxrExtraFeatures.HandTrackingFeature;
using OxrExtraFeatures;

namespace Vrm10XR
{
    public class HandPlayer : MonoBehaviour
    {
        [SerializeField]
        string DataPath = "HandJoints.dat";

        [SerializeField]
        UnityEvent<long, XrHandJointLocationEXT[]> OnJointUpdated;

        FrameReader reader_;
        XrHandJointLocationEXT[] joints_;
        ArrayPin pin_;

        // Start is called before the first frame update
        void Start()
        {
            reader_ = FrameReader.Open(DataPath, Time.time, HandRecorder.ItemSize);
            if (reader_ == null)
            {
                gameObject.SetActive(false);
            }
            Debug.Log($"open: {DataPath}");
            joints_ = new XrHandJointLocationEXT[XR_HAND_JOINT_COUNT_EXT];
            pin_ = new ArrayPin(joints_);
        }

        // Update is called once per frame
        void Update()
        {
            if (reader_.TryGetJoints(Time.time, out var time, out var item))
            {
                Marshal.Copy(item, 0, pin_.Ptr, HandRecorder.ItemSize);
                OnJointUpdated.Invoke(time, joints_);
            }
        }

        void OnDisable()
        {
            reader_.Dispose();
            reader_ = null;
        }
    }
}