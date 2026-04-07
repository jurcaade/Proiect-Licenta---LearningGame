using UnityEngine;

public class DoorTrigger : MonoBehaviour
{
    [Header("Trage butonul de la ușa pe care vrei să o închizi")]
    public InteractButton butonUsa;

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

        if (other.transform.root.CompareTag("Player"))

        {
            Debug.Log($"[Timp: {Time.time}] Player-ul a fost detectat cu succes!");

            butonUsa.InchideUsa();
            hasTriggered = true;

            if (triggerCollider != null)
            {
                triggerCollider.enabled = false;
            }
        }
    }
}
