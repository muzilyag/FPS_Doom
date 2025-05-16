using System;
using UnityEngine;
using UnityEngine.UI;
using AuthSystem;
using UnityEngine.SceneManagement;
public class AuthController : MonoBehaviour
{
    public InputField usernameInputField;
    public InputField passwordInputField;
    public Text feedbackText;

    private UserManager userManager;

    private void Start()
    {
        userManager = new UserManager();
    }

    public void Register()
    {
        try
        {
            userManager.Register(usernameInputField.text, passwordInputField.text);
            feedbackText.text = "Регистрация успешна!";
        }
        catch (Exception ex)
        {
            feedbackText.text = ex.Message;
        }
    }

    public void Login()
    {
        bool success = userManager.Login(usernameInputField.text, passwordInputField.text);
        feedbackText.text = success ? "" : "Ошибка авторизации.";
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}