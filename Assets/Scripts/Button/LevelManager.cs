using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;

    [Header("Prefab Room (structura)")]
    public GameObject roomPrefab;

    [Header("Prefabs nivele (doar obiecte)")]
    public GameObject[] levelPrefabs; // RoomLevel1, RoomLevel2, ...

    private int nivelCurent = 0;
    private GameObject roomCurenta;

    void Awake()
    {
        instance = this;
    }

    // Spawn room + obiecte pentru nivelul curent
    // makeButtonInteractable = true -> permite deschiderea ușii din room imediat (utile pentru room-ul de spawn)
    public void SpawnRoom(Transform spawnPoint, bool makeButtonInteractable = false)
    {
        // Daca am terminat nivelele si nu suntem in modul "activate button imediat", nu spawnam
        if (nivelCurent >= levelPrefabs.Length && !makeButtonInteractable)
        {
            Debug.Log("Toate nivelele au fost spawnate!");
            return;
        }

        // Instantiem structura camerei (room)
        roomCurenta = Instantiate(roomPrefab, spawnPoint.position, spawnPoint.rotation);

        // Stergem eventualul BackWall din structura room-ului
        Transform[] toateObiectele = roomCurenta.GetComponentsInChildren<Transform>(true);
        foreach (Transform t in toateObiectele)
        {
            if (t.name == "BackWall")
                Destroy(t.gameObject);
        }

        // Găsește componenta InteractButton din room (cea mai robustă metodă)
        InteractButton interactBtnComp = roomCurenta.GetComponentInChildren<InteractButton>(true);
        GameObject buttonGO = interactBtnComp != null ? interactBtnComp.gameObject : null;

        Debug.Log($"[LevelManager] makeButtonInteractable={makeButtonInteractable}, interactBtnComp={(interactBtnComp != null ? interactBtnComp.gameObject.name : "null")}");

        // Daca vrem buton activ imediat (ex: prima camera de spawn)
        if (makeButtonInteractable && interactBtnComp != null)
        {
            interactBtnComp.SetInteractable(true);
            Debug.Log("[LevelManager] Buton din room de spawn activat imediat.");
        }

        // Instantiem obiectele nivelului daca exista prefab
        GameObject nivelPrefab = null;
        if (nivelCurent < levelPrefabs.Length)
            nivelPrefab = levelPrefabs[nivelCurent];

        if (nivelPrefab != null)
        {
            GameObject nivelObiecte = Instantiate(nivelPrefab, roomCurenta.transform);
            nivelObiecte.transform.localPosition = Vector3.zero; // aliniere
            nivelObiecte.transform.localRotation = Quaternion.identity;

            // --- Legare explicită: dacă avem buton în room, dă-l la BitManager din nivel ---
            if (buttonGO != null)
            {
                BitManager bm = nivelObiecte.GetComponentInChildren<BitManager>(true);
                if (bm != null)
                {
                    bm.SetupInteractButton(buttonGO);
                    Debug.Log("[LevelManager] Button legat la BitManager.");
                }
                else
                {
                    Debug.LogWarning("[LevelManager] Nu s-a găsit BitManager în nivelul instanțiat pentru a-i atribui butonul!");
                }
            }
        }
        else
        {
            Debug.LogWarning("Prefab pentru nivelul " + (nivelCurent + 1) + " lipseste!");
        }

        nivelCurent++;
        Debug.Log("Camera spawnata pentru nivelul " + nivelCurent);
    }

    // Spawn prima camera la start (butonul din această cameră va fi activabil imediat)
    public void SpawnFirstRoom(Transform spawnPoint)
    {
        // true => butonul din camera de spawn poate fi folosit imediat
        SpawnRoom(spawnPoint, true);
    }
}