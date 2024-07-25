using UnityEngine;
using System.Collections;

public class TeleportationManager : MonoBehaviour
{
    private Camera playerCamera; 
    private Transform playerTransform;


    void Awake()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        playerCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
    }
    public void TeleportToCoffin(Transform coffinTransform)
    {
        if (playerTransform != null && coffinTransform != null)
        {
            //StartCoroutine(MoveCameraToCoffin(coffinTransform.position));

            playerTransform.position = coffinTransform.position;
        }
        else
        {
            Debug.LogError("Référence manquante pour le joueur ou le cercueil.");
        }
    }
    //private IEnumerator MoveCameraToCoffin(Vector3 targetPosition)
    //{
    //    float duration = 0.2f; 
    //    float elapsedTime = 0;
    //    Vector3 startPosition = playerCamera.transform.position;
    //
    //    while (elapsedTime < duration)
    //    {
    //        playerCamera.transform.position = Vector3.Lerp(startPosition, new Vector3(targetPosition.x, startPosition.y, targetPosition.z), (elapsedTime / duration));
    //        elapsedTime += Time.deltaTime; 
    //        yield return null; 
    //    }
    //    playerCamera.transform.position = new Vector3(targetPosition.x, startPosition.y, targetPosition.z);
    //}
}
