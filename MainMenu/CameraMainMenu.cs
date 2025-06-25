using UnityEngine;
using System.Collections;

public class CameraMainMenu : MonoBehaviour
{
    private AudioClip warpAudioClip;
    private AudioClip playAudioClip;

    void Start()
    {
        warpAudioClip = Resources.Load<AudioClip>("audio/warp");
        playAudioClip = Resources.Load<AudioClip>("audio/playSound");
    }

    public void WarpSound()
    {
        PlayOneShotAndDestroy(warpAudioClip, 0.6f);
    }

    public void PlayButtonSound()
    {
        PlayOneShotAndDestroy(playAudioClip, 1.5f);
    }

    private void PlayOneShotAndDestroy(AudioClip clip, float volume)
    {
        if (clip == null) return;

        GameObject tempGO = new GameObject("TempAudio");
        tempGO.transform.SetParent(this.transform);

        AudioSource aSource = tempGO.AddComponent<AudioSource>();
        aSource.clip = clip;
        aSource.volume = volume;
        aSource.Play();

        Destroy(tempGO, clip.length);
    }
}
