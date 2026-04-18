using TMPro;
using UnityEngine;

public class InfoPanelManager : MonoBehaviour
{
    public static InfoPanelManager instance;

    [Header("UI")]
    public GameObject panelRoot;
    public TMP_Text contentText;

    [Header("Input")]
    public KeyCode closeKey = KeyCode.Escape;
    [Min(0f)]
    public float reopenDelay = 0.15f;

    private bool isOpen;
    private float blockedUntilTime;

    public bool IsOpen => isOpen;
    public bool CanOpenInfo => Time.unscaledTime >= blockedUntilTime;

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        if (panelRoot != null)
        {
            panelRoot.SetActive(false);
        }
    }

    private void Update()
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
        if (!CanOpenInfo)
        {
            return;
        }

        if (panelRoot == null)
        {
            Debug.LogWarning("InfoPanelManager: panelRoot nu este setat.");
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
        blockedUntilTime = Time.unscaledTime + reopenDelay;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}
