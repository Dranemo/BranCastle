using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Guard : Enemy
{
    // Start is called before the first frame update
    [SerializeField] GameObject projectilePrefab;



    private void Awake()
    {
        base.Awake();
    }
    void Start()
    {
        base.Start();

    }

    // Update is called once per frame
    void Update()
    {
        base.Update();
        //FollowPath();
    }


    protected override void Attack()
    {
        //GameManager.Instance.TakeDamage(damage);
        GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);



        if(state == State.AttackingPlayer)
        {
            projectile.GetComponent<ProjectileEnemy>().target = player;
        }
        else if(state == State.AttackingRitual)
        {
            projectile.GetComponent<ProjectileEnemy>().target = ritual;
        }
        else if(state == State.AttackingUnit)
        {
            projectile.GetComponent<ProjectileEnemy>().target = closestUnit;
        }

        projectile.GetComponent<ProjectileEnemy>().damage = damage;
    }
}   
