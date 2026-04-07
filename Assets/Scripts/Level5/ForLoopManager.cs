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
    public TMP_Text feedbackText;
    public GameObject feedbackPanel;  // Trage aici obiectul de text din Canvas (UI)

    public ElevatorMovement lift;

    [Header("Audio")]
    public AudioClip errorClip;
    [Range(0f, 1f)]
    public float errorVolume = 0.9f;

    [Header("Resetare Obiecte")]
    private List<GameObject> pacheteInScena = new List<GameObject>();
    private List<Vector3> pozitiiInitiale = new List<Vector3>();
    private List<Quaternion> rotatiiInitiale = new List<Quaternion>();

    private InteractButton levelButton;
    private AudioSource audioSource;

    public void SetupInteractButton(GameObject buttonObj)
    {
        if (buttonObj != null)
        {
            levelButton = buttonObj.GetComponent<InteractButton>();
        }
    }

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        audioSource.playOnAwake = false;
        audioSource.spatialBlend = 0f;

        GameObject[] pachete = GameObject.FindGameObjectsWithTag("DataPacket");

        foreach (GameObject p in pachete)
        {
            pacheteInScena.Add(p);
            pozitiiInitiale.Add(p.transform.position);
            rotatiiInitiale.Add(p.transform.rotation);
        }

        

        if (feedbackText != null)
            feedbackText.text = "";

        ActualizeazaEcran();

        if (lift != null)
        {
            lift.PornesteLiftul();
        }
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
            textStatus.text = $"<size=80%><color=#A0A0A0>STATUS CURENT:</color></size>\n<b><color=#569CD6>i</color> = <color=#B5CEA8>{i}</color></b>";
    }

    public void ApasaExecute()
    {
        if (i == limitaBucla)
        {
            Debug.Log("Bucla FOR corecta!");
            if (lift != null)
            {
                lift.OpresteLiftul();
            }

            if (levelButton != null)
            {
                levelButton.SetInteractable(true);
            }
            LevelManager.instance.ShowLevelCompleteMessage();
        }
        else
        {
            PlayErrorSound();
            string mesajEroare = (i < limitaBucla) ? "PREA PUȚINE PACHETE!" : "PREA MULTE PACHETE!";
            StartCoroutine(EroareSiReset(mesajEroare));
        }
    }

    void PlayErrorSound()
    {
        if (errorClip != null)
        {
            audioSource.PlayOneShot(errorClip, errorVolume);
        }
    }

    IEnumerator EroareSiReset(string mesaj)
    {
        // 1. Activăm Panel-ul și afișăm eroarea
        if (feedbackPanel != null) feedbackPanel.SetActive(true);

        if (feedbackText != null)
        {
            // Folosim tag-uri de marime si bold pentru impact
            feedbackText.text = $"<color=#FF4C4C>Număr incorect de pachete</color>\n{mesaj}";
        }

        yield return new WaitForSeconds(2.5f);

        // 2. Schimbăm mesajul pentru resetare
        if (feedbackText != null)
        {
            feedbackText.text = "<color=#FFEB04>Se resetează nivelul...</color>";
        }

        yield return new WaitForSeconds(1.5f);

        // 3. Resetăm logica și închidem tot UI-ul de feedback
        i = 0;
        ActualizeazaEcran();

        if (feedbackText != null) feedbackText.text = "";
        if (feedbackPanel != null) feedbackPanel.SetActive(false); // Ascundem fundalul negru

        // 4. Resetăm cuburile fizic (codul tău existent...)
        for (int index = 0; index < pacheteInScena.Count; index++)
        {
            GameObject cub = pacheteInScena[index];
            Rigidbody rb = cub.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.velocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
            }
            cub.transform.position = pozitiiInitiale[index];
            cub.transform.rotation = rotatiiInitiale[index];
        }
    }
}
