using UnityEngine;

//détecte si le joueur se trouve à proximité du cercueil
public class Coffin : MonoBehaviour
{
    private bool playerIsNear = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) 
        {
            //////////Debug.Log("Player is near");
            playerIsNear = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerIsNear = false;
        }
    }

    public bool CanInteract()
    {
        return playerIsNear;
    }
}


