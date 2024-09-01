using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class CanvaInGame : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timeText;
    [SerializeField] private TextMeshProUGUI waveText;
    [SerializeField] private TextMeshProUGUI ritualText;
    public GameObject dog;
    public GameObject ghoul;
    public GameObject imp;
    public GameObject gargoyle;
    float time;
    float wave;
    private void Start()
    {
        dog = GameObject.Find("Panel_dog");
        ghoul = GameObject.Find("Panel_ghoul");
        imp = GameObject.Find("Panel_imp");
        gargoyle = GameObject.Find("Panel_gargoyle");
        ritualText.enabled = false;
    }
    // Update is called once per frame
    void Update()
    {
        time = GameManager.Instance.time;
        wave = GameManager.Instance.wave;

        int minutes = Mathf.FloorToInt(time / 60F);
        int seconds = Mathf.FloorToInt(time % 60F);
        timeText.text = string.Format("{0:00}:{1:00}", minutes, seconds);

        waveText.text = "Vague : " + wave;

    }
}
