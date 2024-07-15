using UnityEngine;

public class TeleportationManager : MonoBehaviour
{
    public Camera minimapCamera; // La cam�ra qui projette la minimap
    public Transform playerTransform; // Le transform du joueur

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = minimapCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.GetRayIntersection(ray);

            if (hit.collider != null)
            {
                // V�rifiez si l'objet cliqu� est un cercueil
                if (hit.collider.CompareTag("Coffin"))
                {
                    TeleportToCoffin(hit.collider.transform);
                }
            }
        }
    }
    public void TeleportToCoffin(Transform coffinTransform)
    {
        Debug.Log("T�l�portation");
        if (playerTransform != null && coffinTransform != null)
        {
            playerTransform.position = coffinTransform.position;
            Debug.Log("Joueur t�l�port� au cercueil : " + coffinTransform.name);
        }
        else
        {
            Debug.LogError("R�f�rence manquante pour le joueur ou le cercueil.");
        }
    }
}
