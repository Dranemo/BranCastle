using System.Collections.Generic;
using UnityEngine;

public class Path : MonoBehaviour
{
    public List<Waypoint> waypoints;
    public Path nextPath;
    public float distancePath = 0;



    public void SetDistancePath(bool isSpawn = true)
    {
        foreach (Waypoint waypoint in waypoints)
        {
            waypoint.SetDistanceBetweenWaypoints();
        }


        if (nextPath != null)
        {
            distancePath += nextPath.distancePath;
        }


        for (int i = waypoints.Count - 1; i >= 0; i--)
        {
            distancePath += waypoints[i].distanceNextWaypoint;

            waypoints[i].distanceRitual = distancePath;
        }
    }
}
