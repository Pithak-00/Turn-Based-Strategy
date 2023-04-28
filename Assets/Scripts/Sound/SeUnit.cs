using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TSM;

public class SeUnit : AudioUnitBase
{
    public static SeUnit Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("There's more than one SeUnit! " + transform + " - " + Instance);
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public override void Start()
    {
        base.Start();

        audioSource.outputAudioMixerGroup = SoundManager.Instance.GetAudioMixerManager().GetGameSeAudioMixerGroup();
    }

    public void PlaySe(string seName)
    {
        AudioClip audioClip = audioClipList.FirstOrDefault(clip => clip.name == seName);

        if (audioClip != null)
        {
            audioSource.pitch = 1f;
            audioSource.Play(audioClip);
        }
    }

    public override void PlayDefault()
    {
        audioSource.pitch = 1f;

        base.PlayDefault();
    }

    /// <summary>
    /// ピッチランダム再生
    /// </summary>
    /// <param name="seName"></param>
    public void PlaySeRandomPitch(string seName)
    {
        AudioClip audioClip = audioClipList.FirstOrDefault(clip => clip.name == seName);

        if (audioClip != null)
        {
            audioSource.pitch = Random.Range(0.8f, 1.2f);
            audioSource.Play(audioClip);
        }
    }

    public override void Reset()
    {
        base.Reset();
        audioSource.spatialBlend = 1f;//3Dオーディオ全振り//
    }
}