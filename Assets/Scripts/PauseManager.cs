using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    public GameObject pausePanel; 

    private bool isPaused = false;

    void Start()
    {
        pausePanel.SetActive(false); 
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
                ResumeGame();
            else
                PauseGame();
        }
    }

    public void PauseGame()
    {
        isPaused = true;
        pausePanel.SetActive(true);
        Time.timeScale = 0f; // Остановка игрового времени
        Cursor.lockState = CursorLockMode.None; // Разблокируем курсор
        Cursor.visible = true;
    }

    public void ResumeGame()
    {
        isPaused = false;
        pausePanel.SetActive(false);
        Time.timeScale = 1f; // Восстановление времени
        Cursor.lockState = CursorLockMode.Locked; // Блокируем курсор (для FPS)
        Cursor.visible = false;
    }

    public void SaveAndExit()
    {
        Time.timeScale = 1f;
        // Здесь добавь код сохранения (PlayerPrefs.SetInt(...))
        int MenuSceneIndex = SceneManager.GetActiveScene().buildIndex - 1;
        SceneManager.LoadScene(MenuSceneIndex);
    }
}