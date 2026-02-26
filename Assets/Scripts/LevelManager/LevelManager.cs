using TMPro;
using UnityEngine;
using System.Collections; // Necesar pentru timpul de așteptare (Coroutine)

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
    public GameObject levelCompletePanel; // Panoul negru (Vigneta)
    public TMP_Text levelCompleteText;    // Textul de pe panou

    private int nivelCurent = 0;
    private GameObject roomCurenta;

    void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    // --- FUNCȚIA NOUĂ CARE AFIȘEAZĂ MESAJUL DINAMIC ---
    public void ShowLevelCompleteMessage()
    {
        // Folosim tag-ul <size=50%> pentru a face a doua propozitie de doua ori mai mica
        string mesaj = "NIVELUL " + nivelCurent + " COMPLETAT!\n<size=50%>Apasă butonul verde pentru a deschide ușa.</size>";

        StopAllCoroutines();

        // Pornim afisarea
        StartCoroutine(WinScreenRoutine(mesaj));
    }

    private IEnumerator WinScreenRoutine(string mesaj)
    {
        if (levelCompletePanel != null && levelCompleteText != null)
        {
            levelCompleteText.text = mesaj;

            // Afișăm ecranul întunecat
            levelCompletePanel.SetActive(true);

            // Îl lăsăm pe ecran 3.5 secunde ca să apuce jucătorul să citească
            yield return new WaitForSeconds(3.5f);

            // Ascundem la loc ecranul
            levelCompletePanel.SetActive(false);
        }
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

                SortingManager sm =nivelObiecte.GetComponentInChildren<SortingManager>(true);
                if(sm != null && interactBtnComp != null) sm.SetupInteractButton(interactBtnComp.gameObject);
            }

            nivelCurent++; // Incrementăm nivelul abia aici
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
        if (textComp != null)
        {
            if (isSpawnRoom) textComp.text = gameName;
            else textComp.text = "LEVEL " + nivelCurent;
        }
    }
}