using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] Button[] buttons;
    [SerializeField] Canvas canvaOption;
    [SerializeField] Canvas canvaAchievement;
    [SerializeField] Canvas canvaLanguage;

    void Awake()
    {
        //buttons = transform.Find("Buttons").GetComponentsInChildren<Button>();

        buttons[0].onClick.AddListener(Play);
        buttons[1].onClick.AddListener(Options);
        buttons[2].onClick.AddListener(AchievementButton);
        buttons[3].onClick.AddListener(Quit);
        buttons[4].onClick.AddListener(Language);

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Play()
    {
        ScenesManager.LoadScene("Level-1 Dranemo");
    }

    void Options() {
        canvaOption.gameObject.SetActive(true);
        gameObject.SetActive(false);
        Debug.Log("Options");
    }

    void AchievementButton() {
        canvaAchievement.GetComponent<AchievementMenu>().Activate();
        Debug.Log("Achievement");
    }

    void Quit() {
        Application.Quit();
    }

    void Language() {
        canvaLanguage.gameObject.SetActive(true);
        gameObject.SetActive(false);
        Debug.Log("Language");
    }
}
