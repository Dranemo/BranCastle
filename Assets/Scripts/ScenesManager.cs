using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class ScenesManager : MonoBehaviour
{
    public static ScenesManager Instance { get; private set; }
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
    }
    public static void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
    public void LoadSceneAsync(string sceneName, Button button)
    {
        // Désactivez le bouton
        button.interactable = false;

        StartCoroutine(LoadSceneAsyncRoutine(sceneName, button));
    }

    private IEnumerator LoadSceneAsyncRoutine(string sceneName, Button button)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);

        // Attendez que la scène soit complètement chargée
        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        // Réactivez le bouton
        button.interactable = true;
    }
}
