using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public float health; 
    public float damage; 
    public float attackSpeed;
    public float bloodCost;
    private Coroutine attackCoroutine;
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
            attackCoroutine = StartCoroutine(AttackEnemy(enemy));
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
    private IEnumerator AttackEnemy(GameObject enemy)
    {
        while (enemy != null && enemy.activeInHierarchy) // Continuez d'attaquer tant que l'ennemi est présent
        {
            // Ici, vous pouvez ajouter la logique pour infliger des dégâts à l'ennemi
            Debug.Log($"Attaque l'ennemi pour {damage} dégâts.");

            yield return new WaitForSeconds(1f / attackSpeed); // Attendre la prochaine attaque basée sur la vitesse d'attaque
        }

        attackCoroutine = null; // Assurez-vous de réinitialiser la coroutine si l'ennemi est détruit
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
