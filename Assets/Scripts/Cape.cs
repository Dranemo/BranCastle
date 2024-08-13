using UnityEngine;

public class Cape : MonoBehaviour
{
    GameObject Player;
    public float capeDPS;


    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        capeDPS = Player.GetComponent<PlayerMovement>().capeDMG;
    }

    // Update is called once per frame
    void Update()
    {

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            Enemy enemy = collision.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.isStunned = true;
                enemy.TakeDamage(capeDPS);
            }
        }


    }
} 