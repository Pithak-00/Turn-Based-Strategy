using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class UnitAddressables : MonoBehaviour
{
    public int Index;
    public List<AssetReferenceT<GameObject>> m_CubeHandle;
    GameObject prefab;

    private void Start()
    {
        StartCoroutine(LoadAudioClip());
    }

    IEnumerator LoadAudioClip()
    {
        var assetReference = m_CubeHandle[Index];
        var asyncOperation = assetReference.LoadAssetAsync();
        while (!asyncOperation.IsDone)
        {
            yield return null;
        }

        prefab = asyncOperation.Result;
        Instantiate(prefab, new Vector3(0, 0, 0), Quaternion.identity);
    }
}
