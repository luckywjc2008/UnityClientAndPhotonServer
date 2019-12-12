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


            //switch (operationRequest.OperationCode)
            //{
            //    case 1:
            //        MyGameServer.LogInfo("client send 1");

            //        Dictionary<byte, object> dicData = operationRequest.Parameters;
            //        object intValue;
            //        object stringValue;
            //        dicData.TryGetValue(1, out intValue);
            //        dicData.TryGetValue(2, out stringValue);

            //        MyGameServer.LogInfo("parameter 1 = " + intValue.ToString() +" 2 = "+stringValue.ToString());

            //        OperationResponse opResponse = new OperationResponse(1);

            //        Dictionary<byte, object> dicData2 = new Dictionary<byte, object>();
            //        dicData2.Add(1, 100);
            //        dicData2.Add(2, "还不错");
            //        opResponse.SetParameters(dicData2);
            //        SendOperationRes(opResponse, sendParameters);
            //        SendEventToClient();
            //        break;
            //    default:
            //        break;
            //}
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
