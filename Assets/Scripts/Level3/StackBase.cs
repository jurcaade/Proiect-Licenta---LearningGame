using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StackBase : MonoBehaviour
{
    [Header("Setari Stiva")]
    public float cubeHeight = 1.0f;
    public int maxStackSize = 4;

    [Header("Logica Castig (Jos -> Sus)")]
    public List<string> targetSequence = new List<string> { "Red", "Blue", "Red" };
    public TextMeshPro screenText;
    public GameObject exitDoor;

    private List<GameObject> currentStack = new List<GameObject>();
    private List<string> currentColorStack = new List<string>();

    void Start()
    {
        UpdateScreen();
    }

    // Am schimbat in OnTriggerSTAY ca sa poti face zona inalta
    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("StackCube"))
        {
            StackCube cube = other.GetComponent<StackCube>();
            Rigidbody rb = other.GetComponent<Rigidbody>();

            // Verifica sa nu fie stivuit, sa aiba rigidbody liber si SA NU FIE IN MANA (parent == null)
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

        // Pozitionarea ta originala care mergea bine (nu mai zboara in aer)
        Vector3 snapPos = transform.position + (Vector3.up * currentStack.Count * cubeHeight);
        cubeObj.transform.position = snapPos;
        cubeObj.transform.rotation = Quaternion.Euler(0, 0, 0);

        currentStack.Add(cubeObj);
        currentColorStack.Add(cubeInfo.cubeColor);

        UpdateScreen();
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

            // FOARTE IMPORTANT: Deblocam fizica cubului ca sa poata fi luat si mutat
            Rigidbody rb = cubeObj.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.isKinematic = false;
            }

            UpdateScreen();
        }
    }

    void UpdateScreen()
    {
        if (screenText == null) return;
        string targetStr = string.Join("\n", targetSequence);
        List<string> displayStack = new List<string>(currentColorStack);
        displayStack.Reverse();
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
        if (exitDoor != null) exitDoor.SetActive(false);
        screenText.text = "STIVA CORECTA!\nACCES PERMIS.";
        screenText.color = Color.green;
    }
}