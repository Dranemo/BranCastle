using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Unit : MonoBehaviour
{
    public float health; 
    public float damage; 
    public float attackSpeed;
    public float bloodCost;
    private Coroutine attackCoroutine;
    public List<GameObject> enemiesInRange = new List<GameObject>();

    protected void TakeDamage(float damage)
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
    private IEnumerator AttackEnemy()
    {
        while (enemiesInRange.Count > 0)
        {
            GameObject closestEnemy = GetClosestEnemy();
            if (closestEnemy != null)
            {
                Enemy enemy = closestEnemy.GetComponent<Enemy>();
                enemy.TakeDamage(damage);
                yield return new WaitForSeconds(1f / attackSpeed);
            }
            else
            {
                yield break; // Sortir de la coroutine si aucun ennemi n'est trouvé
            }
        }

        attackCoroutine = null;
    }

    private GameObject GetClosestEnemy()
    {
        enemiesInRange.RemoveAll(item => item == null); // Nettoyer la liste des ennemis nuls
        if (enemiesInRange.Count == 0) return null;
        Debug.Log(enemiesInRange.OrderBy(e => (e.transform.position - transform.position).sqrMagnitude).FirstOrDefault());
        return enemiesInRange.OrderBy(e => (e.transform.position - transform.position).sqrMagnitude).FirstOrDefault();
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
