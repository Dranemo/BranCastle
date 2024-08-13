using System.Collections;
using UnityEngine;

public class Imp : Unit
{
    [SerializeField] GameObject projectilePrefab;
    private bool isAttacking = false;

    protected override void Update()
    {
        if (!isAttacking && enemiesInRange.Count > 0)
        {
            StartCoroutine(AttackEnemy());
        }
    }

    protected override IEnumerator AttackEnemy()
    {
        isAttacking = true;

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
                    Debug.LogWarning("Le projectile n'a pas de Rigidbody2D");
                }
                yield return new WaitForSeconds(attackSpeed);
            }
            else
            {
                break;
            }
        }

        isAttacking = false;
    }
}

