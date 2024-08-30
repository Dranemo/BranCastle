using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverScene : MonoBehaviour
{
    private void Awake()
    {
        try
        {
            GameObject gameManager = GameObject.FindObjectOfType<GameManager>().gameObject;
            if (gameManager != null)
            {
                Destroy(gameManager);
            }
        }
        catch (Exception e)
        {
            //Debug.LogError("Erreur lors de la destruction du GameManager : " + e.Message);

        }

    }
}
