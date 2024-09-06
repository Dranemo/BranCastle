using UnityEngine;
using UnityEngine.UI;

public class OptionMenu : MonoBehaviour
{
    [SerializeField] Canvas mainMenu;
    [SerializeField] Button[] buttons;
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
        mainMenu.gameObject.SetActive(true);
        gameObject.SetActive(false);
    }
}
