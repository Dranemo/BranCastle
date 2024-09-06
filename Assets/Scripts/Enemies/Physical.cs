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
        base.Attack();

        if (state == State.AttackingPlayer)
        {
            GameManager.Instance.TakeDamage(damage);
        }
        else if (state == State.AttackingRitual)
        {
            GameManager.Instance.RitualTakeDamage(damage);
        }
        else if (state == State.AttackingUnit)
        {
            if (closestUnit != null)
            {
                if(closestUnit.gameObject.GetComponent<Unit>() != null)
                    closestUnit.gameObject.GetComponent<Unit>().TakeDamage(damage);
                else if (closestUnit.gameObject.GetComponent<Enemy>() != null)
                    closestUnit.gameObject.GetComponent<Enemy>().TakeDamage(damage, false);
            }
            else if(targetEnemy != null)
            {
                targetEnemy.gameObject.GetComponent<Enemy>().TakeDamage(damage, false);
            }
    }
    }
}
