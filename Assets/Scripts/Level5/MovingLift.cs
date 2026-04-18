using UnityEngine;

public class MovingLift : MonoBehaviour
{
    [Header("Setari lift")]
    public float inaltimeDeUrcare = 4f;
    public float viteza = 2f;

    [Header("Setari siguranta")]
    public float distantaSiguranta = 1.5f;
    public Vector3 marimeVerificare = new Vector3(1.5f, 0.75f, 1.5f);

    private Vector3 pozitieJos;
    private Vector3 pozitieSus;
    private bool seMisca;
    private bool mergeSpreSus = true;
    private Collider[] liftColliders;

    private void Start()
    {
        pozitieJos = transform.localPosition;
        pozitieSus = pozitieJos + new Vector3(0f, inaltimeDeUrcare, 0f);
        liftColliders = GetComponentsInChildren<Collider>();
    }

    private void Update()
    {
        if (!seMisca)
        {
            return;
        }

        if (!mergeSpreSus && EsteJucatorSubLift())
        {
            mergeSpreSus = true;
            return;
        }

        Vector3 tinta = mergeSpreSus ? pozitieSus : pozitieJos;
        transform.localPosition = Vector3.MoveTowards(transform.localPosition, tinta, viteza * Time.deltaTime);

        if (Vector3.Distance(transform.localPosition, tinta) < 0.01f)
        {
            mergeSpreSus = !mergeSpreSus;
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

    private bool EsteJucatorSubLift()
    {
        if (!TryGetLiftBounds(out Bounds bounds))
        {
            return false;
        }

        Vector3 halfExtents = new Vector3(
            Mathf.Max(marimeVerificare.x * 0.5f, bounds.extents.x),
            marimeVerificare.y * 0.5f,
            Mathf.Max(marimeVerificare.z * 0.5f, bounds.extents.z));

        Vector3 centru = bounds.center - new Vector3(0f, bounds.extents.y + distantaSiguranta * 0.5f, 0f);
        Collider[] hituri = Physics.OverlapBox(centru, halfExtents);

        foreach (Collider hit in hituri)
        {
            if (hit.CompareTag("Player"))
            {
                return true;
            }
        }

        return false;
    }

    private bool TryGetLiftBounds(out Bounds bounds)
    {
        bounds = default;

        if (liftColliders == null || liftColliders.Length == 0)
        {
            liftColliders = GetComponentsInChildren<Collider>();
        }

        bool gasit = false;

        foreach (Collider col in liftColliders)
        {
            if (col == null || !col.enabled || col.isTrigger)
            {
                continue;
            }

            if (!gasit)
            {
                bounds = col.bounds;
                gasit = true;
            }
            else
            {
                bounds.Encapsulate(col.bounds);
            }
        }

        return gasit;
    }
}
