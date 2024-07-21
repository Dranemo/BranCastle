using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypoint : MonoBehaviour
{
    public float distanceNextWaypoint;
    public float distanceRitual;

    public Waypoint nextWaypoint;

    public void SetDistanceBetweenWaypoints()
    {
        distanceNextWaypoint = 0;
        distanceRitual = 0;


        distanceNextWaypoint = Vector2.Distance(transform.position, nextWaypoint.transform.position);
    }

}
