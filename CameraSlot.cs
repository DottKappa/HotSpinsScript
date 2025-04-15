using UnityEngine;
using System.Collections;

public class CameraSlot : MonoBehaviour
{
    private AudioSource musicAudioSource;  // AudioSource per la musica di sottofondo
    private AudioSource effectAudioSource;  // AudioSource per l'effetto sonoro
    private AudioSource slotAudioSource; 
    private AudioClip backgroundMusicClip;
    private AudioClip effectAudioClip;
    private FileManager fileManager;

// TODO -> manca che se cambia lo step della waifu devo cambiare audio
    void Awake()
    {
        fileManager = FindFirstObjectByType<FileManager>();
        string waifuName = fileManager.GetActiveWaifuName().ToString();

        musicAudioSource = gameObject.AddComponent<AudioSource>();
        effectAudioSource = gameObject.AddComponent<AudioSource>();

        backgroundMusicClip = Resources.Load<AudioClip>("audio/" + waifuName + "_slot_" + fileManager.GetImageStepByWaifu(fileManager.GetActiveWaifuName()));
        musicAudioSource.clip = backgroundMusicClip;
        musicAudioSource.loop = true;
        musicAudioSource.volume = 0.6f;
        musicAudioSource.Play();

        effectAudioClip = Resources.Load<AudioClip>("audio/stopSpin");
        effectAudioSource.priority = 256;
    }

    public void StopSpinSound()
    {        
        effectAudioSource.volume = 0.4f;
        effectAudioSource.PlayOneShot(effectAudioClip);
    }

    public void StopSlotSound()
    {
        effectAudioSource.volume = 0.4f;
        effectAudioSource.PlayOneShot(effectAudioClip);
    }

    public void StartSlotSpinSound()
    {
        slotAudioSource = gameObject.AddComponent<AudioSource>();
        AudioClip slotAudioClip = Resources.Load<AudioClip>("audio/slotSpin");
        slotAudioSource.loop = true;
        slotAudioSource.volume = 0.6f;
        slotAudioSource.clip = slotAudioClip;
        slotAudioSource.Play();        
    }

    public void StartNormalWinSound()
    {
        AudioSource normalWinAudioSource = gameObject.AddComponent<AudioSource>();
        AudioClip normalWinAudioClip = Resources.Load<AudioClip>("audio/normalWin");
        normalWinAudioSource.volume = 0.4f;
        normalWinAudioSource.PlayOneShot(normalWinAudioClip);        
    }

    public void StopSlotSpinSound()
    {
        if (slotAudioSource != null) {
            slotAudioSource.Stop();
            Destroy(slotAudioSource);
            slotAudioSource = null;
        }
    }

    public void StartSparkInSlotSound()
    {
        AudioSource audioSource = gameObject.AddComponent<AudioSource>();
        AudioClip audioclip = Resources.Load<AudioClip>("audio/sparkSound");
        StartCoroutine(PlaySoundWithDelay(audioSource, audioclip, 0.6f, 0.8f));
    }

    private IEnumerator PlaySoundWithDelay(AudioSource source, AudioClip clip, float delay, float volume)
    {
        if (source == null || clip == null) yield break;

        yield return new WaitForSeconds(delay);
        source.volume = volume;
        source.PlayOneShot(clip);
    }
}
