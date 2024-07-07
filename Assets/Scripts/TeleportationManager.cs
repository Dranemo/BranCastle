using UnityEngine;

public class TeleportationManager : MonoBehaviour
{
    public Transform playerTransform;

    public void TeleportToCoffin(Transform coffinTransform)
    {
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
