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
            // �������� ������ �����
            string username = usernameInputField.text;
            string password = passwordInputField.text;

            if (string.IsNullOrWhiteSpace(username))
                throw new Exception("���� ����� �� ����� ���� ������!");

            if (string.IsNullOrWhiteSpace(password))
                throw new Exception("���� ������ �� ����� ���� ������!");

            userManager.Register(username, password);
            feedbackText.text = "����������� �������!";
            Login();
        }
        catch (Exception ex)
        {
            feedbackText.text = ex.Message;
        }
    }

    public void Login()
    {
        string errorMessage = userManager.Login(
            usernameInputField.text,
            passwordInputField.text
        );

        if (errorMessage == null)
        {
            feedbackText.text = "";
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
        else
        {
            feedbackText.text = errorMessage;
        }
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