using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
using Common.Tools;
using Google.Protobuf;
using LitJson;
using Photon.SocketServer;

namespace MyGameServer.Handler
{
    class SyncPositionHandler : BaseRequestHandler
    {

        public SyncPositionHandler()
        {
            OpCode = OperationCode.SyncPosition;
        }

        public override void OnOperationRequest(OperationRequest operationRequest, SendParameters sendParameters, ClientPeer peer)
        {
            byte[] byteAtrr = (byte[])DictTool.GetValue<byte, object>(operationRequest.Parameters, (byte) ParameterCode.Position);
            VectorData pos = VectorData.Parser.ParseFrom(byteAtrr);

            MyGameServer.LogInfo("x = "+ pos.X + "y= " + pos.Y + "z = " + pos.Z);

            peer.posData = pos;
        }
    }
}
