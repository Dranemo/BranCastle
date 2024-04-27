using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeTrigger : MonoBehaviour
{
    public float shootRate = 3f;
    public float shootSpeed = 2f;
    public GameObject projectilePrefab;
    private GameObject targetEnemy;
    // Start is called before the first frame update
    void Start()
    {
        GameManager gameManager = GameManager.Instance;
        if (gameManager == null)
        {
            Debug.LogError("GameManager instance is null dans rangetrigger");
            return;
        }
        CircleCollider2D collider = this.GetComponent<CircleCollider2D>();
        collider.radius = gameManager.radiusTowerOne;
    }

    // Update is called once per frame
    void Update()
    {
        if (targetEnemy != null)
        {
            // Shoot at the enemy
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            Debug.Log("Enemy detected");
            Shoot(collision);
        }
    }

    void Shoot(Collider2D collision)
    {
        Vector2 direction = collision.gameObject.transform.position - transform.position;
        GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
        projectile.GetComponent<Rigidbody2D>().velocity = direction.normalized * shootSpeed;
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
        }
    }
}
