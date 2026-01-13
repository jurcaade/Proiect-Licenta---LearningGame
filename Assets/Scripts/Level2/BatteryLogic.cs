using UnityEngine;
using TMPro;

public class BatteryLogic : MonoBehaviour
{
    public int bitValue = 0;

    [Header("UI")]
    public TextMeshPro screenText;

    [Header("Material Index")]
    public int emissionMaterialIndex = 1; // elementul cu Blue Emission

    [Header("Emission Colors")]
    public Color onColor = Color.cyan;
    public Color offColor = Color.red;
    public float emissionIntensity = 3f;

    private Renderer rend;
    private Material[] mats;

    void Start()
    {
        rend = GetComponent<Renderer>();
        mats = rend.materials;

        SetEmission(offColor);
        UpdateText();
    }

    void OnMouseDown()
    {
        Interact();
    }

    public void Interact()
    {
        bitValue = (bitValue == 0) ? 1 : 0;
        UpdateVisuals();
    }

    void UpdateVisuals()
    {
        if (bitValue == 1)
            SetEmission(onColor);
        else
            SetEmission(offColor);

        UpdateText();
    }

    // =========================
    // EMISSION
    // =========================
    void SetEmission(Color color)
    {
        if (emissionMaterialIndex >= mats.Length) return;

        mats[emissionMaterialIndex].EnableKeyword("_EMISSION");
        mats[emissionMaterialIndex].SetColor("_EmissionColor", color * emissionIntensity);
        rend.materials = mats;
    }

    void UpdateText()
    {
        if (screenText != null)
        {
            screenText.text = bitValue.ToString();
            screenText.color = (bitValue == 1) ? Color.cyan : Color.red;
        }
    }
}
