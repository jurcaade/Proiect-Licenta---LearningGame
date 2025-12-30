using UnityEngine;

public class InteractButton : MonoBehaviour
{
    [Header("Referinte usi")]
    public GameObject doorLeft;
    public GameObject doorRight;
    public float viteza = 2f;

    [Header("Next Room Spawn")]
    public Transform nextRoomSpawn;

    private bool usaDeschisa = false;

    private Vector3 pozInchisaL = new Vector3(1.4f, 0, 0);
    private Vector3 pozDeschisaL = new Vector3(2.62f, 0, 0);
    private Vector3 pozInchisaR = new Vector3(-1.4f, 0, 0);
    private Vector3 pozDeschisaR = new Vector3(-2.62f, 0, 0);

    void Start()
    {
        // Reset usile la pozitia inchisa la spawn
        usaDeschisa = false;
        if (doorLeft != null)
            doorLeft.transform.localPosition = pozInchisaL;
        if (doorRight != null)
            doorRight.transform.localPosition = pozInchisaR;
    }

    void Update()
    {
        if (!usaDeschisa) return;

        if (doorLeft != null)
            doorLeft.transform.localPosition = Vector3.MoveTowards(doorLeft.transform.localPosition, pozDeschisaL, viteza * Time.deltaTime);

        if (doorRight != null)
            doorRight.transform.localPosition = Vector3.MoveTowards(doorRight.transform.localPosition, pozDeschisaR, viteza * Time.deltaTime);
    }

    void OnMouseDown()
    {
        if (usaDeschisa) return;

        // Deschide usa
        usaDeschisa = true;

        // Spawn urmatoarea camera
        if (LevelManager.instance != null && nextRoomSpawn != null)
        {
            LevelManager.instance.SpawnRoom(nextRoomSpawn.position, nextRoomSpawn.rotation);
        }
    }
}
