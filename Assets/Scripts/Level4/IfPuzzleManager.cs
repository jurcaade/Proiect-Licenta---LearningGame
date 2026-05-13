using TMPro;
using UnityEngine;

public class IfPuzzleManager : MonoBehaviour
{
    [Header("Setari")]
    public GameObject spherePrefab;
    public Transform spawnPoint;
    public Transform decisionPoint;
    public TextMeshPro statusText;
    public float wrongMessageDuration = 2f;

    [Header("Audio")]
    public AudioClip wrongClip;
    [Range(0f, 1f)] public float wrongVolume = 0.8f;

    private readonly Color colorPink = new Color(1f, 0.4f, 0.7f);
    private readonly Color colorOrange = new Color(1f, 0.5f, 0f);
    private const string WrongMessage = "GRESIT! Uita-te atent la conditia IF";

    private GameObject currentSphere;
    private Color currentColor;
    private Color[] culoriPosibile;
    private int score;
    private bool isMoving;
    private bool levelCompleted;
    private LevelDoorButton levelButton;
    private AudioSource audioSource;
    private PlayerGrab playerGrab;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        audioSource.playOnAwake = false;
        audioSource.spatialBlend = 0f;
        audioSource.dopplerLevel = 0f;
        playerGrab = FindObjectOfType<PlayerGrab>();

        culoriPosibile = new Color[]
        {
            Color.blue,
            colorPink,
            colorOrange,
            Color.yellow
        };

        SpawnNewSphere();
    }

    private void Update()
    {
        if (!isMoving || currentSphere == null)
        {
            return;
        }

        currentSphere.transform.position = Vector3.MoveTowards(
            currentSphere.transform.position,
            decisionPoint.position,
            3f * Time.deltaTime);

        if (Vector3.Distance(currentSphere.transform.position, decisionPoint.position) < 0.01f)
        {
            isMoving = false;
        }
    }

    public void SetupInteractButton(GameObject buttonObj)
    {
        if (buttonObj != null)
        {
            levelButton = buttonObj.GetComponent<LevelDoorButton>();
        }
    }

    public void OnPlayerDecision(bool pressedTrue)
    {
        if (levelCompleted || currentSphere == null)
        {
            return;
        }

        bool raspunsCorect = currentColor == Color.blue || currentColor == colorPink;

        if (pressedTrue == raspunsCorect)
        {
            score++;
        }
        else
        {
            score = 0;
            PlayClip(wrongClip, wrongVolume);
            ShowWrongMessage();
        }

        if (statusText != null)
        {
            statusText.text = "SCOR: " + score + "/4";
        }

        if (score >= 4)
        {
            FinishLevel();
            return;
        }

        Destroy(currentSphere);
        SpawnNewSphere();
    }

    private void ShowWrongMessage()
    {
        if (playerGrab == null)
        {
            playerGrab = FindObjectOfType<PlayerGrab>();
        }

        if (playerGrab != null)
        {
            playerGrab.ShowWarningMessage(WrongMessage, wrongMessageDuration);
        }
    }

    private void SpawnNewSphere()
    {
        if (levelCompleted)
        {
            return;
        }

        currentSphere = Instantiate(spherePrefab, spawnPoint.position, Quaternion.identity);
        currentColor = culoriPosibile[Random.Range(0, culoriPosibile.Length)];

        Renderer sphereRenderer = currentSphere.GetComponent<Renderer>();
        if (sphereRenderer != null)
        {
            sphereRenderer.material.color = currentColor;
        }

        isMoving = true;
    }

    private void FinishLevel()
    {
        levelCompleted = true;
        isMoving = false;

        if (statusText != null)
        {
            statusText.text = "ACCES PERMIS";
        }

        Destroy(currentSphere);
        currentSphere = null;

        if (levelButton != null)
        {
            levelButton.SetInteractable(true);
        }

        LevelManager.instance.ShowLevelCompleteMessage();
    }

    private void PlayClip(AudioClip clip, float volume)
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
