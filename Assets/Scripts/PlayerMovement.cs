using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{


    public float moveSpeed = 5f;

    public float coupDistance = 2f;
    public GameObject coupPrefab;
    public float punchForce = 0.1f;
    private GameObject currentCoup;
    private GameObject currentCape;
    private Rigidbody2D rb;
    private Vector2 movement;
    private SpriteRenderer spriteRenderer;
    private bool punchEnabled = true;
    private TrailRenderer trail;

    public GameManager gameManager;
    private ParticleSystem particles;
    private Coffin nearestCoffin = null;
    private bool canMove = true;
    private bool isOverviewActivated = false;
    private MapOverview mapOverview;
    private CanvasFader canvasFader;
    private Animator animator;
    private float angle;
    [SerializeField] Transform spriteTransform;
    private bool isFacingLeft = false;
    private GameObject closestEnemy;

    [Header("Combo Settings")]
    public float comboResetTime = 0.28f; 
    private int comboStep = 0;
    private float lastClickTime = 0;
    private string[] attackAnimations = { "Attack1", "Attack2", "Attack3" };
    private bool isAttacking = false;
    private bool isAttacking2 = false;
    private bool isAttacking3 = false;

    [Header("Inputs")]
    [SerializeField] private KeyCode batKey;
    [SerializeField] private KeyCode hypnosisKey;
    [SerializeField] private KeyCode drainKey;
    [SerializeField] private KeyCode biteKey;

    [Header("UI")]
    private Image cooldownImage; 

    [Header("BatAttack")]
    public float batAttackCooldown = 5f;
    private bool canBatAttack = true;
    public float batDistance = 2.5f;
    private GameObject rectangle;
    public bool isDrawingRectangle = false;
    public GameObject batPrefab;
    [SerializeField] private AudioClip batSound;
    private AudioSource audioSource;
    [Header("DashSettings")]
    public KeyCode dashKey;
    public float dashCooldown;
    public float dashDuration;
    public float dashForce;
    public bool canDash = true;
    public bool isDashing = false;

    [Header("CapeHit")]
    public float capeDMG;
    public float capeCooldown;
    public float capeDuration;
    public float capeRange;
    public float capePushForce;
    public float capePushDuration;
    public GameObject capePrefab;
    public bool canCape = true;
    [SerializeField] private AudioClip capeAudio;

    public bool isHypnotizing = false; 
    public void DisablePunch()
    {
        punchEnabled = false;
    }

    public void EnablePunch()
    {
        punchEnabled = true;
    }
    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        mapOverview = FindObjectOfType<MapOverview>();
        canvasFader = FindObjectOfType<CanvasFader>();
        animator = GetComponent<Animator>();
    }
    void Start()
    {
        canDash = true;
        rectangle = transform.Find("BatArea").gameObject;
        rectangle.SetActive(false);
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        trail = this.gameObject.GetComponentInChildren<TrailRenderer>();
        if (trail != null)
        {
            trail.enabled = false;
        }
        gameManager = GameManager.Instance;
        particles = GetComponentInChildren<ParticleSystem>();
        GameObject cooldownFillObject = GameObject.Find("cooldown_fill");
        if (cooldownFillObject != null)
        {
            cooldownImage = cooldownFillObject.GetComponent<Image>();
        }
        else
        {
            Debug.LogError("L'objet 'cooldown_fill' n'a pas été trouvé dans la scène.");
        }
    }

    void Update()
    {
        if (Time.time - lastClickTime > comboResetTime)
        {
            ResetAttackBools();
            comboStep = 0;
        }
        Vector2 direction = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        if (punchEnabled && Input.GetMouseButtonDown(0) && currentCoup == null && !isDrawingRectangle && !isOverviewActivated)
        {
            HandleCombo();
            //Vector3 coupPosition = transform.position + new Vector3(direction.x, direction.y, 0) * coupDistance;
            //currentCoup = Instantiate(coupPrefab, coupPosition, Quaternion.Euler(0, 0, angle+90));
            //StartCoroutine(DestroyAfterSeconds(currentCoup, 0.1f));
        }
        // Movement
        transform.position += (Vector3)movement * moveSpeed * Time.deltaTime;

        // Dash
        if (Input.GetKeyDown(dashKey) && canDash)
            StartCoroutine(Dash());

        //Cape Attack
        if (punchEnabled && Input.GetMouseButtonDown(1) && currentCape == null && !isDrawingRectangle && canCape)
        {
            canCape = false;
            StartCoroutine(CapeAttack());
        }


        ////////////////////// Bat attack //////////////////////////////

        if (Input.GetKeyDown(batKey))
        {
            isDrawingRectangle = !isDrawingRectangle;
            rectangle.SetActive(isDrawingRectangle);
        }
        if (isDrawingRectangle)
        {
            rectangle.transform.position = transform.position + new Vector3(direction.x, direction.y, 0) * 2.5f;
            rectangle.transform.rotation = Quaternion.LookRotation(Vector3.forward, direction);
            direction = direction.normalized * batDistance; // Keep the rectangle within the specified radius
            rectangle.transform.position = transform.position + new Vector3(direction.x, direction.y, 0);
            rectangle.transform.rotation = Quaternion.Euler(0, 0, angle);
        }
        if (Input.GetMouseButtonDown(0) && isDrawingRectangle)
        {
            BatAttack();
        }

        if (!gameManager.isPlayerInLight)
        {
            //particles.Stop(true, ParticleSystemStopBehavior.StopEmitting);
            particles.Clear();
            particles.Pause();

        }
        else
        {
            particles.Play();
        }
        if (Input.GetButtonDown("Coffin") && nearestCoffin != null && nearestCoffin.CanInteract() && !isOverviewActivated)
        {
            rb.isKinematic = true;
            rb.velocity = Vector2.zero;
            GetComponent<SpriteRenderer>().enabled = false;
            canMove = false;
            isOverviewActivated = true;
            canvasFader.StartFadeIn();

            mapOverview.ActivateOverview();
        }
        // Désactivation de la vue d'ensemble
        else if (Input.GetButtonDown("Coffin") && isOverviewActivated)
        {
            GetComponent<SpriteRenderer>().enabled = true;
            rb.isKinematic = false;
            canMove = true;
            isOverviewActivated = false;
            mapOverview.DeactivateOverview();
        }
        FlipSpriteBasedOnCursor(angle);

        // Hypnosis logic
        if (Input.GetKeyDown(hypnosisKey))
        {
            FindClosestEnemyToCursor();
        }
    }
    void FindClosestEnemyToCursor()
    {
        isHypnotizing = true;
        Vector2 cursorPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        float closestDistance = Mathf.Infinity;

        foreach (GameObject enemy in enemies)
        {
            float distanceToCursor = Vector2.Distance(cursorPosition, enemy.transform.position);
            if (distanceToCursor < closestDistance)
            {
                closestDistance = distanceToCursor;
                closestEnemy = enemy;
            }
        }

        if (closestEnemy != null)
        {
            Debug.Log("Closest enemy to cursor: " + closestEnemy.name);
            closestEnemy.GetComponent<Enemy>().Hypnotize();
            isHypnotizing = false;
        }
        else
        {
            isHypnotizing = false;
        }
    }
    void HandleCombo()
    {
        if (animator == null)
        {
            Debug.LogError("Animator n'est pas assigné !");
            return;
        }

        float timeSinceLastClick = Time.time - lastClickTime;

        if (timeSinceLastClick > comboResetTime)
        {
            comboStep = 0;
            ResetAttackBools();
        }

        lastClickTime = Time.time;

        if (comboStep < attackAnimations.Length)
        {
            animator.Play(attackAnimations[comboStep]);
            SetAttackBool(comboStep);
            comboStep++;
        }
        else
        {
            comboStep = 0;
            ResetAttackBools();
        }
    }

    void SetAttackBool(int step)
    {
        ResetAttackBools();

        switch (step)
        {
            case 0:
                isAttacking = true;
                animator.SetBool("isAttacking", isAttacking);
                break;
            case 1:
                isAttacking2 = true;
                animator.SetBool("isAttacking2", isAttacking2);
                break;
            case 2:
                isAttacking3 = true;
                animator.SetBool("isAttacking3", isAttacking3);
                break;
        }
    }

    void ResetAttackBools()
    {
        isAttacking = false;
        isAttacking2 = false;
        isAttacking3 = false;

        animator.SetBool("isAttacking", isAttacking);
        animator.SetBool("isAttacking2", isAttacking2);
        animator.SetBool("isAttacking3", isAttacking3);
    }
    void BatAttack()
    {
        if (!canBatAttack) return; 
        audioSource.clip = batSound;
        audioSource.Play();
        isDrawingRectangle = false;
        rectangle.SetActive(false);
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 playerPosition = transform.position; 

        Vector2 direction = (mousePosition - playerPosition).normalized;

        for (int i = 0; i < 50; i++)
        {
            Vector2 randomOffset = new Vector2(
                Random.Range(-0.5f, 0.5f), 
                Random.Range(-0.5f, 0.5f)
            );

            Vector2 spawnPosition2D = playerPosition + direction * (i * 0.1f) + randomOffset;
            Vector3 spawnPosition = new Vector3(spawnPosition2D.x, spawnPosition2D.y, 0);

            GameObject bat = Instantiate(batPrefab, spawnPosition, Quaternion.identity);
            BatAttack batScript = bat.GetComponent<BatAttack>();
            if (batScript != null)
            {
                batScript.SetDirection(direction);
            }
        }
        StartCoroutine(BatAttackCooldown());
    }

    IEnumerator BatAttackCooldown()
    {
        canBatAttack = false; // Désactiver l'attaque
        float cooldownTime = 0f;

        while (cooldownTime < batAttackCooldown)
        {
            cooldownTime += Time.deltaTime;
            cooldownImage.fillAmount = cooldownTime / batAttackCooldown; // Mettre à jour le remplissage de l'image
            yield return null;
        }

        cooldownImage.fillAmount = 1f; // Assurez-vous que l'image est complètement remplie à la fin du cooldown
        canBatAttack = true; // Réactiver l'attaque
    }


    public void EnableMovement()
    {
        canMove = true;
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<Coffin>() != null)
        {
            nearestCoffin = other.GetComponent<Coffin>();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.GetComponent<Coffin>() != null && nearestCoffin == other.GetComponent<Coffin>())
        {
            nearestCoffin = null;
        }
    }


    private void FlipSpriteBasedOnCursor(float angle)
    {
        if ((angle <= -90 && angle >= -180) || (angle < 180 && angle > 90))
        {
            isFacingLeft = true;
            animator.SetBool("isFacingLeft", isFacingLeft);
        }
        else
        {
            isFacingLeft = false;
        }
    }



    public IEnumerator WaitForDeathAnimation()
    {
        canMove = false;
        animator.SetBool("Dead", true);
        Debug.Log("Player is dead");

        // Attendre la fin de l'animation de mort
        while (!animator.GetCurrentAnimatorStateInfo(0).IsName("death") || animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
        {
            // Ajouter des logs pour vérifier les conditions
            Debug.Log("Checking animation state...");
            Debug.Log("Is 'death' animation playing: " + animator.GetCurrentAnimatorStateInfo(0).IsName("death"));
            Debug.Log("Normalized time of 'death' animation: " + animator.GetCurrentAnimatorStateInfo(0).normalizedTime);

            yield return null;
        }

        Debug.Log("Game Over");
        ScenesManager.Instance.LoadScene("GameOver");
    }

    IEnumerator CapeAttack()
    {
        audioSource.clip = capeAudio;
        audioSource.Play();
        Vector2 coupCapePosotion = rb.position;
        currentCape = Instantiate(capePrefab, coupCapePosotion, Quaternion.Euler(0, 0, 0));
        yield return new WaitForSeconds(capeDuration); 
        Destroy(currentCape);
        yield return new WaitForSeconds(capeCooldown);
        currentCape = null;
        canCape = true;
    }

    void FixedUpdate()
    {
        if (canMove)
        {
            float moveHorizontal = Input.GetAxis("Horizontal");
            float moveVertical = Input.GetAxis("Vertical");

            Vector2 movement = new Vector2(moveHorizontal, moveVertical);

            rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);

            float speed = movement.magnitude * moveSpeed;
            animator.SetFloat("speed", speed);

            // Mettre à jour le paramètre booléen pour la condition <=
            animator.SetBool("isSpeedLessOrEqual", speed <= 0);

            // Dash
            if (Input.GetKeyDown(dashKey) && movement != Vector2.zero && canDash)
            {
                StartCoroutine(Dash());
            }
            if (isDashing)
            {
                rb.MovePosition(rb.position + movement * dashForce * Time.fixedDeltaTime);
                trail.enabled = true;
            }
            else
            {
                trail.enabled = false;
                rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
            }
        }
    }




    IEnumerator ResetVelocityAfterDash()
    {
        yield return new WaitForSeconds(dashDuration);
        rb.velocity = Vector2.zero;
        isDashing = false;
    }
    IEnumerator DestroyAfterSeconds(GameObject gameObject, float seconds)
    {
        yield return new WaitForSeconds(seconds);
        Destroy(gameObject);
        if (gameObject == currentCoup)
        {
            currentCoup = null;
        }
    }

    private IEnumerator Dash()
    {
        canDash = false;
        isDashing = true;
        yield return new WaitForSeconds(dashDuration);
        isDashing = false;
        Invoke(nameof(ResetDash), dashCooldown);
        StartCoroutine(ResetVelocityAfterDash());
    }

    void ResetDash()
    {
        canDash = true;
    }
}
