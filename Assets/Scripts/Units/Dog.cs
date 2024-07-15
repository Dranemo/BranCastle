using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dog : Unit
{
    // Start is called before the first frame update
    void Start()
    {
        health= 100;
        damage= 10;
        attackSpeed= 0.1f;
        bloodCost= 10;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
