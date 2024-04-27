using UnityEngine;

public class LightDamage : MonoBehaviour
{
    public GameManager gameManager; // Référence à votre GameManager

    void OnTriggerEnter2D(Collider2D other)
    {
        // Vérifiez si l'objet qui est entré dans le trigger est le joueur
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("Player touched the light");
            // Réduisez la valeur de blood dans votre GameManager
            gameManager.AddBlood(-5);
        }
    }
}


