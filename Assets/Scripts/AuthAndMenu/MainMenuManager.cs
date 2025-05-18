using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public TMP_Text highScoreUI;
    public TMP_Text zombieKilledUI;
    public TMP_Text maxTimeAliveUI;

    private string nextScene = "SampleScene";

    public AudioClip bgMusic;
    public AudioSource mainChannel;
    private void Start()
    {
        mainChannel.PlayOneShot(bgMusic);
        PlayerData playerData = SaveLoadManager.GetPlayerData(SaveLoadManager.CurrentUser);
        highScoreUI.text = $"Самая длинная серия: {playerData.maxWavesSurvived} волн";
        zombieKilledUI.text = $"Зомби убито: {playerData.zombieKilled}";
        maxTimeAliveUI.text = $"Рекорд времени: {FormatTime(playerData.maxLiveTime)}";
    }
    private string FormatTime(float timeInSeconds)
    {
        int minutes = Mathf.FloorToInt(timeInSeconds / 60f);
        int seconds = Mathf.FloorToInt(timeInSeconds % 60f);
        return $"{minutes:00}:{seconds:00}";
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