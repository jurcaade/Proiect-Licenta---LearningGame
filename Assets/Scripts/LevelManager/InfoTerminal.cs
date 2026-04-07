using UnityEngine;

public class InfoTerminal : MonoBehaviour
{
    [Header("Continut")]
    [TextArea(4, 10)]
    public string infoText;

    [Header("Interactiune")]
    public float interactDistance = 4f;
    public KeyCode interactKey = KeyCode.E;

    private Camera playerCamera;

    void Start()
    {
        playerCamera = Camera.main;
        if (playerCamera == null)
        {
            playerCamera = FindObjectOfType<Camera>();
        }
    }

    void Update()
    {
        if (InfoPanelController.instance != null && InfoPanelController.instance.IsOpen)
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
                InfoPanelController.instance.ShowInfo(infoText);
            }
        }
    }
}
