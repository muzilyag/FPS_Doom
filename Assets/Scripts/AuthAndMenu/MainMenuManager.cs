using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public TMP_Text highScoreUI;
    private string nextScene = "SampleScene";

    public AudioClip bgMusic;
    public AudioSource mainChannel;
    private void Start()
    {
        mainChannel.PlayOneShot(bgMusic);


        int highScore = SaveLoadManager.Instance.LoadHighScore();
        highScoreUI.text = $"Самая длинная серия: {highScore} волн";
    }
    public void LoadNextScene()
    {
        mainChannel.Stop();
        //int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
        SceneManager.LoadScene(nextScene);
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}