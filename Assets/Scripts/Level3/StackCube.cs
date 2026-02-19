using UnityEngine;

public class StackCube : MonoBehaviour
{
    [Header("Setari Cub")]
    public string cubeColor; // Aici scrii "Red" sau "Blue" direct in Unity

    [HideInInspector]
    public bool isStacked = false; // Ne spune daca cubul e deja in stiva
}