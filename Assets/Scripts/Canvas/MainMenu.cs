using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] Button[] buttons;
    [SerializeField] Canvas canvaOption;
    [SerializeField] Canvas canvaAchievement;
    [SerializeField] Canvas canvaLanguage;

    [SerializeField] GameObject Background;

    void Awake()
    {
        if(GameObject.FindGameObjectWithTag("MenuBG") == null)
        {
            GameObject go = Instantiate(Background, new Vector3(0, 0, 0), Quaternion.identity);
            DontDestroyOnLoad(go);
        }

       
        //buttons = transform.Find("Buttons").GetComponentsInChildren<Button>();

        buttons[0].onClick.AddListener(Play);
        buttons[1].onClick.AddListener(Options);
        buttons[2].onClick.AddListener(AchievementButton);
        buttons[3].onClick.AddListener(Quit);
        buttons[4].onClick.AddListener(Language);

        if(GameManager.Instance != null)
        {
            Destroy(GameManager.Instance.gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Play()
    {
        ScenesManager.Instance.LoadScene("Tuto");
    }

    void Options() {
        canvaOption.gameObject.SetActive(true);
        gameObject.SetActive(false);
        //////////Debug.Log("Options");
    }

    void AchievementButton() {
        canvaAchievement.GetComponent<AchievementMenu>().Activate();
        //////////Debug.Log("Achievement");
    }

    void Quit() {
        Application.Quit();
    }

    void Language() {
        canvaLanguage.gameObject.SetActive(true);
        gameObject.SetActive(false);
        //////////Debug.Log("Language");
    }
}
