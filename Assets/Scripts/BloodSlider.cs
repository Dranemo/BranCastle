using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

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




     [SerializeField] private List<float> bloodValues = new List<float>();

    void Start()
    {
        manager = GameManager.Instance;
        slider = GetComponent<Slider>();
        slider.minValue = 0;
        slider.maxValue = 5000;
        if (bloodGaugeSlider != null && bloodGaugeFill != null)
        {
            bloodValues.Add(bloodGaugeFill.fillAmount);
        }

        //Debug.Log(bloodValues.Count);
    }

    void Update()
    {
        slider.value = manager.blood;
        targetFillAmount = manager.blood / 5000;
        //////Debug.Log("Update: slider.value = " + slider.value);
    }


    void FixedUpdate()
    {
        ResetBloodValue();

        if (bloodGaugeFill != null)
        {
            bloodGaugeFill.fillAmount = Mathf.Lerp(bloodGaugeFill.fillAmount, targetFillAmount, fillSpeed * Time.deltaTime);
            ////Debug.Log("FixedUpdate: bloodGaugeFill.fillAmount = " + bloodGaugeFill.fillAmount);
        }

        if (bloodGaugeFillWhite != null)
        {
             bloodGaugeFillWhite.fillAmount = bloodValues[0];
        }
    }



    private void ResetBloodValue()
    {
        if (bloodValues.Count < 5)
        {
            bloodValues.Add(bloodGaugeFill.fillAmount);
        }
        else
        {
            
            
            List<float> tempValues = new List<float>();

            for (int i = 1; i < bloodValues.Count; i++)
            {
                tempValues.Add(bloodValues[i]);
            }

            if (Mathf.Round(bloodGaugeFill.fillAmount * 100) / 100 == targetFillAmount)
            {
                tempValues.Add(targetFillAmount);
            }
            else
            {
                tempValues.Add(bloodGaugeFill.fillAmount);
            }

            //bloodValues = tempValues;

            for (int i = 0; i < bloodValues.Count; i++)
            {
                bloodValues[i] = tempValues[i];
            }
        }
    }
}
