using System.Collections;
using TMPro;
using UnityEngine;

public class PlayerGrab : MonoBehaviour
{
    [Header("Setari Prindere")]
    public Transform holdPosition;
    public float grabRange = 3f;
    public string grabbableTag = "StackCube";
    public string level5Tag = "DataPacket";

    [Header("UI Feedback")]
    public GameObject warningPanel;
    public TMP_Text warningText;

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
        if (!Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, grabRange))
            return;

        if (!hit.transform.CompareTag(grabbableTag) && !hit.transform.CompareTag(level5Tag))
            return;

        if (hit.transform.CompareTag(grabbableTag))
        {
            StackItem cubeInfo = hit.transform.GetComponent<StackItem>();
            if (cubeInfo != null && cubeInfo.isStacked)
            {
                StackPuzzleManager stackBase = FindObjectOfType<StackPuzzleManager>();
                if (stackBase != null)
                {
                    if (!stackBase.IsTopCube(hit.transform.gameObject))
                    {
                        stackBase.PlayErrorSound();
                        if (warningText != null)
                            StartCoroutine(ShowWarningMessage("EROARE: Poti muta doar cubul din varf! (LIFO)"));

                        return;
                    }

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

            if (heldObjCollider != null)
                heldObjCollider.enabled = false;

            heldObject.transform.position = holdPosition.position;
            heldObject.transform.SetParent(holdPosition);
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

            int wallLayer = LayerMask.NameToLayer("InvisibleWall");
            int layerMask = ~0;

            if (wallLayer != -1)
                layerMask = ~(1 << wallLayer);

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
        if (warningPanel == null || warningText == null)
            yield break;

        warningPanel.SetActive(true);
        warningText.text = message;

        yield return new WaitForSeconds(2f);

        warningPanel.SetActive(false);
    }
}
