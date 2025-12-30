using UnityEngine;

public class InteractButton : MonoBehaviour
{
    [Header("Referinte usi")]
    public GameObject doorLeft;
    public GameObject doorRight;
    public float viteza = 2f;

    private bool usaDeschisa = false;

    private Vector3 pozInchisaL, pozDeschisaL;
    private Vector3 pozInchisaR, pozDeschisaR;

    void Start()
    {
        // Seteaza pozitiile initiale ale usilor
        pozInchisaL = doorLeft.transform.localPosition;
        pozInchisaR = doorRight.transform.localPosition;

        pozDeschisaL = pozInchisaL + new Vector3(1.22f, 0, 0);
        pozDeschisaR = pozInchisaR + new Vector3(-1.22f, 0, 0);
    }

    void Update()
    {
        if (!usaDeschisa) return;

        // Misca usile catre pozitia deschisa
        doorLeft.transform.localPosition =
            Vector3.MoveTowards(doorLeft.transform.localPosition, pozDeschisaL, viteza * Time.deltaTime);

        doorRight.transform.localPosition =
            Vector3.MoveTowards(doorRight.transform.localPosition, pozDeschisaR, viteza * Time.deltaTime);
    }

    void OnMouseDown()
    {
        if (usaDeschisa) return;

        // Deschide usa
        usaDeschisa = true;

        // Spawneaza urmatoarea camera
        if (LevelManager.instance != null)
            LevelManager.instance.SpawnRoom();
    }
}
