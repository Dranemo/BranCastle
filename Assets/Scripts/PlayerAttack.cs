using UnityEngine;
using System.Collections;

public class PlayerAttack : MonoBehaviour
{
    public GameObject hitPrefab;
    public float spawnDistance = 1.0f; 
    public float hitLifetime = 2.0f; 
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
        if (animator == null)
        {
            Debug.LogError("Animator n'est pas assigné !");
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            SpawnHitPrefab();
        }
    }

    void SpawnHitPrefab()
    {
        if (hitPrefab == null)
        {
            Debug.LogError("hitPrefab n'est pas assigné !");
            return;
        }
        Vector3 playerPosition = transform.position;

        // Utiliser la profondeur de la caméra pour obtenir la position correcte du curseur en coordonnées du monde
        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = Camera.main.WorldToScreenPoint(playerPosition).z;
        Vector3 worldMousePosition = Camera.main.ScreenToWorldPoint(mousePosition);

        Vector3 direction = (worldMousePosition - playerPosition).normalized;

        Vector3 spawnPosition = playerPosition + direction * spawnDistance;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.Euler(0, 0, angle + 90);

        GameObject hitInstance = Instantiate(hitPrefab, spawnPosition, rotation);

        Animator hitAnimator = hitInstance.GetComponent<Animator>();
        if (hitAnimator != null)
        {
            hitAnimator.SetBool("isAttacking", animator.GetBool("isAttacking"));
            hitAnimator.SetBool("isAttacking2", animator.GetBool("isAttacking2"));
            hitAnimator.SetBool("isAttacking3", animator.GetBool("isAttacking3"));
        }
        else
        {
            Debug.LogError("hitPrefab n'a pas d'Animator !");
        }

        StartCoroutine(DestroyAfterTime(hitInstance, hitLifetime));
    }


    IEnumerator DestroyAfterTime(GameObject instance, float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(instance);
    }
}

