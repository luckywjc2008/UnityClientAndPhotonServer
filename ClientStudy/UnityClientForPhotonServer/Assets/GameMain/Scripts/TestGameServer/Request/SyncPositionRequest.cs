using System;
using System.Collections;
using System.Collections.Generic;
using Common;
using Common.Tools;
using ExitGames.Client.Photon;
using Google.Protobuf;
using LitJson;
using UnityEngine;

public class SyncPositionRequest : Request
{
    [HideInInspector]
    public Vector3 pos;

    public override void DefaultRequest()
    {
        VectorData vectorData = new VectorData() {X = pos.x,Y = pos.y,Z = pos.z};

        Dictionary<byte, object> data = DictTool.GetDtoDataByProto(vectorData, ParameterCode.Position);

        Debug.Log("SyncPositionRequest send " + OpCode.ToString());
        PhotonEngine.Peer.OpCustom((byte)OpCode, data, true);
    }

    public override void OnOperationResponse(OperationResponse operationResponse)
    {
        throw new NotImplementedException();
    }
}
