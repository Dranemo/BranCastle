using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Farmer : Enemy
{
    // Start is called before the first frame update
    void Start()
    {
        base.Start();
        speed= 1.5f;
        health = 30f;
        damage = 150f;
        bloodCount = 10;
    }

    // Update is called once per frame
    void Update()
    {
        base.Update();
        FollowPath();
    }
}
