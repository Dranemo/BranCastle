using UnityEngine;

public class LightDamage : MonoBehaviour
{
    public GameManager gameManager; // R�f�rence � votre GameManager

    void OnTriggerEnter2D(Collider2D other)
    {
        // V�rifiez si l'objet qui est entr� dans le trigger est le joueur
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("Player touched the light");
            // R�duisez la valeur de blood dans votre GameManager
            gameManager.AddBlood(-5);
        }
    }
}


