using UnityEngine;

public class ElevatorMovement : MonoBehaviour
{
    [Header("Setari Lift")]
    [Tooltip("Cati metri sa urce liftul fata de pozitia initiala?")]
    public float inaltimeDeUrcare = 4f; // Modifica de aici cat de sus vrei sa mearga
    public float viteza = 2f;

    private Vector3 pozitieJos;
    private Vector3 pozitieSus;

    private bool seMisca = false;
    private bool mergeSpreSus = true; // tine minte directia: urca sau coboara

    void Start()
    {
        // 1. Salvam automat pozitia in care ai pus liftul in scena (pe post de Parter)
        pozitieJos = transform.localPosition;

        // 2. Calculam automat etajul, adunand inaltimea dorita la axa Y
        pozitieSus = pozitieJos + new Vector3(0, inaltimeDeUrcare, 0);
    }

    void Update()
    {
        if (seMisca)
        {
            // Stabilim tinta: Sus sau Jos
            Vector3 tinta = mergeSpreSus ? pozitieSus : pozitieJos;

            // Folosim localPosition in loc de position! (Evita problemele de rotatie ale hartii)
            transform.localPosition = Vector3.MoveTowards(transform.localPosition, tinta, viteza * Time.deltaTime);

            // Daca a ajuns exact la tinta, schimbam directia pentru a face ping-pong
            if (Vector3.Distance(transform.localPosition, tinta) < 0.01f)
            {
                mergeSpreSus = !mergeSpreSus; // Inversam
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