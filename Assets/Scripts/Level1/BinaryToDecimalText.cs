using UnityEngine;
using TMPro;

[RequireComponent(typeof(TMP_Text))]
public class BinaryToDecimalText : MonoBehaviour
{
    public BitManager bitManager; // assign în Inspector (sau se caută automat)
    private TMP_Text text3D;

    void Awake()
    {
        text3D = GetComponent<TMP_Text>();
    }

    void OnEnable()
    {
        if (bitManager == null)
            bitManager = FindObjectOfType<BitManager>();

        if (bitManager != null)
            bitManager.OnValueChanged += UpdateText;
    }

    void OnDisable()
    {
        if (bitManager != null)
            bitManager.OnValueChanged -= UpdateText;
    }

    void Start()
    {
        // inițializare text cu valoarea curentă
        if (bitManager == null)
            bitManager = FindObjectOfType<BitManager>();

        if (bitManager != null)
            UpdateText(bitManager.GetValue());
    }

    private void UpdateText(int value)
    {
        if (text3D != null)
            text3D.text = "Rezultat actual: "+value.ToString();
    }
}