using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;



public class Enemy: MonoBehaviour
{
    public List<Path> paths;
    private int currentPathIndex = 0;
    public Transform spawn;
    private int currentWaypointIndex = 0;
    private float detectionRadius = 5f;
    protected float speed = 1;
    protected float health;
    protected float damage;
    protected GameObject player;
    [SerializeField]
    public static GameObject bloodPrefab;
    public int bloodCount;
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
    protected void Awake()
    {
        bloodPrefab = Resources.Load<GameObject>("Blood");
        Debug.Log(bloodPrefab);
    }

    protected void Start()
    {
        //utez vos waypoints à la liste ici, ou initialisez-les dans l'inspecteur Unity
        player = GameObject.FindGameObjectWithTag("Player");
        if (paths != null && paths.Any() && paths[0].waypoints.Count > 0)
        {
            StartCoroutine(FollowPath());
        }
        OnDrawGizmos();
    }
    protected void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
        if (distanceToPlayer < detectionRadius)
        {
            Vector3 directionToPlayer = player.transform.position - transform.position;
            float angle = Mathf.Atan2(directionToPlayer.y, directionToPlayer.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, angle);
        }
        if (paths == null || !paths.Any() || paths[0].waypoints.Count == 0)
            return;

        Transform currentWaypoint = paths[0].waypoints[currentWaypointIndex];
        transform.position = Vector3.MoveTowards(transform.position, currentWaypoint.position, Time.deltaTime);

        if (transform.position == currentWaypoint.position)
        {
            currentWaypointIndex = (currentWaypointIndex + 1) % paths[0].waypoints.Count;
        }
    }

    void OnDrawGizmos()
    {
        if (paths == null || !paths.Any())
            return;

        foreach (Path path in paths)
        {
            if (path == null || path.waypoints == null || path.waypoints.Count < 4)
                continue;

            Vector3[] points = path.waypoints.Select(w => w.position).ToArray();

            // Draw the control point
            Gizmos.color = Color.red;
            foreach (Vector3 point in points)
            {
                Gizmos.DrawSphere(point, 0.1f);
            }

            // Draw the Catmull-Rom spline
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
    }




    int CircularIndex(int i, int len)
    {
        return (i + len) % len;
    }


    protected void Die()
    {
        for (int i = 0; i < bloodCount; i++)
        {
            Instantiate(bloodPrefab, transform.position, Quaternion.identity);
        }
        Destroy(gameObject);
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
    protected IEnumerator FollowPath()
    {
        if (paths == null || !paths.Any())
        {
            yield break;
        }

        while (true)
        {
            Path currentPath = paths[currentPathIndex];
            if (currentPath.waypoints.Count == 0)
            {
                yield break;
            }

            Transform currentWaypoint = currentPath.waypoints[currentWaypointIndex];
            while (Vector3.Distance(transform.position, currentWaypoint.position) > 0.1f)
            {
                transform.position = Vector3.MoveTowards(transform.position, currentWaypoint.position, speed * Time.deltaTime);
                yield return null;
            }

            currentWaypointIndex = (currentWaypointIndex + 1) % currentPath.waypoints.Count;
            if (currentWaypointIndex == 0) // If we've looped back to the start, move to the next path
            {
                currentPathIndex = (currentPathIndex + 1) % paths.Count;
            }
        }
    }





}
