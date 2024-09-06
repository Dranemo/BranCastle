using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class King : Physical
{
    [SerializeField] private GameObject jester;
    private float coolDownAttSpeTimer = 0;
    private float coolDownAttSpe = 3;


    new private void Update()
    {
        

        base.Update();

        if (coolDownAttSpeTimer <= 0)
        {
            SpecialAttack();
            coolDownAttSpeTimer = coolDownAttSpe;
        }

        coolDownAttSpeTimer -= Time.deltaTime;
    }

    void SpecialAttack()
    {
        if (jester != null)
        {
            GameObject jesterO = GameObject.Instantiate(jester, transform.position, Quaternion.identity);
            jesterO.transform.parent = transform;
        }
    }


}
