using System.Collections.Generic;
using UnityEngine;

public class StackPuzzleManager : MonoBehaviour
{
    [Header("Setari stiva")]
    public float cubeHeight = 1f;
    public int maxStackSize = 4;

    [Header("Cuburi initiale")]
    public List<GameObject> initialCubes;

    [Header("Ordine corecta")]
    public List<string> targetSequence = new List<string> { "Red", "Blue", "Red" };

    [Header("Audio")]
    public AudioClip placeClip;
    public AudioClip errorClip;
    [Range(0f, 1f)] public float placeVolume = 0.8f;
    [Range(0f, 1f)] public float errorVolume = 0.8f;

    private readonly List<GameObject> currentStack = new List<GameObject>();
    private readonly List<string> currentColorStack = new List<string>();

    private LevelDoorButton levelButton;
    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        audioSource.playOnAwake = false;
        audioSource.spatialBlend = 1f;
        audioSource.dopplerLevel = 0f;
        audioSource.minDistance = 1f;
        audioSource.maxDistance = 12f;

        foreach (GameObject cube in initialCubes)
        {
            if (cube == null)
            {
                continue;
            }

            StackItem stackItem = cube.GetComponent<StackItem>();
            Rigidbody rb = cube.GetComponent<Rigidbody>();

            if (stackItem != null && rb != null)
            {
                AddCubeToStack(cube, stackItem, rb);
            }
        }
    }

    public void SetupInteractButton(GameObject buttonObj)
    {
        if (buttonObj != null)
        {
            levelButton = buttonObj.GetComponent<LevelDoorButton>();
        }
    }

    public bool IsTopCube(GameObject cubeObj)
    {
        if (currentStack.Count == 0)
        {
            return false;
        }

        return currentStack[currentStack.Count - 1] == cubeObj;
    }

    public void PlayErrorSound()
    {
        PlayClip(errorClip, errorVolume);
    }

    public void RemoveCubeFromStack(GameObject cubeObj)
    {
        if (!IsTopCube(cubeObj))
        {
            return;
        }

        currentStack.RemoveAt(currentStack.Count - 1);
        currentColorStack.RemoveAt(currentColorStack.Count - 1);

        StackItem stackItem = cubeObj.GetComponent<StackItem>();
        if (stackItem != null)
        {
            stackItem.isStacked = false;
        }

        Rigidbody rb = cubeObj.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = false;
        }

        if (levelButton != null)
        {
            levelButton.SetInteractable(false);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (!other.CompareTag("StackCube"))
        {
            return;
        }

        StackItem stackItem = other.GetComponent<StackItem>();
        Rigidbody rb = other.GetComponent<Rigidbody>();

        if (stackItem == null || stackItem.isStacked || rb == null || rb.isKinematic || other.transform.parent != null)
        {
            return;
        }

        AddCubeToStack(other.gameObject, stackItem, rb);
    }

    private void AddCubeToStack(GameObject cubeObj, StackItem stackItem, Rigidbody rb)
    {
        if (currentStack.Count >= maxStackSize)
        {
            return;
        }

        rb.isKinematic = true;
        stackItem.isStacked = true;

        Vector3 snapPos = transform.position + Vector3.up * currentStack.Count * cubeHeight;
        cubeObj.transform.position = snapPos;
        cubeObj.transform.rotation = Quaternion.identity;

        currentStack.Add(cubeObj);
        currentColorStack.Add(stackItem.cubeColor);
        PlayClip(placeClip, placeVolume);
        CheckWinCondition();
    }

    private void CheckWinCondition()
    {
        if (currentColorStack.Count != targetSequence.Count)
        {
            return;
        }

        for (int i = 0; i < targetSequence.Count; i++)
        {
            if (currentColorStack[i].ToLower() != targetSequence[i].ToLower())
            {
                return;
            }
        }

        if (levelButton != null)
        {
            levelButton.SetInteractable(true);
        }

        LevelManager.instance.ShowLevelCompleteMessage();
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
