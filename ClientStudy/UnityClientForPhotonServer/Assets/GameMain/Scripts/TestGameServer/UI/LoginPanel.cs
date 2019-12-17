using System.Collections;
using System.Collections.Generic;
using Common;
using GameFramework;
using StarForce;
using UnityEngine;
using UnityEngine.UI;
using GameEntry = StarForce.GameEntry;

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
            //SceneManager.LoadScene("Game");

            // 卸载所有场景
            string[] loadedSceneAssetNames = GameEntry.Scene.GetLoadedSceneAssetNames();
            for (int i = 0; i < loadedSceneAssetNames.Length; i++)
            {
                GameEntry.Scene.UnloadScene(loadedSceneAssetNames[i]);
            }

            // 还原游戏速度
            GameEntry.Base.ResetNormalGameSpeed();

            GameEntry.Scene.LoadScene(AssetUtility.GetSceneAsset("Game"), this);
        }
        else
        {
            hintMessage.text = "用户名或者密码错误";
        }
    }
}
