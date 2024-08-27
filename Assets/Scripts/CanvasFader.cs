using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CanvasFader : MonoBehaviour
{
    public Image backgroundImage; 
    public RawImage mapImage; 
    public float fadeDuration = 2f; 

    private void Start()
    {
        backgroundImage.color = new Color(255, 255, 255, 0);
        mapImage.gameObject.SetActive(false); 
    }

    public void StartFadeIn()
    {
        StartCoroutine(FadeIn());
    }

    private IEnumerator FadeIn()
    {
        float currentTime = 0f;
        while (currentTime < fadeDuration)
        {
            float alpha = Mathf.Lerp(0f, 0.5f, currentTime / fadeDuration);
            backgroundImage.color = new Color(255, 255, 255, alpha);
            currentTime += Time.deltaTime;
            yield return null;
        }

        mapImage.gameObject.SetActive(true);
    }
}
