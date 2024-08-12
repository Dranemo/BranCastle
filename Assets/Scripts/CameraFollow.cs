using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player; // R�f�rence au joueur
    public float smoothSpeed = 0.125f; // Vitesse de lissage
    public float maxOffset = 2.0f; // Distance maximale que la cam�ra peut se d�placer par rapport au joueur

    private Vector3 offset; // D�calage initial de la cam�ra par rapport au joueur

    void Start()
    {
        if (player == null)
        {
            Debug.LogError("Player transform is not assigned!");
            return;
        }

        // Calculer le d�calage initial
        offset = transform.position - player.position;
    }

    void LateUpdate()
    {
        if (player == null) return;

        // Obtenir la position du curseur de la souris en monde
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0; // Assurez-vous que la position Z est correcte

        // Calculer la distance entre le joueur et le curseur de la souris
        Vector3 direction = mousePosition - player.position;
        float distance = direction.magnitude;

        // Limiter la distance maximale
        float clampedDistance = Mathf.Min(distance, maxOffset);

        // Calculer la position cible de la cam�ra avec un d�calage proportionnel
        Vector3 desiredPosition = player.position + offset + direction.normalized * clampedDistance;

        // Lissage de la position de la cam�ra
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

        // Appliquer la position liss�e � la cam�ra
        transform.position = smoothedPosition;
    }
}
