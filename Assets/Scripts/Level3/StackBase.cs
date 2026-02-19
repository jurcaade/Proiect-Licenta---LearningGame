using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StackBase : MonoBehaviour
{
    [Header("Setari Stiva")]
    public float cubeHeight = 1.0f; // Cat de mult urca fiecare cub nou pe axa Y
    public int maxStackSize = 4;    // Cate cuburi incap maxim

    [Header("Logica Castig (Jos -> Sus)")]
    public List<string> targetSequence = new List<string> { "Red", "Blue", "Red" };
    public TextMeshPro screenText;  // Ecranul care afiseaza statusul
    public GameObject exitDoor;     // Usa care se va deschide

    // Listele noastre din memorie (Stiva)
    private List<GameObject> currentStack = new List<GameObject>();
    private List<string> currentColorStack = new List<string>();

    void Start()
    {
        UpdateScreen();
    }

    // Aceasta functie se activeaza cand un obiect intra in zona bazei
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("StackCube"))
        {
            StackCube cube = other.GetComponent<StackCube>();
            Rigidbody rb = other.GetComponent<Rigidbody>();

            // Daca blocul NU este tinut in mana (isKinematic e false cand pica liber)
            // si nu este deja in stiva
            if (cube != null && !cube.isStacked && rb != null && !rb.isKinematic)
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

        // Il "inghetam" in locul potrivit
        rb.isKinematic = true;
        cubeInfo.isStacked = true;

        // Calculam inaltimea la care trebuie sa stea (unul peste altul)
        Vector3 snapPos = transform.position + (Vector3.up * currentStack.Count * cubeHeight);
        cubeObj.transform.position = snapPos;

        // Ii reparam rotatia ca sa stea drept
        cubeObj.transform.rotation = Quaternion.Euler(0, 0, 0);

        // Adaugam in liste (LIFO - in varf)
        currentStack.Add(cubeObj);
        currentColorStack.Add(cubeInfo.cubeColor);

        UpdateScreen();
        CheckWinCondition();
    }

    // Funcția apelată cand vrei să iei cubul INAPOI din stivă (POP)
    public void RemoveCubeFromStack(GameObject cubeObj)
    {
        if (IsTopCube(cubeObj))
        {
            currentStack.RemoveAt(currentStack.Count - 1);
            currentColorStack.RemoveAt(currentColorStack.Count - 1);
            cubeObj.GetComponent<StackCube>().isStacked = false;
            UpdateScreen();
        }
    }

    // Verifică regula LIFO
    public bool IsTopCube(GameObject cubeObj)
    {
        if (currentStack.Count == 0) return false;
        return currentStack[currentStack.Count - 1] == cubeObj; // E ultimul adaugat?
    }

    void UpdateScreen()
    {
        if (screenText == null) return;

        string targetStr = string.Join("\n", targetSequence);
        List<string> displayStack = new List<string>(currentColorStack);
        displayStack.Reverse(); // Inversam ca sa arate varful sus pe ecran
        string currentStr = string.Join("\n", displayStack);

        screenText.text = "TARGET (Jos->Sus):\n" + targetStr + "\n\nCURENT (Sus->Jos):\n" + currentStr;
    }

    void CheckWinCondition()
    {
        if (currentColorStack.Count != targetSequence.Count) return;

        for (int i = 0; i < targetSequence.Count; i++)
        {
            if (currentColorStack[i] != targetSequence[i]) return;
        }

        Debug.Log("Nivel completat!");
        screenText.text = "STIVA CORECTA!\nACCES PERMIS.";
        screenText.color = Color.green;

        if (exitDoor != null) exitDoor.SetActive(false); // Deschide usa
    }
}