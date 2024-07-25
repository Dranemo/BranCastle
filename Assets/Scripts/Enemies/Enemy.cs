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
    [SerializeField] LayerMask layerMaskRaycast;
    public int currentPathIndex = 0;
    private int currentWaypointIndex = 0;
    protected float health = 50;

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

    [Header("Sprite")]
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



    // ----------------------------------------- Func Unity -----------------------------------------
    protected void Awake()
    {
        paths = new List<Path>();
    }

    protected void Start()
    {
        units = new List<GameObject>();

        //utez vos waypoints � la liste ici, ou initialisez-les dans l'inspecteur Unity
        player = GameObject.FindGameObjectWithTag("Player");
        capeKnockback = player.GetComponent<PlayerMovement>().capePushForce;
        ritual = GameObject.FindGameObjectWithTag("Ritual");
        layerMaskRaycast = LayerMask.GetMask("Wall");
    }




    protected void Update()
    {
        //CheckBetterWaypoint();
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
                RaycastHit2D hit = Physics2D.Linecast(transform.position, player.transform.position, layerMaskRaycast);


                if (hit.collider == null)
                {
                    DetectPlayer();
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


        attackCooldown -= Time.deltaTime;
    }







    // ----------------------------------------- Player & Ritual Related -----------------------------------------
    protected void DetectPlayer()
    {
        onPath = false;

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
        if (Vector3.Distance(transform.position, paths[currentPathIndex].waypoints[currentWaypointIndex].transform.position) < attackRadius)
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
            transform.position = Vector3.MoveTowards(transform.position, paths[currentPathIndex].waypoints[currentWaypointIndex].transform.position, speed * Time.deltaTime);
        }
    }

    protected virtual void Attack()
    {
        Debug.Log("Attack");
    }




    // ----------------------------------------- Sprite Related -----------------------------------------
    void TurningSprite(Direction dir)
    {
        switch (dir)
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


    // ----------------------------------------- Units Related -----------------------------------------
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




    // ----------------------------------------- State & Life Related -----------------------------------------
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
        kbRb.velocity = new Vector2((-knockbackDirection.x * capeKnockback - Time.deltaTime) * Time.deltaTime, (-knockbackDirection.y * capeKnockback - Time.deltaTime) * Time.deltaTime);
        yield return new WaitForSeconds(pushDuration);
        kbRb.velocity = Vector2.zero;
        yield return new WaitForSeconds(stunDuration - pushDuration);
        isStunned = false;
    }




    // ----------------------------------------- Path Related -----------------------------------------
    bool CheckWaypoint(int path, int waypoint)
    {
        Vector3 pos = transform.position;
        Vector3 posNext = paths[path].waypoints[waypoint].transform.position;

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


    // Return la position du meilleur point possible
    void CheckBetterWaypoint()
    {
        List<(int, int)> waypointsPath = new();


        for (int j = 0; j < paths.Count; j++)
        {
            for (int i = 0; i < paths[j].waypoints.Count; i++)
            {
                if (CheckWaypoint(j, i))
                {
                    waypointsPath.Add((j, i));
                }
            }
        }

        if (waypointsPath.Count != 0)
        {
            // Prendre le premier élément de la liste
            Waypoint bestWaypoint = paths[waypointsPath[0].Item1].waypoints[waypointsPath[0].Item2];
            float bestDistance = Vector2.Distance(transform.position, bestWaypoint.transform.position);

            // Parcourir la liste
            for (int i = 1; i < waypointsPath.Count; i++)
            {
                Waypoint testWaypoint = paths[waypointsPath[i].Item1].waypoints[waypointsPath[i].Item2];
                float distanceTest = Vector2.Distance(transform.position, testWaypoint.transform.position);

                if (distanceTest + testWaypoint.distanceRitual < bestDistance + bestWaypoint.distanceRitual)
                {
                    bestWaypoint = testWaypoint;
                }
            }

            SetIndexesPath(bestWaypoint);
        }
    }


    void SetIndexesPath(Waypoint waypoint)
    {
        foreach (Path path in paths)
        {
            if (path.waypoints.Contains(waypoint))
            {
                currentPathIndex = paths.IndexOf(path);
                break;
            }
        }

        currentWaypointIndex = paths[currentPathIndex].waypoints.IndexOf(waypoint);
    }


    void MovingToTheNextCheckpoint()
    {
        state = State.Moving;

        // Si le chemin est vide, ne rien faire
        if (paths == null || !paths.Any() || paths[currentPathIndex].waypoints.Count == 0)
            return;



        if (!onPath)
        {
            onPath = true;
            CheckBetterWaypoint();
        }

        Transform currentWaypoint = paths[currentPathIndex].waypoints[currentWaypointIndex].transform;
        TurningSprite(currentWaypoint.position);


        if (currentWaypoint.GetComponent<Waypoint>().nextWaypoint == null)
        {
            if (Vector3.Distance(transform.position, currentWaypoint.position) < attackRadius)
                DetectRitual();
            else
                transform.position = Vector3.MoveTowards(transform.position, currentWaypoint.position, speed * Time.deltaTime);
        }
        else if (Vector3.Distance(transform.position, ritual.transform.position) < attackRadius)
            DetectRitual();
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, currentWaypoint.position, speed * Time.deltaTime);


            if (transform.position == currentWaypoint.position)
            {
                if(currentWaypointIndex + 1 >= paths[currentPathIndex].waypoints.Count)
                {
                    SetIndexesPath(currentWaypoint.GetComponent<Waypoint>().nextWaypoint);
                }
                else
                {
                    currentWaypointIndex++;
                }
            }
        }
    }
}

