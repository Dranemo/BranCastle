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

        scoreText.GetComponent<TextMeshProUGUI>().text = "NULLLL BOUUUHHH";
    }

    void TaskOnClick()
    {
        SceneManager.LoadScene("Level-1");
    }
}
