using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using static UnityEngine.ParticleSystem;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float dashForce = 10f;
    public float dashDuration = 3f;
    public float coupDistance = 2f;
    public float batDistance = 2.5f;
    public GameObject coupPrefab;
    public float punchForce = 0.1f;
    private GameObject currentCoup;
    public Sprite upSprite;
    public Sprite downSprite;
    public Sprite leftSprite;
    public Sprite rightSprite;
    private Rigidbody2D rb;
    private Vector2 movement;
    private SpriteRenderer spriteRenderer;
    // Define the angle ranges and corresponding sprites
    (float[], Sprite)[] angleSpriteMap;
    private bool punchEnabled = true;
    private GameObject rectangle;
    private bool isDrawingRectangle = false;
    public GameObject batPrefab;
    private TrailRenderer trail;
    private bool isDashing = false;
    public GameManager gameManager;
    private ParticleSystem particles;
    private Coffin nearestCoffin = null;
    private bool canMove = true;
    private bool isOverviewActivated = false;
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
        angleSpriteMap = new (float[], Sprite)[]
        {
        (new float[] {-45f, 45f}, rightSprite),
        (new float[] {45f, 135f}, upSprite),
        (new float[] {135f, 180f}, leftSprite),
        (new float[] {-180f, -135f}, leftSprite),
        (new float[] {-135f, -45f}, downSprite)
        };
    }
    void Start()
    {
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
        if (canMove)
        {
            movement.x = Input.GetAxis("Horizontal");
            movement.y = Input.GetAxis("Vertical");
        }

        Vector2 direction = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        foreach (var (angleRange, sprite) in angleSpriteMap)
        {
            if (angle > angleRange[0] && angle <= angleRange[1])
            {
                spriteRenderer.sprite = sprite;
                break;
            }
        }
        if (punchEnabled && Input.GetMouseButtonDown(0) && currentCoup == null && !isDrawingRectangle)
        {
            StartCoroutine(ResetVelocityAfterDash());
            Vector3 coupPosition = transform.position + new Vector3(direction.x, direction.y, 0) * coupDistance;


            currentCoup = Instantiate(coupPrefab, coupPosition, Quaternion.Euler(0, 0, angle+90));
            StartCoroutine(DestroyAfterSeconds(currentCoup, 0.2f));
        }
        // Movement
        transform.position += (Vector3)movement * moveSpeed * Time.deltaTime;

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
            FindObjectOfType<MapOverview>().ActivateOverview();
            GetComponent<SpriteRenderer>().enabled = false;
            canMove = false;
            isOverviewActivated = true;
        }
        // Désactivation de la vue d'ensemble
        else if (Input.GetButtonDown("Coffin") && isOverviewActivated)
        {
            FindObjectOfType<MapOverview>().DeactivateOverview(transform.position);
            GetComponent<SpriteRenderer>().enabled = true;
            canMove = true;
            isOverviewActivated = false;
        }
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


    IEnumerator DestroyAfterSeconds(GameObject gameObject, float seconds)
    {
        yield return new WaitForSeconds(seconds);
        Destroy(gameObject);
        if (gameObject == currentCoup)
        {
            currentCoup = null;
        }
    }
    IEnumerator ResetVelocityAfterDash()
    {
        yield return new WaitForSeconds(dashDuration);
        rb.velocity = Vector2.zero;
        isDashing=false;
    }
}
