using TMPro;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;

    [Header("Game Info")]
    public string gameName = "Learning Game";

    [Header("Prefab Room (structura)")]
    public GameObject roomPrefab;

    [Header("Prefabs nivele (doar obiecte)")]
    public GameObject[] levelPrefabs;

    private int nivelCurent = 0;
    private GameObject roomCurenta;

    void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    public void SpawnRoom(Transform spawnPoint, bool isSpawnRoom = false)
    {
        // Verificăm dacă mai avem nivele
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
            if (interactBtnComp != null) interactBtnComp.SetInteractable(true);
        }
        else
        {
            if (interactBtnComp != null) interactBtnComp.SetInteractable(true);

            GameObject nivelPrefab = levelPrefabs[nivelCurent];
            if (nivelPrefab != null)
            {
                GameObject nivelObiecte = Instantiate(nivelPrefab, roomCurenta.transform);
                nivelObiecte.transform.localPosition = Vector3.zero;
                nivelObiecte.transform.localRotation = Quaternion.identity;

                // Legăm butonul de BitManager
                BitManager bm = nivelObiecte.GetComponentInChildren<BitManager>(true);
                if (bm != null && interactBtnComp != null)
                {
                    bm.SetupInteractButton(interactBtnComp.gameObject);
                }
            }

            // Incrementăm indexul nivelului abia acum
            nivelCurent++;
        }

        // 5. Actualizăm ecranul din noua cameră
        UpdateScreenText(roomCurenta, isSpawnRoom);

        Debug.Log(isSpawnRoom ? "Spawn Room creată." : "Nivelul " + nivelCurent + " pregătit.");
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
            if (isSpawnRoom)
            {
                textComp.text = gameName;
            }
            else
            {
                // nivelCurent a fost deja incrementat, deci va scrie corect "Level 1" la primul puzzle
                textComp.text = "LEVEL " + nivelCurent;
            }
        }
    }
}