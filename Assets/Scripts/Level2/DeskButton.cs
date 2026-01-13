using UnityEngine;

public class DeskButton : MonoBehaviour
{
    public Level2Manager manager;

    public void Interact()
    {
        if (manager != null)
        {
            // Acum metoda CheckSolution există în manager
            manager.CheckSolution();
            Debug.Log("[DeskButton] S-a verificat starea reactorului.");
        }
        else
        {
            Debug.LogWarning("[DeskButton] Referința către Level2Manager lipsește!");
        }
    }
}