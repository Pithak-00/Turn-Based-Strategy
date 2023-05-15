using UnityEngine;
using System.Collections;
using TSM;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.Networking;

[RequireComponent(typeof(AudioSource))]
public abstract class AudioUnitBase : MonoBehaviour, IAudioPausable
{
    [SerializeField] protected AudioSource audioSource;
    [SerializeField] protected List<AudioClip> audioClipAssetReferenceList;
    [SerializeField] public bool audoPlay = false;
    [SerializeField] private string assetBundleUrl;

    public bool IsPaused { get; private set; }

    private List<AudioClip> assetReference;
    private AsyncOperationHandle<AudioClip> asyncOperation;
    private AssetBundle bundle;

    protected List<AudioClip> audioClipList;

    public virtual void Start()
    {
        StartCoroutine(LoadAudioClip(assetBundleUrl));

        SoundManager.Instance.SetPausableList(this);

        if (audoPlay)
        {
            PlayDefault();
        }
    }


    private IEnumerator LoadAudioClip(string assetBundleUrl)
    {
        audioClipList = new List<AudioClip>();

        using (UnityWebRequest www = UnityWebRequestAssetBundle.GetAssetBundle(assetBundleUrl))
        {
            yield return www.SendWebRequest();

            bundle = DownloadHandlerAssetBundle.GetContent(www);

            foreach (var asset in audioClipAssetReferenceList)
            {
                //AssetBundleRequest request = bundle.LoadAssetAsync<AudioClip>("Assets/Sound/" + asset.name.ToString() + ".mp3");
                AssetBundleRequest request = bundle.LoadAssetAsync<AudioClip>(asset.name.ToString() + ".mp3");
                //Debug.Log("Assets/Sound/" + asset.name.ToString() + ".mp3");
                yield return request;

                AudioClip prefab = request.asset as AudioClip;

                audioClipList.Add(prefab);
                Debug.Log(prefab.name.ToString());
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