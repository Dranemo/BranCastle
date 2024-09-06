using System.Collections.Generic;
using UnityEngine;

public class DistanceEnemy : Enemy
{

    [SerializeField] protected bool searchingTarget = false;
    [SerializeField] List<GameObject> projectiles = new List<GameObject>();

    new private void Awake()
    {
        base.Awake();

        foreach (Transform proj in transform.Find("Projectiles"))
        {
            projectiles.Add(proj.gameObject);
        }

        foreach (GameObject proj in projectiles)
        {
            proj.SetActive(false);
            proj.GetComponent<ProjectileEnemy>().damage = damage;
        }
    }
    new void Start()
    {
        base.Start();

    }

    // Update is called once per frame
    new void Update()
    {
        base.Update();
        //FollowPath();
    }


    protected override void Attack()
    {
        //GameManager.Instance.TakeDamage(damage);
        //GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
        GameObject projectile = null;

        foreach (GameObject proj in projectiles)
        {
            if (proj.activeSelf == false)
            {
                projectile = proj;
                break;
            }
        }




        if (state == State.AttackingPlayer)
        {
        //////////Debug.Log("Player");
            projectile.GetComponent<ProjectileEnemy>().target = player;
        }
        else if (state == State.AttackingRitual)
        {
        //////////Debug.Log("Ritual");
            projectile.GetComponent<ProjectileEnemy>().target = ritual;
        }
        else if (state == State.AttackingUnit && gameObject.layer != 7)
        {
            //////////Debug.Log("Unit");
            projectile.GetComponent<ProjectileEnemy>().target = closestUnit;
        }
        else
        {
            if (projectile.GetComponent<ProjectileEnemy>().target!=null)
            {
                projectile.GetComponent<ProjectileEnemy>().target = targetEnemy;
            }
            ////////Debug.Log("Enemy");
            projectile.GetComponent<ProjectileEnemy>().target = targetEnemy;
        }


        projectile.transform.position = transform.position;
        projectile.GetComponent<ProjectileEnemy>().searchingTarget = searchingTarget;

        projectile.GetComponent<ProjectileEnemy>().Activate();
    }
}
