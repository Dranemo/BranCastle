using UnityEngine;

//d�tecte si le joueur se trouve � proximit� du cercueil
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


