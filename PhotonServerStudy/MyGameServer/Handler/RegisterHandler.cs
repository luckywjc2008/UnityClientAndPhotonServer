using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
using Common.Tools;
using MyGameServer.Manager;
using MyGameServer.Model;
using Photon.SocketServer;

namespace MyGameServer.Handler
{
    class RegisterHandler : BaseRequestHandler
    {
        public RegisterHandler()
        {
            OpCode = OperationCode.Register;
        }

        public override void OnOperationRequest(OperationRequest operationRequest, SendParameters sendParameters, ClientPeer peer)
        {
            RegisterData registerData = DictTool.GetProtoByDtoData<RegisterData>(operationRequest.Parameters,ParameterCode.RegisterData);

            UserManager manager = new UserManager();
            User user = manager.GetByName(registerData.UserName);

            OperationResponse response = new OperationResponse(operationRequest.OperationCode);
            if (user == null)
            {
                user = new User() {Username = registerData.UserName, Password = registerData.PassWord};
                manager.Add(user);
                response.ReturnCode = (short)ReturnCode.Success;
            }
            else
            {
                response.ReturnCode = (short)ReturnCode.Failed;
            }

            peer.SendOperationResponse(response, sendParameters);

        }
    }
}
