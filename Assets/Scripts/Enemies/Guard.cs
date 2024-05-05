using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Guard : Enemy
{
    // Start is called before the first frame update
    private void Awake()
    {
        base.Awake();

        speed = 1.5f;
        health = 100f;
        damage = 100f;
        bloodCount = 75;
    }
    void Start()
    {
        base.Start();

    }

    // Update is called once per frame
    void Update()
    {
        base.Update();
        FollowPath();
    }
}   
