using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CanvaInGame : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timeText;
    [SerializeField] private TextMeshProUGUI waveText;

    float time;
    float wave;

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
