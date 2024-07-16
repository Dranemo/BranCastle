using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;



public class Enemy : MonoBehaviour
{
    public List<Path> paths;
    private int currentPathIndex = 0;
    public Transform spawn;
    private int currentWaypointIndex = 0;
    protected float health=50;

    protected GameObject player;
    protected GameObject ritual;

    float attackCooldown = 1;
    protected State state;


    protected enum State
    {
        Moving,
        AttackingPlayer,
        AttackingRitual,
        AttackingUnit
    }
    private enum Direction
    {
        Up,
        Down,
        Left,
        Right
    }



    [SerializeField] protected GameObject bloodPrefab;


    [SerializeField] protected float bloodCount = 10;
    [SerializeField] protected float detectionRadius = 5f; 
    [SerializeField] protected float attackRadius = 1.5f;
    [SerializeField] protected float damage = 0;
    [SerializeField] protected float speed = 2;
    [SerializeField] protected float attackSpeed = 1;
    [SerializeField] protected List<GameObject> units;
    [SerializeField] protected GameObject closestUnit;


    [SerializeField] Sprite upSprite;
    [SerializeField] Sprite downSprite;
    [SerializeField] Sprite leftSprite;
    [SerializeField] Sprite rightSprite;

    [Header("Status")]
    [SerializeField] public bool isStunned;
    [SerializeField] public float stunDuration;
    [SerializeField] public float capeKnockback;
    [SerializeField] public bool isHypnotized;



    private bool onPath = true;



    protected void Awake()
    {
    }

    protected void Start()
    {
        units = new List<GameObject>();

        //utez vos waypoints � la liste ici, ou initialisez-les dans l'inspecteur Unity
        player = GameObject.FindGameObjectWithTag("Player");
        capeKnockback = player.GetComponent<PlayerMovement>().capePushForce;
        ritual = GameObject.FindGameObjectWithTag("Ritual");
    }
    
    


    protected void Update()
    {
        //Debug.Log(currentWaypointIndex);

        UpdateUnitList();
        float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
        bool isUnitClose = IsUnitClose();

        if (isStunned)
        {
            StartCoroutine(Stunned());
        }
        else if (isUnitClose || distanceToPlayer < detectionRadius)
        {
            if (isUnitClose)
            {
                DetectUnit();
            }
            if (distanceToPlayer < detectionRadius)
            {
                DetectPlayer();
                //Debug.Log("Player Detected");

            }
        }
        else
        {
            //Debug.Log("Player Not Detected");
            MovingToTheNextCheckpoint();

        }


        attackCooldown -= Time.deltaTime;
    }


    void MovingToTheNextCheckpoint()
    {
        state = State.Moving;


        if (paths == null || !paths.Any() || paths[0].waypoints.Count == 0)
            return;




        if (!onPath)
        {
            float leastDistance = 0;

            for (int i = 0; i < paths[0].waypoints.Count; i++)
            {
                if (leastDistance == 0 || Vector3.Distance(transform.position, paths[0].waypoints[i].position) < leastDistance)
                {
                    leastDistance = Vector3.Distance(transform.position, paths[0].waypoints[i].position);
                    currentWaypointIndex = i;
                }


            }
        }



        Transform currentWaypoint = paths[0].waypoints[currentWaypointIndex];
        TurningSprite(currentWaypoint.position);


        if (currentWaypointIndex == paths[0].waypoints.Count - 1)
        {
            DetectRitual();
        }
        else
        {
            // Aller vers le waypoint
            transform.position = Vector3.MoveTowards(transform.position, currentWaypoint.position, speed * Time.deltaTime);

        }




        if (transform.position == currentWaypoint.position)
        {
            if (currentWaypointIndex != paths[0].waypoints.Count - 1)
            {
                currentWaypointIndex++;
            }

            onPath = true;
        }
    }






    protected void DetectPlayer()
    {
        onPath = false;



        /*Vector3 directionToPlayer = player.transform.position - transform.position;
        float angle = Mathf.Atan2(directionToPlayer.y, directionToPlayer.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);*/
        TurningSprite(player.transform.position);


        if (Vector3.Distance(transform.position, player.transform.position) < attackRadius)
        {
            state = State.AttackingPlayer;

            if (attackCooldown <= 0)
            {
                Attack();
                attackCooldown = attackSpeed;
            }
        }
        else
        {
            state = State.Moving;
            transform.position = Vector3.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
        }
    }

    protected void DetectRitual()
    {
        if(Vector3.Distance(transform.position, paths[0].waypoints[currentWaypointIndex].position) < attackRadius)
        {
            state = State.AttackingRitual;

            attackCooldown -= Time.deltaTime;
            if (attackCooldown <= 0)
            {
                Attack();
                attackCooldown = attackSpeed;
            }
        }
        else
        {
            state = State.Moving;
            transform.position = Vector3.MoveTowards(transform.position, paths[0].waypoints[currentWaypointIndex].position, speed * Time.deltaTime);
        }
    }

    protected virtual void Attack()
    {
        Debug.Log("Attack");
    }





    void TurningSprite(Direction dir)
    {
        switch(dir)
        {
            case Direction.Up:
                GetComponent<SpriteRenderer>().sprite = upSprite;
                break;
            case Direction.Down:
                GetComponent<SpriteRenderer>().sprite = downSprite;
                break;
            case Direction.Left:
                GetComponent<SpriteRenderer>().sprite = leftSprite;
                break;
            case Direction.Right:
                GetComponent<SpriteRenderer>().sprite = rightSprite;
                break;
        }
    }
    void TurningSprite(Vector3 pos)
    {
        Vector3 direction = pos - transform.position;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        if (angle > -45 && angle <= 45)
        {
            GetComponent<SpriteRenderer>().sprite = rightSprite;
        }
        else if (angle > 45 && angle <= 135)
        {
            GetComponent<SpriteRenderer>().sprite = upSprite;
        }
        else if (angle > 135 || angle <= -135)
        {
            GetComponent<SpriteRenderer>().sprite = leftSprite;
        }
        else if (angle > -135 && angle <= -45)
        {
            GetComponent<SpriteRenderer>().sprite = downSprite;
        }
    }


    protected void UpdateUnitList()
    {
        // Réinitialiser la liste des unités
        units.Clear();

        // Trouver tous les GameObjects avec le tag "Unit" et les ajouter à la liste
        units.AddRange(GameObject.FindGameObjectsWithTag("Unit"));
    }
    protected bool IsUnitClose()
    {
        foreach (GameObject unit in units)
        {
            float distance = Vector3.Distance(transform.position, unit.transform.position);
            if (distance < detectionRadius)
            {
                return true;
            }
        }
        return false;
    }
    protected void DetectUnit()
    {
        onPath = false;
        float closestDistance = Mathf.Infinity;
        foreach (GameObject unit in units)
        {
            float distance = Vector3.Distance(transform.position, unit.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestUnit = unit;
            }
        }

        if (closestUnit != null)
        {
            Vector3 invertedDirection = transform.position - closestUnit.transform.position;
            Vector3 targetPosition = transform.position + invertedDirection;
            TurningSprite(targetPosition);

            if (closestDistance < attackRadius)
            {
                state = State.AttackingUnit;

                if (attackCooldown <= 0)
                {
                    Attack();
                    attackCooldown = attackSpeed;
                }
            }
            else
            {
                state = State.Moving;
                transform.position = Vector3.MoveTowards(transform.position, closestUnit.transform.position, speed * Time.deltaTime);
            }
        }
    }



   

    bool spawnBlood = false;
    protected void Die()
    {
        spawnBlood = true;

        GameObject blood = Instantiate(bloodPrefab, transform.position, Quaternion.identity);
        blood.GetComponent<Blood>().bloodAmount = bloodCount;


        Debug.Log(bloodCount + " // " + blood.GetComponent<Blood>().bloodAmount);


        Destroy(gameObject);
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        if (health <= 0 && !spawnBlood)
        {
            Debug.Log("Die");
            Die();
        }
    }

    

    IEnumerator Stunned()
    {
        float pushDuration = player.GetComponent<PlayerMovement>().capePushDuration;
        // Calculate direction from enemy to player
        Vector2 knockbackDirection = (player.transform.position - transform.position).normalized;

        // Apply knockback force
        Rigidbody2D kbRb = GetComponent<Rigidbody2D>();
        kbRb.velocity = new Vector2((-knockbackDirection.x * capeKnockback - Time.deltaTime)*Time.deltaTime, (-knockbackDirection.y * capeKnockback - Time.deltaTime) * Time.deltaTime);
        yield return new WaitForSeconds(pushDuration);
        kbRb.velocity = Vector2.zero;
        yield return new WaitForSeconds(stunDuration - pushDuration);
        isStunned = false;
    }


    public void SetPath(Path path)
    {
        paths = new List<Path> { path };
    }

}
