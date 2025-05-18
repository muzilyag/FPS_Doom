using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public TMP_Text highScoreUI;
    public TMP_Text zombieKilledUI;
    //public TMP_Text maxTimeAliveUI;

    private string nextScene = "SampleScene";

    public AudioClip bgMusic;
    public AudioSource mainChannel;
    private void Start()
    {
        mainChannel.PlayOneShot(bgMusic);
        PlayerData playerData = SaveLoadManager.GetPlayerData(SaveLoadManager.CurretnUser);
        int highScore = playerData.maxWavesSurvived;
        highScoreUI.text = $"Самая длинная серия: {highScore} волн";
        int zombieKilled = playerData.zombieKilled;
        zombieKilledUI.text = $"Зомби убито: {zombieKilled}";
        //float maxTimeAlive = playerData.maxLiveTime;
        //maxTimeAliveUI.text = $"Рекорд времени: {maxTimeAlive.ToString("F0")}";
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