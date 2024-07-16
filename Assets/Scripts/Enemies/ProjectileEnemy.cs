using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileEnemy : MonoBehaviour
{
    public GameObject target;

    private Vector3 startPos;
    private Vector3 targetPos;
    Vector3 normalizedPos;

    public float damage = 1;
    [SerializeField] float speed = 1;

    [SerializeField] float distanceDestroy = 10f;

    // Start is called before the first frame update
    private void Awake()
    {

        startPos = transform.position;
    }


    void Start()
    {

        targetPos = target.transform.position;

        normalizedPos = (targetPos - startPos).normalized;
    }

    // Update is called once per frame
    void Update()
    {
        if(Vector3.Distance(startPos, transform.position) > distanceDestroy)
        {
            Destroy(gameObject);
        }
        else
        {
            transform.position += normalizedPos * speed * Time.deltaTime;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject == target && (target.tag == "Player" || target.tag == "Ritual"))
        {
            GameManager.Instance.TakeDamage(damage);
            Debug.Log("Player hit by projectile");
            Destroy(gameObject);
        }
        else if (collision.gameObject == target && target.tag == "Unit")
        {
            collision.gameObject.GetComponent<Unit>().TakeDamage(damage);
            Debug.Log("Unit hit by projectile");
            Destroy(gameObject);
        }
        else if (collision.gameObject.tag == "Wall")
        {
            Destroy(gameObject);
        }
    }
}
