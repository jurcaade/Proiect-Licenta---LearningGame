using UnityEngine;

public class InteractButton : MonoBehaviour
{
    public float distanta = 5.0f;
    public GameObject doorLeft;   // door_1_left
    public GameObject doorRight;  // door_1_right
    public float viteza = 2.0f;

    private Vector3 pozitieInchisaLeft;
    private Vector3 pozitieDeschisaLeft;
    private Vector3 pozitieInchisaRight;
    private Vector3 pozitieDeschisaRight;

    private bool usaDeschisa = false;

    void Start()
    {
        // salvam pozitia Y si Z originale
        if (doorLeft != null && doorRight != null)
        {
            pozitieInchisaLeft = doorLeft.transform.position;
            pozitieDeschisaLeft = doorLeft.transform.position;
            pozitieInchisaRight = doorRight.transform.position;
            pozitieDeschisaRight = doorRight.transform.position;

            // setam X-ul corect pentru inchis si deschis
            pozitieInchisaLeft.x = 1.4f;
            pozitieDeschisaLeft.x = 2.62f;

            pozitieInchisaRight.x = -1.4f;
            pozitieDeschisaRight.x = -2.62f;

            // setam la start la inchis
            doorLeft.transform.position = pozitieInchisaLeft;
            doorRight.transform.position = pozitieInchisaRight;
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
            VerificaObiect();

        // miscarea usilor doar pe X, pastrand Y si Z
        if (doorLeft != null)
        {
            Vector3 target = usaDeschisa ? pozitieDeschisaLeft : pozitieInchisaLeft;
            doorLeft.transform.position = Vector3.MoveTowards(doorLeft.transform.position, target, viteza * Time.deltaTime);
        }

        if (doorRight != null)
        {
            Vector3 target = usaDeschisa ? pozitieDeschisaRight : pozitieInchisaRight;
            doorRight.transform.position = Vector3.MoveTowards(doorRight.transform.position, target, viteza * Time.deltaTime);
        }

        Debug.DrawRay(transform.position, transform.forward * distanta, Color.red);
    }

    void VerificaObiect()
    {
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, distanta))
        {
            Debug.Log("Am lovit: " + hit.collider.gameObject.name);

            if (hit.collider.CompareTag("Buton"))
            {
                usaDeschisa = !usaDeschisa;
                Debug.Log("Usile se deschid/inchid!");
            }
        }
    }
}
