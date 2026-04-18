using TMPro;
using UnityEngine;

public class BitCube : MonoBehaviour
{
    public int bitValue;
    public int correctBit;
    public TMP_Text bitText;

    [Header("Audio")]
    public AudioClip toggleClip;
    [Range(0f, 1f)] public float toggleVolume = 0.8f;

    private AudioSource audioSource;
    private BitManager bitManager;

    private void Start()
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

        bitManager = FindObjectOfType<BitManager>();
        UpdateBitText();
    }

    private void OnMouseDown()
    {
        bitValue = 1 - bitValue;
        UpdateBitText();
        PlayToggleSound();

        if (bitManager != null)
        {
            bitManager.NotifyBitChanged();
        }
    }

    public void ResetCube()
    {
        bitValue = 0;
        UpdateBitText();
    }

    private void UpdateBitText()
    {
        if (bitText != null)
        {
            bitText.text = bitValue.ToString();
        }
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
