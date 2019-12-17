using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using Common;
using Common.Tools;
using Photon.SocketServer;
using ExitGames.Logging;
using ExitGames.Logging.Log4Net;
using log4net.Config;
using MyGameServer.Handler;

namespace MyGameServer
{
    class MyGameServer : ApplicationBase
    {
        private static readonly ILogger log = LogManager.GetCurrentClassLogger();

        public static MyGameServer Instance
        {
            get;
            private set;
        }

        public Dictionary<OperationCode,BaseRequestHandler> DictRequestHandler = new Dictionary<OperationCode, BaseRequestHandler>();

        public List<ClientPeer> peerList = new List<ClientPeer>();

        //当一个客户端请求连接时
        protected override PeerBase CreatePeer(InitRequest initRequest)
        {
            log.Info("Client Connect ConnectionId = " + initRequest.ConnectionId + " Ip:Port = " + initRequest.RemoteIP + ":" + initRequest.RemotePort);
            ClientPeer peer = new ClientPeer(initRequest);
            peerList.Add(peer);
            return peer;
        }
        //初始化
        protected override void Setup()
        {
            Instance = this;
            //日子所在目录
            log4net.GlobalContext.Properties["Photon:ApplicationLogPath"] = Path.Combine(this.ApplicationRootPath,"log");
            //日志配置
            FileInfo configFileInfo = new FileInfo(Path.Combine(this.BinaryPath,"log4net.config"));
            if (configFileInfo.Exists)
            {
                LogManager.SetLoggerFactory( Log4NetLoggerFactory.Instance );
                XmlConfigurator.ConfigureAndWatch(configFileInfo);
            }

            InitHandler();

            log.Info("Setup completed!");
        }
        //服务端关闭
        protected override void TearDown()
        {
            log.Info("Server close!");
        }

        public static void LogInfo(object content)
        {
            log.Info(content);
        }

        private void InitHandler()
        {
            MyGameServer.LogInfo(" InitHandler Start");

            Type[] types = Assembly.GetCallingAssembly().GetTypes();
            Type baseRequestType = typeof(BaseRequestHandler);

            MyGameServer.LogInfo(" baseRequestType FullName = " + baseRequestType.FullName);

            foreach (var typeItem in types)
            {
                Type baseType = typeItem.BaseType;
                if (baseType != null)
                {
                    if (baseType.Name == baseRequestType.Name)
                    {
                        try
                        {
                            Type ObjType = Type.GetType(typeItem.FullName, true);
                            object obj = Activator.CreateInstance(ObjType);
                            if (obj != null)
                            {
                                BaseRequestHandler info = obj as BaseRequestHandler;
                                if (info != null)
                                {
                                    MyGameServer.LogInfo("ObjType FullName = " + ObjType.FullName + "Add To DictRequestHandler");
                                    DictRequestHandler.Add(info.OpCode, info);
                                }
                            }
                        }
                        catch (Exception e)
                        {
                            MyGameServer.LogInfo("typeItem FullName = " + typeItem.FullName + "Not Find");
                        }
                    }
                }

            }

            MyGameServer.LogInfo(" InitHandler Complete");

        }
    }
}
