using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using static UnityEngine.ParticleSystem;

public class PlayerMovement : MonoBehaviour
{


    public float moveSpeed = 5f;

    public float coupDistance = 2f;
    public float batDistance = 2.5f;
    public GameObject coupPrefab;
    public float punchForce = 0.1f;
    private GameObject currentCoup;
    private GameObject currentCape;
    private Rigidbody2D rb;
    private Vector2 movement;
    private SpriteRenderer spriteRenderer;
    private bool punchEnabled = true;
    private GameObject rectangle;
    private bool isDrawingRectangle = false;
    public GameObject batPrefab;
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

    [Header("DashSettings")]
    public KeyCode dashKey;
    public float dashCooldown;
    public float dashDuration = 3f;
    public float dashForce = 50f;
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
    private AudioSource capeAudioSource;

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
        capeAudioSource = GetComponent<AudioSource>();
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

    }

    void Update()
    {

        Vector2 direction = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        if (punchEnabled && Input.GetMouseButtonDown(0) && currentCoup == null && !isDrawingRectangle && !isOverviewActivated)
        {

            Vector3 coupPosition = transform.position + new Vector3(direction.x, direction.y, 0) * coupDistance;


            currentCoup = Instantiate(coupPrefab, coupPosition, Quaternion.Euler(0, 0, angle+90));
            StartCoroutine(DestroyAfterSeconds(currentCoup, 0.1f));
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


        // Bat attack
        if (Input.GetMouseButtonDown(2))
        {
            isDrawingRectangle = true;
            rectangle.SetActive(true);
        }
        if (isDrawingRectangle)
        {
            rectangle.transform.position = transform.position + new Vector3(direction.x, direction.y, 0) * 2.5f;
            rectangle.transform.rotation = Quaternion.LookRotation(Vector3.forward, direction);
            direction = direction.normalized * batDistance; // Keep the rectangle within the specified radius
            rectangle.transform.position = transform.position + new Vector3(direction.x, direction.y, 0);
            rectangle.transform.rotation = Quaternion.Euler(0, 0, angle);
        }
        if (Input.GetMouseButtonDown(1) && isDrawingRectangle)
        {
            isDrawingRectangle = false;
            rectangle.SetActive(false);
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
    void BatAttack()
    {
        isDrawingRectangle = false;
        rectangle.SetActive(false);
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 playerPosition = transform.position; // Get the player's position

        // Calculate the direction vector from the player to the mouse cursor
        Vector2 direction = (mousePosition - playerPosition).normalized;
        Vector2 rectangleSize = rectangle.GetComponent<SpriteRenderer>().bounds.size;
        Vector3 rectanglePosition = rectangle.transform.position;

        // Instantiate bats at random positions within the rectangle
        for (int i = 0; i < 50; i++) // Increase the number of bats to create a swarm
        {
            Vector3 spawnPosition = rectanglePosition + new Vector3(
                Random.Range(-rectangleSize.x / 2, rectangleSize.x / 2),
                Random.Range(-rectangleSize.y / 2, rectangleSize.y / 2),
                0);

        GameObject bat = Instantiate(batPrefab, spawnPosition, Quaternion.identity);
            BatAttack batScript = bat.GetComponent<BatAttack>();
            if (batScript != null)
            {
                batScript.SetDirection(direction);
            }
        }

    }
    private void FlipSpriteBasedOnCursor(float angle)
    {
        if ((angle <= -90 && angle >= -180) || (angle < 180 && angle > 90))
        {
            isFacingLeft = true;
            animator.SetBool("isFacingLeft", isFacingLeft);

            spriteTransform.localScale = new Vector3(-2, 2, 1);
        }
        else
        {
            isFacingLeft = false;
            animator.SetBool("isFacingLeft", isFacingLeft);

            spriteTransform.localScale = new Vector3(2, 2, 1); 
        }
    }


    public IEnumerator WaitForDeathAnimation()
    {
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
        //capeAudioSource.Play();
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

            // Calculer la vitesse en utilisant la magnitude du vecteur de mouvement
            float speed = movement.magnitude * moveSpeed;
            animator.SetFloat("speed", speed);

            // Mettre à jour le paramètre booléen pour la condition <=
            animator.SetBool("isSpeedLessOrEqual", speed <= 0);

            // Dash
            if (Input.GetButtonDown("Jump") && movement != Vector2.zero)
            {
                rb.velocity = movement.normalized * dashForce;
                StartCoroutine(ResetVelocityAfterDash());
                isDashing = true;
            }
            if (isDashing)
            {
                trail.enabled = true;
            }
            else
            {
                trail.enabled = false;
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
        // Lire les entrées de l'utilisateur
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        // Calculer le vecteur de mouvement
        Vector2 movement = new Vector2(moveHorizontal, moveVertical).normalized;
        canDash = false;
        isDashing = true;
        rb.velocity = new Vector2(movement.x * dashForce, movement.y * dashForce);
        yield return new WaitForSeconds(dashDuration);
        isDashing = false;
        Invoke(nameof(ResetDash), dashCooldown);

    }

    void ResetDash()
    {
        canDash = true;
    }
}
