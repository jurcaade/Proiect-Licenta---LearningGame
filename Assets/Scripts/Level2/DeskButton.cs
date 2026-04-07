using UnityEngine;

public class DeskButton : MonoBehaviour
{
    // Aici tragem obiectul Empty care are scriptul Level2Manager
    public Level2Manager levelManager;

    [Header("Audio")]
    public AudioClip pressClip;
    [Range(0f, 1f)] public float pressVolume = 0.8f;

    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        audioSource.playOnAwake = false;
        audioSource.spatialBlend = 1f;
        audioSource.dopplerLevel = 0f;
        audioSource.minDistance = 1f;
        audioSource.maxDistance = 10f;
    }

    // Detectează click-ul pe buton
    void OnMouseDown()
    {
        PlayPressSound();

        if (levelManager != null)
        {
            Debug.Log("Buton birou apăsat! Verificăm soluția...");
            levelManager.CheckSolution();
        }
        else
        {
            Debug.LogError("Referința Level2Manager lipsește de pe butonul de pe birou!");
        }
    }

    void PlayPressSound()
    {
        if (audioSource == null || pressClip == null)
        {
            return;
        }

        audioSource.PlayOneShot(pressClip, pressVolume);
    }
}
