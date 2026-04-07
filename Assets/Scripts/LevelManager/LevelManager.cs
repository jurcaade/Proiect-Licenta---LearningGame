using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;

    [Header("Game Info")]
    public string gameName = "Learning Game";

    [Header("Prefab Room (structura)")]
    public GameObject roomPrefab;

    [Header("Prefabs nivele (doar obiecte)")]
    public GameObject[] levelPrefabs;

    [Header("UI Feedback Final Nivel")]
    public GameObject levelCompletePanel;
    public TMP_Text levelCompleteText;

    [Header("UI Ecran Final (Sfarsit Joc)")]
    public GameObject finalPanel;

    [Header("Audio Final Joc")]
    public AudioClip finalCompleteClip;
    [Range(0f, 1f)]
    public float finalCompleteVolume = 1f;
    public AudioClip finalButtonClip;
    [Range(0f, 1f)]
    public float finalButtonVolume = 0.9f;
    [Range(0f, 0.5f)]
    public float finalButtonDelay = 0.12f;

    private int nivelCurent = 0;
    private GameObject roomCurenta;
    private AudioSource audioSource;
    private bool finalActionInProgress = false;

    void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
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

        if (finalPanel != null)
        {
            finalPanel.SetActive(false);
        }
    }

    public void ShowLevelCompleteMessage()
    {
        StopAllCoroutines();

        string mesaj = "<b>NIVELUL " + nivelCurent + " COMPLETAT!</b>\n<size=50%><color=#00FFFF>Apasa butonul verde pentru a deschide usa.</size></color>";
        StartCoroutine(WinScreenRoutine(mesaj));
    }

    public bool IsGameComplete()
    {
        return nivelCurent >= levelPrefabs.Length;
    }

    public void ShowFinalGameScreen()
    {
        if (finalPanel != null)
        {
            finalPanel.SetActive(true);
            PlayFinalCompleteSound();

            if (levelCompletePanel != null)
            {
                levelCompletePanel.SetActive(false);
            }

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            finalActionInProgress = false;
        }
        else
        {
            Debug.LogError("Final Panel nu este asignat in Inspector!");
        }
    }

    private void PlayFinalCompleteSound()
    {
        if (audioSource != null && finalCompleteClip != null)
        {
            audioSource.PlayOneShot(finalCompleteClip, finalCompleteVolume);
        }
    }

    private void PlayFinalButtonSound()
    {
        if (audioSource != null && finalButtonClip != null)
        {
            audioSource.PlayOneShot(finalButtonClip, finalButtonVolume);
        }
    }

    private IEnumerator WinScreenRoutine(string mesaj)
    {
        if (levelCompletePanel != null && levelCompleteText != null)
        {
            levelCompleteText.text = mesaj;
            levelCompletePanel.SetActive(true);
            yield return new WaitForSeconds(3.5f);
            levelCompletePanel.SetActive(false);
        }
    }

    public void RestartGame()
    {
        if (finalActionInProgress)
        {
            return;
        }

        StartCoroutine(RestartAfterButtonSound());
    }

    public void QuitGame()
    {
        if (finalActionInProgress)
        {
            return;
        }

        StartCoroutine(QuitAfterButtonSound());
    }

    private IEnumerator RestartAfterButtonSound()
    {
        finalActionInProgress = true;
        PlayFinalButtonSound();

        if (finalButtonClip != null)
        {
            yield return new WaitForSecondsRealtime(finalButtonDelay);
        }

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private IEnumerator QuitAfterButtonSound()
    {
        finalActionInProgress = true;
        PlayFinalButtonSound();

        if (finalButtonClip != null)
        {
            yield return new WaitForSecondsRealtime(finalButtonDelay);
        }

        Application.Quit();
        Debug.Log("Jocul s-a inchis.");

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

    public void SpawnRoom(Transform spawnPoint, bool isSpawnRoom = false)
    {
        if (!isSpawnRoom && nivelCurent >= levelPrefabs.Length)
        {
            Debug.Log("Toate nivelele au fost finalizate!");
            return;
        }

        roomCurenta = Instantiate(roomPrefab, spawnPoint.position, spawnPoint.rotation);

        Transform[] toateObiectele = roomCurenta.GetComponentsInChildren<Transform>(true);
        foreach (Transform t in toateObiectele)
        {
            if (t.name == "BackWall")
            {
                Destroy(t.gameObject);
            }
        }

        InteractButton interactBtnComp = roomCurenta.GetComponentInChildren<InteractButton>(true);

        if (isSpawnRoom)
        {
            if (interactBtnComp != null)
            {
                interactBtnComp.SetInteractable(false);
            }
        }
        else
        {
            if (interactBtnComp != null)
            {
                interactBtnComp.SetInteractable(false);
            }

            GameObject nivelPrefab = levelPrefabs[nivelCurent];
            if (nivelPrefab != null)
            {
                GameObject nivelObiecte = Instantiate(nivelPrefab, roomCurenta.transform);
                nivelObiecte.transform.localPosition = Vector3.zero;
                nivelObiecte.transform.localRotation = Quaternion.identity;

                BitManager bm = nivelObiecte.GetComponentInChildren<BitManager>(true);
                if (bm != null && interactBtnComp != null) bm.SetupInteractButton(interactBtnComp.gameObject);

                Level2Manager l2 = nivelObiecte.GetComponentInChildren<Level2Manager>(true);
                if (l2 != null && interactBtnComp != null) l2.SetupInteractButton(interactBtnComp.gameObject);

                StackBase sb = nivelObiecte.GetComponentInChildren<StackBase>(true);
                if (sb != null && interactBtnComp != null) sb.SetupInteractButton(interactBtnComp.gameObject);

                SortingManager sm = nivelObiecte.GetComponentInChildren<SortingManager>(true);
                if (sm != null && interactBtnComp != null) sm.SetupInteractButton(interactBtnComp.gameObject);

                ForLoopManager lm = nivelObiecte.GetComponentInChildren<ForLoopManager>(true);
                if (lm != null && interactBtnComp != null) lm.SetupInteractButton(interactBtnComp.gameObject);
            }

            nivelCurent++;
        }

        UpdateScreenText(roomCurenta, isSpawnRoom);
    }

    public void SpawnFirstRoom(Transform spawnPoint)
    {
        SpawnRoom(spawnPoint, true);
    }

    private void UpdateScreenText(GameObject cameraInstance, bool isSpawnRoom)
    {
        TextMeshPro textComp = cameraInstance.GetComponentInChildren<TextMeshPro>();
        if (textComp == null) return;

        if (isSpawnRoom)
        {
            textComp.text = $"<size=120%>{gameName}</size>\n<size=60%><color=#A0A0A0>Sistem de testare activat</color></size>";
        }
        else
        {
            string descriere = "";

            switch (nivelCurent)
            {
                case 1:
                    descriere = "Conversie Binar - Zecimal";
                    break;
                case 2:
                    descriere = "Circuite si Logica";
                    break;
                case 3:
                    descriere = "Structuri de Date (Stiva)";
                    break;
                case 4:
                    descriere = "Operatori Logici (IF / OR)";
                    break;
                case 5:
                    descriere = "Structuri Repetitive (FOR)";
                    break;
                default:
                    descriere = "Testare in curs...";
                    break;
            }

            textComp.text = $"LEVEL <color=#00FFFF>{nivelCurent}</color>\n" +
                            $"<size=50%><color=#A0A0A0>{descriere}</color></size>";
        }
    }
}
