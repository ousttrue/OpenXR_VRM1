using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Events;
using static OxrExtraFeatures.EyeTrackingFeature;
using OxrExtraFeatures;

namespace Vrm10XR
{
    public class EyePlayer : MonoBehaviour
    {
        [SerializeField]
        string DataPath = "EyeJoints.dat";

        [SerializeField]
        UnityEvent<long, XrEyeGazeV2FB[]> OnJointUpdated;

        FrameReader reader_;
        XrEyeGazeV2FB[] joints_;
        ArrayPin pin_;

        // Start is called before the first frame update
        void Start()
        {
            reader_ = FrameReader.Open(DataPath, Time.time, EyeRecorder.ItemSize);
            if (reader_ == null)
            {
                gameObject.SetActive(false);
            }
            Debug.Log($"open: {DataPath}");
            joints_ = new XrEyeGazeV2FB[2];
            pin_ = new ArrayPin(joints_);
        }

        // Update is called once per frame
        void Update()
        {
            if (reader_.TryGetJoints(Time.time, out var time, out var item))
            {
                Marshal.Copy(item, 0, pin_.Ptr, EyeRecorder.ItemSize);
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