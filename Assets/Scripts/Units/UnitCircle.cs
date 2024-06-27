using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitCircle : MonoBehaviour
{
    private Unit parentUnitCombat;

    private void Awake()
    {
        parentUnitCombat = GetComponentInParent<Unit>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            parentUnitCombat.StartAttack(collision.gameObject);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            parentUnitCombat.StartAttack(collision.gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            parentUnitCombat.StopAttack();
        }
    }
}
