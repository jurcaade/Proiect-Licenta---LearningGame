using UnityEngine;
using TMPro; // Necesar pentru textul de pe ecran
using System.Collections; // Necesar pentru temporizator

public class PlayerGrab : MonoBehaviour
{
    [Header("Setari Prindere")]
    public Transform holdPosition;
    public float grabRange = 3f;
    public string grabbableTag = "StackCube";
    public string level5Tag = "DataPacket"; // Noul tag pentru nivelul 5

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
            // VERIFICARE: Acceptă ori StackCube, ori DataPacket
            if (hit.transform.CompareTag(grabbableTag) || hit.transform.CompareTag(level5Tag))
            {
                // Logica de stivă se aplică DOAR dacă obiectul este un StackCube
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
                }

                // PRINDEREA PROPRIU-ZISĂ (valabilă pentru ambele tipuri)
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

            if (heldObjRb != null)
            {
                heldObjRb.isKinematic = false;
                if (heldObjCollider != null) heldObjCollider.enabled = true;
            }

            heldObject = null;
        }
    }

    IEnumerator ShowWarningMessage(string message)
    {
        warningText.text = message;
        warningText.color = Color.red;
        yield return new WaitForSeconds(2.0f);
        warningText.text = "";
    }
}