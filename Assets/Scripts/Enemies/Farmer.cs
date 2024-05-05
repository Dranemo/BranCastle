using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Farmer : Enemy
{
    // Start is called before the first frame updatee
    private void Awake()
    {
        base.Awake();

        speed = 1f;
        health = 30f;
        damage = 150f;
        bloodCount = 10;
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
