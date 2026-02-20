using UnityEngine;

public class StackCube : MonoBehaviour
{
    [Header("Setari Cub")]
    public string cubeColor; // Red, Blue, Orange etc.

    [HideInInspector]
    public bool isStacked = false;

    void Start()
    {
        Renderer rend = GetComponentInChildren<Renderer>();
        if (rend != null)
        {
            Color c = Color.white;
            if (cubeColor.ToLower() == "red") c = Color.red;
            else if (cubeColor.ToLower() == "blue") c = Color.blue;
            else if (cubeColor.ToLower() == "green") c = Color.green;

            // Schimbă culoarea pentru toate sub-materialele obiectului
            foreach (Material mat in rend.materials)
            {
                mat.color = c;
            }
        }
    }
}