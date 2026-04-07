using UnityEngine;
using TMPro;

public class BitCube : MonoBehaviour
{
    public int bitValue = 0;
    public int correctBit = 0;
    public TMP_Text bitText;

    [Header("Audio")]
    public AudioClip toggleClip;
    [Range(0f, 1f)] public float toggleVolume = 0.8f;

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

        if (bitText != null)
            bitText.text = bitValue.ToString();
    }

    void OnMouseDown()
    {
        // Schimba valoarea bitului (0->1, 1->0)
        bitValue = 1 - bitValue;
        if (bitText != null)
            bitText.text = bitValue.ToString();

        PlayToggleSound();

        // Notifică BitManager că s-a schimbat un bit (nu ștergem SetupInteractButton)
        BitManager bitManager = FindObjectOfType<BitManager>();
        if (bitManager != null)
            bitManager.NotifyBitChanged();
    }

    // Metoda pentru resetare cub (optional)
    public void ResetCube()
    {
        bitValue = 0;
        if (bitText != null)
            bitText.text = "0";
    }

    private void PlayToggleSound()
    {
        if (audioSource == null || toggleClip == null)
        {
            return;
        }

        audioSource.PlayOneShot(toggleClip, toggleVolume);
    }
}
