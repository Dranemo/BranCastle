using UnityEngine;
using System.Collections;

public class MapOverview : MonoBehaviour
{
    public Camera mainCamera;
    private Vector3 originalCameraPosition;
    private Quaternion originalCameraRotation;
    private bool isOverviewActive = false;
    public AnimationCurve zoomCurve;

    private float initialOrthographicSize;
    void Update()
    {
        if (isOverviewActive)
        {
            if (Input.GetMouseButtonDown(0)) // Clic gauche pour choisir un cercueil
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.collider.GetComponent<Coffin>() != null)
                    {
                        TeleportPlayer(hit.collider.transform.position);
                    }
                }
            }
        }
    }

    public void ActivateOverview()
    {
        originalCameraPosition = mainCamera.transform.position;
        originalCameraRotation = mainCamera.transform.rotation;
        initialOrthographicSize = mainCamera.orthographicSize;
        // Démarrez la coroutine pour ajuster progressivement le zooms
        StartCoroutine(AdjustZoom());
    }
    public void DeactivateOverview(Vector3 coffinPosition)
    {
        mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, coffinPosition, 0.5f); 
        mainCamera.transform.rotation = originalCameraRotation;
        StartCoroutine(ResetZoom());
    }
    IEnumerator AdjustZoom()
    {
        float targetOrthographicSize = mainCamera.orthographicSize + 60; // Ajustez selon le zoom désiré
        float duration = 0.5f; // Durée sur laquelle le zoom se produit
        float elapsed = 0.0f;

        zoomCurve.preWrapMode = WrapMode.ClampForever;
        zoomCurve.postWrapMode = WrapMode.ClampForever;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;
            // Utilisez la courbe d'animation pour ajuster le facteur d'interpolation
            float curveT = zoomCurve.Evaluate(t);
            mainCamera.orthographicSize = Mathf.LerpUnclamped(initialOrthographicSize, targetOrthographicSize, curveT);

            yield return null;
        }

        // Assurez-vous que la taille orthographique est bien celle ciblée à la fin
        mainCamera.orthographicSize = targetOrthographicSize;
    }

    IEnumerator ResetZoom()
    {
        float currentOrthographicSize = mainCamera.orthographicSize; // La taille actuelle au début de la coroutine
        float duration = 0.5f; // Durée sur laquelle le zoom inverse se produit
        float elapsed = 0.0f;

        zoomCurve.preWrapMode = WrapMode.ClampForever;
        zoomCurve.postWrapMode = WrapMode.ClampForever;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;
            // Utilisez la courbe d'animation pour ajuster le facteur d'interpolation
            float curveT = zoomCurve.Evaluate(t);
            mainCamera.orthographicSize = Mathf.LerpUnclamped(currentOrthographicSize, initialOrthographicSize, curveT);

            yield return null;
        }

        // Assurez-vous que la taille orthographique est bien celle initiale à la fin
        mainCamera.orthographicSize = initialOrthographicSize;
    }


    void TeleportPlayer(Vector3 coffinPosition)
    {
        // Ajustez selon la position où vous voulez que le joueur réapparaisse
        Vector3 playerPosition = coffinPosition + Vector3.forward * 2; // Exemple de position devant le cercueil
        FindObjectOfType<PlayerMovement>().transform.position = playerPosition;

        // Restaure la position et rotation originale de la caméra
        mainCamera.transform.position = originalCameraPosition;
        mainCamera.transform.rotation = originalCameraRotation;
        FindObjectOfType<PlayerMovement>().EnableMovement();
    }
}

