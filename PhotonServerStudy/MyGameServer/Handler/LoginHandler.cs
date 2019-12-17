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
            LoginData loginData = DictTool.GetProtoByDtoData<LoginData>(operationRequest.Parameters,ParameterCode.LoginData);

            UserManager manager = new UserManager();
            bool isSuccess = manager.VerifyUser(loginData.UserName, loginData.PassWord);

            OperationResponse response = new OperationResponse(operationRequest.OperationCode);

            if (isSuccess)
            {
                response.ReturnCode = (short)ReturnCode.Success;
                peer.userName = loginData.UserName;
            }
            else
            {
                response.ReturnCode = (short)ReturnCode.Failed;
            }

            peer.SendOperationResponse(response, sendParameters);
        }
    }
}
