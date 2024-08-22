using UnityEngine;
using System.Collections;
public class Ghoul : Unit
{
    private Animator animator;
    private bool isDeadCoroutineStarted = false;
    [SerializeField] private AudioClip deathSound;
    private AudioSource audioSource;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
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
        isDeadCoroutineStarted = true;
        Debug.Log("isDeadCoroutineStarted = " + isDeadCoroutineStarted);
        animator.SetBool("dead", true);
        Debug.Log("dead :"+animator.GetBool("dead"));
        audioSource.clip = deathSound;
        Debug.Log("deathSound :"+audioSource.clip);
        audioSource.Play();
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
        Destroy(gameObject);
    }
    protected override IEnumerator AttackEnemy()
    {
        while (enemiesInRange.Count > 0)
        {
            animator.SetBool("isAttacking", true);
            GameObject closestEnemy = GetClosestEnemy();
            if (closestEnemy != null)
            {
                Enemy enemy = closestEnemy.GetComponent<Enemy>();
                enemy.TakeDamage(damage);
                yield return new WaitForSeconds(attackSpeed);
            }
            else
            {
                yield break;
            }
        }
        animator.SetBool("isAttacking", false);
        attackCoroutine = null;
    }
    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }
}
