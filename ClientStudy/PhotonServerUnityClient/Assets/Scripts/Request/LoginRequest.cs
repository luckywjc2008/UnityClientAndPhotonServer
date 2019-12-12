using System;
using System.Collections;
using System.Collections.Generic;
using Common;
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
        Dictionary<byte,object> data = new Dictionary<byte, object>();
        data.Add((byte)ParameterCode.UserName,UserName);
        data.Add((byte)ParameterCode.PassWord, PassWord);

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
