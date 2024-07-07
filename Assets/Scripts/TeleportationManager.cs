using UnityEngine;

public class TeleportationManager : MonoBehaviour
{
    public Transform playerTransform;

    public void TeleportToCoffin(Transform coffinTransform)
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
    }
}
