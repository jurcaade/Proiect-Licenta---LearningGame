using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement; // Necesar pentru Restart
using System.Collections;

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
    public GameObject finalPanel; // Panoul cu Quit și Restart

    private int nivelCurent = 0;
    private GameObject roomCurenta;

    void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        // Ne asigurăm că panoul final este închis la început
        if (finalPanel != null) finalPanel.SetActive(false);
    }

    // --- FUNCȚIA MODIFICATĂ ---
    public void ShowLevelCompleteMessage()
    {
        StopAllCoroutines();

        // Așteptăm ca jucătorul să apese butonul fizic!
        string mesaj;
      
            mesaj = "<b>NIVELUL " + nivelCurent + " COMPLETAT!</b>\n<size=50%><color=#00FFFF>Apasă butonul verde pentru a deschide ușa.</size></color>";

        StartCoroutine(WinScreenRoutine(mesaj));
    }

    // --- Funcție nouă pentru a verifica dacă suntem la final ---
    public bool IsGameComplete()
    {
        return nivelCurent >= levelPrefabs.Length;
    }

    // --- Făcută PUBLICĂ pentru a fi apelată de InteractButton ---
    public void ShowFinalGameScreen()
    {
        if (finalPanel != null)
        {
            finalPanel.SetActive(true);

            // Dezactivăm panoul temporar dacă era activ
            if (levelCompletePanel != null) levelCompletePanel.SetActive(false);

            // Activăm cursorul pentru ca jucătorul să poată da click pe butoane
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            Debug.LogError("Final Panel nu este asignat în Inspector!");
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

    // --- METODE PENTRU BUTOANELE DE PE FINAL PANEL ---
    public void RestartGame()
    {
        // Reîncarcă scena curentă
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void QuitGame()
    {
        // Închide aplicația
        Application.Quit();
        Debug.Log("Jocul s-a închis.");

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
            if (t.name == "BackWall") Destroy(t.gameObject);
        }

        InteractButton interactBtnComp = roomCurenta.GetComponentInChildren<InteractButton>(true);

        if (isSpawnRoom)
        {
            if (interactBtnComp != null) interactBtnComp.SetInteractable(false);
        }
        else
        {
            if (interactBtnComp != null) interactBtnComp.SetInteractable(false);

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
                if ((lm != null) && interactBtnComp != null) lm.SetupInteractButton(interactBtnComp.gameObject);
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

            // Folosim switch pentru a alege descrierea în funcție de nivel
            switch (nivelCurent)
            {
                case 1:
                    descriere = "Conversie Binar - Zecimal";
                    break;
                case 2:
                    descriere = "Circuite și Logică";
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
                    descriere = "Testare în curs...";
                    break;
            }

            // Formatăm textul: Titlu mare și Subtitlu mic colorat gri
            textComp.text = $"LEVEL <color=#00FFFF>{nivelCurent}</color>\n" +
                            $"<size=50%><color=#A0A0A0>{descriere}</color></size>";
        }
    }
}
