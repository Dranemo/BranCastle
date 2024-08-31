using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BloodSlider : MonoBehaviour
{
    private Slider slider;
    GameManager manager;

    [SerializeField] private Slider bloodGaugeSlider;
    [SerializeField] private Image bloodGaugeFill;
    [SerializeField] private Image bloodGaugeFillWhite;
    [SerializeField] private float fillSpeed = 1f;
    [SerializeField] private float maxFillSpeedWhite = 2f;
    [SerializeField] private float minFillSpeedWhite = 1f; // Adjusted to 1 for testing
    [SerializeField] private float delay = 10f; // Adjusted to 10 for testing

    private float targetFillAmount;
    private float targetFillAmountWhite;
    private bool isCoroutineRunning = false;

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
        UpdateBloodGauge(manager.blood);
        Debug.Log("Update: slider.value = " + slider.value);
    }

    private void UpdateBloodGauge(float value)
    {
        float fillValue = Mathf.Clamp01(value / bloodGaugeSlider.maxValue);
        targetFillAmount = fillValue;
        if (!isCoroutineRunning)
        {
            StartCoroutine(UpdateWhiteFillWithDelay(fillValue));
        }
        Debug.Log("UpdateBloodGauge: targetFillAmount = " + targetFillAmount);
    }

    private IEnumerator UpdateWhiteFillWithDelay(float fillValue)
    {
        isCoroutineRunning = true;
        yield return new WaitForSeconds(delay);
        targetFillAmountWhite = fillValue;
        isCoroutineRunning = false;
        Debug.Log("UpdateWhiteFillWithDelay: targetFillAmountWhite = " + targetFillAmountWhite);
    }

    void FixedUpdate()
    {
        if (bloodGaugeFill != null)
        {
            bloodGaugeFill.fillAmount = Mathf.Lerp(bloodGaugeFill.fillAmount, targetFillAmount, fillSpeed * Time.deltaTime);
            Debug.Log("FixedUpdate: bloodGaugeFill.fillAmount = " + bloodGaugeFill.fillAmount);
        }

        if (bloodGaugeFillWhite != null)
        {
            // Calculer la différence entre les deux valeurs de remplissage
            float difference = Mathf.Abs(bloodGaugeFill.fillAmount - bloodGaugeFillWhite.fillAmount);

            // Ajuster dynamiquement fillSpeedWhite en fonction de la différence
            float dynamicFillSpeedWhite = Mathf.Lerp(minFillSpeedWhite, maxFillSpeedWhite, difference);

            float whiteFillAmount = Mathf.Lerp(bloodGaugeFillWhite.fillAmount, targetFillAmountWhite, dynamicFillSpeedWhite * Time.deltaTime);
            bloodGaugeFillWhite.fillAmount = Mathf.Max(whiteFillAmount, bloodGaugeFill.fillAmount);
            Debug.Log("FixedUpdate: bloodGaugeFillWhite.fillAmount = " + bloodGaugeFillWhite.fillAmount);
            Debug.Log("FixedUpdate: dynamicFillSpeedWhite = " + dynamicFillSpeedWhite);
        }
    }
}
