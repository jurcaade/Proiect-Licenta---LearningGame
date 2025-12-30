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
    public void SpawnRoom(Transform spawnPoint)
    {
        if (nivelCurent >= levelPrefabs.Length)
        {
            Debug.Log("Toate nivelele au fost spawnate!");
            return;
        }

        roomCurenta = Instantiate(roomPrefab, spawnPoint.position, spawnPoint.rotation);

        // Sterge BackWall daca exista
        Transform[] toateObiectele = roomCurenta.GetComponentsInChildren<Transform>(true);
        foreach (Transform t in toateObiectele)
        {
            if (t.name == "BackWall")
                Destroy(t.gameObject);
        }

        GameObject nivelPrefab = levelPrefabs[nivelCurent];
        if (nivelPrefab != null)
        {
            GameObject nivelObiecte = Instantiate(nivelPrefab, roomCurenta.transform);
            nivelObiecte.transform.localPosition = Vector3.zero; // aliniere
            nivelObiecte.transform.localRotation = Quaternion.identity;
        }
        else
        {
            Debug.LogWarning("Prefab pentru nivelul " + (nivelCurent + 1) + " lipseste!");
        }

        nivelCurent++;
        Debug.Log("Camera spawnata pentru nivelul " + nivelCurent);
    }

    // Spawn prima camera la start
    public void SpawnFirstRoom(Transform spawnPoint)
    {
        SpawnRoom(spawnPoint);
    }
}
