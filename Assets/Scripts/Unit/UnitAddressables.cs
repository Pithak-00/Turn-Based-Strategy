using UnityEngine;
using System;
using System.Collections;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine.Events;
using System.IO;

public class UnitAddressables : MonoBehaviour
{
    //[SerializeField] private AssetReferenceT<GameObject> unitGameObject;
    [SerializeField] private Transform[] unitPosition;

    //private GameObject[] spawnedGameObject;

    [SerializeField]
    private AssetReference unitGameObject;

    private AsyncOperationHandle<GameObject> prefabHandle;

    private GameObject loadPrefab;
    //　インスタンス化したゲームオブジェクトのリスト
    [SerializeField]
    private List<GameObject> spawnedGameObjectList = new List<GameObject>();


    [SerializeField]
    private GameObject testgameobject;

    private void Start()
    {
        //LoadUnit();
        //TODO: 本番では恐らくLoadAssetAsyncはできないかもしれない
        /*
        Addressables.LoadAssetAsync<GameObject>(unitGameObject).Completed += (obj) => {
            prefabHandle = obj;
            loadPrefab = obj.Result;
            for (int i = 0; i < unitPosition.Length; i++)
            {
                spawnedGameObjectList.Add(Instantiate(loadPrefab, unitPosition[i].position, unitPosition[i].rotation));
                Instantiate(spawnedGameObjectList[0], unitPosition[i].position, unitPosition[i].rotation);
                Instantiate(loadPrefab, unitPosition[i].position, unitPosition[i].rotation);

            }
        };
        */
        StartCoroutine(LoadBundle());
    }

    IEnumerator LoadBundle()
    {
        //using (UnityWebRequest www = UnityWebRequestAssetBundle.GetAssetBundle("https://23666d26-dd61-401d-b8f5-4967934b4faa.client-api.unity3dusercontent.com/client_api/v1/environments/22ced0ec-c5c6-4cf9-8215-5e3152a47cce/buckets/8f346108-2eff-4145-b0fb-b03ae3e61dbf/entries/1e92a659-c50e-4ce7-aa77-f2fb3496f730/versions/09a7085c-3e4d-47c8-996a-61ed78e0aeeb/content/"))
        using (UnityWebRequest www = UnityWebRequestAssetBundle.GetAssetBundle("https://23666d26-dd61-401d-b8f5-4967934b4faa.client-api.unity3dusercontent.com/client_api/v1/environments/22ced0ec-c5c6-4cf9-8215-5e3152a47cce/buckets/8f346108-2eff-4145-b0fb-b03ae3e61dbf/entries/a9fc97fb-50ee-4cdf-be43-4e9eeabd595b/versions/d6982625-1e9e-4469-993e-72c9dcf3bc8e/content/"))
        {
            yield return www.SendWebRequest();

            AssetBundle bundle = DownloadHandlerAssetBundle.GetContent(www);

            // Load an asset from the bundle asynchronously
            AssetBundleRequest request = bundle.LoadAssetAsync<GameObject>("Assets/Prefabs/Unit/Player/UnitWarrior.prefab");
            //var test = bundle.GetAllAssetNames();
            //Debug.Log(test[0].ToString());
            yield return request;

            GameObject prefab = request.asset as GameObject;

            for (int i = 0; i < unitPosition.Length; i++)
            {
                Instantiate(prefab, unitPosition[i].position, unitPosition[i].rotation);
            }
        }
    }

    /*
    private IEnumerator LoadPrefabAsync()
    {
        string path = "UnitWarrior";
        UnityWebRequest www = UnityWebRequestAssetBundle.GetAssetBundle(path);
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError(www.error);
            yield break;
        }

        AssetBundle bundle = DownloadHandlerAssetBundle.GetContent(www);
        AssetBundleRequest request = bundle.LoadAssetAsync<GameObject>("UnitWarrior");
        yield return request;

        GameObject prefab = request.asset as GameObject;
        for (int i = 0; i < unitPosition.Length; i++)
        {
            Instantiate(prefab, unitPosition[i].position, unitPosition[i].rotation);
        }
    }
    */

    /* private async void LoadUnit()
     {
         spawnedGameObject = new GameObject[unitPosition.Length];

         for (int i = 0; i < unitPosition.Length; i++)
         {
             AsyncOperationHandle<GameObject> handle = unitGameObject.InstantiateAsync(unitPosition[i].position, unitPosition[i].rotation);
             AsyncOperationHandle<GameObject> hand2le = unitGameObject.Instantiate(unitPosition[i].position, unitPosition[i].rotation);

             spawnedGameObject[i] = await handle.Task;
         }
     }*/

    private void OnDestroy()
    {
        Addressables.ReleaseInstance(prefabHandle);
        //unitGameObject.ReleaseInstance(spawnedGameObject[0]);
    }
}
