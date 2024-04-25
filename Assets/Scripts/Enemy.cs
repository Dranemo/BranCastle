using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float detectionRadius = 5f;
    private GameObject player;
    public float speed = 2f;
    public float health = 10;
    public GameObject bloodPrefab;
    public int bloodCount = 5;
    public Transform[] waypoints;
    Vector3 CalculateCatmullRomPoint(float t, Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3)
    {
        float t2 = t * t;
        float t3 = t2 * t;

        Vector3 a = -0.5f * p0 + 1.5f * p1 - 1.5f * p2 + 0.5f * p3;
        Vector3 b = p0 - 2.5f * p1 + 2f * p2 - 0.5f * p3;
        Vector3 c = -0.5f * p0 + 0.5f * p2;
        Vector3 d = p1;

        return a * t3 + b * t2 + c * t + d;
    }
    void Die()
    {
        for (int i = 0; i < bloodCount; i++)
        {
            Instantiate(bloodPrefab, transform.position, Quaternion.identity);
        }
        Destroy(gameObject);
    }
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        StartCoroutine(FollowPath());
        OnDrawGizmos();
    }
    void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
        if (distanceToPlayer < detectionRadius)
        {
            Vector3 directionToPlayer = player.transform.position - transform.position;
            float angle = Mathf.Atan2(directionToPlayer.y, directionToPlayer.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, angle);
        }
    }
    void OnDrawGizmos()
    {
        if (waypoints == null || waypoints.Length < 4)
            return;

        Vector3[] points = waypoints.Select(w => w.position).ToArray();

        // Draw the control points
        Gizmos.color = Color.red;
        foreach (Vector3 point in points)
        {
            Gizmos.DrawSphere(point, 0.1f);
        }

        // Draw the Catmull-Rom splinee
        Gizmos.color = Color.white;
        for (int i = 0; i < points.Length; i++)
        {
            Vector3 p0 = points[CircularIndex(i - 1, points.Length)];
            Vector3 p1 = points[CircularIndex(i, points.Length)];
            Vector3 p2 = points[CircularIndex(i + 1, points.Length)];
            Vector3 p3 = points[CircularIndex(i + 2, points.Length)];

            Vector3 previousPosition = p1;
            for (float t = 0; t <= 1; t += 0.01f)
            {
                Vector3 position = CalculateCatmullRomPoint(t, p0, p1, p2, p3);
                Gizmos.DrawLine(previousPosition, position);
                previousPosition = position;
            }
        }
    }

    int CircularIndex(int i, int len)
    {
        return (i + len) % len;
    }



    public void TakeDamage(float damage)
    {
        health -= damage;
        Debug.Log(health);
        if (health <= 0)
        {
            Die();
        }
    }
    IEnumerator FollowPath()
    {
        if (waypoints.Length < 4)
        {
            yield break;
        }

        int currentWaypoint = 0;
        float t = 0;
        while (true)
        {
            Vector3 p0 = waypoints[CircularIndex(currentWaypoint - 1, waypoints.Length)].position;
            Vector3 p1 = waypoints[CircularIndex(currentWaypoint, waypoints.Length)].position;
            Vector3 p2 = waypoints[CircularIndex(currentWaypoint + 1, waypoints.Length)].position;
            Vector3 p3 = waypoints[CircularIndex(currentWaypoint + 2, waypoints.Length)].position;

            while (t < 1)
            {
                t += Time.deltaTime * speed;
                transform.position = CalculateCatmullRomPoint(t, p0, p1, p2, p3);
                yield return null;
            }

            t = 0;
            currentWaypoint = (currentWaypoint + 1) % waypoints.Length;
        }
    }
    


}
