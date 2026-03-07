using UnityEngine;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class ForLoopManager : MonoBehaviour
{
    [Header("Setari Bucla")]
    public int limitaBucla = 4;
    private int i = 0;

    [Header("Referinte UI (Ecrane 3D)")]
    public TextMeshPro textStatus;

    [Header("Referinta UI (Canvas/Acelasi ca la Stiva)")]
    public TMP_Text warningText; // Trage aici obiectul de text din Canvas (UI)

    [Header("Resetare Obiecte")]
    private List<GameObject> pacheteInScena = new List<GameObject>();
    private List<Vector3> pozitiiInitiale = new List<Vector3>();
    private List<Quaternion> rotatiiInitiale = new List<Quaternion>();

    private InteractButton levelButton;

    public void SetupInteractButton(GameObject buttonObj)
    {
        if (buttonObj != null)
        {
            levelButton = buttonObj.GetComponent<InteractButton>();
        }
    }

    void Start()
    {
        GameObject[] pachete = GameObject.FindGameObjectsWithTag("DataPacket");

        foreach (GameObject p in pachete)
        {
            pacheteInScena.Add(p);
            pozitiiInitiale.Add(p.transform.position);
            rotatiiInitiale.Add(p.transform.rotation);
        }

        

        if (warningText != null)
            warningText.text = "";

        ActualizeazaEcran();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("DataPacket"))
        {
            i++;
            ActualizeazaEcran();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("DataPacket") && i > 0)
        {
            i--;
            ActualizeazaEcran();
        }
    }

    void ActualizeazaEcran()
    {
        if (textStatus != null)
            textStatus.text = "Status curent: i = " + i;
    }

    public void ApasaExecute()
    {
        if (i == limitaBucla)
        {
            Debug.Log("Bucla FOR corecta!");
            if (levelButton != null)
            {
                levelButton.SetInteractable(true);
            }
            LevelManager.instance.ShowLevelCompleteMessage();
        }
        else
        {
            string mesajEroare = (i < limitaBucla) ? "PREA PUȚINE PACHETE!" : "INDEX OUT OF BOUNDS!";
            StartCoroutine(EroareSiReset(mesajEroare));
        }
    }

    IEnumerator EroareSiReset(string mesaj)
    {
        // 1. Afisam mesajul pe Canvas (rosu)
        if (warningText != null)
        {
            warningText.text = "EROARE: " + mesaj;
            warningText.color = Color.red;
        }

        yield return new WaitForSeconds(2.5f);

        // 2. Afisam resetarea (galben)
        if (warningText != null)
        {
            warningText.text = "RESETARE SISTEM...";
            warningText.color = Color.yellow;
        }

        yield return new WaitForSeconds(1f);

        // 3. Resetam logic si curatam textul
        i = 0;
        ActualizeazaEcran();

        if (warningText != null)
            warningText.text = "";

        // 4. Resetam cuburile fizic
        for (int index = 0; index < pacheteInScena.Count; index++)
        {
            GameObject cub = pacheteInScena[index];
            Rigidbody rb = cub.GetComponent<Rigidbody>();

            if (rb != null)
            {
                rb.velocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
                rb.isKinematic = false;
            }

            cub.transform.position = pozitiiInitiale[index];
            cub.transform.rotation = rotatiiInitiale[index];
        }
    }
}