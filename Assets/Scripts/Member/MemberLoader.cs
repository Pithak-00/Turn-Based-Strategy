using UnityEngine;
using Cysharp.Threading.Tasks;

namespace Member
{
    public class MemberLoader : MonoBehaviour
    {
        [SerializeField] private GameObject assetObject;
        [SerializeField] private Transform[] memberPosition;

        private async UniTaskVoid Start()
        {
            AssetBundleRequest request = AddressablesLoader.Instance.GetAssetBundle().LoadAssetAsync<GameObject>(assetObject.name.ToString() + ".prefab");

            await request.ToUniTask();

            GameObject prefab = request.asset as GameObject;

            for (int i = 0; i < memberPosition.Length; i++)
            {
                var prefabInstance = Instantiate(prefab, memberPosition[i].position, memberPosition[i].rotation);

                //TODO:なんでshaderは勝手にHidden/InternalErrorShaderに変更されていたのかまだ不明だけど、とりあえず無理矢理にStandardに戻す
                Renderer[] renderer = prefabInstance.GetComponentsInChildren<Renderer>();
                foreach (var rendererAll in renderer)
                {
                    Material material = rendererAll.material;
                    material.shader = Shader.Find("Standard");
                }
            }
        }
    }
}