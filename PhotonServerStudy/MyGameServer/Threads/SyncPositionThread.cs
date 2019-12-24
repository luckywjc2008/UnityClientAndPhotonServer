using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Common;
using Common.Tools;
using Photon.SocketServer;
using Photon.SocketServer.Rpc;

namespace MyGameServer.Threads
{
    public class SyncPositionThread
    {
        private Thread t;
        private bool stopState = false;

        public void Run()
        {
            MyGameServer.LogInfo("SyncPositionThread Run");
            t = new Thread(UpdatePosition);
            t.IsBackground = true;
            t.Priority = ThreadPriority.Normal;
            stopState = false;
            t.Start();
        }

        public void Stop()
        {
            MyGameServer.LogInfo("SyncPositionThread Stop");
            stopState = true;
        }

        private void UpdatePosition()
        {
            Thread.Sleep(5000);
            while (true)
            {
                Thread.Sleep(100);
                if (stopState)
                {
                    try
                    {
                        t.Abort();
                        MyGameServer.LogInfo("SyncPositionThread Abort");
                    }
                    catch (Exception e)
                    {
                        MyGameServer.LogInfo(e.Message);
                        throw;
                    }
                }
                else
                {
                    SendPosition();
                }
            }
        }

        private void SendPosition()
        {
             PlayerList playerList = new PlayerList();
            foreach (ClientPeer peer in MyGameServer.Instance.peerList)
            {
                if (!string.IsNullOrEmpty(peer.userName) && peer.posData != null)
                {
                    PlayerData playerData = new PlayerData();
                    playerData.UsrName = peer.userName;
                    playerData.Pos = new VectorData() {X = peer.posData.X,Y = peer.posData.Y, Z = peer.posData.Z};
                    playerList.PlayerList_.Add(playerData);
                }
            }

            //MyGameServer.LogInfo("playerList = " + playerList.ToString());

            Dictionary<byte,object> data = DictTool.GetDtoDataByProto(playerList, ParameterCode.PlayerDataList);

            foreach (ClientPeer peer in MyGameServer.Instance.peerList)
            {
                if (!string.IsNullOrEmpty(peer.userName))
                {
                    EventData ed = new EventData((byte)EventCode.SyncPosition);
                    ed.Parameters = data;
                    //MyGameServer.LogInfo("peer.SendEvent userName = " + peer.userName);
                    peer.SendEvent(ed, new SendParameters());
                }
            }

        }
    }
}
