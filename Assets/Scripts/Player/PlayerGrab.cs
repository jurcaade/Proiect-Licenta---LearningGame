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
    public GameObject warningPanel;
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
                                    StartCoroutine(ShowWarningMessage("EROARE: Poți muta doar cubul din vârf! (LIFO)"));
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

            Vector3 dirToCube = heldObject.transform.position - transform.position;
            float distanceToCube = dirToCube.magnitude;
            RaycastHit hit;

            // NOU: Creăm o mască ce ignoră layer-ul "InvisibleWall"
            int wallLayer = LayerMask.NameToLayer("InvisibleWall");
            int layerMask = ~0; // ~0 înseamnă că lovește absolut tot

            if (wallLayer != -1)
            {
                // Scoatem layer-ul peretelui din mască
                layerMask = ~(1 << wallLayer);
            }

            // Aplicăm masca în Raycast
            if (Physics.Raycast(transform.position, dirToCube.normalized, out hit, distanceToCube, layerMask))
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

    IEnumerator ShowWarningMessage(string message)
    {
        warningPanel.SetActive(true);   // arată panel-ul
        warningText.text = message;

        yield return new WaitForSeconds(2f);

        warningPanel.SetActive(false);  // ascunde panel-ul
    }
}