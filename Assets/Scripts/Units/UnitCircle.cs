using UnityEngine;

public class UnitCircle : MonoBehaviour
{
    private Unit unitScript;
    public bool isPlayerInside = false;
    public bool triggerActive = true;


    private void Awake()
    {
        unitScript = GetComponentInParent<Unit>();
    }

    void Start()
    {
        if (unitScript == null)
        {
            ////////Debug.LogError("Script Unit non trouvé sur le parent!");
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        ////////Debug.Log("triggerActive");

        if (other.gameObject.CompareTag("Enemy") && triggerActive)
        {
            ////////Debug.Log("enemy added normalement");

            unitScript.enemiesInRange.Add(other.gameObject);
            unitScript.StartAttack(other.gameObject);
        }
        else if (other.CompareTag("Player"))
        {
            isPlayerInside = true;
        }
    }


    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            unitScript.enemiesInRange.Remove(other.gameObject);
        }
        else if (other.CompareTag("Player"))
        {
            isPlayerInside = false;
        }
    }
}