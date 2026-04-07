using UnityEngine;

public class DeskButton : MonoBehaviour
{
    // Aici tragem obiectul Empty care are scriptul Level2Manager
    public Level2Manager levelManager;

    [Header("Animatie")]
    public float pressDistance = 0.04f;
    public float pressSpeed = 12f;
    public Vector3 pressAxis = Vector3.down;

    [Header("Audio")]
    public AudioClip pressClip;
    [Range(0f, 1f)] public float pressVolume = 0.8f;

    private AudioSource audioSource;
    private Vector3 initialLocalPosition;
    private Vector3 targetLocalPosition;

    void Start()
    {
        initialLocalPosition = transform.localPosition;
        targetLocalPosition = initialLocalPosition;

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

    void Update()
    {
        transform.localPosition = Vector3.Lerp(transform.localPosition, targetLocalPosition, Time.deltaTime * pressSpeed);

        if (Vector3.Distance(transform.localPosition, targetLocalPosition) < 0.001f && targetLocalPosition != initialLocalPosition)
        {
            targetLocalPosition = initialLocalPosition;
        }
    }

    // Detectează click-ul pe buton
    void OnMouseDown()
    {
        targetLocalPosition = initialLocalPosition + (pressAxis.normalized * pressDistance);
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
