using UnityEngine;

public class Punch : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            //////////Debug.Log("Hit Enemy");
            Enemy enemy = collision.GetComponent<Enemy>();
            enemy.TakeDamage(10, true);
        }
    }
}   