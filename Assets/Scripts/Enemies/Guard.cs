using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Guard : Enemy
{
    // Start is called before the first frame update
    void Start()
    {
        base.Start();
        speed = 5f;
        health = 100f;
        damage = 100f;
        bloodCount = 75;

    }

    // Update is called once per frame
    void Update()
    {
        base.Update();
        FollowPath();
    }
}   
