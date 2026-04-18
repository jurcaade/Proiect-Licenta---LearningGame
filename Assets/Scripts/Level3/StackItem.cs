using UnityEngine;

public class StackItem : MonoBehaviour
{
    [Header("Setari cub")]
    public string cubeColor;

    [HideInInspector]
    public bool isStacked;

    private void Start()
    {
        Renderer rend = GetComponentInChildren<Renderer>();
        if (rend == null)
        {
            return;
        }

        Color culoare = GetCubeColor();
        foreach (Material mat in rend.materials)
        {
            mat.color = culoare;
        }
    }

    private Color GetCubeColor()
    {
        string colorName = cubeColor.ToLower();

        if (colorName == "red") return Color.red;
        if (colorName == "blue") return Color.blue;
        if (colorName == "green") return Color.green;

        return Color.white;
    }
}
