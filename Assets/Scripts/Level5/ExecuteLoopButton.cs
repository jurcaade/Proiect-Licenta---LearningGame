using UnityEngine;

public class ExecuteLoopButton : MonoBehaviour
{
    [Header("Referinta manager")]
    public LoopPuzzleManager manager;

    [Header("Audio")]
    public AudioClip pressClip;
    [Range(0f, 1f)] public float pressVolume = 0.8f;

    private Camera jucatorCamera;
    private AudioSource audioSource;

    private void Start()
    {
        jucatorCamera = Camera.main;
        if (jucatorCamera == null)
        {
            jucatorCamera = FindObjectOfType<Camera>();
        }

        if (manager == null)
        {
            manager = FindObjectOfType<LoopPuzzleManager>();
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
        if (!Input.GetMouseButtonDown(0) || jucatorCamera == null)
        {
            return;
        }

        Ray raza = jucatorCamera.ScreenPointToRay(new Vector3(Screen.width / 2f, Screen.height / 2f, 0f));
        if (!Physics.Raycast(raza, out RaycastHit hit, 4f))
        {
            return;
        }

        if (hit.collider.gameObject != gameObject || manager == null)
        {
            return;
        }

        if (pressClip != null)
        {
            audioSource.PlayOneShot(pressClip, pressVolume);
        }

        manager.ApasaExecute();
    }
}
