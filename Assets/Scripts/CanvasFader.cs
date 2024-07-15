using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CanvasFader : MonoBehaviour
{
    public Image backgroundImage; // Assignez l'image de fond noire ici
    public RawImage mapImage; // Assignez l'image de la carte ici
    public float fadeDuration = 2f; // Dur�e du fondu

    private void Start()
    {
        // Commencez avec l'image de fond pleinement visible et la carte cach�e
        backgroundImage.color = new Color(0, 0, 0, 0);
        mapImage.gameObject.SetActive(false); // Cachez la carte au d�but
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
            float alpha = Mathf.Lerp(0f, 1f, currentTime / fadeDuration);
            backgroundImage.color = new Color(0, 0, 0, alpha); // Modifie uniquement l'alpha
            currentTime += Time.deltaTime;
            yield return null;
        }

        // Une fois le fondu termin�, affichez la carte sans fondu
        mapImage.gameObject.SetActive(true);
    }
}
