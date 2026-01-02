using UnityEngine;
using TMPro;

public class BitCube : MonoBehaviour
{
    public int bitValue = 0;
    public int correctBit = 0;
    public TMP_Text bitText;

    void Start()
    {
        bitText.text = bitValue.ToString();
        UpdateColor();
    }

    void OnMouseDown()
    {
        // Schimba valoarea bitului (0->1, 1->0)
        bitValue = 1 - bitValue;
        bitText.text = bitValue.ToString();
        UpdateColor();

        // Verifica daca toate cuburile sunt corecte
        BitManager bm = FindObjectOfType<BitManager>();
        if (bm != null)
            bm.CheckAllBits();
    }

    // Schimbat din private la public
    public void UpdateColor()
    {
        if (bitText != null)
            bitText.text = bitValue.ToString();

        Renderer rend = GetComponent<Renderer>();
        if (rend != null)
        {
            if (bitValue == correctBit)
                rend.material.color = Color.green;
            else
                rend.material.color = Color.red;
        }
    }

    // Metoda pentru resetare cub (optional)
    public void ResetCube()
    {
        bitValue = 0;
        if (bitText != null)
            bitText.text = "0";
        UpdateColor();
    }
}