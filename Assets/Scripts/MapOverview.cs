using UnityEngine;

public class MapOverview : MonoBehaviour
{
    public Camera mainCamera;
    private Vector3 originalCameraPosition;
    private Quaternion originalCameraRotation;
    private bool isOverviewActive = false;

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
        for (int i=0; i < 30; i++)
        {
            mainCamera.orthographicSize += 2;
        }
        

        isOverviewActive = true;
    }

    void TeleportPlayer(Vector3 coffinPosition)
    {
        // Ajustez selon la position où vous voulez que le joueur réapparaisse
        Vector3 playerPosition = coffinPosition + Vector3.forward * 2; // Exemple de position devant le cercueil
        FindObjectOfType<PlayerMovement>().transform.position = playerPosition;

        // Restaure la position et rotation originale de la caméra
        mainCamera.transform.position = originalCameraPosition;
        mainCamera.transform.rotation = originalCameraRotation;

        isOverviewActive = false;
    }
}

