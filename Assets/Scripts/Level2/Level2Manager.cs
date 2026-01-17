using UnityEngine;
using System.Collections;

public class Level2Manager : MonoBehaviour
{
    [Header("Baterii")]
    public BatteryLogic batteryA;
    public BatteryLogic batteryB;
    public BatteryLogic batteryC;

    [Header("Referinte Reactor")]
    public Renderer projectorRenderer; // Mesh-ul cu 4 materiale

    private InteractButton interactButton;
    private bool isSolved = false;

    void Start()
    {
        DisableEmissionAtStart();
    }

    public void SetupInteractButton(GameObject buttonObj)
    {
        interactButton = buttonObj.GetComponent<InteractButton>();
        if (interactButton != null)
        {
            interactButton.SetInteractable(false);
            Debug.Log("[Level2] Button linked and forced RED at start.");
        }
    }

    // Apelata de butonul de verificare
    public void CheckSolution()
    {
        if (isSolved) return;

        bool A = batteryA != null && batteryA.bitValue == 1;
        bool B = batteryB != null && batteryB.bitValue == 1;
        bool C = batteryC != null && batteryC.bitValue == 1;

        Debug.Log($"A={A}  B={B}  C={C}");

        // (A && B) || !C
        if ((A && B) || !C)
        {
            Debug.Log("✅ REACTOR ACTIVAT");
            ActivateSuccessEffects();
        }
        else
        {
            Debug.Log("❌ EROARE LOGICA");
            StartCoroutine(FlashErrorEffect());
        }
    }

    // =========================
    // SUCCESS
    // =========================
    void ActivateSuccessEffects()
    {
        isSolved = true;

        if (projectorRenderer != null)
        {
            Material[] mats = projectorRenderer.materials;

            // Material 1 - ALB
            mats[1].EnableKeyword("_EMISSION");
            mats[1].SetColor("_EmissionColor", Color.white * 4f);

            // Material 3 - CYAN
            mats[3].EnableKeyword("_EMISSION");
            mats[3].SetColor("_EmissionColor", Color.cyan * 5f);

            projectorRenderer.materials = mats;
        }
        // 🟢 UNLOCK BUTTON
        if (interactButton != null)
        {
            interactButton.SetInteractable(true);
            Debug.Log("[Level2] Button unlocked!");
        }

    }

    // =========================
    // ERROR FLASH
    // =========================
    IEnumerator FlashErrorEffect()
    {
        if (projectorRenderer == null) yield break;

        Material[] mats = projectorRenderer.materials;

        Color prev1 = mats[1].GetColor("_EmissionColor");
        Color prev3 = mats[3].GetColor("_EmissionColor");

        mats[1].EnableKeyword("_EMISSION");
        mats[3].EnableKeyword("_EMISSION");

        mats[1].SetColor("_EmissionColor", Color.red * 3f);
        mats[3].SetColor("_EmissionColor", Color.red * 3f);
        projectorRenderer.materials = mats;

        yield return new WaitForSeconds(0.5f);

        mats[1].SetColor("_EmissionColor", prev1);
        mats[3].SetColor("_EmissionColor", prev3);
        projectorRenderer.materials = mats;
    }

    // =========================
    // INITIAL STATE
    // =========================
    void DisableEmissionAtStart()
    {
        if (projectorRenderer == null) return;

        Material[] mats = projectorRenderer.materials;

        mats[1].DisableKeyword("_EMISSION");
        mats[1].SetColor("_EmissionColor", Color.black);

        mats[3].DisableKeyword("_EMISSION");
        mats[3].SetColor("_EmissionColor", Color.black);

        projectorRenderer.materials = mats;

       
    }

    // =========================
    // ROTATION
    // =========================
    void Update()
    {
        if (isSolved && projectorRenderer != null)
        {
            projectorRenderer.transform.Rotate(Vector3.up * Time.deltaTime * 60f);
        }
    }
}