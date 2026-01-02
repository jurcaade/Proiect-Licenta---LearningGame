using UnityEngine;

public class BitManager : MonoBehaviour
{
    [Header("Cuburi pentru acest nivel")]
    public BitCube[] bitCubes;

    [Header("Referinta buton (optional)")]
    public GameObject interactButtonObject; 

    private bool nivelCompletat = false;
    private InteractButton interactButton;

    void Start()
    {
        if (interactButtonObject != null && interactButton == null)
        {
            InitializeInteractButtonFromObject();
        }

        CheckAllBits();
    }

    public void SetupInteractButton(GameObject buttonObject)
    {
        if (buttonObject == null) return;

        interactButtonObject = buttonObject;
        InitializeInteractButtonFromObject();
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

        Debug.Log("[BitManager] Nivel resetat!");
    }
}