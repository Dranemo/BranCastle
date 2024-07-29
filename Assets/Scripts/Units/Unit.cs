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
    private Coroutine attackCoroutine;
    public List<GameObject> enemiesInRange = new List<GameObject>();
    [SerializeField] private float delayBeforeAppearance = 2f; // Temps en secondes avant que l'unité apparaisse

    private SpriteRenderer spriteRenderer;
    public void TakeDamage(float damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Die();
        }
    }
    protected void Die()
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
                enemy.TakeDamage(damage);
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
        enemiesInRange.RemoveAll(item => item == null); // Nettoyer la liste des ennemis nuls
        if (enemiesInRange.Count == 0) return null;
        return enemiesInRange.OrderBy(e => (e.transform.position - transform.position).sqrMagnitude).FirstOrDefault();
    }

    void Start()
    {
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        AttackEnemy();
    }
}
