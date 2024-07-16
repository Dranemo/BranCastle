using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Farmer : Enemy
{

    



    // Start is called before the first frame updatee
    private void Awake()
    {
        base.Awake();
    }
    void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    void Update()
    {
        base.Update();
    }


    protected override void Attack()
    {
        if (state == State.AttackingPlayer)
        {
            GameManager.Instance.TakeDamage(damage);
        }
        else if (state == State.AttackingRitual)
        {
            GameManager.Instance.TakeDamage(damage);
        }
        else if (state == State.AttackingUnit)
        {
            closestUnit.GetComponent<Unit>().TakeDamage(damage);
        }
    }

}
