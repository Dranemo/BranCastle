using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public float health; 
    public float damage; 
    public float attackSpeed;
    public float bloodCost;

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
        Destroy(gameObject);
    }   
void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
