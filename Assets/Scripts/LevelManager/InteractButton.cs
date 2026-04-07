
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
    public Color colorInactiv = Color.red;

    [Header("Animatie Apasare")]
    public float distantaApasare = 0.05f;
    public float vitezaAnimatie = 15f;
    public Vector3 axaApasare = Vector3.down;

    [Header("Next Room Spawn")]
    public Transform nextRoomSpawn;

    [Header("Stare buton")]
    public bool interactable = false;

    // --- VARIABILE MODIFICATE AICI ---
    private bool seDeschide = false;
    private bool seInchide = false;
    private bool actiuneFinalizata = false; // Ca sa nu putem apasa de 100 de ori

    private Vector3 pozitieInitiala;
    private Vector3 pozitieTinta;
    private Material instanceMaterial;
    private Collider buttonCollider;
    private InteractButtonAudio audioFeedback;

    public bool DoorWasOpened => actiuneFinalizata;

    void Awake()
    {
        pozitieInitiala = transform.localPosition;
        pozitieTinta = pozitieInitiala;
        buttonCollider = GetComponent<Collider>();

        if (buttonRenderer != null)
        {
            instanceMaterial = buttonRenderer.material;
        }
        audioFeedback = GetComponent<InteractButtonAudio>();
    }

    void Start()
    {
        seDeschide = false;
        seInchide = false;
        actiuneFinalizata = false;

        // Pozitiile initiale (inchise)
        if (doorLeft != null) doorLeft.transform.localPosition = new Vector3(1.4f, 0, 0);
        if (doorRight != null) doorRight.transform.localPosition = new Vector3(-1.4f, 0, 0);

        UpdateVisuals();
    }

    void Update()
    {
        // 1. Logica usilor (Deschidere)
        if (seDeschide)
        {
            if (doorLeft != null)
                doorLeft.transform.localPosition = Vector3.MoveTowards(doorLeft.transform.localPosition, new Vector3(2.62f, 0, 0), vitezaUsi * Time.deltaTime);
            if (doorRight != null)
                doorRight.transform.localPosition = Vector3.MoveTowards(doorRight.transform.localPosition, new Vector3(-2.62f, 0, 0), vitezaUsi * Time.deltaTime);
        }
        // 1.5 Logica usilor (Inchidere)
        else if (seInchide)
        {
            if (doorLeft != null)
                doorLeft.transform.localPosition = Vector3.MoveTowards(doorLeft.transform.localPosition, new Vector3(1.4f, 0, 0), vitezaUsi * Time.deltaTime);
            if (doorRight != null)
                doorRight.transform.localPosition = Vector3.MoveTowards(doorRight.transform.localPosition, new Vector3(-1.4f, 0, 0), vitezaUsi * Time.deltaTime);
        }

        // 2. Logica animatiei butonului
        transform.localPosition = Vector3.Lerp(transform.localPosition, pozitieTinta, Time.deltaTime * vitezaAnimatie);

        if (Vector3.Distance(transform.localPosition, pozitieTinta) < 0.001f && pozitieTinta != pozitieInitiala)
        {
            pozitieTinta = pozitieInitiala;
        }
    }

    public void SetInteractable(bool value)
    {
        bool wasInteractable = interactable;
        interactable = value;
        UpdateVisuals();

        if (!wasInteractable && interactable)
        {
            audioFeedback?.PlaySuccess();
        }
    }

    void UpdateVisuals()
    {
        if (buttonRenderer != null)
            buttonRenderer.enabled = true;

        if (buttonCollider != null)
            buttonCollider.enabled = interactable;

        if (instanceMaterial != null)
        {
            instanceMaterial.color = interactable ? colorActiv : colorInactiv;
        }
    }

    void OnMouseDown()
    {
        // Daca butonul nu e interactabil SAU daca deja am apasat pe el, iesim
        if (!interactable || actiuneFinalizata) return;

        pozitieTinta = pozitieInitiala + (axaApasare * distantaApasare);

        if (LevelManager.instance != null && LevelManager.instance.IsGameComplete())
        {
            audioFeedback?.PlayButtonPress();
            LevelManager.instance.ShowFinalGameScreen();
            SetInteractable(false);
        }
        else
        {
            audioFeedback?.PlayButtonPress();
            audioFeedback?.PlayDoorOpen();
            seDeschide = true;
            seInchide = false; // Ne asiguram ca nu se inchide in timp ce se deschide
            actiuneFinalizata = true; // Blocăm butonul ca să nu mai deschidă uși duplicate

            if (LevelManager.instance != null && nextRoomSpawn != null)
            {
                LevelManager.instance.SpawnRoom(nextRoomSpawn);
            }
        }
    }

    // --- FUNCTIE NOUA PE CARE O VA APELA TRIGGER-UL ---
    public void InchideUsa()
    {
        seDeschide = false; // Oprim deschiderea
        seInchide = true;   // Pornim inchiderea spre pozitita 1.4f
    }
}
