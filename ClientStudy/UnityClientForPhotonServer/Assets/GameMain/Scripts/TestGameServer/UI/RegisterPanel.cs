using System.Collections;
using System.Collections.Generic;
using Common;
using UnityEngine;
using UnityEngine.UI;

public class RegisterPanel : MonoBehaviour
{
    [SerializeField]
    private GameObject loginPanel;

    public InputField userNameIF;
    public InputField PassWordIF;
    public Text hintMessage;

    private RegisterRequest registerRequest;

    void Start()
    {
        registerRequest = GetComponent<RegisterRequest>();
    }

    public void OnRegisterButton()
    {
        hintMessage.text = "";
        registerRequest.userName = userNameIF.text;
        registerRequest.passWord = PassWordIF.text;
        registerRequest.DefaultRequest();
    }

    public void OnBackButton()
    {
        gameObject.SetActive(false);
        loginPanel.SetActive(true);
    }

    public void OnRegisterResponse(ReturnCode returnCode)
    {
        if (returnCode == ReturnCode.Success)
        {
            hintMessage.text = "注册成功,请登录";
        }
        else
        {
            hintMessage.text = "用户名重复，请更改用户名";
        }
    }
}
