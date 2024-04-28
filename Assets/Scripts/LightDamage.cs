using UnityEngine;

public class LightDamage : MonoBehaviour
{
    public GameManager gameManager;

    private void OnTriggerStay2D(Collider2D collision)
    {
        // V�rifiez si l'objet qui est entr� dans le trigger est le joueur
        if (collision.gameObject.CompareTag("Player"))
        {
            gameManager.isPlayerInLight = true;
            gameManager.AddBlood(-50);
        }
    }
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            gameManager.isPlayerInLight = false;
        }
    }
}


