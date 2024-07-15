using UnityEngine;

public class TeleportationManager : MonoBehaviour
{
    private Camera minimapCamera; // La cam�ra qui projette la minimap
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
                    // V�rifiez si l'objet cliqu� est un cercueil
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
