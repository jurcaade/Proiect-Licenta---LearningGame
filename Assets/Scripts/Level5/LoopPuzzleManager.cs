using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LoopPuzzleManager : MonoBehaviour
{
    [Header("Setari bucla")]
    public int limitaBucla = 4;

    [Header("UI")]
    public TextMeshPro textStatus;
    public TMP_Text feedbackText;
    public GameObject feedbackPanel;

    [Header("Referinte")]
    public MovingLift lift;

    [Header("Audio")]
    public AudioClip errorClip;
    [Range(0f, 1f)] public float errorVolume = 0.9f;

    private readonly List<GameObject> pacheteInScena = new List<GameObject>();
    private readonly List<Vector3> pozitiiInitiale = new List<Vector3>();
    private readonly List<Quaternion> rotatiiInitiale = new List<Quaternion>();

    private int i;
    private LevelDoorButton levelButton;
    private AudioSource audioSource;

    public void SetupInteractButton(GameObject buttonObj)
    {
        if (buttonObj != null)
        {
            levelButton = buttonObj.GetComponent<LevelDoorButton>();
        }
    }

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        audioSource.playOnAwake = false;
        audioSource.spatialBlend = 0f;

        GameObject[] pachete = GameObject.FindGameObjectsWithTag("DataPacket");
        foreach (GameObject pachet in pachete)
        {
            pacheteInScena.Add(pachet);
            pozitiiInitiale.Add(pachet.transform.position);
            rotatiiInitiale.Add(pachet.transform.rotation);
        }

        if (feedbackText != null)
        {
            feedbackText.text = "";
        }

        ActualizeazaEcran();
        lift?.PornesteLiftul();
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

    public void ApasaExecute()
    {
        if (i == limitaBucla)
        {
            lift?.OpresteLiftul();
            levelButton?.SetInteractable(true);
            LevelManager.instance.ShowLevelCompleteMessage();
            return;
        }

        if (errorClip != null)
        {
            audioSource.PlayOneShot(errorClip, errorVolume);
        }

        string mesajEroare = i < limitaBucla ? "PREA PUTINE PACHETE!" : "PREA MULTE PACHETE!";
        StartCoroutine(EroareSiReset(mesajEroare));
    }

    private void ActualizeazaEcran()
    {
        if (textStatus != null)
        {
            textStatus.text = $"<size=80%><color=#A0A0A0>STATUS CURENT:</color></size>\n<b><color=#569CD6>i</color> = <color=#B5CEA8>{i}</color></b>";
        }
    }

    private IEnumerator EroareSiReset(string mesaj)
    {
        if (feedbackPanel != null)
        {
            feedbackPanel.SetActive(true);
        }

        if (feedbackText != null)
        {
            feedbackText.text = $"<color=#FF4C4C>Numar incorect de pachete</color>\n{mesaj}";
        }

        yield return new WaitForSeconds(2.5f);

        if (feedbackText != null)
        {
            feedbackText.text = "<color=#FFEB04>Se reseteaza nivelul...</color>";
        }

        yield return new WaitForSeconds(1.5f);

        i = 0;
        ActualizeazaEcran();

        if (feedbackText != null)
        {
            feedbackText.text = "";
        }

        if (feedbackPanel != null)
        {
            feedbackPanel.SetActive(false);
        }

        for (int index = 0; index < pacheteInScena.Count; index++)
        {
            GameObject pachet = pacheteInScena[index];
            Rigidbody rb = pachet.GetComponent<Rigidbody>();

            if (rb != null)
            {
                rb.velocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
            }

            pachet.transform.position = pozitiiInitiale[index];
            pachet.transform.rotation = rotatiiInitiale[index];
        }
    }
}
