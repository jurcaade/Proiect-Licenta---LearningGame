using UnityEngine;

public class Level2Manager : MonoBehaviour
{
    [Header("Stare Baterii")]
    public bool battery1Placed = false;
    public bool battery2Placed = false;
    public bool battery3Placed = false;

    private bool nivelCompletat = false;
    private InteractButton interactButton;

    void Start()
    {
        // Resetăm variabilele pentru a fi siguri că nu pornește gata completat
        nivelCompletat = false;

        // Căutăm butonul în scenă după Tag-ul "ExitButton"
        GameObject buttonObj = GameObject.FindWithTag("ExitButton");

        if (buttonObj != null)
        {
            interactButton = buttonObj.GetComponent<InteractButton>();

            if (interactButton != null)
            {
                // FORȚĂM butonul să fie inactiv și ascuns la început
                interactButton.SetInteractable(false);

                Renderer buttonRenderer = interactButton.GetComponent<Renderer>();
                if (buttonRenderer != null) buttonRenderer.enabled = false;

                Collider buttonCollider = interactButton.GetComponent<Collider>();
                if (buttonCollider != null) buttonCollider.enabled = false;

                Debug.Log("[Level2Manager] Initializare: Butonul a fost forțat pe INACTIV.");
            }
        }
    }
    public void SetupInteractButton(GameObject buttonObj)
    {
        interactButton = buttonObj.GetComponent<InteractButton>();
        if (interactButton != null)
        {
            // Forțăm butonul să fie oprit când este legat la un puzzle nou
            interactButton.SetInteractable(false);
            Debug.Log("[BitManager] Buton legat și forțat pe ROȘU la început de nivel.");
        }
    }
    void Update()
    {
        // Verificăm dacă toate bateriile sunt puse corect
        // DOAR dacă nivelul nu este deja marcat ca terminat
        if (battery1Placed && battery2Placed && battery3Placed && !nivelCompletat)
        {
            ActivateButton();
        }
    }

    public void CheckSolution()
    {
        if (battery1Placed && battery2Placed && battery3Placed)
        {
            ActivateButton();
        }
    }

    void ActivateButton()
    {
        if (interactButton != null && !nivelCompletat)
        {
            nivelCompletat = true;
            Debug.Log("[Level2Manager] Conditii indeplinite! Activez butonul.");

            // Facem butonul vizibil
            Renderer buttonRenderer = interactButton.GetComponent<Renderer>();
            if (buttonRenderer != null) buttonRenderer.enabled = true;

            // Activăm coliziunea
            Collider buttonCollider = interactButton.GetComponent<Collider>();
            if (buttonCollider != null) buttonCollider.enabled = true;

            // Schimbăm starea în InteractButton (care ar trebui să îl facă VERDE)
            interactButton.SetInteractable(true);
        }
    }
}