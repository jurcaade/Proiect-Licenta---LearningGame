using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;

    [Header("Spawn camere")]
    public GameObject roomPrefab;      // Prefab-ul camerei (Room)
    public Transform spawnPoint;       // Locul unde apare camera

    [Header("Setari nivel")]
    public int nivelCurent = 0;

    private GameObject roomCurenta;

    void Awake()
    {
        // Singleton simplu
        instance = this;
    }

    void Start()
    {
        SpawnRoom();
    }

    // Spawn camera noua
    public void SpawnRoom()
    {
        // Sterge camera veche daca exista
        if (roomCurenta != null)
            Destroy(roomCurenta);

        // Creeaza camera noua
        roomCurenta = Instantiate(roomPrefab, spawnPoint.position, spawnPoint.rotation);

        // Sterge BackWall pentru a permite intrarea
        Transform[] toateObiectele = roomCurenta.GetComponentsInChildren<Transform>(true);
        foreach (Transform t in toateObiectele)
        {
            if (t.name == "BackWall")
            {
                Destroy(t.gameObject);
                break;
            }
        }

        nivelCurent++;
        Debug.Log("Camera " + nivelCurent + " spawnata.");
    }
}
