using UnityEngine;

public class TeleportationManager : MonoBehaviour
{
    private Camera playerCamera;
    private Transform playerTransform;

    void Awake()
    {
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null)
        {
            playerTransform = playerObject.transform;
        }
        else
        {
            ////////Debug.LogError("Aucun objet avec le tag 'Player' trouvé.");
        }

        GameObject cameraObject = GameObject.Find("Main Camera");
        if (cameraObject != null)
        {
            playerCamera = cameraObject.GetComponent<Camera>();
            if (playerCamera == null)
            {
                ////////Debug.LogError("L'objet 'Main Camera' n'a pas de composant Camera.");
            }
        }
    }

    public void TeleportToCoffin(Transform coffinTransform)
    {
        if (playerTransform != null && coffinTransform != null)
        {
            playerTransform.position = coffinTransform.position + new Vector3(0, -1, 0);
        }
        else
        {
            ////////Debug.LogError("Référence manquante pour le joueur ou le cercueil.");
        }
    }

}
