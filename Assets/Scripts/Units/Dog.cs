using UnityEngine;
using System.Collections;

public class Dog : Unit
{
    private AudioSource audioSource;
    private Animator animator;
    [SerializeField] private AudioClip deathSound;
    private bool isDeadCoroutineStarted = false;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    protected override void Start()
    {
        base.Start();
        animator = GetComponent<Animator>();

        if (audioSource != null)
        {
            audioSource.Play();
        }
    }

    protected override void Update()
    {
        base.Update();
        Die();
    }
    protected override IEnumerator AttackEnemy()
    {
        while (enemiesInRange.Count > 0)
        {
            GameObject closestEnemy = GetClosestEnemy();
            if (closestEnemy != null)
            {
                animator.SetBool("isAttacking", true);
                Enemy enemy = closestEnemy.GetComponent<Enemy>();
                enemy.TakeDamage(damage);
                yield return new WaitForSeconds(attackSpeed);
            }
            else
            {
                yield break;
            }
        }
        attackCoroutine = null;
        animator.SetBool("isAttacking", false); 
    }
    protected override void Die()
    {
        if (health <= 0 && !isDeadCoroutineStarted)
        {
            StartCoroutine(HandleDeath());
        }
    }

    private IEnumerator HandleDeath()
    {
        isDeadCoroutineStarted = true;
        if (animator != null)
        {
            animator.SetBool("dead", true);
            if (deathSound != null && audioSource != null)
            {
                audioSource.PlayOneShot(deathSound);
            }
            else
            {
                ////Debug.LogWarning("HandleDeath: deathSound ou audioSource est null.");
            }
            yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
        }
        else
        {
            ////Debug.LogError("HandleDeath: Animator est null.");
        }
        Destroy(gameObject);
    }

}
