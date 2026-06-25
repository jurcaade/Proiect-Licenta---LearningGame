using UnityEngine;

public class MoveCamera : MonoBehaviour
{
    public Transform cameraPosition;

    void LateUpdate()
    {
        transform.position = Vector3.Lerp(transform.position, cameraPosition.position, Time.deltaTime * 20f);
    }
}
