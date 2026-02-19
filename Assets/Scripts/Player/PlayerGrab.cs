using UnityEngine;

public class PlayerGrab : MonoBehaviour
{
    [Header("Setari Prindere")]
    public Transform holdPosition;
    public float grabRange = 3f;
    public string grabbableTag = "StackCube";

    private GameObject heldObject;
    private Rigidbody heldObjRb;
    private Collider heldObjCollider;

    void Update()
    {
        // NOU: Acum verifica daca apesi E "SAU" (||) Click Stanga (0)
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
                // NOU: Verificam regulile stivei inainte sa il luam
                StackCube cubeInfo = hit.transform.GetComponent<StackCube>();
                if (cubeInfo != null && cubeInfo.isStacked)
                {
                    StackBase stackBase = FindObjectOfType<StackBase>();
                    if (stackBase != null)
                    {
                        // Daca NU e in varf, nu te lasam sa il iei
                        if (!stackBase.IsTopCube(hit.transform.gameObject))
                        {
                            Debug.Log("Eroare LIFO: Poti lua doar cubul din varf!");
                            return; // Oprim prinderea
                        }
                        else
                        {
                            // Daca e in varf, il scoti din stiva oficial (POP)
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

            if (heldObjRb != null)
            {
                heldObjRb.isKinematic = false;

                // Pornim coliziunea inapoi ca sa poata lovi podeaua
                if (heldObjCollider != null) heldObjCollider.enabled = true;
            }
            heldObject = null;
        }
    }
}