using UnityEngine;

public class InteractButton : MonoBehaviour
{
    [Header("Referinte usi")]
    public GameObject doorLeft;
    public GameObject doorRight;
    public float vitezaUsi = 2f;

    [Header("Referinte Vizuale Buton")]
    public MeshRenderer buttonRenderer;
    [Header("Culori")]
    [ColorUsage(true, true)]
    public Color colorActiv = Color.green;
    [ColorUsage(true, true)]
    public Color colorInactiv = Color.red; // culoare când nu e interactiv


    [Header("Animatie Apasare")]
    public float distantaApasare = 0.05f;
    public float vitezaAnimatie = 15f;
    public Vector3 axaApasare = Vector3.down;

    [Header("Next Room Spawn")]
    public Transform nextRoomSpawn;

    [Header("Stare buton")]
    public bool interactable = false;

    private bool usaDeschisa = false;
    private Vector3 pozitieInitiala;
    private Vector3 pozitieTinta;
    private Material instanceMaterial;
    private Collider buttonCollider;

    void Awake()
    {
        // Memorăm poziția locală pentru a știi unde să revenim sau unde să apăsăm
        pozitieInitiala = transform.localPosition;
        pozitieTinta = pozitieInitiala;
        buttonCollider = GetComponent<Collider>();

        if (buttonRenderer != null)
        {
            instanceMaterial = buttonRenderer.material;
        }
    }

    void Start()
    {
        usaDeschisa = false;
        if (doorLeft != null) doorLeft.transform.localPosition = new Vector3(1.4f, 0, 0);
        if (doorRight != null) doorRight.transform.localPosition = new Vector3(-1.4f, 0, 0);

        UpdateVisuals();
    }

    void Update()
    {
        // 1. Logica usilor
        if (usaDeschisa)
        {
            if (doorLeft != null)
                doorLeft.transform.localPosition = Vector3.MoveTowards(doorLeft.transform.localPosition, new Vector3(2.62f, 0, 0), vitezaUsi * Time.deltaTime);
            if (doorRight != null)
                doorRight.transform.localPosition = Vector3.MoveTowards(doorRight.transform.localPosition, new Vector3(-2.62f, 0, 0), vitezaUsi * Time.deltaTime);
        }

        // 2. Logica animatiei butonului (Lerp intre pozitia actuala si cea tinta)
        transform.localPosition = Vector3.Lerp(transform.localPosition, pozitieTinta, Time.deltaTime * vitezaAnimatie);

        // Resetam tinta la pozitia initiala dupa ce a fost atinsa pozitia de "apasat" 
        // (asta face butonul sa revina singur inapoi daca nu ar fi ascuns imediat)
        if (Vector3.Distance(transform.localPosition, pozitieTinta) < 0.001f && pozitieTinta != pozitieInitiala)
        {
            pozitieTinta = pozitieInitiala;
        }
    }

    public void SetInteractable(bool value)
    {
        interactable = value;
        UpdateVisuals();
    }

    void UpdateVisuals()
    {
        // Lasă renderer-ul vizibil mereu
        if (buttonRenderer != null)
            buttonRenderer.enabled = true;

        // Collider-ul se activează/dezactivează doar dacă interactable este true
        if (buttonCollider != null)
            buttonCollider.enabled = interactable;

        // Schimbă culoarea materialului în funcție de starea interactable
        if (instanceMaterial != null)
        {
            instanceMaterial.color = interactable ? colorActiv : colorInactiv;
        }
    }


    void OnMouseDown()
    {
        if (!interactable || usaDeschisa) return;

        // 1. Declanșăm mișcarea fizică a butonului (Animația)
        pozitieTinta = pozitieInitiala + (axaApasare * distantaApasare);

        // 2. Deschidem ușile
        usaDeschisa = true;

        // 3. Spawnăm camera următoare
        if (LevelManager.instance != null && nextRoomSpawn != null)
        {
            LevelManager.instance.SpawnRoom(nextRoomSpawn);
        }

        // 4. Așteptăm 0.2 secunde ca ochiul să vadă mișcarea butonului, apoi îl ascundem
        // Folosim Invoke pentru a nu dezactiva collider-ul instantaneu
        Invoke("HideButtonAfterClick", 0.2f);
    }

    void HideButtonAfterClick()
    {
        SetInteractable(false);

    }
}