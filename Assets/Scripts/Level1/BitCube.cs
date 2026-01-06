using UnityEngine;
using TMPro;

public class BitCube : MonoBehaviour
{
    public int bitValue = 0;
    public int correctBit = 0;
    public TMP_Text bitText;

    void Start()
    {
        if (bitText != null)
            bitText.text = bitValue.ToString();
    }

    void OnMouseDown()
    {
        // Schimba valoarea bitului (0->1, 1->0)
        bitValue = 1 - bitValue;
        if (bitText != null)
            bitText.text = bitValue.ToString();

        // Notifică BitManager că s-a schimbat un bit (nu ștergem SetupInteractButton)
        BitManager bm = FindObjectOfType<BitManager>();
        if (bm != null)
            bm.NotifyBitChanged();
    }

    // Metoda pentru resetare cub (optional)
    public void ResetCube()
    {
        bitValue = 0;
        if (bitText != null)
            bitText.text = "0";
    }
}