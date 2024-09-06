using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;
    public float smoothSpeed = 0.125f;
    public float maxOffset = 2.0f;

    private Vector3 offset;

    void Start()
    {
        if (player == null)
        {
            //////////Debug.LogError("Player transform is not assigned!");
            return;
        }

        offset = transform.position - player.position;
    }

    void LateUpdate()
    {
        if (player == null) return;

        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0;
        Vector3 direction = mousePosition - player.position;
        float distance = direction.magnitude;
        float clampedDistance = Mathf.Min(distance, maxOffset);
        Vector3 desiredPosition = player.position + offset + direction.normalized * clampedDistance;

        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime*20);
        transform.position = smoothedPosition;
    }
}
