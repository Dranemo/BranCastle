using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CanvaGameOver : MonoBehaviour
{
    [SerializeField] Button buttonLevelOne;
    [SerializeField] GameObject scoreText;


    void Start()
    {
        Button btn = buttonLevelOne.GetComponent<Button>();
        btn.onClick.AddListener(TaskOnClick);


        GameManager gameManager = GameManager.Instance;


        string text = "Vous avez survécu jusqu'à ";
        if(gameManager.time > 1439)
        {
            text += "minuit !";
        }
        else
        {
            text += "la " + gameManager.wave + "ème vague !";
        }

        scoreText.GetComponent<TextMeshProUGUI>().text = text;
    }

    void TaskOnClick()
    {
        GameManager.Instance.RestartGame();
    }
}
