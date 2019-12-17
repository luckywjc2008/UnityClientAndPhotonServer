using System;
using System.Collections;
using System.Collections.Generic;
using Common;
using Common.Tools;
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
        RegisterData registerData = new RegisterData()
        {
            UserName = userName,
            PassWord = passWord,
        };
        Dictionary<byte, object> data = DictTool.GetDtoDataByProto(registerData, ParameterCode.RegisterData);

        Debug.Log("RegisterRequest send " + OpCode.ToString());
        PhotonEngine.Peer.OpCustom((byte)OpCode, data, true);
    }

    public override void OnOperationResponse(OperationResponse operationResponse)
    {
        ReturnCode returnCode = (ReturnCode)operationResponse.ReturnCode;
        registerPanel.OnRegisterResponse(returnCode);
    }
}
