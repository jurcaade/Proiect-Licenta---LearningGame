using UnityEngine;

public class ChoiceButton : MonoBehaviour
{
    [Header("Setare buton")]
    public bool esteButonTrue;

    [Header("Audio")]
    public AudioClip pressClip;
    [Range(0f, 1f)] public float pressVolume = 0.8f;

    private IfPuzzleManager managerNivel;
    private Camera jucatorCamera;
    private AudioSource audioSource;

    private void Start()
    {
        managerNivel = FindObjectOfType<IfPuzzleManager>();

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        audioSource.playOnAwake = false;
        audioSource.spatialBlend = 1f;
        audioSource.dopplerLevel = 0f;
        audioSource.minDistance = 1f;
        audioSource.maxDistance = 10f;

        jucatorCamera = Camera.main;
        if (jucatorCamera == null)
        {
            jucatorCamera = FindObjectOfType<Camera>();
        }
    }

    private void Update()
    {
        if (!Input.GetMouseButtonDown(0) || jucatorCamera == null)
        {
            return;
        }

        Ray raza = jucatorCamera.ScreenPointToRay(new Vector3(Screen.width / 2f, Screen.height / 2f, 0f));
        if (!Physics.Raycast(raza, out RaycastHit hit, 5f))
        {
            return;
        }

        if (hit.collider.gameObject != gameObject)
        {
            return;
        }

        PlayPressSound();

        if (managerNivel != null)
        {
            managerNivel.OnPlayerDecision(esteButonTrue);
        }
    }

    private void PlayPressSound()
    {
        if (audioSource == null || pressClip == null)
        {
            return;
        }

        audioSource.Stop();
        audioSource.clip = pressClip;
        audioSource.volume = pressVolume;
        audioSource.Play();
    }
}
