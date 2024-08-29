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
        slider.maxValue = 5000;
        if (bloodGaugeSlider != null && bloodGaugeFill != null)
        {
            bloodGaugeSlider.onValueChanged.AddListener(UpdateBloodGauge);
        }
    }
    
    void Update()
    {
        slider.value = manager.blood;
    }
    [SerializeField] private Slider bloodGaugeSlider;
    [SerializeField] private Image bloodGaugeFill;

    private void UpdateBloodGauge(float value)
    {
        float fillValue = Mathf.Clamp01(value / bloodGaugeSlider.maxValue);
        bloodGaugeFill.fillAmount = fillValue;
    }
}