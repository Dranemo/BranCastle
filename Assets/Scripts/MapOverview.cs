using UnityEngine;

public class MapOverview : MonoBehaviour
{
    public Camera minimapCamera; // La cam�ra qui projette la minimap
    public Transform playerTransform; // Le transform du joueur
    public LayerMask coffinLayerMask; // Le layer des cercueils
    //private CanvasGroup canvasGroup;
    [SerializeField] GameObject[] canvasGroupMain;
    [SerializeField] GameObject[] minimap;

    public bool minimapState = false;
    

    void Awake()
    {
        // Obtenez la r�f�rence au CanvasGroup attach� au m�me GameObject
        //canvasGroup = GetComponent<CanvasGroup>();
        /*if (canvasGroup == null)
        {
            //////////Debug.LogError("CanvasGroup n'est pas trouv� sur le GameObject.");
        }*/

        foreach (GameObject canvas in canvasGroupMain)
        {
            canvas.SetActive(false);
        }
        foreach (GameObject minimap in minimap)
        {
            minimap.SetActive(true);
            minimapState = true;
        }
    }

    private void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    public void ActivateOverview()
    {
        /*if (canvasGroup != null)
        {
            //canvasGroup.alpha = 1f;
            canvasGroup.blocksRaycasts = true;
            canvasGroup.interactable = true;
        }*/


        foreach (GameObject canvas in canvasGroupMain)
        {
            canvas.SetActive(true);
        }
        foreach (GameObject minimap in minimap)
        {
            minimap.SetActive(false);
        }

    }

    // Fonction pour d�sactiver l'overlay (optionnelle)
    public void DeactivateOverview()
    {
        /*if (canvasGroup != null)
        {
            //canvasGroup.alpha = 0f;
            canvasGroup.blocksRaycasts = false;
            canvasGroup.interactable = false;
        }*/

        foreach (GameObject canvas in canvasGroupMain)
        {
            canvas.SetActive(false);
        }
        foreach (GameObject minimap in minimap)
        {
            minimap.SetActive(true);
        }
    }
    void Update()
    {
        if (minimapState)
        {
            minimapCamera.transform.position = new Vector3(playerTransform.position.x, playerTransform.position.y, -10);
        }
        else
        {
            minimapCamera.transform.position = new Vector3(25, 2, -10);

            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = minimapCamera.ScreenPointToRay(Input.mousePosition);
                RaycastHit2D hit = Physics2D.GetRayIntersection(ray, Mathf.Infinity, coffinLayerMask);

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

    void TeleportToCoffin(Transform coffinTransform)
    {
        if (playerTransform != null && coffinTransform != null)
        {
            playerTransform.position = coffinTransform.position;
        }
        else
        {
            //////////Debug.LogError("R�f�rence manquante pour le joueur ou le cercueil.");
        }
        DeactivateOverview();
    }
}
