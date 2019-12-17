using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
using Photon.SocketServer;

namespace MyGameServer.Handler
{
    class DefaultHandler : BaseRequestHandler
    {
        public DefaultHandler()
        {
            OpCode = OperationCode.Default;
        }

        public override void OnOperationRequest(OperationRequest operationRequest, SendParameters sendParameters, ClientPeer peer)
        {
            MyGameServer.LogInfo("Execute DefaultHandler OperationCode = " + operationRequest.OperationCode);
        }
    }
}
