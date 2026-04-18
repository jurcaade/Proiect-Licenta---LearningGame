using System.Collections;
using UnityEngine;

public class ReactorPuzzleManager : MonoBehaviour
{
    [Header("Baterii")]
    public BatterySwitch batteryA;
    public BatterySwitch batteryB;
    public BatterySwitch batteryC;

    [Header("Reactor")]
    public Renderer projectorRenderer;

    [Header("Audio")]
    public AudioClip successClip;
    public AudioClip errorClip;
    [Range(0f, 1f)] public float successVolume = 0.9f;
    [Range(0f, 1f)] public float errorVolume = 0.8f;

    private const int WhiteMaterialIndex = 1;
    private const int CyanMaterialIndex = 3;

    private LevelDoorButton interactButton;
    private AudioSource audioSource;
    private bool isSolved;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        audioSource.playOnAwake = false;
        audioSource.spatialBlend = 0f;
        audioSource.dopplerLevel = 0f;

        SetReactorIdle();
    }

    private void Update()
    {
        if (isSolved && projectorRenderer != null)
        {
            projectorRenderer.transform.Rotate(Vector3.up * Time.deltaTime * 60f);
        }
    }

    public void SetupInteractButton(GameObject buttonObj)
    {
        if (buttonObj == null)
        {
            return;
        }

        interactButton = buttonObj.GetComponent<LevelDoorButton>();
        if (interactButton != null)
        {
            interactButton.SetInteractable(false);
        }
    }

    public void CheckSolution()
    {
        if (isSolved) return;

        bool a = IsBatteryOn(batteryA);
        bool b = IsBatteryOn(batteryB);
        bool c = IsBatteryOn(batteryC);

        if ((a && b) || !c)
        {
            SolvePuzzle();
            return;
        }

        PlayClip(errorClip, errorVolume);
        StartCoroutine(FlashErrorEffect());
    }

    private void SolvePuzzle()
    {
        isSolved = true;
        PlayClip(successClip, successVolume);

        if (projectorRenderer != null)
        {
            Material[] mats = projectorRenderer.materials;
            SetEmission(mats, WhiteMaterialIndex, Color.white, 4f);
            SetEmission(mats, CyanMaterialIndex, Color.cyan, 5f);
            projectorRenderer.materials = mats;
        }

        if (interactButton != null)
        {
            interactButton.SetInteractable(true);
        }

        LevelManager.instance.ShowLevelCompleteMessage();
    }

    private IEnumerator FlashErrorEffect()
    {
        if (projectorRenderer == null)
        {
            yield break;
        }

        Material[] mats = projectorRenderer.materials;
        Color previousWhite = mats[WhiteMaterialIndex].GetColor("_EmissionColor");
        Color previousCyan = mats[CyanMaterialIndex].GetColor("_EmissionColor");

        SetEmission(mats, WhiteMaterialIndex, Color.red, 3f);
        SetEmission(mats, CyanMaterialIndex, Color.red, 3f);
        projectorRenderer.materials = mats;

        yield return new WaitForSeconds(0.5f);

        mats[WhiteMaterialIndex].SetColor("_EmissionColor", previousWhite);
        mats[CyanMaterialIndex].SetColor("_EmissionColor", previousCyan);
        projectorRenderer.materials = mats;
    }

    private void SetReactorIdle()
    {
        if (projectorRenderer == null)
        {
            return;
        }

        Material[] mats = projectorRenderer.materials;
        mats[WhiteMaterialIndex].DisableKeyword("_EMISSION");
        mats[WhiteMaterialIndex].SetColor("_EmissionColor", Color.black);
        mats[CyanMaterialIndex].DisableKeyword("_EMISSION");
        mats[CyanMaterialIndex].SetColor("_EmissionColor", Color.black);
        projectorRenderer.materials = mats;
    }

    private void SetEmission(Material[] mats, int materialIndex, Color color, float intensity)
    {
        mats[materialIndex].EnableKeyword("_EMISSION");
        mats[materialIndex].SetColor("_EmissionColor", color * intensity);
    }

    private bool IsBatteryOn(BatterySwitch battery)
    {
        return battery != null && battery.bitValue == 1;
    }

    private void PlayClip(AudioClip clip, float volume)
    {
        if (audioSource == null || clip == null)
        {
            return;
        }

        audioSource.PlayOneShot(clip, volume);
    }
}
