using System.Threading.Tasks;
using UniGLTF;
using UnityEngine;
using UniVRM10;


namespace Vrm10XR
{
    public static class VRM1Loader
    {
        // load vrm1
        public static async Task<Vrm10Instance> LoadAsync(byte[] bytes)
        {
            var vrm10Instance = await Vrm10.LoadBytesAsync(bytes,
                controlRigGenerationOption: ControlRigGenerationOption.Vrm0XCompatibleWithXR_EXT_hand_tracking);
            if (vrm10Instance == null)
            {
                Debug.LogWarning("LoadPathAsync is null");
                return null;
            }
            var instance = vrm10Instance.GetComponent<RuntimeGltfInstance>();
            instance.ShowMeshes();
            instance.EnableUpdateWhenOffscreen();

            return instance.GetComponent<Vrm10Instance>();
        }
    }
}