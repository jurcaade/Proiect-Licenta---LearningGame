using System;
using UnityEngine;

public class BitManager : MonoBehaviour
{
    // Event emis când se schimbă valoarea (decimală 0..15)
    public event Action<int> OnValueChanged;

    [Header("Cuburi pentru acest nivel (ordinea: 0 = MSB, ultima = LSB)")]
    public BitCube[] bitCubes;

    [Header("Referinta buton (optional)")]
    public GameObject interactButtonObject;

    private bool nivelCompletat = false;
    private InteractButton interactButton;

    private int lastValue = -1;

    void Start()
    {
        if (interactButtonObject != null && interactButton == null)
        {
            InitializeInteractButtonFromObject();
        }

        // Notifică inițial valoarea curentă
        NotifyIfChanged();
        CheckAllBits();
    }

    // În BitManager.cs
    public void SetupInteractButton(GameObject buttonObj)
    {
        interactButton = buttonObj.GetComponent<InteractButton>();
        if (interactButton != null)
        {
            // Forțăm butonul să fie oprit când este legat la un puzzle nou
            interactButton.SetInteractable(false);
            Debug.Log("[BitManager] Buton legat și forțat pe ROȘU la început de nivel.");
        }
    }

    void InitializeInteractButtonFromObject()
    {
        interactButton = interactButtonObject.GetComponent<InteractButton>();
        if (interactButton == null)
        {
            Debug.LogWarning("[BitManager] Obiectul atribuit nu contine componenta InteractButton!");
            return;
        }

        interactButton.SetInteractable(false);

        Renderer buttonRenderer = interactButton.GetComponent<Renderer>();
        if (buttonRenderer != null)
            buttonRenderer.enabled = false;

        Collider buttonCollider = interactButton.GetComponent<Collider>();
        if (buttonCollider != null)
            buttonCollider.enabled = false;
    }

    // Metodă apelată de BitCube când se schimbă un bit
    public void NotifyBitChanged()
    {
        // Actualizează valoarea și notifica UI-urile abonate
        NotifyIfChanged();

        // Verifică dacă nivelul e complet (doar dacă nu e deja complet)
        CheckAllBits();
    }

    // Calculul valorii ca număr zecimal din array-ul bitCubes
    public int GetValue()
    {
        if (bitCubes == null || bitCubes.Length == 0) return 0;

        int val = 0;
        // Presupunem ordine: index 0 = MSB, ultima = LSB
        for (int i = 0; i < bitCubes.Length; i++)
        {
            int b = 0;
            if (bitCubes[i] != null)
                b = Mathf.Clamp(bitCubes[i].bitValue, 0, 1);
            val = (val << 1) | b;
        }
        return val;
    }

    private void NotifyIfChanged()
    {
        int current = GetValue();
        if (current != lastValue)
        {
            lastValue = current;
            OnValueChanged?.Invoke(current);
        }
    }

    public void CheckAllBits()
    {
        if (nivelCompletat) return; // Nu mai verifica daca nivelul e deja complet

        bool toateCorecte = true;

        foreach (BitCube cube in bitCubes)
        {
            if (cube == null) continue;

            if (cube.bitValue != cube.correctBit)
            {
                toateCorecte = false;
                break;
            }
        }

        if (toateCorecte)
        {
            nivelCompletat = true;
            Debug.Log("[BitManager] Nivel completat! Toate cuburile sunt corecte.");

            // Activeaza butonul (il face vizibil/interactabil)
            ActivateButton();
        }
    }

    void ActivateButton()
    {
        if (interactButton != null)
        {
            Renderer buttonRenderer = interactButton.GetComponent<Renderer>();
            if (buttonRenderer != null)
                buttonRenderer.enabled = true;

            Collider buttonCollider = interactButton.GetComponent<Collider>();
            if (buttonCollider != null)
                buttonCollider.enabled = true;

            interactButton.SetInteractable(true);

            Debug.Log("[BitManager] Buton activat!");
        }
        else
        {
            Debug.LogWarning("[BitManager] Nu s-a gasit InteractButton pentru activare!");
        }
    }

    // Reset nivel (optional)
    public void ResetLevel()
    {
        nivelCompletat = false;

        foreach (BitCube cube in bitCubes)
        {
            if (cube != null)
            {
                cube.ResetCube();
            }
        }

        if (interactButton != null)
        {
            Renderer buttonRenderer = interactButton.GetComponent<Renderer>();
            if (buttonRenderer != null)
                buttonRenderer.enabled = false;

            Collider buttonCollider = interactButton.GetComponent<Collider>();
            if (buttonCollider != null)
                buttonCollider.enabled = false;

            interactButton.SetInteractable(false);
        }

        // resetează valoarea notificate
        lastValue = -1;
        NotifyIfChanged();

        Debug.Log("[BitManager] Nivel resetat!");
    }
}