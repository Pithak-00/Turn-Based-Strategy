using UnityEngine;
using System.Collections;
using TSM;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

[RequireComponent(typeof(AudioSource))]
public abstract class AudioUnitBase : MonoBehaviour, IAudioPausable
{
    [SerializeField]
    protected AudioSource audioSource;

    [SerializeField]
    protected List<AssetReferenceT<AudioClip>> audioClipList;

    [SerializeField]
    public bool audoPlay = false;

    public bool IsPaused { get; private set; }

    public virtual void Start()
    {
        StartCoroutine(LoadAudioClip());

        SoundManager.Instance.SetPausableList(this);

        if (audoPlay)
        {
            PlayDefault();
        }
    }

    List<AssetReferenceT<AudioClip>> assetReference;
    AsyncOperationHandle<AudioClip> asyncOperation;
    protected List<AudioClip> audioClipList01;
    IEnumerator LoadAudioClip()
    {
        audioClipList01 = new List<AudioClip>();
        assetReference = audioClipList;
        foreach (var asset in assetReference)
        {
            asyncOperation = asset.LoadAssetAsync();
            while (!asyncOperation.IsDone)
            {
                yield return null;
            }
            //audioSource.clip = asyncOperation.Result;
            if (asyncOperation.Status == AsyncOperationStatus.Succeeded)
            {
                audioClipList01.Add(asyncOperation.Result);
            }
            
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
