using UnityEngine;

public class InteractButton : MonoBehaviour
{
    [Header("Referinte Usi")]
    public GameObject doorLeft;
    public GameObject doorRight;
    public float viteza = 2.0f;

    [Header("Next Room")]
    public GameObject nextRoomPrefab;
    public Transform spawnPoint;

    [Header("Setari Raycast")]
    public float distantaInteractiune = 5.0f;

    private bool usaDeschisa = false;
    private bool cameraSpawnata = false;

    private Vector3 pozInchisaL, pozDeschisaL, pozInchisaR, pozDeschisaR;

    void Start()
    {
        // Initializam pozitiile locale bazate pe valorile tale
        pozInchisaL = new Vector3(1.4f, doorLeft.transform.localPosition.y, doorLeft.transform.localPosition.z);
        pozDeschisaL = new Vector3(2.62f, doorLeft.transform.localPosition.y, doorLeft.transform.localPosition.z);

        pozInchisaR = new Vector3(-1.4f, doorRight.transform.localPosition.y, doorRight.transform.localPosition.z);
        pozDeschisaR = new Vector3(-2.62f, doorRight.transform.localPosition.y, doorRight.transform.localPosition.z);
    }

    void Update()
    {
        // Detectam click stanga
        if (Input.GetMouseButtonDown(0))
        {
            VerificaObiect();
        }

        // --- MISCAREA USILOR ---
        Vector3 targetL = usaDeschisa ? pozDeschisaL : pozInchisaL;
        Vector3 targetR = usaDeschisa ? pozDeschisaR : pozInchisaR; // CORECTAT AICI (era pozInchisaR in ambele parti)

        if (doorLeft != null)
            doorLeft.transform.localPosition = Vector3.MoveTowards(doorLeft.transform.localPosition, targetL, viteza * Time.deltaTime);

        if (doorRight != null)
            doorRight.transform.localPosition = Vector3.MoveTowards(doorRight.transform.localPosition, targetR, viteza * Time.deltaTime);

        // Vizualizare raza in Scene View
        Debug.DrawRay(transform.position, transform.forward * distantaInteractiune, Color.red);
    }

    void VerificaObiect()
    {
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, distantaInteractiune))
        {
            // AFISAM IN CONSOLA CE AM LOVIT (indiferent daca e buton sau nu)
            Debug.Log("<color=cyan>Raycast lovit: </color>" + hit.collider.gameObject.name + " | Tag: " + hit.collider.tag);

            if (hit.collider.CompareTag("Buton"))
            {
                usaDeschisa = !usaDeschisa;
                Debug.Log("<color=green>Interactiune Buton!</color> Stare usa: " + (usaDeschisa ? "Deschisa" : "Inchisa"));

                if (usaDeschisa && !cameraSpawnata)
                {
                    Spawn();
                }
            }
        }
        else
        {
            Debug.Log("<color=yellow>Raycast nu a lovit nimic.</color>");
        }
    }

    void Spawn()
    {
        if (nextRoomPrefab == null || spawnPoint == null)
        {
            Debug.LogError("Lipsesc referintele pentru Spawn (Prefab sau Point)!");
            return;
        }

        GameObject room = Instantiate(nextRoomPrefab, spawnPoint.position, spawnPoint.rotation);
        cameraSpawnata = true;
        Debug.Log("<color=magenta>Camera noua a fost creata!</color>");

        // Cautam si stergem peretele BackWall
        Transform[] toateObiectele = room.GetComponentsInChildren<Transform>(true);
        foreach (Transform t in toateObiectele)
        {
            if (t.name == "BackWall")
            {
                Destroy(t.gameObject);
                Debug.Log("<color=white>BackWall sters din camera noua.</color>");
                break;
            }
        }
    }
}