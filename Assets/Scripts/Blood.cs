using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blood : MonoBehaviour
{
    public float attractionDistance = 5f;
    public float speed = 2f;
    private GameObject player;
    GameManager manager;

    void Start()
    {
        manager = GameManager.Instance;
        player = GameObject.FindGameObjectWithTag("Player");
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
        if (other.gameObject.CompareTag("Player"))
        {
            // Add the blood to the player
            manager.AddBlood(1);
            Destroy(gameObject);
        }
    }
}
