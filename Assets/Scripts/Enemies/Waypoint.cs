using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypoint : MonoBehaviour
{
    public float distanceNextWaypoint;
    public float distanceRitual;
    [SerializeField] Color colorLine;

    public Waypoint nextWaypoint;

    public void SetDistanceBetweenWaypoints()
    {
        distanceNextWaypoint = 0;
        distanceRitual = 0;

        if(nextWaypoint != null)
            distanceNextWaypoint = Vector2.Distance(transform.position, nextWaypoint.transform.position);

    }


    private void OnDrawGizmos()
    {
        Gizmos.color = colorLine;

        if(nextWaypoint != null)
        {
            Gizmos.DrawLine(transform.position, nextWaypoint.transform.position);
        }
    }

}
