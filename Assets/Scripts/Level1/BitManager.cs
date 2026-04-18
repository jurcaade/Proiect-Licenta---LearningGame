using System;
using UnityEngine;

public class BitManager : MonoBehaviour
{
    public event Action<int> OnValueChanged;

    [Header("Cuburi nivel")]
    public BitCube[] bitCubes;

    [Header("Buton nivel")]
    public GameObject interactButtonObject;

    private bool nivelCompletat;
    private int lastValue = -1;
    private LevelDoorButton interactButton;

    private void Start()
    {
        if (interactButtonObject != null)
        {
            SetupInteractButton(interactButtonObject);
        }

        NotifyIfChanged();
        CheckAllBits();
    }

    public void SetupInteractButton(GameObject buttonObj)
    {
        if (buttonObj == null)
        {
            return;
        }

        interactButton = buttonObj.GetComponent<LevelDoorButton>();
        if (interactButton == null)
        {
            Debug.LogWarning("[BitManager] Butonul nu are componenta LevelDoorButton.");
            return;
        }

        SetButtonState(false);
    }

    public void NotifyBitChanged()
    {
        NotifyIfChanged();
        CheckAllBits();
    }

    public int GetValue()
    {
        if (bitCubes == null || bitCubes.Length == 0)
        {
            return 0;
        }

        int value = 0;

        for (int i = 0; i < bitCubes.Length; i++)
        {
            int bit = 0;
            if (bitCubes[i] != null)
            {
                bit = Mathf.Clamp(bitCubes[i].bitValue, 0, 1);
            }

            value = (value << 1) | bit;
        }

        return value;
    }

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

        SetButtonState(false);
        lastValue = -1;
        NotifyIfChanged();
    }

    public void CheckAllBits()
    {
        if (nivelCompletat)
        {
            return;
        }

        foreach (BitCube cube in bitCubes)
        {
            if (cube == null)
            {
                continue;
            }

            if (cube.bitValue != cube.correctBit)
            {
                return;
            }
        }

        nivelCompletat = true;
        SetButtonState(true);
        LevelManager.instance.ShowLevelCompleteMessage();
    }

    private void NotifyIfChanged()
    {
        int currentValue = GetValue();
        if (currentValue == lastValue)
        {
            return;
        }

        lastValue = currentValue;
        OnValueChanged?.Invoke(currentValue);
    }

    private void SetButtonState(bool isActive)
    {
        if (interactButton == null)
        {
            return;
        }

        Renderer buttonRenderer = interactButton.GetComponent<Renderer>();
        if (buttonRenderer != null)
        {
            buttonRenderer.enabled = isActive;
        }

        Collider buttonCollider = interactButton.GetComponent<Collider>();
        if (buttonCollider != null)
        {
            buttonCollider.enabled = isActive;
        }

        interactButton.SetInteractable(isActive);
    }
}
