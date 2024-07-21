using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypoint : MonoBehaviour
{
    public int distanceRitual;

    [SerializeField] Waypoint nextWaypoint;

    private void Awake()
    {
        
    }
}
