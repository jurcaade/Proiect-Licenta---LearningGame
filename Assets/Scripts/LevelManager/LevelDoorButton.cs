using UnityEngine;

public class LevelDoorButton : MonoBehaviour
{
    [Header("Referinte usi")]
    public GameObject doorLeft;
    public GameObject doorRight;
    public float vitezaUsi = 2f;

    [Header("Aspect buton")]
    public MeshRenderer buttonRenderer;
    [ColorUsage(true, true)]
    public Color colorActiv = Color.green;
    [ColorUsage(true, true)]
    public Color colorInactiv = Color.red;

    [Header("Animatie apasare")]
    public float distantaApasare = 0.05f;
    public float vitezaAnimatie = 15f;
    public Vector3 axaApasare = Vector3.down;

    [Header("Spawn camera urmatoare")]
    public Transform nextRoomSpawn;

    [Header("Stare buton")]
    public bool interactable;

    private const float ClosedLeftX = 1.4f;
    private const float ClosedRightX = -1.4f;
    private const float OpenLeftX = 2.62f;
    private const float OpenRightX = -2.62f;

    private bool seDeschide;
    private bool seInchide;
    private bool actiuneFinalizata;
    private Vector3 pozitieInitiala;
    private Vector3 pozitieTinta;
    private Material instanceMaterial;
    private Collider buttonCollider;
    private LevelDoorButtonAudio audioFeedback;

    public bool DoorWasOpened => actiuneFinalizata;

    private void Awake()
    {
        pozitieInitiala = transform.localPosition;
        pozitieTinta = pozitieInitiala;
        buttonCollider = GetComponent<Collider>();
        audioFeedback = GetComponent<LevelDoorButtonAudio>();

        if (buttonRenderer != null)
        {
            instanceMaterial = buttonRenderer.material;
        }
    }

    private void Start()
    {
        SetDoorPositions(ClosedLeftX, ClosedRightX);
        UpdateVisuals();
    }

    private void Update()
    {
        if (seDeschide)
        {
            MoveDoors(OpenLeftX, OpenRightX);
        }
        else if (seInchide)
        {
            MoveDoors(ClosedLeftX, ClosedRightX);
        }

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

    public void InchideUsa()
    {
        seDeschide = false;
        seInchide = true;
    }

    private void OnMouseDown()
    {
        if (!interactable || actiuneFinalizata)
        {
            return;
        }

        pozitieTinta = pozitieInitiala + (axaApasare * distantaApasare);
        audioFeedback?.PlayButtonPress();

        if (LevelManager.instance != null && LevelManager.instance.IsGameComplete())
        {
            LevelManager.instance.ShowFinalGameScreen();
            SetInteractable(false);
            return;
        }

        audioFeedback?.PlayDoorOpen();
        seDeschide = true;
        seInchide = false;
        actiuneFinalizata = true;

        if (LevelManager.instance != null && nextRoomSpawn != null)
        {
            LevelManager.instance.SpawnRoom(nextRoomSpawn);
        }
    }

    private void MoveDoors(float leftX, float rightX)
    {
        if (doorLeft != null)
        {
            Vector3 leftTarget = new Vector3(leftX, 0f, 0f);
            doorLeft.transform.localPosition = Vector3.MoveTowards(doorLeft.transform.localPosition, leftTarget, vitezaUsi * Time.deltaTime);
        }

        if (doorRight != null)
        {
            Vector3 rightTarget = new Vector3(rightX, 0f, 0f);
            doorRight.transform.localPosition = Vector3.MoveTowards(doorRight.transform.localPosition, rightTarget, vitezaUsi * Time.deltaTime);
        }
    }

    private void SetDoorPositions(float leftX, float rightX)
    {
        if (doorLeft != null)
        {
            doorLeft.transform.localPosition = new Vector3(leftX, 0f, 0f);
        }

        if (doorRight != null)
        {
            doorRight.transform.localPosition = new Vector3(rightX, 0f, 0f);
        }
    }

    private void UpdateVisuals()
    {
        if (buttonRenderer != null)
        {
            buttonRenderer.enabled = true;
        }

        if (buttonCollider != null)
        {
            buttonCollider.enabled = interactable;
        }

        if (instanceMaterial != null)
        {
            instanceMaterial.color = interactable ? colorActiv : colorInactiv;
        }
    }
}
