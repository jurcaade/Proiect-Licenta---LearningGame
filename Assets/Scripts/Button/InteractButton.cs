using UnityEngine;

public class InteractButton : MonoBehaviour
{
    [Header("Referinte usi")]
    public GameObject doorLeft;
    public GameObject doorRight;
    public float viteza = 2f;

    [Header("Next Room Spawn")]
    public Transform nextRoomSpawn;

    [Header("Stare buton")]
    public bool interactable = true; // default true — permite butonului din camera de spawn să funcționeze

    private bool usaDeschisa = false;

    private Vector3 pozInchisaL = new Vector3(1.4f, 0, 0);
    private Vector3 pozDeschisaL = new Vector3(2.62f, 0, 0);
    private Vector3 pozInchisaR = new Vector3(-1.4f, 0, 0);
    private Vector3 pozDeschisaR = new Vector3(-2.62f, 0, 0);

    void Start()
    {
        // Reset usile la pozitia inchisa
        usaDeschisa = false;
        if (doorLeft != null)
            doorLeft.transform.localPosition = pozInchisaL;
        if (doorRight != null)
            doorRight.transform.localPosition = pozInchisaR;

        // Asigură vizual/collider conform stării interactable la start (pe toate componentele din ierarhie)
        ApplyRendererAndColliderState(interactable);
    }

    void Update()
    {
        if (!usaDeschisa) return;

        if (doorLeft != null)
            doorLeft.transform.localPosition = Vector3.MoveTowards(doorLeft.transform.localPosition, pozDeschisaL, viteza * Time.deltaTime);

        if (doorRight != null)
            doorRight.transform.localPosition = Vector3.MoveTowards(doorRight.transform.localPosition, pozDeschisaR, viteza * Time.deltaTime);
    }

    // Setter public pentru a schimba starea din BitManager / LevelManager
    public void SetInteractable(bool value)
    {
        interactable = value;
        ApplyRendererAndColliderState(interactable);
        Debug.Log($"[InteractButton] SetInteractable -> {interactable} pe {gameObject.name}");
    }

    void ApplyRendererAndColliderState(bool state)
    {
        // Activează/dezactivează toate Renderer-urile din obiect și copii
        Renderer[] rends = GetComponentsInChildren<Renderer>(true);
        foreach (var r in rends)
            r.enabled = state;

        // Activează/dezactivează toate Collider-ele din obiect și copii
        Collider[] cols = GetComponentsInChildren<Collider>(true);
        foreach (var c in cols)
            c.enabled = state;
    }

    void OnMouseDown()
    {
        // Daca butonul nu e activ pentru interactiune, ignora click-ul
        if (!interactable)
        {
            Debug.Log("[InteractButton] Buton indisponibil: nivelul nu e finalizat.");
            return;
        }

        if (usaDeschisa) return;

        // Deschide usa
        usaDeschisa = true;

        // Spawn urmatoarea camera + obiecte nivel
        if (LevelManager.instance != null && nextRoomSpawn != null)
        {
            LevelManager.instance.SpawnRoom(nextRoomSpawn);
        }
    }
}