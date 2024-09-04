using System.Collections;
using TMPro;
using UnityEngine;
//using UnityEngine.UIElements;
using UnityEngine.UI;

public class CanvaInGame : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timeText;
    [SerializeField] private TextMeshProUGUI waveText;
    [SerializeField] private TextMeshProUGUI ritualText;
    public GameObject dog;
    public GameObject ghoul;
    public GameObject imp;
    public GameObject gargoyle;
    [SerializeField] private GameObject bloodOnCanvaGO;
    private Image bloodOnCanva;



    private Coroutine currentCoroutine;
    bool isTintingUp = false;
    bool isTintingDown = false;


    float tintMax = 0.4f;

    float time;
    float wave;
    private void Start()
    {
        dog = GameObject.Find("Panel_dog");
        ghoul = GameObject.Find("Panel_ghoul");
        imp = GameObject.Find("Panel_imp");
        gargoyle = GameObject.Find("Panel_gargoyle");
        ritualText.enabled = false;

        bloodOnCanva = bloodOnCanvaGO.GetComponent<Image>();

        bloodOnCanva.color = new Color(bloodOnCanva.color.r, bloodOnCanva.color.g, bloodOnCanva.color.b, 0);

    }
    // Update is called once per frame
    void Update()
    {
        time = GameManager.Instance.time;
        wave = GameManager.Instance.wave;

        int totalSeconds = Mathf.FloorToInt(time);
        int minutes = (totalSeconds / 60) % 24;
        int seconds = totalSeconds % 60;

        if (totalSeconds == 1440) // 1440 seconds = 24 minutes
        {
            minutes = 0;
            seconds = 0;
        }

        timeText.text = string.Format("{0:00}:{1:00}", minutes, seconds);

        waveText.text = "Vague : " + wave;



        if (GameManager.Instance.blood < 200 && bloodOnCanva.color.a != tintMax && !isTintingUp)
        {
            if (currentCoroutine != null)
            {
                StopCoroutine(currentCoroutine);
                isTintingDown = false;
            }
            currentCoroutine = StartCoroutine(TintUp());
        }
        else if (GameManager.Instance.blood >= 200 && bloodOnCanva.color.a != 0 && !isTintingDown)
        {
            if (currentCoroutine != null)
            {
                StopCoroutine(currentCoroutine);
                isTintingUp = false;
            }
            currentCoroutine = StartCoroutine(TintDown());
        }
    }

    IEnumerator TintUp()
    {
        //Debug.Log("TintUp");
        isTintingUp = true;
        while (bloodOnCanva.color.a < tintMax)
        {
            bloodOnCanva.color = new Color(bloodOnCanva.color.r, bloodOnCanva.color.g, bloodOnCanva.color.b, bloodOnCanva.color.a + 0.01f);
            yield return new WaitForSeconds(0.01f);
        }
    }

    IEnumerator TintDown()
    {
        //Debug.Log("TintDown");
        isTintingDown = true;
        while (bloodOnCanva.color.a > 0)
        {
            bloodOnCanva.color = new Color(bloodOnCanva.color.r, bloodOnCanva.color.g, bloodOnCanva.color.b, bloodOnCanva.color.a - 0.01f);
            yield return new WaitForSeconds(0.01f);
        }
    }
}
