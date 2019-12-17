using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Common;
using Common.Tools;
using Google.Protobuf;
using Photon.SocketServer;

namespace MyGameServer.Handler
{
    public class SyncPlayerHandler : BaseRequestHandler
    {
        public SyncPlayerHandler()
        {
            OpCode = OperationCode.SyncPlayer;
        }

        public override void OnOperationRequest(OperationRequest operationRequest, SendParameters sendParameters, ClientPeer peer)
        {
            //取得所有已经登录的用户
            List<string> userNameList = new List<string>();
            foreach (var tempPeer in MyGameServer.Instance.peerList)
            {
                if (!string.IsNullOrEmpty(tempPeer.userName) && tempPeer != peer)
                {
                    userNameList.Add(tempPeer.userName);
                }
            }

            //StringWriter sw = new StringWriter();
            //XmlSerializer serializer = new XmlSerializer(typeof(List<string>));
            //serializer.Serialize(sw,userNameList);
            //sw.Close();
            //string userNameListString = sw.ToString();

            UserNameList userNameListString = new UserNameList();
            userNameListString.NameList.AddRange(userNameList);
            Dictionary<byte, object> data = DictTool.GetDtoDataByProto(userNameListString, ParameterCode.UserNameList);


            OperationResponse response = new OperationResponse(operationRequest.OperationCode);
            response.Parameters = data;
            peer.SendOperationResponse(response,sendParameters);


            NewPlayer newPlayer = new NewPlayer();
            newPlayer.UserName = peer.userName;

            EventData eventData = new EventData((byte)EventCode.NewPlayer);
            Dictionary<byte, object> data2 = DictTool.GetDtoDataByProto(newPlayer, ParameterCode.UserName);
            eventData.Parameters = data2;

            foreach (var tempPeer in MyGameServer.Instance.peerList)
            {
                if (!string.IsNullOrEmpty(tempPeer.userName) && tempPeer != peer)
                {
                    tempPeer.SendEvent(eventData, sendParameters);
                }
            }
        }
    }
}
