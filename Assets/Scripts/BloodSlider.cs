using UnityEngine;
using UnityEngine.UI;

public class BloodSlider : MonoBehaviour
{
    private Slider slider;
    GameManager manager;

    void Start()
    {
        manager = GameManager.Instance;
        slider = GetComponent<Slider>();
        slider.minValue = 0;
        slider.maxValue = 7000;
    }

    void Update()
    {
        slider.value = manager.GetBlood();
    }
}