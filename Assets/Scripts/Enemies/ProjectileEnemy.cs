using UnityEngine;

public class ProjectileEnemy : MonoBehaviour
{
    public GameObject target;

    private Vector3 startPos;
    private Vector3 targetPos;
    Vector3 normalizedPos;
    public bool searchingTarget = false;

    public float damage = 1;
    public float timeUntilDestroy = 10;
    private float timeElapsed = 0;

    [SerializeField] float speed = 1;

    public void Activate()
    {
        gameObject.SetActive(true);
        //Debug.Break();

        startPos = transform.position;
        timeElapsed = 0;

        targetPos = target.transform.position;
        normalizedPos = (targetPos - startPos).normalized;

        gameObject.layer = 10;

        // Initial rotation towards the target
        RotateTowardsTarget();
    }

    private void Deactivate()
    {
        gameObject.SetActive(false);

        gameObject.layer = 12;
    }

    // Update is called once per frame
    void Update()
    {
        if (gameObject.activeSelf == true)
        {
            if (timeElapsed >= timeUntilDestroy)
            {
                Deactivate();
            }
        }
    }

    private void FixedUpdate()
    {
        if (gameObject.activeSelf == true && timeElapsed <= timeUntilDestroy)
        {
            if (searchingTarget)
            {
                if (target != null)
                {
                    targetPos = target.transform.position;
                }
                normalizedPos = (targetPos - transform.position).normalized;
            }

            // Rotate towards the target
            RotateTowardsTarget();

            transform.position += normalizedPos * speed * Time.deltaTime;
            timeElapsed += Time.deltaTime;
        }
    }

    private void RotateTowardsTarget()
    {
        Vector3 direction = targetPos - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle - 90)); 
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject == target && (target.tag == "Player" || target.tag == "Ritual"))
        {
            GameManager.Instance.TakeDamage(damage);
            //Debug.Log("Player hit by projectile");
            Deactivate();
        }
        else if (collision.gameObject == target && target.tag == "Unit")
        {
            if (collision.gameObject.GetComponent<Unit>() != null)
            {
                collision.gameObject.GetComponent<Unit>().TakeDamage(damage);
            }
            else
            {
                collision.gameObject.GetComponent<Enemy>().TakeDamage(damage);
            }

            //Debug.Log("Unit hit by projectile");
            Deactivate();
        }
        else if (collision.gameObject.layer == 11)
        {
            Deactivate();
        }
    }
}
