using UnityEngine;

public class Projectile : MonoBehaviour
{
    public string enemyTag = "Enemy";
    public Transform towerTransform; // The transform of the tower that shot this projectile
    [SerializeField] float range = 2;
    GameManager gameManager;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == enemyTag)
        {
            Enemy enemy = collision.gameObject.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(5);
            }
            Destroy(gameObject);
        }
    }


    private void Update()
    {
        // Destroy the projectile when it leaves the range
        if (Vector2.Distance(transform.position, towerTransform.position) > range)
        {
            Destroy(gameObject);
        }
    }
}
