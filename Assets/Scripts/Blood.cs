using UnityEngine;

public class Blood : MonoBehaviour
{
    [SerializeField] float attractionDistance = 0f;
    [SerializeField] float speed = 2f;
    [SerializeField] GameObject player;
    //[SerializeField] float pickupRange = 0.5f;
    GameManager manager;


    public static float bloodAmountBase = 100;
    public float bloodAmount = 100;
    private float timeMoving = 0;

    // Direction angle
    public int directionAngleProjection = 0;
    private float directionAngleRad = 0;
    private float directionAngleX = 0;
    private float directionAngleY = 0;
    private Vector3 directionVector = Vector3.zero;

    //private bool spawning = true;

    void Start()
    {
        manager = GameManager.Instance;
        player = GameObject.FindGameObjectWithTag("Player");

        directionAngleRad = directionAngleProjection * Mathf.Deg2Rad;
        directionAngleX = Mathf.Cos(directionAngleRad);
        directionAngleY = Mathf.Sin(directionAngleRad);
        directionVector = new Vector3(directionAngleX, directionAngleY, 0);
        directionVector.Normalize();

        directionVector = transform.position + directionVector;


        ////Debug.Log(bloodAmount);
    }

    void Update()
    {
        if (timeMoving < 2)
        {
            //transform.position = Vector3.MoveTowards(transform.position, directionVector, 5 * Time.deltaTime);
            transform.position = Vector3.Lerp(transform.position, directionVector, timeMoving);

            timeMoving += Time.deltaTime;
        }

        else if (Vector3.Distance(transform.position, player.transform.position) < attractionDistance)
        {
            transform.position = Vector3.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject == player)
        {
            // Add the blood to the player
            ////Debug.Log("Blood collected");

            manager.AddBlood(bloodAmount);
            Destroy(gameObject);
        }
    }


}
