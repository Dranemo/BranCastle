using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CanvaGameOver : MonoBehaviour
{
    [SerializeField] Button buttonLevelOne;
    [SerializeField] GameObject VictoryText;
    [SerializeField] GameObject scoreText;


    void Start()
    {
        Button btn = buttonLevelOne.GetComponent<Button>();
        btn.onClick.AddListener(TaskOnClick);


        GameManager gameManager = GameManager.Instance;


        string text = "";
        string victoryText = "";
        if(gameManager.time > 1439)
        {
            text = "Vous avez survécu !";
            victoryText = "Victoire !";
        }
        else
        {
            text = "Vous avez atteint la " + gameManager.wave + "ème vague !";
            victoryText = "Game Over !";
        }

        scoreText.GetComponent<TextMeshProUGUI>().text = text;
        VictoryText.GetComponent<TextMeshProUGUI>().text = victoryText;
    }

    void TaskOnClick()
    {
        GameManager.Instance.RestartGame();
    }
}
