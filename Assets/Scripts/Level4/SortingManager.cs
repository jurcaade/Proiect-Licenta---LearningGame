using TMPro;
using UnityEngine;

public class SortingManager : MonoBehaviour
{
    [Header("Setari")]
    public GameObject spherePrefab; // Trage UN SINGUR prefab aici (ex: Sphere_32_extended)
    public Transform spawnPoint;    // Punctul de unde pornește sfera
    public Transform decisionPoint; // Punctul unde se oprește sfera
    public TextMeshPro statusText;  // Textul de pe monitor (Scor: 0/4)

    [Header("Audio")]
    public AudioClip wrongClip;
    [Range(0f, 1f)] public float wrongVolume = 0.8f;

    private GameObject currentSphere;
    private Color currentColor;
    private int score = 0;
    private bool isMoving = false;
    private bool levelCompleted = false;

    private InteractButton levelButton;
    private AudioSource audioSource;

    // DEFINIM CULORILE PERSONALIZATE AICI:
    // (Valorile din paranteză reprezintă cantitatea de Red, Green, Blue pe o scară de la 0 la 1)
    private Color colorPink = new Color(1f, 0.4f, 0.7f);   // Roz personalizat
    private Color colorOrange = new Color(1f, 0.5f, 0f);

    private Color[] culoriPosibile;

    public void SetupInteractButton(GameObject buttonObj)
    {
        if (buttonObj != null)
        {
            levelButton = buttonObj.GetComponent<InteractButton>();
        }
    }

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        audioSource.playOnAwake = false;
        audioSource.spatialBlend = 0f;
        audioSource.dopplerLevel = 0f;

        // Inițializăm paleta de culori pe care le va alege codul
        culoriPosibile = new Color[]
        {
            Color.blue,
            colorPink,
            colorOrange,
            Color.yellow
        };

        SpawnNewSphere();
    }

    void Update()
    {
        // Mută sfera pe bandă până ajunge la jucător
        if (isMoving && currentSphere != null)
        {
            currentSphere.transform.position = Vector3.MoveTowards(
                currentSphere.transform.position,
                decisionPoint.position,
                3f * Time.deltaTime // Viteza cu care vine sfera
            );

            if (Vector3.Distance(currentSphere.transform.position, decisionPoint.position) < 0.01f)
            {
                isMoving = false; // S-a oprit și așteaptă decizia jucătorului
            }
        }
    }

    void SpawnNewSphere()
    {
        if (levelCompleted)
        {
            return;
        }

        currentSphere = Instantiate(spherePrefab, spawnPoint.position, Quaternion.identity);

        // Alegem o culoare la întâmplare din lista noastră nouă
        int index = Random.Range(0, culoriPosibile.Length);
        currentColor = culoriPosibile[index];

        // Schimbăm culoarea materialului sferei
        Renderer sphereRenderer = currentSphere.GetComponent<Renderer>();
        if (sphereRenderer != null)
        {
            sphereRenderer.material.color = currentColor;
        }

        isMoving = true;
    }

    // Funcție apelată dacă s-a apăsat TRUE
    public void ApasaTrue()
    {
        OnPlayerDecision(true);
    }

    // Funcție apelată dacă s-a apăsat FALSE
    public void ApasaFalse()
    {
        OnPlayerDecision(false);
    }

    // Logica de decizie
    public void OnPlayerDecision(bool pressedTrue)
    {
        if (levelCompleted)
        {
            return;
        }

        Debug.Log("[TEST] Managerul a primit comanda! isMoving este: " + isMoving);

        if (currentSphere == null)
        {
            Debug.LogWarning("[TEST] Nu exista nicio sfera activa.");
            return;
        }

        // Condiția jocului ACUM: Dacă e Albastru SAU Roz, răspunsul corect e TRUE. (Mov și Galben sunt FALSE).
        bool isBlueOrPink = (currentColor == Color.blue || currentColor == colorPink);

        // Verificăm dacă alegerea jucătorului se potrivește cu adevărul logic
        if (pressedTrue == isBlueOrPink)
        {
            score++;
            Debug.Log("Corect! Ai ajuns la scorul: " + score);
        }
        else
        {
            score = 0; // Greșeală -> o ia de la capăt
            PlayClip(wrongClip, wrongVolume);
            Debug.Log("Greșit! Se resetează secvența de la 0.");
        }

        // Actualizăm ecranul din joc
        if (statusText != null) statusText.text = "SCOR: " + score + "/4";

        // Verificăm dacă a finalizat nivelul
        if (score >= 4)
        {
            levelCompleted = true;
            if (statusText != null) statusText.text = "ACCES PERMIS";
            Debug.Log("NIVEL FINALIZAT! Ai sortat 4 la rând corect.");
            Destroy(currentSphere);
            currentSphere = null;
            isMoving = false;

            if (levelButton != null)
            {
                levelButton.SetInteractable(true);
            }
            LevelManager.instance.ShowLevelCompleteMessage();
        }
        else
        {
            // Dacă nu a terminat încă, vine următoarea sferă
            Destroy(currentSphere);
            SpawnNewSphere();
        }

    }

    void PlayClip(AudioClip clip, float volume)
    {
        if (audioSource == null || clip == null)
        {
            return;
        }

        audioSource.Stop();
        audioSource.clip = clip;
        audioSource.volume = volume;
        audioSource.Play();
    }

}
