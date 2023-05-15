using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Networking;
using UnityEngine.ResourceManagement.AsyncOperations;

public class UnitAddressables : MonoBehaviour
{
    [SerializeField] private GameObject assetObject;
    [SerializeField] private Transform[] unitPosition;
    [SerializeField] private string assetBundleUrl;

    private AssetBundle bundle;

    private void Start()
    {
        StartCoroutine(LoadBundle(assetBundleUrl));
    }

    IEnumerator LoadBundle(string assetBundleUrl)
    {
        using (UnityWebRequest www = UnityWebRequestAssetBundle.GetAssetBundle(assetBundleUrl))
        {
            yield return www.SendWebRequest();

            bundle = DownloadHandlerAssetBundle.GetContent(www);

            //AssetBundleRequest request = bundle.LoadAssetAsync<GameObject>("Assets/Prefabs/Unit/Player/"+assetObject.name.ToString()+".prefab");
            AssetBundleRequest request = bundle.LoadAssetAsync<GameObject>(assetObject.name.ToString() + ".prefab");
            yield return request;

            GameObject prefab = request.asset as GameObject;

            for (int i = 0; i < unitPosition.Length; i++)
            {
                var prefabInstance = Instantiate(prefab, unitPosition[i].position, unitPosition[i].rotation);

                //TODO:なんでshaderは勝手にHidden/InternalErrorShaderに変更されていたのかまだ不明だけど、とりあえず無理矢理にStandardに戻す
                Renderer[] renderer = prefabInstance.GetComponentsInChildren<Renderer>();
                foreach (var rendererAll in renderer) {
                    Material material = rendererAll.material;
                    material.shader = Shader.Find("Standard");
                }
            }
        }
    }

    private void OnDestroy()
    {
        bundle.UnloadAsync(false);
    }
}
