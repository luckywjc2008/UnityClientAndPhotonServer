using System;
using System.Collections;
using System.Collections.Generic;
using Common;
using ExitGames.Client.Photon;
using UnityEngine;

public class RegisterRequest : Request
{
    [HideInInspector]
    public string userName;
    [HideInInspector]
    public string passWord;

    private RegisterPanel registerPanel;

    public override void Start()
    {
        base.Start();
        registerPanel = GetComponent<RegisterPanel>();
    }

    public override void DefaultRequest()
    {
        Dictionary<byte, object> data = new Dictionary<byte, object>();
        data.Add((byte)ParameterCode.UserName, userName);
        data.Add((byte)ParameterCode.PassWord, passWord);

        PhotonEngine.Peer.OpCustom((byte)OpCode, data, true);
    }

    public override void OnOperationResponse(OperationResponse operationResponse)
    {
        ReturnCode returnCode = (ReturnCode)operationResponse.ReturnCode;
        registerPanel.OnRegisterResponse(returnCode);
    }
}
