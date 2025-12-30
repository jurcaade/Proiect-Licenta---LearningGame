using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;

    [Header("Prefab Camera")]
    public GameObject roomPrefab;  // Prefab-ul camerei

    private GameObject roomCurenta;
    private int nivelCurent = 0;

    void Awake()
    {
        instance = this;
    }

    // Spawn camera noua la pozitia si rotatia data
    public void SpawnRoom(Vector3 pozitie, Quaternion rotatie)
    {
        // Nu mai distruge camera veche
        // if (roomCurenta != null)
        //     Destroy(roomCurenta);

        // Creeaza camera noua
        GameObject cameraNoua = Instantiate(roomPrefab, pozitie, rotatie);

        // Sterge BackWall din camera noua
        Transform[] toateObiectele = cameraNoua.GetComponentsInChildren<Transform>(true);
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

    // Optional: spawn initial la start
    public void SpawnFirstRoom(Transform spawnPoint)
    {
        SpawnRoom(spawnPoint.position, spawnPoint.rotation);
    }
}
