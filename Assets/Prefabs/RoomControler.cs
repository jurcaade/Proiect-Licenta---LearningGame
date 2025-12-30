using UnityEngine;

public class RoomController : MonoBehaviour
{
    [Header("Referinte Usi")]
    public GameObject doorLeft;
    public GameObject doorRight;
    public float viteza = 2.0f;

    [Header("Next Room")]
    public GameObject nextRoomPrefab;
    public Transform spawnPoint;

    private bool usaDeschisa = false;
    private bool cameraSpawnata = false;
    private Vector3 pozInchisaL, pozDeschisaL, pozInchisaR, pozDeschisaR;

    void Start()
    {
        // Calculam pozitiile relative la aceasta camera specifica
        pozInchisaL = new Vector3(1.4f, doorLeft.transform.localPosition.y, doorLeft.transform.localPosition.z);
        pozDeschisaL = new Vector3(2.62f, doorLeft.transform.localPosition.y, doorLeft.transform.localPosition.z);
        pozInchisaR = new Vector3(-1.4f, doorRight.transform.localPosition.y, doorRight.transform.localPosition.z);
        pozDeschisaR = new Vector3(-2.62f, doorRight.transform.localPosition.y, doorRight.transform.localPosition.z);
    }

    void Update()
    {
        Vector3 targetL = usaDeschisa ? pozDeschisaL : pozInchisaL;
        Vector3 targetR = usaDeschisa ? pozDeschisaR : pozInchisaR;

        doorLeft.transform.localPosition = Vector3.MoveTowards(doorLeft.transform.localPosition, targetL, viteza * Time.deltaTime);
        doorRight.transform.localPosition = Vector3.MoveTowards(doorRight.transform.localPosition, targetR, viteza * Time.deltaTime);
    }

    public void Interactioneaza()
    {
        usaDeschisa = !usaDeschisa;
        Debug.Log("<color=green>Butonul camerei " + gameObject.name + " a fost activat!</color>");

        if (usaDeschisa && !cameraSpawnata)
        {
            SpawnNextRoom();
        }
    }

    void SpawnNextRoom()
    {
        if (nextRoomPrefab == null || spawnPoint == null) return;

        GameObject room = Instantiate(nextRoomPrefab, spawnPoint.position, spawnPoint.rotation);
        cameraSpawnata = true;

        // Stergem BackWall din camera noua
        Transform[] toate = room.GetComponentsInChildren<Transform>(true);
        foreach (Transform t in toate)
        {
            if (t.name == "BackWall") { Destroy(t.gameObject); break; }
        }
    }
}