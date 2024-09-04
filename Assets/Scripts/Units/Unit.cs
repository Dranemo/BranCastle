using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Unit : MonoBehaviour
{
    [SerializeField] protected float health;
    [SerializeField] protected float damage;
    [SerializeField] protected float attackSpeed;
    [SerializeField] protected float bloodCost;
    protected Coroutine attackCoroutine;
    public List<GameObject> enemiesInRange = new List<GameObject>();

    private SpriteRenderer spriteRenderer;

    protected virtual void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
        {
            //////Debug.LogError("SpriteRenderer is not attached to the GameObject.");
        }
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Die();
        }
    }

    protected virtual void Die()
    {
        StopAttack();
        Destroy(gameObject);
    }

    public void StartAttack(GameObject enemy)
    {
        if (attackCoroutine == null)
        {
            attackCoroutine = StartCoroutine(AttackEnemy());
        }
    }

    public void StopAttack()
    {
        if (attackCoroutine != null)
        {
            StopCoroutine(attackCoroutine);
            attackCoroutine = null;
        }
    }

    protected virtual IEnumerator AttackEnemy()
    {
        while (enemiesInRange.Count > 0)
        {
            GameObject closestEnemy = GetClosestEnemy();
            if (closestEnemy != null)
            {
                Enemy enemy = closestEnemy.GetComponent<Enemy>();
                enemy.TakeDamage(damage, false);
                yield return new WaitForSeconds(attackSpeed);
            }
            else
            {
                yield break;
            }
        }

        attackCoroutine = null;
    }

    protected GameObject GetClosestEnemy()
    {
        enemiesInRange.RemoveAll(item => item == null);
        if (enemiesInRange.Count == 0) return null;
        return enemiesInRange.OrderBy(e => (e.transform.position - transform.position).sqrMagnitude).FirstOrDefault();
    }

    public float GetHealthUnit()
    {
        return health;
    }

    protected virtual void Update()
    {
        if (enemiesInRange.Count > 0)
        {
            FlipSprite();
        }
        AttackEnemy();
    }

    private void FlipSprite()
    {
        GameObject closestEnemy = GetClosestEnemy();
        //////Debug.Log("Closest enemy: " + (closestEnemy != null ? closestEnemy.name : "None"));

        if (closestEnemy != null)
        {
            //////Debug.Log("Closest enemy position: " + closestEnemy.transform.position);
            //////Debug.Log("Unit position: " + transform.position);

            if (closestEnemy.transform.position.x < transform.position.x)
            {
                spriteRenderer.flipX = true;
                //////Debug.Log("Flipping sprite to face left.");
            }
            else
            {
                spriteRenderer.flipX = false;
                //////Debug.Log("Flipping sprite to face right.");
            }
        }
        else
        {
            //////Debug.Log("No closest enemy found.");
        }
    }

}

