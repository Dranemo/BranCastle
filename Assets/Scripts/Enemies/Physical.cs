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
            GameManager.Instance.TakeDamage(damage, true);
        }
        else if (state == State.AttackingRitual)
        {
            GameManager.Instance.TakeDamage(damage);
        }
        else if (state == State.AttackingUnit && gameObject.layer != 7)
        {
            if (closestUnit.gameObject.GetComponent<Unit>() != null)
            {
                closestUnit.gameObject.GetComponent<Unit>().TakeDamage(damage);
            }
            else
            {
                closestUnit.gameObject.GetComponent<Enemy>().TakeDamage(damage);
            }
    }
    }
}
