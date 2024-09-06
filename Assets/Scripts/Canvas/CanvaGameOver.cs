using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CanvaGameOver : MonoBehaviour
{
    [SerializeField] Button buttonLevelOne;
    [SerializeField] Button buttonMainMenuo;
    [SerializeField] GameObject VictoryText;
    [SerializeField] GameObject scoreText;
    [SerializeField] GameObject sceneManager;


    void Start()
    {
        sceneManager = GameObject.Find("SceneManager");

        Button btn = buttonLevelOne.GetComponent<Button>();
        btn.onClick.AddListener(TaskOnClick);

        buttonMainMenuo.onClick.AddListener(() => sceneManager.GetComponent<ScenesManager>().LoadScene("MainMenu"));

        GameManager gameManager = GameManager.Instance;


        string text = "";
        string victoryText = "";
        if(gameManager.kingKilled)
        {
            AchievementManager.instance.UnlockAchievement(AchievementManager.AchievementID.firstFinish);

            text = "Le portail est ouvert !";
            victoryText = "Victoire !";

            if(gameManager.blood >= 700)
            {
                AchievementManager.instance.UnlockAchievement(AchievementManager.AchievementID.WinBlood);
            }
        }
        else
        {
            AchievementManager.instance.UnlockAchievement(AchievementManager.AchievementID.firstLose);

            text = "Vous avez atteint la " + gameManager.wave + "ème vague !";
            victoryText = "Game Over !";

            if(gameManager.wave >= 9)
            {
                AchievementManager.instance.UnlockAchievement(AchievementManager.AchievementID.LoseNine);
            }
        }

        scoreText.GetComponent<TextMeshProUGUI>().text = text;
        VictoryText.GetComponent<TextMeshProUGUI>().text = victoryText;
    }

    void TaskOnClick()
    {
        GameManager.Instance.RestartGame();
    }
}
