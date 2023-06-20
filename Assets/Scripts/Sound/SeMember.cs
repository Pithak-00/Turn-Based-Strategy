using System.Linq;
using UnityEngine;
using TSM;

public class SeMember : AudioMemberBase
{
    public override void Start()
    {
        base.Start();

        audioSource.outputAudioMixerGroup = SoundManager.Instance.GetAudioMixerManager().GetGameSeAudioMixerGroup();
    }

    /// <summary>
    /// サウンドエフェクト再生（fbxファイルでのイベントで再生可能）
    /// </summary>
    /// <param name="seName"></param>
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
            audioSource.pitch = UnityEngine.Random.Range(0.8f, 1.2f);
            audioSource.Play(audioClip);
        }
    }

    public override void Reset()
    {
        base.Reset();
        audioSource.spatialBlend = 1f;//3Dオーディオ全振り//
    }
}
