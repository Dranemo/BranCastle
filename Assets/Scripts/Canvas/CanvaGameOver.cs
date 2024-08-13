using TMPro;
using UnityEngine;
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


        string text = "";
        if(gameManager.time > 1439)
        {
            text = "Vous avez survécu !";
        }
        else
        {
            text = "Vous avez atteint la " + gameManager.wave + "ème vague !";
        }

        scoreText.GetComponent<TextMeshProUGUI>().text = text;
    }

    void TaskOnClick()
    {
        GameManager.Instance.RestartGame();
    }
}
