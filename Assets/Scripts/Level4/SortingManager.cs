using UnityEngine;
using TMPro;

public class SortingManager : MonoBehaviour
{
    [Header("Setari")]
    public GameObject spherePrefab; // Trage UN SINGUR prefab aici (ex: Sphere_32_extended)
    public Transform spawnPoint;    // Punctul de unde pornește sfera
    public Transform decisionPoint; // Punctul unde se oprește sfera
    public TextMeshPro statusText;  // Textul de pe monitor (Scor: 0/4)

    private GameObject currentSphere;
    private Color currentColor;
    private int score = 0;
    private bool isMoving = false;

    private InteractButton levelButton;

    // DEFINIM CULORILE PERSONALIZATE AICI:
    // (Valorile din paranteză reprezintă cantitatea de Red, Green, Blue pe o scară de la 0 la 1)
    private Color colorPink = new Color(1f, 0.4f, 0.7f);   // Roz personalizat
    private Color colorPurple = new Color(0.6f, 0.1f, 0.8f); // Mov personalizat

    private Color[] culoriPosibile;

    public void SetupInteractButton(GameObject buttonObj)
    {
        if (buttonObj != null)
        {
            levelButton = buttonObj.GetComponent<InteractButton>();
        }
    }

    void Start()
    {
        // Inițializăm paleta de culori pe care le va alege codul
        culoriPosibile = new Color[]
        {
            Color.blue,
            colorPink,
            colorPurple,
            Color.yellow
        };

        SpawnNewSphere();
    }

    void Update()
    {
        // Mută sfera pe bandă până ajunge la jucător
        if (isMoving && currentSphere != null)
        {
            currentSphere.transform.position = Vector3.MoveTowards(
                currentSphere.transform.position,
                decisionPoint.position,
                3f * Time.deltaTime // Viteza cu care vine sfera
            );

            if (Vector3.Distance(currentSphere.transform.position, decisionPoint.position) < 0.01f)
            {
                isMoving = false; // S-a oprit și așteaptă decizia jucătorului
            }
        }
    }

    void SpawnNewSphere()
    {
        currentSphere = Instantiate(spherePrefab, spawnPoint.position, Quaternion.identity);

        // Alegem o culoare la întâmplare din lista noastră nouă
        int index = Random.Range(0, culoriPosibile.Length);
        currentColor = culoriPosibile[index];

        // Schimbăm culoarea materialului sferei
        Renderer sphereRenderer = currentSphere.GetComponent<Renderer>();
        if (sphereRenderer != null)
        {
            sphereRenderer.material.color = currentColor;
        }

        isMoving = true;
    }

    // Funcție apelată dacă s-a apăsat TRUE
    public void ApasaTrue()
    {
        OnPlayerDecision(true);
    }

    // Funcție apelată dacă s-a apăsat FALSE
    public void ApasaFalse()
    {
        OnPlayerDecision(false);
    }

    // Logica de decizie
    public void OnPlayerDecision(bool pressedTrue)
    {
        Debug.Log("[TEST] Managerul a primit comanda! isMoving este: " + isMoving);

        if (isMoving || currentSphere == null)
        {
            Debug.LogWarning("[TEST] Click ignorat! Sfera încă se mișcă sau nu există.");
            return;
        }

        // Condiția jocului ACUM: Dacă e Albastru SAU Roz, răspunsul corect e TRUE. (Mov și Galben sunt FALSE).
        bool isBlueOrPink = (currentColor == Color.blue || currentColor == colorPink);

        // Verificăm dacă alegerea jucătorului se potrivește cu adevărul logic
        if (pressedTrue == isBlueOrPink)
        {
            score++;
            Debug.Log("Corect! Ai ajuns la scorul: " + score);
        }
        else
        {
            score = 0; // Greșeală -> o ia de la capăt
            Debug.Log("Greșit! Se resetează secvența de la 0.");
        }

        // Actualizăm ecranul din joc
        if (statusText != null) statusText.text = "Sincronizare: " + score + "/4";

        // Verificăm dacă a finalizat nivelul
        if (score >= 4)
        {
            if (statusText != null) statusText.text = "ACCES PERMIS";
            Debug.Log("NIVEL FINALIZAT! Ai sortat 4 la rând corect.");
            Destroy(currentSphere);

            if (levelButton != null)
            {
                levelButton.SetInteractable(true);
            }
            LevelManager.instance.ShowLevelCompleteMessage();
        }
        else
        {
            // Dacă nu a terminat încă, vine următoarea sferă
            Destroy(currentSphere);
            SpawnNewSphere();
        }
    }
}