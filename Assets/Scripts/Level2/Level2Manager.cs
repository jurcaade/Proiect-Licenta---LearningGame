using UnityEngine;

public class Level2Manager : MonoBehaviour
{
    public BatteryLogic batteryA;
    public BatteryLogic batteryB;
    public BatteryLogic batteryC;

    [Header("Referinta Buton Usa")]
    public GameObject butonUsa; // Obiectul fizic al butonului de langa usa
    public Material materialVerde; // Materialul verde pentru "Deblocat"

    public void CheckSolution()
    {
        bool A = (batteryA.bitValue == 1);
        bool B = (batteryB.bitValue == 1);
        bool C = (batteryC.bitValue == 1);

        // Formula: if ((A && B || !C))
        if ((A && B) || !C)
        {
            Debug.Log("Logica corecta! Butonul usii s-a activat.");

            // Schimbam culoarea butonului de la usa in Verde
            if (butonUsa != null)
            {
                butonUsa.GetComponent<Renderer>().material = materialVerde;

                // Aici activam scriptul de deschidere al usii (InteractButton)
                // Presupunem ca butonul are scriptul InteractButton pe el:
                var interactScript = butonUsa.GetComponent<InteractButton>();
                if (interactScript != null)
                {
                    interactScript.enabled = true; // Il activam doar acum
                }
            }
        }
        else
        {
            Debug.Log("Logica incorecta.");
        }
    }
}