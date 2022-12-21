using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Events;
using static OxrExtraFeatures.BodyTrackingFeature;
using OxrExtraFeatures;

namespace Vrm10XR
{
    public class BodyPlayer : MonoBehaviour
    {
        [SerializeField]
        string DataPath = "BodyJoints.dat";

        [SerializeField]
        UnityEvent<long, XrBodyJointLocationFB[]> OnJointUpdated;

        FrameReader reader_;
        XrBodyJointLocationFB[] joints_;
        ArrayPin pin_;

        // Start is called before the first frame update
        void Start()
        {
            reader_ = FrameReader.Open(DataPath, Time.time, BodyRecorder.ItemSize);
            if (reader_ == null)
            {
                gameObject.SetActive(false);
            }
            Debug.Log($"open: {DataPath}");
            joints_ = new XrBodyJointLocationFB[(int)XrBodyJointFB.XR_BODY_JOINT_COUNT_FB];
            pin_ = new ArrayPin(joints_);
        }

        // Update is called once per frame
        void Update()
        {
            if (reader_.TryGetJoints(Time.time, out var time, out var item))
            {
                Marshal.Copy(item, 0, pin_.Ptr, BodyRecorder.ItemSize);
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