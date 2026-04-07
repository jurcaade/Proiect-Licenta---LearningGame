using UnityEngine;

public class InfoTerminal : MonoBehaviour
{
    [Header("Continut")]
    [TextArea(4, 10)]
    public string infoText;

    [Header("Interactiune")]
    public float interactDistance = 4f;
    public KeyCode interactKey = KeyCode.E;

    [Header("Audio")]
    public AudioClip openClip;
    [Range(0f, 1f)]
    public float openVolume = 0.8f;

    private Camera playerCamera;
    private AudioSource audioSource;

    void Start()
    {
        playerCamera = Camera.main;
        if (playerCamera == null)
        {
            playerCamera = FindObjectOfType<Camera>();
        }

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        audioSource.playOnAwake = false;
        audioSource.spatialBlend = 0f;
    }

    void Update()
    {
        if (InfoPanelController.instance != null &&
            (InfoPanelController.instance.IsOpen || !InfoPanelController.instance.CanOpenInfo))
        {
            return;
        }

        if (Input.GetKeyDown(interactKey) || Input.GetMouseButtonDown(0))
        {
            TryOpenInfo();
        }
    }

    void TryOpenInfo()
    {
        if (playerCamera == null || InfoPanelController.instance == null)
        {
            return;
        }

        Ray ray = playerCamera.ScreenPointToRay(new Vector3(Screen.width / 2f, Screen.height / 2f, 0f));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, interactDistance))
        {
            if (hit.collider != null && hit.collider.gameObject == gameObject)
            {
                if (openClip != null)
                {
                    audioSource.PlayOneShot(openClip, openVolume);
                }

                InfoPanelController.instance.ShowInfo(infoText);
            }
        }
    }
}
