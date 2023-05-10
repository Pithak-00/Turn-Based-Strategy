using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;


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

            AssetBundleRequest request = bundle.LoadAssetAsync<GameObject>("Assets/Prefabs/Unit/Player/"+assetObject.name.ToString()+".prefab");
            yield return request;

            GameObject prefab = request.asset as GameObject;

            for (int i = 0; i < unitPosition.Length; i++)
            {
                Instantiate(prefab, unitPosition[i].position, unitPosition[i].rotation);
            }
        }
    }

    private void OnDestroy()
    {
        bundle.UnloadAsync(false);
    }
}
