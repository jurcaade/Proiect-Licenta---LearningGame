using UnityEngine;

public class ElevatorMovement : MonoBehaviour
{
    [Header("Setări Lift")]
    [Tooltip("Câți metri să urce liftul față de poziția inițială?")]
    public float inaltimeDeUrcare = 4f;
    public float viteza = 2f;

    [Header("Setări Siguranță (Anti-Strivire)")]
    [Tooltip("Cât de lungă să fie raza care verifică dacă ești sub lift? Ajustează în funcție de grosimea podelei liftului.")]
    public float distantaSiguranta = 1.5f;

    private Vector3 pozitieJos;
    private Vector3 pozitieSus;

    private bool seMisca = false;
    private bool mergeSpreSus = true;

    void Start()
    {
        // Salvăm automat poziția în care ai pus liftul în scenă
        pozitieJos = transform.localPosition;

        // Calculăm automat etajul
        pozitieSus = pozitieJos + new Vector3(0, inaltimeDeUrcare, 0);
    }

    void Update()
    {
        if (seMisca)
        {
            // --- SISTEMUL DE SIGURANȚĂ ---
            // Verificăm DOAR dacă liftul coboară (nu vrem să se oprească aiurea când urcă)
            if (!mergeSpreSus)
            {
                // Desenăm o linie roșie în fereastra Scene ca să vezi raza (te ajută să o ajustezi)
                Debug.DrawRay(transform.position, Vector3.down * distantaSiguranta, Color.red);

                // Tragem raza în jos
                if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, distantaSiguranta))
                {
                    // Dacă raza lovește un obiect care are eticheta "Player"
                    if (hit.collider.CompareTag("Player"))
                    {
                        Debug.LogWarning("Jucător detectat sub lift! Se oprește pentru a evita strivirea.");

                        // Oprim liftul
                        seMisca = false;

                        // OPȚIONAL: Dacă în loc să îl oprești vrei să îl trimiți înapoi sus, șterge linia de mai sus și decomentează-o pe cea de mai jos:
                        // mergeSpreSus = true; 

                        return; // Oprim execuția aici ca liftul să nu mai coboare în acest cadru
                    }
                }
            }
            // -----------------------------

            // Stabilim ținta: Sus sau Jos
            Vector3 tinta = mergeSpreSus ? pozitieSus : pozitieJos;

            // Mișcăm liftul
            transform.localPosition = Vector3.MoveTowards(transform.localPosition, tinta, viteza * Time.deltaTime);

            // Dacă a ajuns exact la țintă, schimbăm direcția
            if (Vector3.Distance(transform.localPosition, tinta) < 0.01f)
            {
                mergeSpreSus = !mergeSpreSus;
            }
        }
    }

    public void PornesteLiftul()
    {
        seMisca = true;
    }

    public void OpresteLiftul()
    {
        seMisca = false;
    }
}