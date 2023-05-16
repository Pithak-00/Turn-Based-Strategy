using System;
using UnityEngine;
using System.Collections;
using TSM;
using System.Collections.Generic;
using UnityEngine.ResourceManagement.AsyncOperations;

[RequireComponent(typeof(AudioSource))]
public abstract class AudioUnitBase : MonoBehaviour, IAudioPausable
{
    [SerializeField] protected AudioSource audioSource;
    [SerializeField] protected List<AudioClip> audioClipAssetReferenceList;
    [SerializeField] public bool audoPlay = false;

    public bool IsPaused { get; private set; }

    private AsyncOperationHandle<AudioClip> asyncOperation;

    //TODO:データロードチェック
    [SerializeField] protected List<AudioClip> audioClipList;

    public virtual void Start()
    {
        AddressablesLoader.OnAnyAssetBundleLoaded += AddressablesLoader_OnAnyAssetBundleLoaded;
        //UnitAddressables.OnAnyUnitLoaded += UnitAddressables_OnAnyUnitLoaded;

        SoundManager.Instance.SetPausableList(this);

        if (audoPlay)
        {
            PlayDefault();
        }

        UnitAddressables_OnAnyUnitLoaded();
    }

    private void AddressablesLoader_OnAnyAssetBundleLoaded(object sender, EventArgs e)
    {
        AddressablesLoader addressablesLoader = sender as AddressablesLoader;

        StartCoroutine(LoadAudioClip(addressablesLoader));
    }

    private void UnitAddressables_OnAnyUnitLoaded()
    {
        StartCoroutine(LoadAudioClip02());
    }

    private IEnumerator LoadAudioClip(AddressablesLoader addressablesLoader)
    {
        audioClipList = new List<AudioClip>();

        foreach (var asset in audioClipAssetReferenceList)
        {
            AssetBundleRequest request = addressablesLoader.GetAssetBundle().LoadAssetAsync<AudioClip>(asset.name.ToString() + ".mp3");

            yield return request;

            AudioClip prefab = request.asset as AudioClip;

            audioClipList.Add(prefab);
            //Debug.Log(prefab.name.ToString());
        }
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

    public virtual void PlayDefault()
    {
        if (audioSource.clip == null)
        {
            AudioClip audioClip = asyncOperation.Result;

            if (audioClip != null)
            {
                audioSource.Play(audioClip);
            }
        }
        else
        {
            audioSource.Play();
        }
    }

    //再生中だったら再生をキャンセルする//
    public virtual bool PlayIfPossible()
    {
        if (audioSource.isPlaying)
        {
            return false;
        }
        else
        {
            PlayDefault();
            return true;
        }
    }

    public virtual void Pause()
    {
        IsPaused = true;
        audioSource.Pause();
    }

    public virtual void Resume()
    {
        IsPaused = false;
        audioSource.UnPause();
    }

    public virtual void OnDestroy()
    {
        SoundManager.Instance.RemovePausableList(this);
    }

    public virtual void Reset()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.playOnAwake = false;
    }
}