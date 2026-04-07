using UnityEngine;

[DisallowMultipleComponent]
public class InteractButtonAudio : MonoBehaviour
{
    [Header("Audio")]
    public AudioClip successClip;
    public AudioClip buttonPressClip;
    public AudioClip doorOpenClip;
    [Range(0f, 1f)] public float successVolume = 0.85f;
    [Min(0f)] public float successStartTime = 0f;
    [Range(0f, 1f)] public float buttonVolume = 0.8f;
    [Range(0f, 1f)] public float doorVolume = 0.9f;

    private AudioSource worldAudioSource;
    private AudioSource successAudioSource;

    private void Awake()
    {
        worldAudioSource = GetComponent<AudioSource>();
        if (worldAudioSource == null)
        {
            worldAudioSource = gameObject.AddComponent<AudioSource>();
        }

        worldAudioSource.playOnAwake = false;
        worldAudioSource.spatialBlend = 1f;
        worldAudioSource.rolloffMode = AudioRolloffMode.Linear;
        worldAudioSource.dopplerLevel = 0f;
        worldAudioSource.minDistance = 1.5f;
        worldAudioSource.maxDistance = 12f;

        successAudioSource = gameObject.AddComponent<AudioSource>();
        successAudioSource.playOnAwake = false;
        successAudioSource.spatialBlend = 0f;
        successAudioSource.dopplerLevel = 0f;
    }

    public void PlaySuccess()
    {
        if (successAudioSource == null || successClip == null)
        {
            return;
        }

        successAudioSource.Stop();
        successAudioSource.clip = successClip;
        successAudioSource.volume = successVolume;
        successAudioSource.time = Mathf.Clamp(successStartTime, 0f, successClip.length);
        successAudioSource.Play();
    }

    public void PlayButtonPress()
    {
        PlayWorldClip(buttonPressClip, buttonVolume);
    }

    public void PlayDoorOpen()
    {
        PlayWorldClip(doorOpenClip, doorVolume);
    }

    private void PlayWorldClip(AudioClip clip, float volume)
    {
        if (worldAudioSource == null || clip == null)
        {
            return;
        }

        worldAudioSource.PlayOneShot(clip, volume);
    }
}
