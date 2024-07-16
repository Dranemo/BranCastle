using UnityEngine;

public class TeleportationManager : MonoBehaviour
{
    private Camera minimapCamera; // La caméra qui projette la minimap
    private Transform playerTransform; // Le transform du joueur



    void Awake()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        minimapCamera = GameObject.FindGameObjectWithTag("MinimapCamera").GetComponent<Camera>();

        Debug.Log(minimapCamera);
    }


    void Update()
    {
        if(!GameManager.Instance.isGameOver)
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
