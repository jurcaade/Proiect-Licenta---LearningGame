using UnityEngine;

public class BitManager : MonoBehaviour
{
    [Header("Cuburi pentru acest nivel")]
    public BitCube[] bitCubes;

    [Header("Referinta buton")]
    public GameObject interactButtonObject; // Asigneaza butonul din prefab

    private bool nivelCompletat = false;
    private InteractButton interactButton;

    void Start()
    {
        // Gaseste butonul in camera
        if (interactButtonObject != null)
        {
            interactButton = interactButtonObject.GetComponent<InteractButton>();
        }
        else
        {
            // Cauta butonul in scena daca nu e asignat manual
            interactButton = FindObjectOfType<InteractButton>();
        }

        // Verifica initial toate cuburile
        CheckAllBits();
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
            // Metoda 1: Daca butonul este ascuns initial
            Renderer buttonRenderer = interactButton.GetComponent<Renderer>();
            if (buttonRenderer != null)
                buttonRenderer.enabled = true;

            Collider buttonCollider = interactButton.GetComponent<Collider>();
            if (buttonCollider != null)
                buttonCollider.enabled = true;

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
                // Foloseste metoda publica ResetCube din BitCube
                cube.ResetCube();
            }
        }

        // Dezactiveaza butonul daca este necesar
        if (interactButton != null)
        {
            Renderer buttonRenderer = interactButton.GetComponent<Renderer>();
            if (buttonRenderer != null)
                buttonRenderer.enabled = false;

            Collider buttonCollider = interactButton.GetComponent<Collider>();
            if (buttonCollider != null)
                buttonCollider.enabled = false;
        }

        Debug.Log("[BitManager] Nivel resetat!");
    }
}