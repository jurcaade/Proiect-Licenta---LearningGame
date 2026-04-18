using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;

    [Header("Game Info")]
    public string gameName = "CodeScape 3D";

    [Header("Prefab Room")]
    public GameObject roomPrefab;

    [Header("Prefabs nivele")]
    public GameObject[] levelPrefabs;

    [Header("UI final nivel")]
    public GameObject levelCompletePanel;
    public TMP_Text levelCompleteText;

    [Header("UI final joc")]
    public GameObject finalPanel;

    [Header("Audio final joc")]
    public AudioClip finalCompleteClip;
    [Range(0f, 1f)]
    public float finalCompleteVolume = 1f;

    private int nivelCurent;
    private AudioSource audioSource;

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    private void Start()
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
        if (finalPanel == null)
        {
            Debug.LogError("Final Panel nu este asignat in Inspector!");
            return;
        }

        finalPanel.SetActive(true);
        PlayFinalCompleteSound();

        if (levelCompletePanel != null)
        {
            levelCompletePanel.SetActive(false);
        }

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void QuitGame()
    {
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

        GameObject roomCurenta = Instantiate(roomPrefab, spawnPoint.position, spawnPoint.rotation);
        RemoveBackWall(roomCurenta);

        LevelDoorButton levelButton = roomCurenta.GetComponentInChildren<LevelDoorButton>(true);
        if (levelButton != null) levelButton.SetInteractable(false);

        if (!isSpawnRoom)
        {
            SpawnLevelObjects(roomCurenta, levelButton);
            nivelCurent++;
        }

        UpdateScreenText(roomCurenta, isSpawnRoom);
    }

    public void SpawnFirstRoom(Transform spawnPoint)
    {
        SpawnRoom(spawnPoint, true);
    }

    private void PlayFinalCompleteSound()
    {
        if (audioSource != null && finalCompleteClip != null)
        {
            audioSource.PlayOneShot(finalCompleteClip, finalCompleteVolume);
        }
    }

    private IEnumerator WinScreenRoutine(string mesaj)
    {
        if (levelCompletePanel == null || levelCompleteText == null)
        {
            yield break;
        }

        levelCompleteText.text = mesaj;
        levelCompletePanel.SetActive(true);
        yield return new WaitForSeconds(3.5f);
        levelCompletePanel.SetActive(false);
    }

    private void RemoveBackWall(GameObject roomInstance)
    {
        Transform[] toateObiectele = roomInstance.GetComponentsInChildren<Transform>(true);
        foreach (Transform obiect in toateObiectele)
        {
            if (obiect.name == "BackWall")
            {
                Destroy(obiect.gameObject);
            }
        }
    }

    private void SpawnLevelObjects(GameObject roomCurenta, LevelDoorButton levelButton)
    {
        GameObject nivelPrefab = levelPrefabs[nivelCurent];
        if (nivelPrefab == null) return;

        GameObject nivelObiecte = Instantiate(nivelPrefab, roomCurenta.transform);
        nivelObiecte.transform.localPosition = Vector3.zero;
        nivelObiecte.transform.localRotation = Quaternion.identity;

        if (levelButton == null) return;

        GameObject buttonObject = levelButton.gameObject;

        BitManager bitManager = nivelObiecte.GetComponentInChildren<BitManager>(true);
        if (bitManager != null) bitManager.SetupInteractButton(buttonObject);

        ReactorPuzzleManager reactorManager = nivelObiecte.GetComponentInChildren<ReactorPuzzleManager>(true);
        if (reactorManager != null) reactorManager.SetupInteractButton(buttonObject);

        StackPuzzleManager stackManager = nivelObiecte.GetComponentInChildren<StackPuzzleManager>(true);
        if (stackManager != null) stackManager.SetupInteractButton(buttonObject);

        IfPuzzleManager ifManager = nivelObiecte.GetComponentInChildren<IfPuzzleManager>(true);
        if (ifManager != null) ifManager.SetupInteractButton(buttonObject);

        LoopPuzzleManager loopManager = nivelObiecte.GetComponentInChildren<LoopPuzzleManager>(true);
        if (loopManager != null) loopManager.SetupInteractButton(buttonObject);
    }

    private void UpdateScreenText(GameObject roomInstance, bool isSpawnRoom)
    {
        TextMeshPro textComp = roomInstance.GetComponentInChildren<TextMeshPro>();
        if (textComp == null)
        {
            return;
        }

        if (isSpawnRoom)
        {
            textComp.text = $"<size=120%>{gameName}</size>\n<size=60%><color=#A0A0A0>Sistem de testare activat</color></size>";
            return;
        }

        textComp.text = $"LEVEL <color=#00FFFF>{nivelCurent}</color>\n<size=50%><color=#A0A0A0>{GetLevelDescription()}</color></size>";
    }

    private string GetLevelDescription()
    {
        switch (nivelCurent)
        {
            case 1:
                return "Conversie Binar - Zecimal";
            case 2:
                return "Circuite si Logica";
            case 3:
                return "Structuri de Date (Stiva)";
            case 4:
                return "Operatori Logici (IF / OR)";
            case 5:
                return "Structuri Repetitive (FOR)";
            default:
                return "";
        }
    }
}
