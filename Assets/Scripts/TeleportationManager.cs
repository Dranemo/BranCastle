using UnityEngine;

public class TeleportationManager : MonoBehaviour
{
    public Camera minimapCamera; // La caméra qui projette la minimap
    public Transform playerTransform; // Le transform du joueur

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = minimapCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.GetRayIntersection(ray);

            if (hit.collider != null)
            {
                // Vérifiez si l'objet cliqué est un cercueil
                if (hit.collider.CompareTag("Coffin"))
                {
                    TeleportToCoffin(hit.collider.transform);
                }
            }
        }
    }
    public void TeleportToCoffin(Transform coffinTransform)
    {
        Debug.Log("Téléportation");
        if (playerTransform != null && coffinTransform != null)
        {
            playerTransform.position = coffinTransform.position;
            Debug.Log("Joueur téléporté au cercueil : " + coffinTransform.name);
        }
        else
        {
            Debug.LogError("Référence manquante pour le joueur ou le cercueil.");
        }
    }
}
