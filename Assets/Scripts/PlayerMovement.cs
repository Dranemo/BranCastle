using System.Collections;
using System.Drawing;
using TMPro;
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
    public bool canPunch = true;
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
    private bool isFacingLeft;
    private GameObject closestEnemy;
    public Image coffin_input;

    [Header("Sound")]

    private AudioSource audioSourceBat;
    private AudioSource audioSourceCape;
    [SerializeField] private AudioClip batSound;
    [SerializeField] private AudioClip capeAudio;

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
    private Image cooldownBatImage;
    private Image cooldownIceImage;
    private Image cooldownHypnosisImage;

    [Header("BatAttack")]
    public float batAttackCooldown = 5f;
    public bool canBatAttack = true;
    public float batDistance = 2.5f;
    private GameObject rectangle;
    public bool isDrawingRectangle = false;
    public bool canDrawRectangle = true;
    public GameObject batPrefab;
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

    [Header("Hypnosis")]
    public bool isHypnotizing = false;
    private float hypnosisDuration = 5f;
    public bool canHypnotize = true;

    private PauseMenu pause;

    public void DisablePunch()
    {
        canPunch = false;
    }

    public void EnablePunch()
    {
        canPunch = true;
    }
    void Awake()
    {
        AudioSource[] audioSources = GetComponents<AudioSource>();
        if (audioSources.Length >= 2)
        {
            audioSourceBat = audioSources[0];
            audioSourceCape = audioSources[1];
        }
        else
        {
            Debug.LogError("Pas assez de composants AudioSource attachés au GameObject.");
        }
        audioSourceBat.clip = batSound;
        audioSourceCape.clip = capeAudio;
        mapOverview = FindObjectOfType<MapOverview>();
        canvasFader = FindObjectOfType<CanvasFader>();
        animator = GetComponent<Animator>();
        GameObject canvas = GameObject.Find("CanvasInput");
        coffin_input = canvas.GetComponentInChildren<Image>();
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        trail = this.gameObject.GetComponentInChildren<TrailRenderer>();


    }
    void Start()
    {
        canDash = true;
        rectangle = transform.Find("BatArea").gameObject;
        rectangle.SetActive(false);
        if (trail != null)
        {
            trail.enabled = false;
        }
        gameManager = GameManager.Instance;
        particles = GetComponentInChildren<ParticleSystem>();
        GameObject cooldownBatObject = GameObject.Find("cooldown_bat");
        cooldownBatImage = cooldownBatObject.GetComponent<Image>();
        GameObject cooldownIceObject = GameObject.Find("cooldown_ice");
        cooldownIceImage = cooldownIceObject.GetComponent<Image>();
        GameObject cooldownHypnosisObject = GameObject.Find("cooldown_hypnosis");
        cooldownHypnosisImage = cooldownHypnosisObject.GetComponent<Image>();
        coffin_input.enabled = false;
        pause = FindObjectOfType<PauseMenu>();
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
        if (canPunch && Input.GetMouseButtonDown(0) && currentCoup == null && !isDrawingRectangle && !isOverviewActivated)
        {
            HandleCombo();
        }
        // Movement
        transform.position += (Vector3)movement * moveSpeed * Time.deltaTime;

        // Dash
        if (Input.GetKeyDown(dashKey) && canDash)
            StartCoroutine(Dash());

        //Cape Attack
        if (canPunch && Input.GetMouseButtonDown(1) && currentCape == null && !isDrawingRectangle && canCape)
        {
            CapeAttack();
        }


        ///////////////////////// Bat attack //////////////////////////////

        if (Input.GetKeyDown(batKey) && canDrawRectangle)
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

        ///////////////////////// Light //////////////////////////////

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


        ///////////////////////// Coffin /////////////////////////////////
       

        if (nearestCoffin != null && nearestCoffin.CanInteract())
        {
            coffin_input.enabled = true;

        }
        else
        {
            coffin_input.enabled = false;

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
        if(pause.isPaused==false)
        {
            FlipSpriteBasedOnCursor(angle);
        }
        

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
        float closestDistance = 20f;

        foreach (GameObject enemy in enemies)
        {
            float distanceToCursor = Vector2.Distance(cursorPosition, enemy.transform.position);
            if (distanceToCursor < closestDistance)
            {
                closestDistance = distanceToCursor;
                closestEnemy = enemy;
            }
        }

        if (closestEnemy != null && canHypnotize)
        {
            Debug.Log("Closest enemy to cursor: " + closestEnemy.name);
            closestEnemy.GetComponent<Enemy>().Hypnotize();
            StartCoroutine(AttackCooldown(cooldownHypnosisImage, hypnosisDuration, isHypnotizing));
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
        audioSourceBat.Play();
        isDrawingRectangle = false;
        rectangle.SetActive(false);
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 playerPosition = transform.position;

        Vector2 direction = (mousePosition - playerPosition).normalized;

        StartCoroutine(SpawnBats(playerPosition, direction));
        StartCoroutine(AttackCooldown(cooldownBatImage, batAttackCooldown, canBatAttack));
    }

    IEnumerator SpawnBats(Vector2 playerPosition, Vector2 direction)
    {
        for (int i = 0; i < 20; i++)
        {
            Vector2 randomOffset = new Vector2(
                Random.Range(-0.2f, 0.2f),
                Random.Range(-0.2f, 0.2f)
            );

            Vector2 spawnPosition2D = playerPosition + direction + randomOffset; 
            Vector3 spawnPosition = new Vector3(spawnPosition2D.x, spawnPosition2D.y, 0);

            GameObject bat = Instantiate(batPrefab, spawnPosition, Quaternion.identity);
            BatAttack batScript = bat.GetComponent<BatAttack>();
            if (batScript != null)
            {
                batScript.SetDirection(direction);
            }

            yield return new WaitForSeconds(0.01f);
        }
    }


    IEnumerator AttackCooldown(Image cooldownImage, float attackCooldown, bool canAttack)
    {
        canAttack = false;
        float cooldownTime = 0f;

        while (cooldownTime < attackCooldown)
        {
            cooldownTime += Time.deltaTime;
            cooldownImage.fillAmount = cooldownTime / attackCooldown; 
            yield return null;
        }

        cooldownImage.fillAmount = 1f; 
        canAttack = true; 
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

        bool shouldFaceLeft = (angle <= -90 && angle >= -180) || (angle < 180 && angle > 90);

        if (shouldFaceLeft != isFacingLeft)
        {
            isFacingLeft = shouldFaceLeft;
            spriteRenderer.flipX = isFacingLeft;
            animator.SetBool("isFacingLeft", isFacingLeft);
        }
    }





    public IEnumerator WaitForDeathAnimation()
    {
        canMove = false;
        animator.SetBool("Dead", true);
        Debug.Log("Player is dead");

        float timeout = 5.0f; // Timeout de 5 secondes
        float elapsedTime = 0f;

        // Attendre la fin de l'animation de mort
        while ((!animator.GetCurrentAnimatorStateInfo(0).IsName("death") || animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f) && elapsedTime < timeout)
        {
            // Ajouter des logs pour vérifier les conditions
            Debug.Log("Checking animation state...");
            Debug.Log("Is 'death' animation playing: " + animator.GetCurrentAnimatorStateInfo(0).IsName("death"));
            Debug.Log("Normalized time of 'death' animation: " + animator.GetCurrentAnimatorStateInfo(0).normalizedTime);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        if (elapsedTime >= timeout)
        {
            Debug.LogError("Timeout reached while waiting for death animation to finish.");
        }

        Debug.Log("Game Over");
        ScenesManager.Instance.LoadScene("GameOver");
    }


    void CapeAttack()
    {
        audioSourceCape.Play();
        Vector2 coupCapePosition = rb.position;
        currentCape = Instantiate(capePrefab, coupCapePosition, Quaternion.Euler(0, 0, 0));
        Debug.Log("Cape attack");
        Debug.Log(currentCape.transform);
        StartCoroutine(HandleCapeAttack(currentCape));
    }

    IEnumerator HandleCapeAttack(GameObject cape)
    {
        yield return StartCoroutine(ScaleOverTime(cape, Vector3.zero, new Vector3(5, 5, 1), 0.2f));
        StartCoroutine(AttackCooldown(cooldownIceImage, capeCooldown, canCape));
        yield return StartCoroutine(FadeOutOverTime(cape, capeDuration));
        Destroy(cape);
        currentCape = null;
    }

    IEnumerator ScaleOverTime(GameObject target, Vector3 initialScale, Vector3 finalScale, float duration)
    {
        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            if (target == null)
            {
                yield break; // Sortir de la coroutine si l'objet a été détruit
            }

            target.transform.localScale = Vector3.Lerp(initialScale, finalScale, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        if (target != null)
        {
            target.transform.localScale = finalScale; // Assurez-vous que la mise à l'échelle finale est appliquée
        }
    }

    IEnumerator FadeOutOverTime(GameObject target, float duration)
    {
        SpriteRenderer spriteRenderer = target.GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
        {
            yield break;
        }

        float elapsedTime = 0f;
        UnityEngine.Color initialColor = spriteRenderer.color;
        UnityEngine.Color finalColor = initialColor;
        finalColor.a = 0f; // Alpha final à 0 (complètement transparent)

        while (elapsedTime < duration)
        {
            if (target == null)
            {
                yield break; // Sortir de la coroutine si l'objet a été détruit
            }

            spriteRenderer.color = UnityEngine.Color.Lerp(initialColor, finalColor, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        if (target != null)
        {
            spriteRenderer.color = finalColor; // Assurez-vous que l'alpha final est appliqué
        }
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
