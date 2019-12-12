using System.Collections;
using System.Collections.Generic;
using Common;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoginPanel : MonoBehaviour
{
    [SerializeField]
    public GameObject registerPanel;

    public InputField userNameInput;
    public InputField passWordInput;

    private LoginRequest loginRequest;

    public Text hintMessage;

    private void Start()
    {
        loginRequest = GetComponent<LoginRequest>();
    }

    public void OnLoginButton()
    {
        hintMessage.text = "";
        loginRequest.UserName = userNameInput.text;
        loginRequest.PassWord = passWordInput.text;
        loginRequest.DefaultRequest();
    }

    public void OnRegisterButton()
    {
        gameObject.SetActive(false);
        registerPanel.SetActive(true);
    }

    public void OnLoginResponse(ReturnCode returnCode)
    {
        if (returnCode == ReturnCode.Success)
        {
            //todo跳转场景
            SceneManager.LoadScene("Game");
        }
        else
        {
            hintMessage.text = "用户名或者密码错误";
        }
    }
}
