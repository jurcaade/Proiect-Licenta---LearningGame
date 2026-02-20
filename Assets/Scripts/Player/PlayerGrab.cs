using UnityEngine;
using TMPro; // Necesar pentru textul de pe ecran
using System.Collections; // Necesar pentru temporizator

public class PlayerGrab : MonoBehaviour
{
    [Header("Setari Prindere")]
    public Transform holdPosition;
    public float grabRange = 3f;
    public string grabbableTag = "StackCube";

    [Header("UI Feedback")]
    public TMP_Text warningText; // Slot-ul unde vei trage textul din Canvas

    private GameObject heldObject;
    private Rigidbody heldObjRb;
    private Collider heldObjCollider;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) || Input.GetMouseButtonDown(0))
        {
            if (heldObject == null)
            {
                TryGrab();
            }
            else
            {
                Drop();
            }
        }
    }

    void TryGrab()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, grabRange))
        {
            if (hit.transform.CompareTag(grabbableTag))
            {
                StackCube cubeInfo = hit.transform.GetComponent<StackCube>();
                if (cubeInfo != null && cubeInfo.isStacked)
                {
                    StackBase stackBase = FindObjectOfType<StackBase>();
                    if (stackBase != null)
                    {
                        if (!stackBase.IsTopCube(hit.transform.gameObject))
                        {
                            // AICI AFIȘĂM MESAJUL PE ECRAN
                            if (warningText != null)
                            {
                                StartCoroutine(ShowWarningMessage("EROARE: Poți lua doar cubul din vârf!"));
                            }
                            return;
                        }
                        else
                        {
                            stackBase.RemoveCubeFromStack(hit.transform.gameObject);
                        }
                    }
                }

                heldObject = hit.transform.gameObject;
                heldObjRb = heldObject.GetComponent<Rigidbody>();
                heldObjCollider = heldObject.GetComponent<Collider>();

                if (heldObjRb != null)
                {
                    heldObjRb.isKinematic = true;
                    if (heldObjCollider != null) heldObjCollider.enabled = false;

                    heldObject.transform.position = holdPosition.position;
                    heldObject.transform.SetParent(holdPosition);
                }
            }
        }
    }

    void Drop()
    {
        if (heldObject != null)
        {
            heldObject.transform.SetParent(null);

            Vector3 dirToCube = heldObject.transform.position - transform.position;
            float distanceToCube = dirToCube.magnitude;
            RaycastHit hit;

            if (Physics.Raycast(transform.position, dirToCube.normalized, out hit, distanceToCube))
            {
                heldObject.transform.position = hit.point - (dirToCube.normalized * 0.25f);
            }

            if (heldObjRb != null)
            {
                heldObjRb.isKinematic = false;
                if (heldObjCollider != null) heldObjCollider.enabled = true;
            }

            heldObject = null;
        }
    }

    // Funcția care afișează mesajul și îl face să dispară automat
    IEnumerator ShowWarningMessage(string message)
    {
        warningText.text = message;
        warningText.color = Color.red;

        // Așteaptă 2 secunde
        yield return new WaitForSeconds(2.0f);

        // Șterge mesajul
        warningText.text = "";
    }
}