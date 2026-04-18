using TMPro;
using UnityEngine;

public class BatterySwitch : MonoBehaviour
{
    public int bitValue;

    [Header("UI")]
    public TextMeshPro screenText;

    [Header("Material index")]
    public int emissionMaterialIndex = 1;

    [Header("Emission")]
    public Color onColor = Color.cyan;
    public Color offColor = Color.red;
    public float emissionIntensity = 3f;

    [Header("Audio")]
    public AudioClip toggleClip;
    [Range(0f, 1f)] public float toggleVolume = 0.8f;
    [Min(0f)] public float toggleStartTime = 0f;

    private Renderer obiectRenderer;
    private Material[] materiale;
    private AudioSource audioSource;

    private void Start()
    {
        obiectRenderer = GetComponent<Renderer>();
        materiale = obiectRenderer.materials;

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

        UpdateVisuals();
    }

    private void OnMouseDown()
    {
        bitValue = 1 - bitValue;
        PlayToggleSound();
        UpdateVisuals();
    }

    private void UpdateVisuals()
    {
        SetEmission(bitValue == 1 ? onColor : offColor);

        if (screenText != null)
        {
            screenText.text = bitValue.ToString();
            screenText.color = bitValue == 1 ? Color.cyan : Color.red;
        }
    }

    private void SetEmission(Color color)
    {
        if (materiale == null || emissionMaterialIndex >= materiale.Length)
        {
            return;
        }

        materiale[emissionMaterialIndex].EnableKeyword("_EMISSION");
        materiale[emissionMaterialIndex].SetColor("_EmissionColor", color * emissionIntensity);
        obiectRenderer.materials = materiale;
    }

    private void PlayToggleSound()
    {
        if (audioSource == null || toggleClip == null)
        {
            return;
        }

        audioSource.Stop();
        audioSource.clip = toggleClip;
        audioSource.volume = toggleVolume;
        audioSource.time = Mathf.Clamp(toggleStartTime, 0f, toggleClip.length);
        audioSource.Play();
    }
}
