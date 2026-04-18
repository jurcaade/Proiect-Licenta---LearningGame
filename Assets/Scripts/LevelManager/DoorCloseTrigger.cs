using UnityEngine;

public class DoorCloseTrigger : MonoBehaviour
{
    [Header("Trage butonul usii care trebuie inchisa")]
    public LevelDoorButton butonUsa;

    private Collider triggerCollider;
    private bool hasTriggered;

    private void Awake()
    {
        triggerCollider = GetComponent<Collider>();
    }

    private void OnEnable()
    {
        hasTriggered = false;

        if (triggerCollider != null)
        {
            triggerCollider.enabled = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        TryCloseDoor(other);
    }

    private void OnTriggerStay(Collider other)
    {
        TryCloseDoor(other);
    }

    private void TryCloseDoor(Collider other)
    {
        if (hasTriggered || other == null)
        {
            return;
        }

        if (butonUsa == null || !butonUsa.DoorWasOpened)
        {
            return;
        }

        if (!other.transform.root.CompareTag("Player"))
        {
            return;
        }

        butonUsa.InchideUsa();
        hasTriggered = true;

        if (triggerCollider != null)
        {
            triggerCollider.enabled = false;
        }
    }
}
