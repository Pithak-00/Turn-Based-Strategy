using UnityEngine;
using System;
using System.Collections;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;


public class UnitAddressables : MonoBehaviour
{
    [SerializeField] private AssetReferenceT<GameObject> unitGameObject;
    [SerializeField] private Transform[] unitPosition;

    private GameObject[] spawnedGameObject;

    private void Start()
    {
        LoadUnit();
    }

    private async void LoadUnit()
    {
        spawnedGameObject = new GameObject[unitPosition.Length];

        for (int i = 0; i < unitPosition.Length; i++)
        {
            AsyncOperationHandle<GameObject> handle = unitGameObject.InstantiateAsync(unitPosition[i].position, unitPosition[i].rotation);

            spawnedGameObject[i] = await handle.Task;
        }
    }

    private void OnDestroy()
    {
        unitGameObject.ReleaseInstance(spawnedGameObject[0]);
    }
}
