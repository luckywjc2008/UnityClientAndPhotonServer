using System;
using System.Collections;
using System.Collections.Generic;
using Common;
using Common.Tools;
using ExitGames.Client.Photon;
using UnityEngine;

public class LoginRequest : Request
{
    [HideInInspector]
    public string UserName;
    [HideInInspector]
    public string PassWord;

    private LoginPanel loginPanel;

    public override void Start()
    {
        base.Start();
        loginPanel = GetComponent<LoginPanel>();
    }

    public override void DefaultRequest()
    {
        LoginData loginData = new LoginData();
        loginData.UserName = UserName;
        loginData.PassWord = PassWord;
        Dictionary<byte, object> data = DictTool.GetDtoDataByProto(loginData, ParameterCode.LoginData);

        Debug.Log("LoginRequest send " + OpCode.ToString());
        PhotonEngine.Peer.OpCustom((byte)OpCode, data,true);
    }

    public override void OnOperationResponse(OperationResponse operationResponse)
    {
        Debug.Log("ReturnCode =" + operationResponse.ReturnCode);
        ReturnCode returnCode = (ReturnCode)operationResponse.ReturnCode;
        if (returnCode == ReturnCode.Success)
        {
            PhotonEngine.userName = UserName;
        }

        loginPanel.OnLoginResponse(returnCode);
    }

}
