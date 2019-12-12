using Common;
using Photon.SocketServer;

namespace MyGameServer.Handler
{
    public abstract class BaseRequestHandler
    {
        public OperationCode OpCode;

        public abstract void OnOperationRequest(OperationRequest operationRequest, Photon.SocketServer.SendParameters sendParameters,ClientPeer peer);
    }
}
