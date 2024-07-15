using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class MapOverview : MonoBehaviour
{
    public Camera minimapCamera; // La caméra qui projette la minimap
    public Transform playerTransform; // Le transform du joueur
    public LayerMask coffinLayerMask; // Le layer des cercueils
    private CanvasGroup canvasGroup;
    

    void Awake()
    {
        // Obtenez la référence au CanvasGroup attaché au même GameObject
        canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            Debug.LogError("CanvasGroup n'est pas trouvé sur le GameObject.");
        }
    }

    public void ActivateOverview()
    {
        Debug.Log(canvasGroup);
        if (canvasGroup != null)
        {
            canvasGroup.alpha = 1f;
            Debug.Log(canvasGroup.alpha);
            canvasGroup.blocksRaycasts = true;
            canvasGroup.interactable = true;
        }
    }

    // Fonction pour désactiver l'overlay (optionnelle)
    public void DeactivateOverview()
    {
        if (canvasGroup != null)
        {
            canvasGroup.alpha = 0f;
            Debug.Log(canvasGroup.alpha);
            canvasGroup.blocksRaycasts = false;
            canvasGroup.interactable = false;
        }
    }
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = minimapCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.GetRayIntersection(ray, Mathf.Infinity, coffinLayerMask);

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

    void TeleportToCoffin(Transform coffinTransform)
    {
        if (playerTransform != null && coffinTransform != null)
        {
            playerTransform.position = coffinTransform.position;
            Debug.Log("Joueur téléporté au cercueil : " + coffinTransform.name);
        }
        else
        {
            Debug.LogError("Référence manquante pour le joueur ou le cercueil.");
        }
        DeactivateOverview();
    }
}
