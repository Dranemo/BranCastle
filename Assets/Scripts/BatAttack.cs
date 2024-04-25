using UnityEngine;
using System.Collections;
public class BatAttack : MonoBehaviour
{
    public float minSpeed = 4f; // The minimum speed of the bat
    public float maxSpeed = 5f; // The maximum speed of the bat
    private float speed; // The actual speed of the bat
    private Vector2 direction;
    private Vector2 startPosition;
    private Rigidbody2D rb;
    public float maxDistance = 5f; // The maximum distance the bat can travel

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        startPosition = transform.position;
        speed = Random.Range(minSpeed, maxSpeed); // Set the speed to a random value between minSpeed and maxSpeed
    }

    void Update()
    {
        if (rb != null)
        {
            // Move the bat in the direction of the vector
            rb.velocity = direction * speed;
        }

        // Check if the bat has traveled beyond the maximum distance
        if (Vector2.Distance(startPosition, transform.position) > maxDistance)
        {
            Destroy(gameObject);
        }
    }

    public void SetDirection(Vector2 newDirection)
    {
        direction = newDirection;
    }



private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            Enemy enemy = other.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(1);
            }
            Destroy(gameObject);
        }
    }
}    