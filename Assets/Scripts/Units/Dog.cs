using UnityEngine;
using System.Collections;

public class Dog : Unit
{
    private AudioSource audioSource;
    private Animator animator;

    private bool isDeadCoroutineStarted = false;

    void Awake()
    {
        // Obtenez la r�f�rence � l'AudioSource
        audioSource = GetComponent<AudioSource>();
        Debug.Log("Awake: AudioSource initialis�.");
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
            Debug.Log("Start: Animator initialis�.");
        }

        if (audioSource != null)
        {
            audioSource.Play();
            Debug.Log("Start: AudioSource jou�.");
        }
        else
        {
            Debug.LogWarning("AudioSource n'est pas attach� au GameObject.");
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
        Debug.Log("HandleDeath: Coroutine commenc�e.");
        if (animator != null)
        {
            animator.SetBool("dead", true);
            Debug.Log("HandleDeath: Animation de mort jou�e.");
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
