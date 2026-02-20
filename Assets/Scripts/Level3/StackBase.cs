using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StackBase : MonoBehaviour
{
    [Header("Setari Stiva")]
    public float cubeHeight = 1.0f;
    public int maxStackSize = 4;

    [Header("Setup Initial (Aseaza gresit)")]
    [Tooltip("Trage aici cuburile din scena pe care le vrei deja in stiva de la inceput (Ordine: Jos -> Sus)")]
    public List<GameObject> initialCubes;

    [Header("Logica Castig (Ordine Corecta: Jos -> Sus)")]
    public List<string> targetSequence = new List<string> { "Red", "Blue", "Red" };

    private List<GameObject> currentStack = new List<GameObject>();
    private List<string> currentColorStack = new List<string>();

    // Variabila care va ține referința la buton
    private InteractButton levelButton;

    void Start()
    {
        // Punem cuburile initiale pe stiva automat la pornirea jocului
        foreach (GameObject cube in initialCubes)
        {
            if (cube != null)
            {
                StackCube cubeInfo = cube.GetComponent<StackCube>();
                Rigidbody rb = cube.GetComponent<Rigidbody>();

                if (cubeInfo != null && rb != null)
                {
                    AddCubeToStack(cube, cubeInfo, rb);
                }
            }
        }
    }

    // Funcția apelată de LevelManager ca să ne dea butonul
    public void SetupInteractButton(GameObject buttonObj)
    {
        if (buttonObj != null)
        {
            levelButton = buttonObj.GetComponent<InteractButton>();
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("StackCube"))
        {
            StackCube cube = other.GetComponent<StackCube>();
            Rigidbody rb = other.GetComponent<Rigidbody>();

            if (cube != null && !cube.isStacked && rb != null && !rb.isKinematic && other.transform.parent == null)
            {
                AddCubeToStack(other.gameObject, cube, rb);
            }
        }
    }

    void AddCubeToStack(GameObject cubeObj, StackCube cubeInfo, Rigidbody rb)
    {
        if (currentStack.Count >= maxStackSize)
        {
            Debug.Log("Stiva e plina!");
            return;
        }

        rb.isKinematic = true;
        cubeInfo.isStacked = true;

        Vector3 snapPos = transform.position + (Vector3.up * currentStack.Count * cubeHeight);
        cubeObj.transform.position = snapPos;
        cubeObj.transform.rotation = Quaternion.Euler(0, 0, 0);

        currentStack.Add(cubeObj);
        currentColorStack.Add(cubeInfo.cubeColor);

        CheckWinCondition();
    }

    public bool IsTopCube(GameObject cubeObj)
    {
        if (currentStack.Count == 0) return false;
        return currentStack[currentStack.Count - 1] == cubeObj;
    }

    public void RemoveCubeFromStack(GameObject cubeObj)
    {
        if (IsTopCube(cubeObj))
        {
            currentStack.RemoveAt(currentStack.Count - 1);
            currentColorStack.RemoveAt(currentColorStack.Count - 1);

            cubeObj.GetComponent<StackCube>().isStacked = false;

            Rigidbody rb = cubeObj.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.isKinematic = false;
            }

            // Daca a scos un cub, cu siguranta nivelul nu mai e completat corect,
            // asa ca dezactivam butonul (il facem rosu la loc).
            if (levelButton != null)
            {
                levelButton.SetInteractable(false);
            }
        }
    }

    void CheckWinCondition()
    {
        // Daca nu sunt destule cuburi inca, renuntam
        if (currentColorStack.Count != targetSequence.Count) return;

        // Verificam sa vedem daca culorile se potrivesc (IGNORAM majusculele)
        for (int i = 0; i < targetSequence.Count; i++)
        {
            if (currentColorStack[i].ToLower() != targetSequence[i].ToLower())
            {
                return; // Daca nu se potrivesc, iesim din functie
            }
        }

        // --- DACA AJUNGE AICI, A CASTIGAT! ---
        Debug.Log("Nivelul 3 completat! Butonul a devenit verde.");

        if (levelButton != null)
        {
            levelButton.SetInteractable(true);
        }
        LevelManager.instance.ShowLevelCompleteMessage();
    }

}