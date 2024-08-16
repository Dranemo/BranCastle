using UnityEngine;
using System.Collections;

public class Dog : Unit
{
    private AudioSource audioSource;
    private Animator animator;

    private bool isDeadCoroutineStarted = false;

    void Awake()
    {
        // Obtenez la référence à l'AudioSource
        audioSource = GetComponent<AudioSource>();
        Debug.Log("Awake: AudioSource initialisé.");
    }

    void Start()
    {
        animator = GetComponent<Animator>();
        if (animator == null)
        {
            Debug.LogError("Animator component not found on " + gameObject.name);
        }
        else
        {
            Debug.Log("Start: Animator initialisé.");
        }

        if (audioSource != null)
        {
            audioSource.Play();
            Debug.Log("Start: AudioSource joué.");
        }
        else
        {
            Debug.LogWarning("AudioSource n'est pas attaché au GameObject.");
        }
    }

    protected override void Update()
    {
        base.Update();
        Die();
    }

    protected override void Die()
    {
        //Debug.Log("Die: Health = " + health);
        if (health <= 0 && !isDeadCoroutineStarted)
        {
            //Debug.Log("Die: Lancement de la coroutine HandleDeath.");
            StartCoroutine(HandleDeath());
        }
    }

    private IEnumerator HandleDeath()
    {
        isDeadCoroutineStarted = true;
        Debug.Log("HandleDeath: Coroutine commencée.");
        if (animator != null)
        {
            animator.SetBool("dead", true);
            Debug.Log("HandleDeath: Animation de mort jouée.");
            yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
        }
        else
        {
            Debug.LogError("HandleDeath: Animator est null.");
        }
        Debug.Log("HandleDeath: Destruction du GameObject.");
        Destroy(gameObject);
    }
}
