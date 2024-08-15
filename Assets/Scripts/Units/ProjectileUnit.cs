using UnityEngine;
using System.Collections;

public class ProjectileUnit : MonoBehaviour
{
    [SerializeField] private float damage;

    private void Start()
    {
        StartCoroutine(DestroyAfterTime(5f));
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            other.gameObject.GetComponent<Enemy>().TakeDamage(damage);
            Destroy(gameObject);
        }
    }

    private IEnumerator DestroyAfterTime(float time)
    {
        // Attendre le temps spécifié
        yield return new WaitForSeconds(time);
        // Détruire l'objet
        Destroy(gameObject);
    }
}
