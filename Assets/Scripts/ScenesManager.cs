using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class ScenesManager : MonoBehaviour
{
    public static ScenesManager Instance { get; private set; }
    [SerializeField] private CanvasGroup fadeCanvasGroup;
    [SerializeField] private float fadeDuration = 1f;
    [SerializeField] Canvas fadeCanvas;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        if (fadeCanvas != null)
        {
            fadeCanvasGroup = fadeCanvas.GetComponentInChildren<CanvasGroup>();
            if (fadeCanvasGroup == null)
            {
                Debug.LogError("Aucun CanvasGroup trouv� sur le Canvas.");
            }
        }
        else
        {
            Debug.LogError("Canvas non assign� dans l'inspecteur.");
        }
    }

    private void Start()
    {
    }
    public void LoadSceneAsync(string sceneName, Button button)
    {
        // D�sactivez le bouton
        button.interactable = false;

        StartCoroutine(LoadSceneAsyncRoutine(sceneName, button));
    }
    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    //private IEnumerator FadeIn()
    //{
    //    fadeCanvasGroup.alpha = 1f;
    //    float elapsedTime = 0f;
    //
    //    while (elapsedTime < fadeDuration)
    //    {
    //        elapsedTime += Time.deltaTime;
    //        fadeCanvasGroup.alpha = 1f - Mathf.Clamp01(elapsedTime / fadeDuration);
    //        yield return null;
    //    }
    //
    //    fadeCanvasGroup.alpha = 0f;
    //}
    //
    //public IEnumerator FadeOutAndLoadScene(string sceneName)
    //{
    //    float elapsedTime = 0f;
    //
    //    while (elapsedTime < fadeDuration)
    //    {
    //        elapsedTime += Time.deltaTime;
    //        fadeCanvasGroup.alpha = Mathf.Clamp01(elapsedTime / fadeDuration);
    //        yield return null;
    //    }
    //
    //    fadeCanvasGroup.alpha = 1f;
    //    Debug.Log("Chargement de la sc�ne: " + sceneName);
    //    SceneManager.LoadScene(sceneName);
    //}
    private IEnumerator LoadSceneAsyncRoutine(string sceneName, Button button)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);

        // Attendez que la sc�ne soit compl�tement charg�e
        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        // R�activez le bouton
        button.interactable = true;
    }
}
