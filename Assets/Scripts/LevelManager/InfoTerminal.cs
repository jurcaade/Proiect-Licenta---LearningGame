using UnityEngine;

public class InfoTerminal : MonoBehaviour
{
    [Header("Continut")]
    [TextArea(4, 10)]
    public string infoText;

    [Header("Interactiune")]
    public float interactDistance = 4f;

    [Header("Audio")]
    public AudioClip openClip;
    [Range(0f, 1f)]
    public float openVolume = 0.8f;

    private Camera playerCamera;
    private AudioSource audioSource;

    private void Start()
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

    private void Update()
    {
        if (InfoPanelManager.instance != null &&
            (InfoPanelManager.instance.IsOpen || !InfoPanelManager.instance.CanOpenInfo))
        {
            return;
        }

        if (Input.GetMouseButtonDown(0))
        {
            TryOpenInfo();
        }
    }

    private void TryOpenInfo()
    {
        if (playerCamera == null || InfoPanelManager.instance == null)
        {
            return;
        }

        Ray ray = playerCamera.ScreenPointToRay(new Vector3(Screen.width / 2f, Screen.height / 2f, 0f));
        if (!Physics.Raycast(ray, out RaycastHit hit, interactDistance))
        {
            return;
        }

        if (hit.collider == null || hit.collider.gameObject != gameObject)
        {
            return;
        }

        if (openClip != null)
        {
            audioSource.PlayOneShot(openClip, openVolume);
        }

        InfoPanelManager.instance.ShowInfo(infoText);
    }
}
