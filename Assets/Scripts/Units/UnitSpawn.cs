using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitSpawn : MonoBehaviour
{
    public GameObject dogPrefab;
    public int dogCost = 10;
    public float spawnDistance = 1f;

    private GameManager gameManager;
    private Unit dogUnit;
    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();

        // Instanciez un dog temporaire pour obtenir une référence à son script Unit
        GameObject tempDog = Instantiate(dogPrefab, Vector3.zero, Quaternion.identity);
        dogUnit = dogPrefab.GetComponent<Unit>();

        // Détruisez le dog temporaire
        Destroy(tempDog);
    }

    void Update()
    {
        if (Input.GetButtonDown("UnitSpawn"))
        {
            Debug.Log("Spawn dog");
            if (gameManager.blood >= dogCost)
            {
                Vector3 spawnPosition = transform.position + transform.forward * spawnDistance;
                GameObject dog = Instantiate(dogPrefab, spawnPosition, Quaternion.identity);
                gameManager.blood -= dogCost;
            }
        }
    }
}


