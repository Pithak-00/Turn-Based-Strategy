using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TSM;
using UnityEngine.AddressableAssets;

public class SeUnit : AudioUnitBase
{
    public List<AssetReferenceT<AudioClip>> AudioClips;

    public override void Start()
    {
        base.Start();

        audioSource.outputAudioMixerGroup = SoundManager.Instance.GetAudioMixerManager().GetGameSeAudioMixerGroup();
    }

    public void PlaySe(string seName)
    {
        AudioClip audioClip = audioClipList01.FirstOrDefault(clip => clip.name == seName);

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
        AudioClip audioClip = audioClipList01.FirstOrDefault(clip => clip.name == seName);

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
