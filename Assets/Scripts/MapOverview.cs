using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MapOverview : MonoBehaviour
{
    private CanvasGroup canvasGroup;
    public Transform coffinsParent;
    private List<GameObject> coffinIcons = new List<GameObject>();
    public GameObject coffinIconPrefab;
    float sceneWidth = 60 - (-33); // 93
    float sceneHeight = 60 - (-8); // 68
    void Awake()
    {
        // R�cup�re le composant CanvasGroup attach� au GameObject
        canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            Debug.LogError("CanvasGroup manquant sur l'objet.");
        }
    }
    void Start()
    {
        CreateCoffinIcons();
    }

    void CreateCoffinIcons()
    {
        Vector2 sceneBottomLeft = new Vector2(-10, -10);
        Vector2 sceneTopRight = new Vector2(10, 10);

        foreach (Transform coffin in coffinsParent)
        {
            GameObject icon = Instantiate(coffinIconPrefab, transform);
            coffinIcons.Add(icon);

            // Convertir la position du monde en position relative � la sc�ne
            float relativeX = (coffin.position.x - sceneBottomLeft.x) / (sceneTopRight.x - sceneBottomLeft.x);
            float relativeY = (coffin.position.y - sceneBottomLeft.y) / (sceneTopRight.y - sceneBottomLeft.y);

            // Convertir la position relative en position sur le canvas
            // Supposons que le canvas est configur� pour correspondre � la taille de la sc�ne
            RectTransform canvasRect = GetComponent<RectTransform>();
            float canvasX = relativeX * canvasRect.sizeDelta.x;
            float canvasY = relativeY * canvasRect.sizeDelta.y;

            // Positionner l'ic�ne
            RectTransform iconRect = icon.GetComponent<RectTransform>();
            iconRect.anchoredPosition = new Vector2(canvasX, canvasY);
        }
    }


    void Update()
    {
    }
    // Fonction pour activer l'overlay
    public void ActivateOverview()
    {
        if (canvasGroup != null)
        {
            canvasGroup.alpha = 1f; 
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
            canvasGroup.blocksRaycasts = false;
            canvasGroup.interactable = false;
        }
    }
void TeleportPlayer(Vector3 coffinPosition)
    {
        // Ajustez selon la position o� vous voulez que le joueur r�apparaisse
        Vector3 playerPosition = coffinPosition + Vector3.forward * 2; // Exemple de position devant le cercueil
        FindObjectOfType<PlayerMovement>().transform.position = playerPosition;
        FindObjectOfType<PlayerMovement>().EnableMovement();
    }
}


