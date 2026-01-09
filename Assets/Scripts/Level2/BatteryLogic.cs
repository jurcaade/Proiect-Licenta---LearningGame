using UnityEngine;
using TMPro;

public class BatteryLogic : MonoBehaviour
{
    public int bitValue = 0;
    public TextMeshPro screenText; // Child-ul 'meshtext'
    public Material matOn;  // Blue Emission
    public Material matOff; // Red Emission
    private Renderer rend;

    void Start()
    {
        rend = GetComponent<Renderer>();
        UpdateVisuals();
    }

    public void Interact()
    {
        bitValue = (bitValue == 0) ? 1 : 0;
        UpdateVisuals();
    }

    void UpdateVisuals()
    {
        if (rend != null) rend.material = (bitValue == 1) ? matOn : matOff;
        if (screenText != null)
        {
            screenText.text = bitValue.ToString();
            screenText.color = (bitValue == 1) ? Color.cyan : Color.red;
        }
    }
}