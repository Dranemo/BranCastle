using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OptionMenu : MonoBehaviour
{
    [SerializeField] Canvas mainMenu;

    [SerializeField] GameObject[] texts;

    [SerializeField] Button[] buttons;
    [SerializeField] GameObject languageDropDown;

    public AudioManager audioManager;
    public Slider musicVolumeSlider;
    public Slider SFXVolumeSlider;


    // Start is called before the first frame update
    void Awake()
    {
        gameObject.SetActive(false);


        buttons[0].onClick.AddListener(Back);

        audioManager = GameObject.Find("SceneManager").GetComponent<AudioManager>();

    }
    private void Start()
    {
        float currentVolume;
        audioManager.mixer.GetFloat("Music", out currentVolume);
        musicVolumeSlider.value = currentVolume;

        audioManager.mixer.GetFloat("SFX", out currentVolume);
        SFXVolumeSlider.value = currentVolume;


        // Ajouter un listener pour gérer les changements de sélection
        languageDropDown.GetComponent<TMP_Dropdown>().value = (int)LanguageManager.currentLanguage;
        languageDropDown.GetComponent<TMP_Dropdown>().onValueChanged.AddListener(ChangeLanguage);
    }
    public void OnMusicVolumeChange(float volume)
    {
        if (audioManager != null)
        {
            audioManager.SetMusicVolume(volume);
        }
        else
        {
            ////////Debug.LogError("AudioManager n'est pas assigné dans OptionsMenu.");
        }
    }

    public void OnSFXVolumeChange(float volume)
    {
        audioManager.SetSFXVolume(volume);
    }

    void Back()
    {
        mainMenu.gameObject.GetComponent<MainMenu>().Activate();
        gameObject.SetActive(false);
    }

    void ChangeLanguage(int index)
    {
        LanguageManager.currentLanguage = (LanguageManager.language)index;
        Activate();
    }

    public void Activate()
    {
        texts[0].GetComponent<TextMeshProUGUI>().text = LanguageManager.GetText("options");
        texts[1].GetComponent<TextMeshProUGUI>().text = LanguageManager.GetText("back");
        texts[2].GetComponent<TextMeshProUGUI>().text = LanguageManager.GetText("display");

        texts[3].GetComponent<TMP_Dropdown>().options[0].text = LanguageManager.GetText("windowedFullscreen");
        texts[3].GetComponent<TMP_Dropdown>().options[1].text = LanguageManager.GetText("windowed");
        texts[3].GetComponent<TMP_Dropdown>().options[2].text = LanguageManager.GetText("fullscreen");


        texts[4].GetComponent<TextMeshProUGUI>().text = LanguageManager.GetText("music");
        texts[5].GetComponent<TextMeshProUGUI>().text = LanguageManager.GetText("sound");
        texts[6].GetComponent<TextMeshProUGUI>().text = LanguageManager.GetText("language");
        gameObject.SetActive(true);
    }
}
