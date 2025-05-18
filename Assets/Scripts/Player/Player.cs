using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public int HP = 100;
    public GameObject bloodyScreen;
    public GameObject gameOverUI;
    //public TextMeshProUGUI playerHealthUI;
    public Slider playerHealthBarUI;
    private PlayerData playerData;
    private float timeAlive = 0;
    public bool isDead;
    private bool isCountingTime = true;
    private void Start()
    {
        //playerHealthUI.text = $"Health: {HP}";
        playerHealthBarUI.value = HP;
        playerData = SaveLoadManager.GetPlayerData(SaveLoadManager.CurrentUser);
        GlobalReferences.Instance.playerData = playerData;
        StartCoroutine(TimeCounter());
    }

    private IEnumerator TimeCounter()
    {
        while (isCountingTime)
        {
            timeAlive += Time.deltaTime;
            yield return null;
        }
    }

    public void TakenDamage(int damageAmount)
    {
        HP -= damageAmount;

        if (HP < 1)
        {
            print("Player Dead");
            PlayerDead();
        }
        else
        {
            print("Player Hit");
            StartCoroutine(BloodyScreenEffect());
            //playerHealthUI.text = $"Health: {HP}";
            playerHealthBarUI.value = HP;
            SoundManager.Instance.PlayerChannel.PlayOneShot(SoundManager.Instance.playerHurt);
        }
    }

    private void PlayerDead()
    {
        isDead = true;
        isCountingTime = false;
        foreach (Weapon weapon in GetComponentsInChildren<Weapon>(true))
        {
            weapon.enabled = false; 
        }

        SoundManager.Instance.PlayerChannel.PlayOneShot(SoundManager.Instance.playerDie);
        SoundManager.Instance.PlayerChannel.clip = SoundManager.Instance.gameOverMusic;
        SoundManager.Instance.PlayerChannel.PlayDelayed(0.5f);

        GetComponent<MouseMovement>().enabled = false;
        GetComponent<PlayerMovement>().enabled = false;

        // Dying animation
        GetComponentInChildren<Animator>().enabled = true;
        //playerHealthUI.gameObject.SetActive(false);
        playerHealthBarUI.gameObject.SetActive(false);
        GetComponent<ScreenBlackOut>().StartFade();
        StartCoroutine(ShowGameOverUI());
    }

    private IEnumerator ShowGameOverUI()
    {
        yield return new WaitForSeconds(1f);
        gameOverUI.gameObject.SetActive(true);
        int waveSurvived = GlobalReferences.Instance.waveNumber;
        if(waveSurvived - 1 > playerData.maxWavesSurvived)
        {
            playerData.maxWavesSurvived = waveSurvived;
        }
        playerData.zombieKilled = GlobalReferences.Instance.playerData.zombieKilled;
        playerData.maxLiveTime = timeAlive;

        SaveLoadManager.SaveAllData();
        StartCoroutine(ReturnToMainMenu());
    }

    private IEnumerator ReturnToMainMenu()
    {
        yield return new WaitForSeconds(4f);
        Cursor.lockState = CursorLockMode.None;
        SceneManager.LoadScene("MainMenuScene");

    }

    private IEnumerator BloodyScreenEffect()
    {
        if(bloodyScreen.activeInHierarchy == false)
        {
            bloodyScreen.SetActive(true); ;
        }

        var image = bloodyScreen.GetComponent<Image>();
        Color startColor = image.color;
        startColor.a = 1f;
        image.color = startColor;

        float duration = 2f;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            float alpha = Mathf.Lerp(1f, 0f, elapsedTime / duration);

            Color newColor = image.color;
            newColor.a = alpha;
            image.color = newColor;

            elapsedTime += Time.deltaTime;

            yield return null;
        }

        if(bloodyScreen.activeInHierarchy)
        {
            bloodyScreen.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("ZombieAttackHand") && isDead == false)
        {
            TakenDamage(other.gameObject.GetComponent<ZombieHand>().damage);
        }
    }
}
