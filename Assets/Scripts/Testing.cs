using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Testing : MonoBehaviour
{
    [SerializeField] private List<AudioClip> audioClipAssetReferenceList;
    //[SerializeField] private Unit unit;
    [SerializeField] private List<AudioClip> audioClipList;

    private void Start()
    {
      //  UnitAddressables.OnAnyUnitLoaded += UnitAddressables_OnAnyUnitLoaded;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            UnitAddressables_OnAnyUnitLoaded();
        }
    }

    private void UnitAddressables_OnAnyUnitLoaded()
    {
       // AddressablesLoader addressablesLoader = sender as AddressablesLoader;
        
        StartCoroutine(LoadAudioClip02());
    }

    private IEnumerator LoadAudioClip02()
    {
        audioClipList = new List<AudioClip>();
        Debug.Log("LoadAudioClip02");
        foreach (var asset in audioClipAssetReferenceList)
        {
            AssetBundleRequest request = AddressablesLoader.Instance.GetAssetBundle().LoadAssetAsync<AudioClip>(asset.name.ToString() + ".mp3");

            yield return request;

            AudioClip prefab = request.asset as AudioClip;

            audioClipList.Add(prefab);
            //Debug.Log(prefab.name.ToString());
        }
    }
}
