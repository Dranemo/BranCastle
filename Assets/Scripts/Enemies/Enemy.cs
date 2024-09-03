using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Enemy : MonoBehaviour
{

    [Header("Path")]
    public List<Path> paths;
    [SerializeField] protected LayerMask layerMaskRaycast;
    public int currentPathIndex = 0;
    private int currentWaypointIndex = 0;



    protected GameObject player;
    protected GameObject ritual;
    [SerializeField] protected GameObject targetEnemy;

    protected Canvas canvasinGame;
    protected TextMeshProUGUI ritualText;

    protected State state;
    protected enum State
    {
        Moving,
        AttackingPlayer,
        AttackingRitual,
        AttackingUnit
    }



    [SerializeField] protected GameObject bloodPrefab;

    [Header ("Variables")]
    [SerializeField] public  float health = 50;
    protected float damageByPlayer = 0;
    [SerializeField] public float maxHealth = 50;
    [SerializeField] protected float bloodCount = 10;
    [SerializeField] protected float detectionRadius = 5f;
    [SerializeField] protected float attackRadius = 1.5f;
    [SerializeField] protected float attackSpeed = 1;
    [SerializeField] protected float damage = 0;
    [SerializeField] protected float speed = 2;
    protected float attackCooldown = 1;

    [Header("Units")]
    [SerializeField] protected List<GameObject> units;
    [SerializeField] protected GameObject closestUnit;

    [Header("Sprite")]
    bool flashingSprite;
    bool shrinkingSprite;
    bool resetFlashing = false;
    bool stopFlashingCoroutine = false;
    private SpriteRenderer spriteHypno;
    private SpriteRenderer spriteIce;

    [Header("Animator")]
    protected Animator animator;
    protected Vector3 positionFrameBefore;

    [Header("Status")]
    [SerializeField] public bool isStunned;
    [SerializeField] public float stunDuration;
    [SerializeField] public float capeKnockback;
    [SerializeField] public bool isHypnotized;

    [Header("Sounds")]
    private AudioSource audioSource;
    private AudioClip hitSound;
    private AudioClip deathSound;
    private AudioClip deathSound2;
    private AudioClip deathSound3;
    private AudioClip deathSound4;
    private AudioClip[] deathSounds;

    [SerializeField] protected List<GameObject> enemies = new List<GameObject>();
    protected bool onPath = true;


    public float GetHealthEnemy()
    {
       return health;
    }
    // ----------------------------------------- Func Unity -----------------------------------------
    protected void Awake()
    {
        paths = new List<Path>();
    }

    protected void Start()
    {
        units = new List<GameObject>();
        health = maxHealth;
        flashingSprite = false;
        shrinkingSprite = false;

        //utez vos waypoints � la liste ici, ou initialisez-les dans l'inspecteur Unity
        player = GameObject.FindGameObjectWithTag("Player");
        capeKnockback = player.GetComponent<PlayerMovement>().capePushForce;
        ritual = GameObject.FindGameObjectWithTag("Ritual");
        canvasinGame = GameObject.Find("CanvasInGame(Clone)").GetComponent<Canvas>();
        //Debug.Log("CanvasInGame: " + canvasinGame);
        ritualText = canvasinGame.transform.Find("RitualText").GetComponent<TextMeshProUGUI>();
        //Debug.Log("RitualText: " + ritualText);
        ritualText.enabled = false;
        layerMaskRaycast = LayerMask.GetMask("Wall");

        animator = GetComponent<Animator>();

        positionFrameBefore = transform.position;

        Transform hypnoTransform = transform.Find("hypno");
        if (hypnoTransform != null)
        {
            spriteHypno = hypnoTransform.GetComponent<SpriteRenderer>();
            spriteHypno.enabled = false;
            if (spriteHypno == null)
            {
                //////Debug.LogError("Le SpriteRenderer n'est pas trouvé sur l'enfant 'hypno'.");
            }
        }
        else
        {
            //////Debug.LogError("L'enfant 'hypno' n'est pas trouvé.");
        }
    Transform iceTransform = transform.Find("ice");
        if (iceTransform != null)
        {
            spriteIce = iceTransform.GetComponent<SpriteRenderer>();
            spriteIce.enabled = false;
            if (spriteIce == null)
            {
                //////Debug.LogError("Le SpriteRenderer n'est pas trouvé sur l'enfant 'ice'.");
            }
        }
        else
        {
            //////Debug.LogError("L'enfant 'ice' n'est pas trouvé.");
        }

        audioSource = GetComponent<AudioSource>();
        hitSound = Resources.Load<AudioClip>("HitSound");
        deathSound = Resources.Load<AudioClip>("DeathSound");
        if (deathSound == null)
            //////Debug.LogError("Le son de mort n'a pas été trouvé.");
        deathSound2 = Resources.Load<AudioClip>("DeathSound2");
        if ( deathSound2 == null)
        {
            //////Debug.LogError("Le son de mort2 n'a pas été trouvé.");
        }
        deathSound3 = Resources.Load<AudioClip>("DeathSound3");
        if (deathSound3 == null)
        {
            //////Debug.LogError("Le son de mort3 n'a pas été trouvé.");
        }
        deathSound4 = Resources.Load<AudioClip>("DeathSound4");
        if (deathSound4 == null)
        {
            //////Debug.LogError("Le son de mort4 n'a pas été trouvé.");
        }
        //////Debug.Log("deathSound4" + deathSound4);
        deathSounds = new AudioClip[] { deathSound, deathSound2, deathSound3, deathSound4 };


    }




    protected void Update()
    {
        //IsDead();
        if (animator != null)
            animator.SetBool("Idle", Vector3.Distance(transform.position, positionFrameBefore) == 0);

        positionFrameBefore = transform.position;

        UpdateUnitList();
        UpdateEnemyList();

        float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
        bool isUnitClose = IsUnitClose();

        targetEnemy = GetClosestEnemy();



        if ((animator != null && !animator.GetCurrentAnimatorStateInfo(0).IsName("Hit")) || animator == null)
        {



            if (isHypnotized)
            {
                spriteHypno.enabled = true;
                GetClosestEnemy();
                if (targetEnemy != null)
                {
                    FollowAndAttack(targetEnemy);
                }
            }

            else
            {
                if (isStunned)
                {
                    spriteIce.enabled = true;
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
                    ////////Debug.Log("Player Not Detected");
                    MovingToTheNextCheckpoint();

                }
            }

            attackCooldown -= Time.deltaTime;
        }
    }






    // ----------------------------------------- Player & Ritual Related -----------------------------------------
    protected void DetectPlayer()
    {
        onPath = false;

        TurningSprite(player.transform.position);


        if (Vector3.Distance(transform.position, player.transform.position) < attackRadius)
        {
            state = State.AttackingPlayer;

            if (attackCooldown <= 0 && health > 0)
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
            if (attackCooldown <= 0 && health > 0)
            {
                Attack();
                attackCooldown = attackSpeed;
                ritualText.enabled = true;
            }
        }
        else
        {
            ritualText.enabled = false;
            state = State.Moving;
            transform.position = Vector3.MoveTowards(transform.position, paths[currentPathIndex].waypoints[currentWaypointIndex].transform.position, speed * Time.deltaTime);
        }
    }

    protected virtual void Attack()
    {
        if(animator != null)
        {

            
            animator.Play("Attack1");
            //animator.SetTrigger("Attack");
        }
    }




    // ----------------------------------------- Sprite Related -----------------------------------------
    protected void TurningSprite(Vector3 pos)
    {
        Vector3 direction = pos - transform.position;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        if (animator != null)
        {
            if (angle >= -90 && angle <= 90)
            {
                animator.SetBool("isFacingLeft", false);
                this.GetComponent<SpriteRenderer>().flipX = false;
            }
            else
            {
                animator.SetBool("isFacingLeft", true);
                this.GetComponent<SpriteRenderer>().flipX = true;
            }
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

    protected void UpdateEnemyList()
    {
        // Réinitialiser la liste des ennemis
        enemies.Clear();

        // Trouver tous les GameObjects avec le tag "Enemy" et les ajouter à la liste
        List<GameObject> enemiesTemp = new List<GameObject>();
        enemiesTemp.AddRange(GameObject.FindGameObjectsWithTag("Enemy"));


        for(int i = 0; i < enemiesTemp.Count; i++)
        {
            if (enemiesTemp[i] != null)
            {
                if (enemiesTemp[i].GetComponent<Enemy>())
                    enemies.Add(enemiesTemp[i]);
            }
        }
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
    [SerializeField] bool spawnBlood = false;
    protected void Die()
    {
        Debug.Log(name + " is dead");

        spawnBlood = true;
        //////Debug.Log("Damage by player: " + damageByPlayer);

        tag = "Untagged";
        foreach (Transform child in transform)
        {
            child.gameObject.tag = "Untagged";
        }

        float percentDamageF = damageByPlayer / maxHealth;
        int percentDamage = Mathf.RoundToInt(percentDamageF);

        float numberBloodF = bloodCount * percentDamage;
        int numberBlood = Mathf.RoundToInt(numberBloodF);

        AudioClip selectedDeathSound = deathSounds[Random.Range(0, deathSounds.Length)];

        // Jouer le clip audio sélectionné
        if (audioSource != null && selectedDeathSound != null)
        {
            audioSource.clip = selectedDeathSound;
            audioSource.Play();
        }

        if(animator == null)
        {
            StartCoroutine(Shrinking());
        }

        StartCoroutine(SpawnBlood(numberBlood));
    }

    IEnumerator SpawnBlood(int amount)
    {
        float numberOfBloodF = amount / Blood.bloodAmountBase;
        int numberOfBlood = (int)numberOfBloodF;



        List<int> directionList = new();
        int lastDirection = 0;


        int bloodAmountNotRound = 0;
        if (numberOfBlood != numberOfBloodF)
            bloodAmountNotRound = 1;

        if(animator != null)
        {
            while (!animator.GetCurrentAnimatorStateInfo(0).IsName("BloodAnim"))
                yield return null;
        }
        else
        {
            while (shrinkingSprite)
                yield return null;
        }

        gameObject.layer = 1;
        foreach (Transform child in transform)
        {
            child.gameObject.layer = 1;
            foreach (Transform child2 in child)
            {
                child2.gameObject.layer = 1;
            }
        }


        // Directions de base (séparées de minimum 30°
        for (int i = 0; i < numberOfBlood + bloodAmountNotRound; i++)
        {
            int direction = -1;

            if(lastDirection != -1)
            {
                int security = 50;
                for (int j = 0; j < security; j++)
                {
                    direction = Random.Range(0, 360);
                    foreach (int dir in directionList)
                    {
                        if (dir >= 30 && dir <= 330)
                        {
                            if (direction >= dir - 30 && direction <= dir + 30)
                            {
                                direction = -1;
                                break;
                            }
                        }
                        else if (dir < 30)
                        {
                            if (direction >= dir - 30 && direction <= dir + 30 || direction >= 360 + dir - 30 && direction <= 360)
                            {
                                direction = -1;
                                break;
                            }
                        }
                        else if (dir > 330)
                        {
                            if (direction >= dir - 30 && direction <= dir + 30 || direction >= 0 && direction <= 30 - (360 - dir))
                            {
                                direction = -1;
                                break;
                            }
                        }
                    }

                    if (direction != -1)
                    {
                        break;
                    }
                }
            }

            //////Debug.Log("direction : " + direction);
            lastDirection = direction;

            // Plus de place dans le cercle de base, envoie le sang dans le trou le plus gros du cercle
            if(lastDirection == -1)
            {
                directionList.Sort();

                int biggestHole = 0;
                int holeStartWhereID = 0;

                for(int j = 0; j < directionList.Count - 1; j++)
                {
                    int hole = directionList[j + 1] - directionList[j];
                    if(hole > biggestHole)
                    {
                        holeStartWhereID = j;
                        biggestHole = hole;
                    }
                }

                int holeZero = 360 - directionList[directionList.Count - 1] + directionList[0];
                if(holeZero > biggestHole)
                {
                    holeStartWhereID = directionList.Count - 1;
                    biggestHole = holeZero;
                }




                float placeF = biggestHole / 2;
                int place = Mathf.RoundToInt(placeF);

                if (holeStartWhereID != directionList.Count - 1)
                {
                    direction = directionList[holeStartWhereID] + place;
                }
                else
                {
                    direction = directionList[holeStartWhereID] + place;
                    if (direction > 360)
                    {
                        direction -= 360;
                    }
                }
            }







            GameObject blood = Instantiate(bloodPrefab, transform.position, Quaternion.identity);
            if(direction != -1)
            {
                blood.GetComponent<Blood>().directionAngleProjection = direction;
                directionList.Add(direction);
            }
            else
                blood.GetComponent<Blood>().directionAngleProjection = directionList[Random.Range(0, directionList.Count)];

            if(i == numberOfBlood)
            {
                blood.GetComponent<Blood>().bloodAmount = amount - Blood.bloodAmountBase * numberOfBlood ;
            }
        }

        if (animator != null)
        {
            while (animator.GetCurrentAnimatorStateInfo(0).IsName("BloodAnim"))
                yield return null;
        }

        StartCoroutine(DestroyAfterFrame());
    }

    public void TakeDamage(float damage, bool playerDealtDamage = true)
    {
        
        if (gameObject != null && !spawnBlood && health > 0 && !animator.GetBool("isDead")) 
        {
            float tempHealth = health;
            tempHealth -= damage;


            if (animator != null)
                animator.Play("Hit");



            // Mort ou dégâts
            if (tempHealth <= 0 && !spawnBlood)
            {
                if (animator != null)
                {
                    animator.SetBool("isDead", true);
                }

                stopFlashingCoroutine = true;

                //////Debug.Log("Die");
                if(playerDealtDamage)
                {
                    ////Debug.Log("Damage by player: " + damageByPlayer);
                    audioSource.clip=hitSound;
                    audioSource.Play();
                    damageByPlayer += (health);
                }

                

                health = 0;

                Die();
            }
            else
            {
            

                if(animator == null)
                {
                    // Cligotement
                    if (!flashingSprite)
                        StartCoroutine(Flashing());
                }

                if (playerDealtDamage)
                    damageByPlayer += damage;
                health = tempHealth;
            }
        }
    }

    void IsDead()
    {
        if (health <= 0 && !animator.GetCurrentAnimatorStateInfo(0).IsName("BloodAnim") && !animator.GetCurrentAnimatorStateInfo(0).IsName("Death"))
        {
            Die();
        }
    }

    protected IEnumerator Stunned()
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
        spriteIce.enabled = false;
    }


    IEnumerator Flashing()
    {
        //////Debug.Log("flashing");

        flashingSprite = true;
        SpriteRenderer spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        float actualSpeed = speed;
        speed = 0;

        spriteRenderer.enabled = false;


        for (int i = 0; i < 2; i++)
        {
            yield return new WaitForSeconds(0.2f);

            spriteRenderer.enabled = true;

            if (resetFlashing)
            {
                resetFlashing = false;
                i = 0;
            }
            if (stopFlashingCoroutine)
            {
                stopFlashingCoroutine = false;

                ResetFlashingState(spriteRenderer, actualSpeed);

                yield break;
            }

            yield return new WaitForSeconds(0.2f);

            spriteRenderer.enabled = false;

            if (resetFlashing)
            {
                resetFlashing = false;
                i = 0;
            }
            if (stopFlashingCoroutine)
            {
                stopFlashingCoroutine = false;

                ResetFlashingState(spriteRenderer, actualSpeed);

                yield break;
            }
        }

        yield return new WaitForSeconds(0.2f);

        ResetFlashingState(spriteRenderer, actualSpeed);
    }

    IEnumerator Shrinking()
    {
        shrinkingSprite = true;
        //////Debug.Log("shrinking");

        float time = 0;
        while (time <= 0.5f)
        {
            time += Time.deltaTime;
            transform.localScale = Vector3.Lerp(transform.localScale, Vector3.zero, time);
            yield return null;
        }

        shrinkingSprite = false;
    }


    void ResetFlashingState(SpriteRenderer spriteRenderer, float actualSpeed)
    {
        spriteRenderer.enabled = true;

        attackCooldown = attackSpeed;
        speed = actualSpeed;

        flashingSprite = false;
    }

    IEnumerator DestroyAfterFrame()
    {
        gameObject.GetComponent<SpriteRenderer>().enabled = false;


        // Attendez la fin de la frame
        yield return new WaitForEndOfFrame();

        // Détruisez l'objet de jeu
        Destroy(gameObject);
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


    protected void MovingToTheNextCheckpoint()
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

    // ----------------------------------------- Hypnosis Related -----------------------------------------
    public void Hypnotize()
    {
        gameObject.tag = "Unit";
        gameObject.layer = LayerMask.NameToLayer("Unit");

        foreach (Transform child in transform)
        {
            ////Debug.Log("Child: " + child.gameObject.name);
            if (child.gameObject.name == "Projectiles")
            {
                ////Debug.Log("hypnotized projectiles");
                foreach (Transform proj in child)
                {
                    proj.gameObject.layer = 12;
                }
            }
        }


        ////Debug.Log("Enemy hypnotized: " + gameObject.name);
        isHypnotized = true;

        state = State.Moving;
    }





    protected GameObject GetClosestEnemy()
    {
        List<GameObject> enemiesToHeal = new List<GameObject>();

        // Trouver l'ennemi le plus proche
        GameObject closestEnemy = null;
        float closestDistance = Mathf.Infinity;


        for(int i = 0; i < enemies.Count; i++)
        {
            if (enemies[i] != null)
            {

                float distance = Vector3.Distance(transform.position, enemies[i].transform.position);

                if (distance < closestDistance && CheckEnemyWall(enemies[i]))
                {
                    closestDistance = distance;
                    closestEnemy = enemies[i];
                }
            }
        }


        return closestEnemy;
    }

    protected bool CheckEnemyWall(GameObject enemyChecking)
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



    void FollowAndAttack(GameObject enemy)
    {
        float distanceToEnemy = Vector2.Distance(transform.position, enemy.transform.position);

        if (distanceToEnemy > attackRadius)
        {
            state = State.Moving;
            // Suivre l'ennemi
            Vector2 direction = (enemy.transform.position - transform.position).normalized;
            transform.position = Vector2.MoveTowards(transform.position, enemy.transform.position, speed * Time.deltaTime);
        }
        else
        {
            state = State.AttackingUnit; 
            
            if (attackCooldown <= 0 && health > 0)
            {
                Attack();
                attackCooldown = attackSpeed;
            }
        }
    }
}

