using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Rendering.UI;

public class Pretre : Enemy
{
    [SerializeField] private GameObject enemyToHeal;


    new void Awake()
    {
        base.Awake();
    }

    new void Start()
    {
        base.Start();
    }   

    // Update is called once per frame
    new void Update()
    {



        UpdateEnemyList();


        if (animator != null)
            animator.SetBool("Idle", Vector3.Distance(transform.position, positionFrameBefore) == 0);

        positionFrameBefore = transform.position;



        enemyToHeal = GetClosestEnemyToHeal();


        if ((animator != null && !animator.GetCurrentAnimatorStateInfo(0).IsName("Hit")) || animator == null)
        {



            if (isHypnotized)
            {

            }

            else
            {
                if (isStunned)
                {
                    StartCoroutine(Stunned());
                }

                else if (enemyToHeal != null)
                {
                    if (Vector3.Distance(transform.position, enemyToHeal.transform.position) < detectionRadius)
                    {
                        RaycastHit2D hit = Physics2D.Linecast(transform.position, enemyToHeal.transform.position, layerMaskRaycast);


                        if (hit.collider == null)
                        {
                            DetectEnemy();
                            Debug.DrawLine(transform.position, player.transform.position, Color.green);
                        }
                        else
                        {
                            MovingToTheNextCheckpoint();
                            Debug.DrawLine(transform.position, player.transform.position, Color.red);
                        }
                    }
                }


                else
                {
                    ////////Debug.Log("Player Not Detected");
                    MovingToTheNextCheckpoint();

                }
            }

            attackCooldown -= Time.deltaTime;
        }

    }




    protected void DetectEnemy()
    {
        onPath = false;

        TurningSprite(player.transform.position);

        if(enemyToHeal != null)
        {

            if (Vector3.Distance(transform.position, enemyToHeal.transform.position) < attackRadius)
            {
                state = State.AttackingPlayer;

                if (attackCooldown <= 0 && health > 0)
                {
                    Heal();
                    attackCooldown = attackSpeed;
                }
            }
            else
            {
                state = State.Moving;
                transform.position = Vector3.MoveTowards(transform.position, enemyToHeal.transform.position, speed * Time.deltaTime);
            }
        }
        else
        {
            state = State.Moving;
            transform.position = Vector3.MoveTowards(transform.position, enemyToHeal.transform.position, speed * Time.deltaTime);
        }
    }





    

    void Heal()
    {
        if(enemyToHeal != null)
        {
            animator.Play("Attack1");
            if(enemyToHeal.GetComponent<Enemy>().health + damage < enemyToHeal.GetComponent<Enemy>().maxHealth)
                enemyToHeal.GetComponent<Enemy>().health += damage;
            else
            {
                enemyToHeal.GetComponent<Enemy>().health = enemyToHeal.GetComponent<Enemy>().maxHealth;
            }
        }
    }




    private GameObject GetClosestEnemyToHeal()
    {
        List<GameObject> enemiesToHeal = new List<GameObject>();

        // Trouver l'ennemi le plus proche
        GameObject closestEnemy = null;
        float closestDistance = Mathf.Infinity;


        for (int i = 0; i < enemies.Count; i++)
        {
            if (enemies[i] != null)
            {

                float distance = Vector3.Distance(transform.position, enemies[i].transform.position);

                if (distance < closestDistance && CheckEnemyWall(enemies[i]) && enemies[i].GetComponent<Enemy>().health < enemies[i].GetComponent<Enemy>().maxHealth && enemies[i] != gameObject)
                {
                    closestDistance = distance;
                    closestEnemy = enemies[i];
                }
            }
        }

        //Debug.Log("Closest enemy to heal: " + closestEnemy);

        return closestEnemy;
    }
}
