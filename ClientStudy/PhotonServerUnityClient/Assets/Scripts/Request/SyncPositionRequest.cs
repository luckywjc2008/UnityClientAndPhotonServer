using System;
using System.Collections;
using System.Collections.Generic;
using Common;
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
        Dictionary<byte, object> data = new Dictionary<byte, object>();
        VectorData vectorData = new VectorData() {X = pos.x,Y = pos.y,Z = pos.z};

        byte[] byteAtrr = vectorData.ToByteArray();
        data.Add((byte)ParameterCode.Position, byteAtrr);

        PhotonEngine.Peer.OpCustom((byte)OpCode, data, true);
    }

    public override void OnOperationResponse(OperationResponse operationResponse)
    {
        throw new NotImplementedException();
    }
}
