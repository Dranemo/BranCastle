using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

    public class Physical : Enemy
    {

        // Start is called before the first frame updatee
        new private void Awake()
        {
            base.Awake();
        }
        new void Start()
        {
            base.Start();
        }

        // Update is called once per frame
        new void Update()
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
