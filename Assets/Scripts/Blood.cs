using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blood : MonoBehaviour
{
    [SerializeField] float attractionDistance = 0f;
    [SerializeField] float speed = 2f;
    [SerializeField] GameObject player;
    [SerializeField] float pickupRange = 0.5f;
    GameManager manager;


    public float bloodAmount = 1;

    void Start()
    {
        manager = GameManager.Instance;
        player = GameObject.FindGameObjectWithTag("Player");

        Debug.Log(bloodAmount);
    }

    void Update()
    {
        if (Vector3.Distance(transform.position, player.transform.position) < attractionDistance)
        {
            transform.position = Vector3.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject == player)
        {
            // Add the blood to the player
            Debug.Log("Blood collected");

            manager.AddBlood(bloodAmount);
            Destroy(gameObject);
        }
    }


}
