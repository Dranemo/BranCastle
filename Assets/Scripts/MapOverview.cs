using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class MapOverview : MonoBehaviour
{
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
}
