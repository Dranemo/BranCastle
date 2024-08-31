using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Rendering.UI;

public class Pretre : Enemy
{
    [SerializeField] List<GameObject> enemies = new List<GameObject>();
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
                    //Debug.Log("Player Not Detected");
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





    void UpdateEnemyList()
    {
        // Réinitialiser la liste des ennemis
        enemies.Clear();

        // Trouver tous les GameObjects avec le tag "Enemy" et les ajouter à la liste
        List<GameObject> enemiesTemp = new List<GameObject>();
        enemiesTemp.AddRange(GameObject.FindGameObjectsWithTag("Enemy"));

        foreach (GameObject enemy in enemiesTemp)
        {
            if(enemy.GetComponent<Enemy>())
                enemies.Add(enemy);
        }
    }

    GameObject GetClosestEnemyToHeal()
    {
        List<GameObject> enemiesToHeal = new List<GameObject>();

        // Trouver l'ennemi le plus proche
        GameObject closestEnemy = null;
        float closestDistance = Mathf.Infinity;

        foreach (GameObject enemy in enemies)
        {
            float distance = Vector3.Distance(transform.position, enemy.transform.position);

            if (distance < closestDistance && CheckEnemyWall(enemy) && enemy.GetComponent<Enemy>().health != enemy.GetComponent<Enemy>().maxHealth)
            {
                closestDistance = distance;
                closestEnemy = enemy;
            }
        }

        return closestEnemy;
    }

    bool CheckEnemyWall(GameObject enemyChecking)
    {
        Vector3 pos = transform.position;
        Vector3 posNext = enemyChecking.transform.position;

        Vector2 size = new Vector2(0.5f, Vector2.Distance(pos, posNext));
        float angle = Mathf.Atan2(posNext.y - pos.y, posNext.x - pos.x) * Mathf.Rad2Deg;


        RaycastHit2D hit = Physics2D.Linecast(pos, posNext, layerMaskRaycast);
        //RaycastHit2D hit = Physics2D.BoxCast(pos, size, angle, posNext);

        if (hit.collider != null && hit.collider.gameObject.GetComponent<Enemy>() == null)
        {
            //Debug.DrawLine(pos, posNext, Color.red);
            return false;
        }
        else
        {
            Debug.DrawLine(pos, posNext, Color.green);
            return true;
        }
    }

    void Heal()
    {
        if(enemyToHeal != null)
        {
            if(enemyToHeal.GetComponent<Enemy>().health + damage < enemyToHeal.GetComponent<Enemy>().maxHealth)
                enemyToHeal.GetComponent<Enemy>().health += damage;
            else
            {
                enemyToHeal.GetComponent<Enemy>().health = enemyToHeal.GetComponent<Enemy>().maxHealth;
            }
        }
    }
}
