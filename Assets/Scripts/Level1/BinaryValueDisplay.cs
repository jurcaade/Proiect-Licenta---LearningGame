using TMPro;
using UnityEngine;

[RequireComponent(typeof(TMP_Text))]
public class BinaryValueDisplay : MonoBehaviour
{
    public BitManager bitManager;

    private TMP_Text rezultatText;

    private void Awake()
    {
        rezultatText = GetComponent<TMP_Text>();
    }

    private void OnEnable()
    {
        if (bitManager == null)
        {
            bitManager = FindObjectOfType<BitManager>();
        }

        if (bitManager != null)
        {
            bitManager.OnValueChanged += UpdateText;
            UpdateText(bitManager.GetValue());
        }
    }

    private void OnDisable()
    {
        if (bitManager != null)
        {
            bitManager.OnValueChanged -= UpdateText;
        }
    }

    private void UpdateText(int value)
    {
        if (rezultatText == null)
        {
            return;
        }

        rezultatText.text = "<b><color=#A0A0A0>REZULTAT ACTUAL:</color></b> <size=150%><color=#00FFFF>" + value + "</color></size>";
    }
}
