using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Imp : Unit
{
    [SerializeField] GameObject projectilePrefab;
    private bool isAttacking = false;

    private void Update()
    {
        if (isAttacking)
        {
            Attack();
        }
    }

    void Attack()
    {
        GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
    }
}
