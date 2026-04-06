using UnityEngine;

public class DoorTrigger : MonoBehaviour
{
    [Header("Trage butonul de la ușa pe care vrei să o închizi")]
    public InteractButton butonUsa;

    private void OnTriggerEnter(Collider other)
    {
        // 1. Log detaliat: Vedem exact secunda în care s-a declanșat și ce tag are obiectul

        if (other.transform.root.CompareTag("Player"))

        {
            // 2. Playerul a fost detectat
            Debug.Log($"[Timp: {Time.time}] Player-ul a fost detectat cu succes!");

            if (butonUsa != null)
            {
                // 3. Butonul există și dăm comanda
                butonUsa.InchideUsa();
            }
            else
            {
                // 4. Eroarea de referință
            }

            // Oprim trigger-ul
            GetComponent<Collider>().enabled = false;
        }
        else
        {
            // 5. Dacă altceva a lovit trigger-ul (nu player-ul)
        }
    }
}