using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;

    [Header("Game Info")]
    public string gameName = "Learning Game"; // Numele pe care îl scrii în Inspector

    [Header("Prefab Room (structura)")]
    public GameObject roomPrefab;

    [Header("Prefabs nivele (doar obiecte)")]
    public GameObject[] levelPrefabs; // Index 0 = Obiecte Level 1, Index 1 = Level 2, etc.

    private int nivelCurent = 0; // Indexul pentru levelPrefabs
    private GameObject roomCurenta;

    void Awake()
    {
        instance = this;
    }

    // SpawnRoom este acum mai inteligent: știe dacă e camera de start sau un nivel de joc
    public void SpawnRoom(Transform spawnPoint, bool isSpawnRoom = false)
    {
        // 1. Verificăm dacă mai avem nivele (doar dacă nu e camera de start)
        if (!isSpawnRoom && nivelCurent >= levelPrefabs.Length)
        {
            Debug.Log("Toate nivelele au fost spawnate!");
            return;
        }

        // 2. Instanțiem structura de bază a camerei
        roomCurenta = Instantiate(roomPrefab, spawnPoint.position, spawnPoint.rotation);

        // 3. Curățare perete spate (logica ta existentă)
        Transform[] toateObiectele = roomCurenta.GetComponentsInChildren<Transform>(true);
        foreach (Transform t in toateObiectele)
        {
            if (t.name == "BackWall") Destroy(t.gameObject);
        }

        // 4. Gestionare Buton
        InteractButton interactBtnComp = roomCurenta.GetComponentInChildren<InteractButton>(true);
        GameObject buttonGO = interactBtnComp != null ? interactBtnComp.gameObject : null;

        // Dacă e camera de start, activăm butonul imediat să putem pleca
        if (isSpawnRoom && interactBtnComp != null)
        {
            interactBtnComp.SetInteractable(true);
        }

        // 5. Instanțiem puzzle-ul nivelului (doar dacă NU suntem în camera de spawn)
        if (!isSpawnRoom)
        {
            GameObject nivelPrefab = levelPrefabs[nivelCurent];
            if (nivelPrefab != null)
            {
                GameObject nivelObiecte = Instantiate(nivelPrefab, roomCurenta.transform);
                nivelObiecte.transform.localPosition = Vector3.zero;
                nivelObiecte.transform.localRotation = Quaternion.identity;

                // Legăm butonul din cameră la BitManager-ul din puzzle
                if (buttonGO != null)
                {
                    BitManager bm = nivelObiecte.GetComponentInChildren<BitManager>(true);
                    if (bm != null) bm.SetupInteractButton(buttonGO);
                }
            }
            // Incrementăm nivelul abia după ce am spawnat obiectele
            nivelCurent++;
        }

        // 6. Actualizăm textul de pe ecran pentru instanța CURENTĂ de cameră
        UpdateScreenText(roomCurenta, isSpawnRoom);

        Debug.Log(isSpawnRoom ? "Spawn Room creată." : "Nivelul " + nivelCurent + " spawnat.");
    }

    public void SpawnFirstRoom(Transform spawnPoint)
    {
        SpawnRoom(spawnPoint, true); // true indică faptul că e Lobby-ul
    }

    private void UpdateScreenText(GameObject cameraInstance, bool isSpawnRoom)
    {
        // Căutăm componenta TextMeshPro doar în interiorul noii camere create
        TextMeshPro textComp = cameraInstance.GetComponentInChildren<TextMeshPro>();

        if (textComp != null)
        {
            if (isSpawnRoom)
            {
                // În camera de start punem numele jocului
                textComp.text = gameName;
            }
            else
            {
                // În nivele punem numărul corect
                textComp.text = "LEVEL " + nivelCurent;
            }
        }
        else
        {
            Debug.LogWarning("Nu am găsit componenta Text pe ecranul camerei noi!");
        }
    }
}