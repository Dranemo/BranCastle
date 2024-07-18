using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitCircle : MonoBehaviour
{
    [SerializeField] private Unit unitScript;
    private bool continueAttack;
    private void Awake()
    {
        unitScript = GetComponentInParent<Unit>();
    }

    void Start()
    {
        if (unitScript == null)
        {
            Debug.LogError("Script Unit non trouvé sur le parent!");
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            unitScript.enemiesInRange.Add(other.gameObject);
            unitScript.StartAttack(other.gameObject); // Utilisez directement other.gameObject
        }
    }


    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            unitScript.enemiesInRange.Remove(other.gameObject);
        }
    }
}