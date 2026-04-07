using UnityEngine;

public class ExecuteButton : MonoBehaviour
{
    [Header("Referinta Manager")]
    public ForLoopManager manager; // Trage aici obiectul cu scriptul ForLoopManager

    [Header("Audio")]
    public AudioClip pressClip;
    [Range(0f, 1f)]
    public float pressVolume = 0.8f;

    private Camera jucatorCamera;
    private AudioSource audioSource;

    void Start()
    {
        // Găsim camera automat (ca în DecisionButton)
        jucatorCamera = Camera.main;
        if (jucatorCamera == null)
        {
            jucatorCamera = FindObjectOfType<Camera>();
        }

        if (manager == null)
        {
            // Încearcă să găsească managerul automat dacă ai uitat să-l tragi în slot
            manager = FindObjectOfType<ForLoopManager>();
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
        // Verificăm dacă a dat click stânga sau a apăsat E
        if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.E))
        {
            if (jucatorCamera != null)
            {
                // Tragem raza fix din centrul ecranului (unde e crosshair-ul)
                Ray raza = jucatorCamera.ScreenPointToRay(new Vector3(Screen.width / 2f, Screen.height / 2f, 0f));
                RaycastHit hit;

                // Dacă raza lovește acest buton la o distanță de maxim 4 metri
                if (Physics.Raycast(raza, out hit, 4f))
                {
                    if (hit.collider.gameObject == this.gameObject)
                    {
                        if (manager != null)
                        {
                            if (pressClip != null)
                            {
                                audioSource.PlayOneShot(pressClip, pressVolume);
                            }

                            Debug.Log("Butonul EXECUTE a fost activat prin Raycast!");
                            manager.ApasaExecute();
                        }
                    }
                }
            }
        }
    }
}
