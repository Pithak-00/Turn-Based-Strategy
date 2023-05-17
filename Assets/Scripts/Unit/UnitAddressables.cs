using System;
using System.Collections;
using UnityEngine;

public class UnitAddressables : MonoBehaviour
{
    public static event EventHandler OnAnyUnitLoaded;

    [SerializeField] private GameObject assetObject;
    [SerializeField] private Transform[] unitPosition;

    private void Start()
    {
        AddressablesLoader.OnAnyAssetBundleLoaded += AddressablesLoader_OnAnyAssetBundleLoaded;
    }

    private void AddressablesLoader_OnAnyAssetBundleLoaded(object sender, EventArgs e)
    {
        AddressablesLoader addressablesLoader = sender as AddressablesLoader;

        StartCoroutine(LoadBundle(addressablesLoader));
    }

    IEnumerator LoadBundle(AddressablesLoader addressablesLoader)
    {
        AssetBundleRequest request = addressablesLoader.GetAssetBundle().LoadAssetAsync<GameObject>(assetObject.name.ToString() + ".prefab");

        yield return request;

        GameObject prefab = request.asset as GameObject;

        for (int i = 0; i < unitPosition.Length; i++)
        {
            var prefabInstance = Instantiate(prefab, unitPosition[i].position, unitPosition[i].rotation);

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
