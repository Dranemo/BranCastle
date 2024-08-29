using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenuUI;
    public bool isPaused = false;
    public GameManager gameManager;
    private PlayerMovement player;
    private Canvas canvasInGame;
    void Start()
    {
        pauseMenuUI.SetActive(false);
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
        canvasInGame = GameObject.Find("CanvasInGame(Clone)").GetComponent<Canvas>();

    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
        TogglePlayerAbilities(true);
    }

    void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
        TogglePlayerAbilities(false);
    }

    private void TogglePlayerAbilities(bool state)
    {
        player.canBatAttack = state;
        player.canCape = state;
        player.canDash = state;
        player.canPunch = state;
        player.canHypnotize = state;
        player.canDrawRectangle = state;
        canvasInGame.gameObject.SetActive(state);
    }

    public void ReturnToMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
        if (gameManager != null)
        {
            gameManager.Reset();
        }

    }
}

