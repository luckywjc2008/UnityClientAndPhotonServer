using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using Common;
using Common.Tools;
using ExitGames.Client.Photon;
using UnityEngine;

public class SyncPlayerRequest : Request
{

    private Player player;

    public override void Start()
    {
        base.Start();
        player = GetComponent<Player>();
    }


    public override void DefaultRequest()
    {
        Debug.Log("SyncPlayerRequest send " + OpCode.ToString());
        PhotonEngine.Peer.OpCustom((byte)OpCode, null, true);
    }

    public override void OnOperationResponse(OperationResponse operationResponse)
    {

        //string resString = (string)DictTool.GetValue<byte, object>(operationResponse.Parameters, (byte)ParameterCode.UserNameList);
        //Debug.Log("resString = " + resString);

        //using (StringReader reader = new StringReader(resString))
        //{
        //    XmlSerializer serializer = new XmlSerializer(typeof(List<string>));
        //    List<string> userNameList = (List<string>)serializer.Deserialize(reader);

        //    for (int i = 0; i < userNameList.Count; i++)
        //    {
        //        Debug.Log("userName =" + userNameList[i]);
        //    }
        //    player.OnSyncPlayResponse(userNameList);
        //}

        UserNameList userNameList = DictTool.GetProtoByDtoData<UserNameList>(operationResponse.Parameters,ParameterCode.UserNameList);
        for (int i = 0; i < userNameList.NameList.Count; i++)
        {
            Debug.Log("userName =" + userNameList.NameList[i]);
        }
        player.OnSyncPlayResponse(userNameList);
    }
}
