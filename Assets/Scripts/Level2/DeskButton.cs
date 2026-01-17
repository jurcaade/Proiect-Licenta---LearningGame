using UnityEngine;

public class DeskButton : MonoBehaviour
{
    // Aici tragem obiectul Empty care are scriptul Level2Manager
    public Level2Manager levelManager;

    // Detectează click-ul pe buton
    void OnMouseDown()
    {
        if (levelManager != null)
        {
            Debug.Log("Buton birou apăsat! Verificăm soluția...");
            levelManager.CheckSolution();
        }
        else
        {
            Debug.LogError("Referința Level2Manager lipsește de pe butonul de pe birou!");
        }
    }
}