using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerUnitSpawn : MonoBehaviour
{
    List<GameObject> unitList = new List<GameObject>();
    public bool isTriggered = false;


    private void Update()
    {
        if (unitList.Count != 0)
        {
            isTriggered = true;
        }
        else 
            if (unitList.Count == 0)
        {
            isTriggered = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Unit")
        {
            unitList.Add(collision.gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Unit")
        {
            unitList.Remove(collision.gameObject);
        }
    }
}
