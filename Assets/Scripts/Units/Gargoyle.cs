using UnityEngine;

public class Gargoyle : Unit
{
    private UnitCircle unitCircleScript;
    private float healthWall;
    private GameObject unitCircle;
    private enum GargoyleState
    {
        Wall,
        Idle,
        Attack,
        Move
    }
    private GargoyleState state = GargoyleState.Idle;

    private void Start()
    {
        healthWall = health * 2;
        Debug.Log("Gargoyle health is " + health); 
        Debug.Log("Gargoyle healthwall is " + healthWall);
        TakeDamage(100);
        Debug.Log("Gargoyle health is " + health);
        unitCircleScript = GetComponentInChildren<UnitCircle>();

        if (unitCircleScript != null)
        {
            unitCircle = unitCircleScript.gameObject;
        }
        else
        {
            Debug.LogWarning("UnitCircle script not found on any children of Gargoyle.");
        }
    }
    private void Update()
        {
            if (unitCircleScript != null && unitCircleScript.isPlayerInside)
            {
                if (Input.GetButtonDown("Coffin"))
                {
                    if (state == GargoyleState.Idle)
                    {
                        state = GargoyleState.Wall;
                        Debug.Log("Gargoyle is wall.");
                    }
                    else if (state == GargoyleState.Wall)
                    {
                        state = GargoyleState.Idle;
                        Debug.Log("Gargoyle is now idle.");
                    }
                }
            }
            if (state == GargoyleState.Wall)
        {
            //Animation wall
            health = healthWall;
            unitCircleScript.triggerActive = false ;
        }
        else if (state == GargoyleState.Idle)
        {
            health = healthWall / 2;
            unitCircleScript.triggerActive = true;
        }
        else if (state == GargoyleState.Attack)
        {
                // Do attack stuff
                health = healthWall / 2;
            unitCircleScript.triggerActive = true;
        }
            else if (state == GargoyleState.Move)
        {
                // Do move stuffs
                health = healthWall / 2;
            unitCircleScript.triggerActive = true;
        }
        }

    }
