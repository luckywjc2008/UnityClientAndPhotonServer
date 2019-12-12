using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
using Common.Tools;
using MyGameServer.Manager;
using Photon.SocketServer;

namespace MyGameServer.Handler
{
    class LoginHandler : BaseRequestHandler
    {
        public LoginHandler()
        {
            OpCode = OperationCode.Login;
        }

        public override void OnOperationRequest(OperationRequest operationRequest, SendParameters sendParameters, ClientPeer peer)
        {
            string userName = DictTool.GetValue<byte, object>(operationRequest.Parameters,(byte)ParameterCode.UserName) as string;
            string passWord = DictTool.GetValue<byte, object>(operationRequest.Parameters, (byte)ParameterCode.PassWord) as string;

            UserManager manager = new UserManager();
            bool isSuccess = manager.VerifyUser(userName, passWord);

            OperationResponse response = new OperationResponse(operationRequest.OperationCode);

            if (isSuccess)
            {
                response.ReturnCode = (short)ReturnCode.Success;
                peer.userName = userName;
            }
            else
            {
                response.ReturnCode = (short)ReturnCode.Failed;
            }

            peer.SendOperationResponse(response, sendParameters);
        }
    }
}
