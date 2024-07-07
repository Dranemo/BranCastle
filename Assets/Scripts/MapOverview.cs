using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class MapOverview : MonoBehaviour
{
    private CanvasGroup canvasGroup;

    void Awake()
    {
        // Obtenez la r�f�rence au CanvasGroup attach� au m�me GameObject
        canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            Debug.LogError("CanvasGroup n'est pas trouv� sur le GameObject.");
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

    // Fonction pour d�sactiver l'overlay (optionnelle)
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
}
