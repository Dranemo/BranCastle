using UnityEngine;
using System.Collections;

public class LightDamage : MonoBehaviour
{
    public GameManager gameManager;
    private WaitForSecondsRealtime wait = new WaitForSecondsRealtime(1);
    private Coroutine damageCoroutine;
    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

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
        while (gameManager.isPlayerInLight)
        {
            audioSource.Play();
            gameManager.TakeDamage(10, true);
            yield return wait; 
        }

        damageCoroutine = null;
    }
}
