using openxr;
using UniGLTF;
using UnityEngine;
using UniVRM10;


namespace Vrm10XR
{
    public class VRM1Loader : MonoBehaviour
    {
        // rename .vrm to .txt and set
        [SerializeField]
        TextAsset VRM1Binary;
        Vrm10Instance vrm_;

        VRM1HandUpdater leftUpdater_;
        VRM1HandUpdater rightUpdater_;

        // Start is called before the first frame update
        void Start()
        {
            LoadAsync();
        }

        async void LoadAsync()
        {
            // load vrm1
            var vrm10Instance = await Vrm10.LoadBytesAsync(VRM1Binary.bytes,
                controlRigGenerationOption: ControlRigGenerationOption.Vrm0XCompatibleWithXR_EXT_hand_tracking);
            if (vrm10Instance == null)
            {
                Debug.LogWarning("LoadPathAsync is null");
                return;
            }
            var instance = vrm10Instance.GetComponent<RuntimeGltfInstance>();
            instance.ShowMeshes();
            instance.EnableUpdateWhenOffscreen();

            instance.transform.SetParent(transform, false);

            vrm_ = instance.GetComponent<Vrm10Instance>();
            leftUpdater_ = new VRM1HandUpdater(vrm_, true);
            rightUpdater_ = new VRM1HandUpdater(vrm_, false);
        }

        public void OnLeftJointsUpdated(HandTrackingFeature.XrHandJointLocationEXT[] joints)
        {
            if (leftUpdater_ == null)
            {
                return;
            }
            leftUpdater_.Update(joints, 0);
        }

        public void OnRightJointsUpdated(HandTrackingFeature.XrHandJointLocationEXT[] joints)
        {
            if (rightUpdater_ == null)
            {
                return;
            }
            rightUpdater_.Update(joints, 1);
        }
    }
}