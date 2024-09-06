using System.Collections;
using UnityEngine;

public class Imp : Unit
{
    [SerializeField] GameObject projectilePrefab;
    private bool isAttacking = false;
    private Animator animator;
    private bool isDeadCoroutineStarted = false;
    private AudioSource audioSource;
    protected override void Start()
    {
        base.Start();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }
    protected override void Update()
    {
        base.Update();
        if (!isAttacking && enemiesInRange.Count > 0)
        {
            StartCoroutine(AttackEnemy());
        }
        Die();
    }
    override protected void Die()
    {
        if (health <= 0 && !isDeadCoroutineStarted)
        {
            StartCoroutine(HandleDeath());
        }
    }
    private IEnumerator HandleDeath()
    {
        //////////Debug.Log("HandleDeath: Coroutine commencée.");
        isDeadCoroutineStarted = true;
        //////////Debug.Log("HandleDeath: isDeadCoroutineStarted = " + isDeadCoroutineStarted);

        if (animator != null)
        {
            animator.SetBool("dead", true);
            //////////Debug.Log("HandleDeath: Animation de mort déclenchée.");
        }
        else
        {
            //////////Debug.LogError("HandleDeath: Animator est null.");
        }

        if (animator != null)
        {
            float animationLength = animator.GetCurrentAnimatorStateInfo(0).length;
            //////////Debug.Log("HandleDeath: Durée de l'animation de mort = " + animationLength);
            yield return new WaitForSeconds(animationLength);
        }
        else
        {
            //////////Debug.LogError("HandleDeath: Impossible d'obtenir la durée de l'animation car l'Animator est null.");
        }

        //////////Debug.Log("HandleDeath: Destruction du GameObject.");
        Destroy(gameObject);
    }

    protected override IEnumerator AttackEnemy()
    {
        isAttacking = true;
        animator.SetBool("isAttacking", true);
        while (enemiesInRange.Count > 0)
        {
            GameObject closestEnemy = GetClosestEnemy();
            if (closestEnemy != null)
            {
                Transform enemyTransform = closestEnemy.transform;

                // Instancier le projectile
                GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
                

                // Calculer la direction vers l'ennemi
                Vector3 direction = (enemyTransform.position - transform.position).normalized;
                
                Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
                if (rb != null)
                {
                    rb.velocity = direction * 10f;
                }
                else
                {
                    //////////Debug.LogWarning("Le projectile n'a pas de Rigidbody2D");
                }
                yield return new WaitForSeconds(attackSpeed);
            }
            else
            {
                break;
            }
        }

        isAttacking = false;

        animator.SetBool("isAttacking", false);
    }
}

