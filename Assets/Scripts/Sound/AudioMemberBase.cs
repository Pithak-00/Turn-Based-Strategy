using UnityEngine;
using TSM;
using System.Collections.Generic;
using UnityEngine.ResourceManagement.AsyncOperations;
using Cysharp.Threading.Tasks;

[RequireComponent(typeof(AudioSource))]
public abstract class AudioMemberBase : MonoBehaviour, IAudioPausable
{
    [SerializeField] protected AudioSource audioSource;
    [SerializeField] protected List<AudioClip> audioClipAssetReferenceList;
    [SerializeField] public bool autoPlay = false;

    public bool IsPaused { get; private set; }

    private AsyncOperationHandle<AudioClip> asyncOperation;

    //TODO:データロードチェック,後でSerializeField削除
    [SerializeField] protected List<AudioClip> audioClipList;

    public virtual void Start()
    {
        SoundManager.Instance.SetPausableList(this);

        if (autoPlay)
        {
            PlayDefault();
        }

        LoadAudioClip().Forget();
    }

    /// <summary>
    /// サウンドアセットバンドル取得し、audioClipListに追加
    /// </summary>
    private async UniTaskVoid LoadAudioClip()
    {
        audioClipList = new List<AudioClip>();

        foreach (var asset in audioClipAssetReferenceList)
        {
            AssetBundleRequest request = AddressablesLoader.Instance.GetAssetBundle().LoadAssetAsync<AudioClip>(asset.name.ToString() + ".mp3");
            await request.ToUniTask();
            AudioClip prefab = request.asset as AudioClip;
            audioClipList.Add(prefab);
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