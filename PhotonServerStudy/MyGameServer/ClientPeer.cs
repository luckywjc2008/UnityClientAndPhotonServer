using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
using Common.Tools;
using MyGameServer.Handler;
using Photon.SocketServer;
using PhotonHostRuntimeInterfaces;

namespace MyGameServer
{

    public class ClientPeer : Photon.SocketServer.ClientPeer
    {
        public float x, y, z;
        public VectorData posData;
        public string userName;

        public ClientPeer(InitRequest initRequest) : base(initRequest)
        {

        }
        //客户端断开连接
        protected override void OnDisconnect(DisconnectReason reasonCode, string reasonDetail)
        {
            MyGameServer.Instance.peerList.Remove(this);
        }

        //处理客户端请求
        protected override void OnOperationRequest(OperationRequest operationRequest, SendParameters sendParameters)
        {
            BaseRequestHandler handler =
                DictTool.GetValue<OperationCode, BaseRequestHandler>(MyGameServer.Instance.DictRequestHandler,
                    (OperationCode)operationRequest.OperationCode);
            if (handler != null)
            {
                handler.OnOperationRequest(operationRequest, sendParameters, this);
            }
            else
            {
                handler = DictTool.GetValue<OperationCode, BaseRequestHandler>(MyGameServer.Instance.DictRequestHandler,
                    OperationCode.Default);
                handler.OnOperationRequest(operationRequest,sendParameters,this);
            }
           
        }

        private void SendOperationRes(OperationResponse opResponse, SendParameters parameter)
        {
            SendOperationResponse(opResponse, parameter);
        }

        private void SendEventToClient()
        {
            EventData ed = new EventData(1);
            Dictionary<byte, object> dicData3 = new Dictionary<byte, object>();
            dicData3.Add(1, 100);
            dicData3.Add(2, "事件还不错");
            ed.Parameters = dicData3;

            SendEvent(ed, new SendParameters());
        }
    }
}
