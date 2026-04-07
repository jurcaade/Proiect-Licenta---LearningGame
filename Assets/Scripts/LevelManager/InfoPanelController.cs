using TMPro;
using UnityEngine;

public class InfoPanelController : MonoBehaviour
{
    public static InfoPanelController instance;

    [Header("UI")]
    public GameObject panelRoot;
    public TMP_Text contentText;

    [Header("Input")]
    public KeyCode closeKey = KeyCode.Escape;

    private bool isOpen = false;

    public bool IsOpen => isOpen;

    void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        if (panelRoot != null)
        {
            panelRoot.SetActive(false);
        }
    }

    void Update()
    {
        if (!isOpen)
        {
            return;
        }

        if (Input.GetKeyDown(closeKey) || Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
        {
            HideInfo();
        }
    }

    public void ShowInfo(string infoText)
    {
        if (panelRoot == null)
        {
            Debug.LogWarning("InfoPanelController: panelRoot nu este setat.");
            return;
        }

        if (contentText != null)
        {
            contentText.text = infoText;
        }

        panelRoot.SetActive(true);
        isOpen = true;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void HideInfo()
    {
        if (panelRoot != null)
        {
            panelRoot.SetActive(false);
        }

        isOpen = false;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}
