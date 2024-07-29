using UnityEngine;

public class ProjectileUnit : MonoBehaviour
{
    [SerializeField] private float damage;

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Trigger d�tect� avec : " + other.gameObject.name);

        if (other.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("Projectile a touch� un ennemi (trigger) : " + other.gameObject.name);
            other.gameObject.GetComponent<Enemy>().TakeDamage(damage);
            Destroy(gameObject);
        }
    }
}


