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


            List<string> userNameList2 = new List<string>();
            foreach (var temPeer in MyGameServer.Instance.peerList)
            {
                if (!string.IsNullOrEmpty(temPeer.userName) && temPeer != peer)
                {
                    userNameList2.Add(temPeer.userName);
                }
            }

            UserNameList userNameListString2 = new UserNameList();
            userNameListString2.NameList.AddRange(userNameList2);

            Dictionary<byte, object> data2 = DictTool.GetDtoDataByProto(userNameListString2, ParameterCode.UserNameList);
            EventData eventData = new EventData((byte)EventCode.NewPlayer);
            eventData.Parameters = data2;
            peer.SendEvent(eventData, sendParameters);
        }
    }
}
