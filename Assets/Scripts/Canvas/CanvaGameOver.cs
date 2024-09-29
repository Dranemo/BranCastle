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

    [SerializeField] GameObject[] texts;


    void Start()
    {
        texts[0].GetComponent<TextMeshProUGUI>().text = LanguageManager.GetText("score");
        texts[1].GetComponent<TextMeshProUGUI>().text = LanguageManager.GetText("replay");
        texts[2].GetComponent<TextMeshProUGUI>().text = LanguageManager.GetText("menu");

        DontDestroyOnLoad(GameObject.Find("CanvasBackground"));
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

            text = LanguageManager.GetText("portalOpen");
            victoryText = LanguageManager.GetText("victory");

            if(gameManager.blood >= 700)
            {
                AchievementManager.instance.UnlockAchievement(AchievementManager.AchievementID.WinBlood);
            }
        }
        else
        {
            AchievementManager.instance.UnlockAchievement(AchievementManager.AchievementID.firstLose);

            text = LanguageManager.GetText("waveDisplay");
            foreach (var c in text)
            {
                if (c == 'X')
                {
                    text = text.Replace("X", gameManager.wave.ToString());
                    break;
                }
            }


            victoryText = LanguageManager.GetText("gameOver");

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
