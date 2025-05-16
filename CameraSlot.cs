using UnityEngine;
using System.Collections;

public class CameraSlot : MonoBehaviour
{
    private AudioSource musicAudioSource;
    private GameObject slotSpinGO;
    private FileManager fileManager;

    private AudioClip backgroundMusicClip;
    private AudioClip effectAudioClip;
    private AudioClip slotAudioClip;
    private AudioClip normalWinAudioClip;
    private AudioClip sparkAudioClip;
    private AudioClip vfxThunderAudioClip;
    private AudioClip vfxLaserBeamAudioClip;
    private AudioClip vfxExplosionAudioClip;
    private AudioClip vfxCoinAudioClip;

    void Awake()
    {
        fileManager = FindFirstObjectByType<FileManager>();
        string waifuName = fileManager.GetActiveWaifuName().ToString();

        // Musica di sottofondo
        musicAudioSource = gameObject.AddComponent<AudioSource>();
        backgroundMusicClip = Resources.Load<AudioClip>($"audio/{waifuName}_slot");
        musicAudioSource.clip = backgroundMusicClip;
        musicAudioSource.loop = true;
        musicAudioSource.volume = 0.6f;
        musicAudioSource.Play();

        // Precaricamento effetti (opzionale, puoi anche caricarli al volo)
        effectAudioClip = Resources.Load<AudioClip>("audio/stopSpin");
        slotAudioClip = Resources.Load<AudioClip>("audio/slotSpin");
        normalWinAudioClip = Resources.Load<AudioClip>("audio/normalWin");
        sparkAudioClip = Resources.Load<AudioClip>("audio/sparkSound");
        vfxThunderAudioClip = Resources.Load<AudioClip>("audio/VfxThunder");
        vfxLaserBeamAudioClip = Resources.Load<AudioClip>("audio/VfxLaserBeam");
        vfxExplosionAudioClip = Resources.Load<AudioClip>("audio/VfxExplosion");
        vfxCoinAudioClip = Resources.Load<AudioClip>("audio/VfxCoin");
    }

    public void StartSlotSpinSound()
    {
        if (slotSpinGO != null) return;

        slotSpinGO = new GameObject("SlotSpinAudio");
        slotSpinGO.transform.SetParent(this.transform);

        AudioSource aSource = slotSpinGO.AddComponent<AudioSource>();
        aSource.clip = slotAudioClip;
        aSource.loop = true;
        aSource.volume = 0.6f;
        aSource.Play();
    }

    public void StopSlotSpinSound()
    {
        if (slotSpinGO != null)
        {
            Destroy(slotSpinGO);
            slotSpinGO = null;
        }
    }

    public void StopSpinSound()
    {
        PlayOneShotAndDestroy(effectAudioClip, 0.4f);
    }

    public void StopSlotSound()
    {
        PlayOneShotAndDestroy(effectAudioClip, 0.4f);
    }

    public void StartNormalWinSound()
    {
        PlayOneShotAndDestroy(normalWinAudioClip, 0.4f);
    }

    public void StartSparkInSlotSound()
    {
        StartCoroutine(PlayOneShotWithDelay(sparkAudioClip, 0.6f, 0.8f));
    }

    public void StartVfxThunder()
    {
        PlayOneShotAndDestroy(vfxThunderAudioClip, 0.6f);
    }
    
    public void StartVfxLaserBeam()
    {
        PlayOneShotAndDestroy(vfxLaserBeamAudioClip, 0.6f);
    }
    
    public void StartVfxExplosion()
    {
        PlayOneShotAndDestroy(vfxExplosionAudioClip, 0.6f);
    }

    public void StartVfxCoin()
    {
        PlayOneShotAndDestroy(vfxCoinAudioClip, 0.6f);
        PlayOneShotAndDestroy(vfxCoinAudioClip, 0.6f);
        PlayOneShotAndDestroy(vfxCoinAudioClip, 0.6f);
        PlayOneShotAndDestroy(vfxCoinAudioClip, 0.6f);
        PlayOneShotAndDestroy(vfxCoinAudioClip, 0.6f);
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

    private IEnumerator PlayOneShotWithDelay(AudioClip clip, float delay, float volume)
    {
        yield return new WaitForSeconds(delay);
        PlayOneShotAndDestroy(clip, volume);
    }
}
