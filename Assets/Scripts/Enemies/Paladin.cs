using UnityEngine;

public class Paladin : Physical
{
    [SerializeField] private float coolDownAttSpe = 30;
    private float coolDownAttSpeTimer = 0;


    new private void Update()
    {

        base.Update();

        if (closestUnit != null)
        {
            ////////Debug.Log("Disctance dog : " + Vector3.Distance(transform.position, closestUnit.transform.position));

            if (coolDownAttSpeTimer <= 0)
            {
                SpecialAttack();
                coolDownAttSpeTimer = coolDownAttSpe;
            }
        }


        coolDownAttSpeTimer -= Time.deltaTime;
    }

    void SpecialAttack()
    {
        if(GetComponent<Animator>() != null) 
            GetComponent<Animator>().Play("AttackSpecial1");
        if (closestUnit != null)
        {
        closestUnit.GetComponent<Unit>().TakeDamage(damage * 5); 
        }
    }

}
