using UnityEngine;
using System.Collections;

public class LightDamage : MonoBehaviour
{
    public GameManager gameManager;
    private WaitForSecondsRealtime wait = new WaitForSecondsRealtime(1);
    private Coroutine damageCoroutine;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            gameManager.isPlayerInLight = true;
            if (damageCoroutine == null)
            {
                damageCoroutine = StartCoroutine(DamagePlayerOverTime());
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            gameManager.isPlayerInLight = false;
            if (damageCoroutine != null)
            {
                StopCoroutine(damageCoroutine);
                damageCoroutine = null;
            }
        }
    }

    private IEnumerator DamagePlayerOverTime()
    {
        yield return wait;

        if (gameManager.isPlayerInLight)
        {
            while (gameManager.isPlayerInLight)
            {
                gameManager.TakeDamage(5);
                yield return null; 
            }
        }

        damageCoroutine = null;
    }
}
