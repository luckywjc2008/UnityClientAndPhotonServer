using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
using Photon.SocketServer;
using ExitGames.Logging;
using ExitGames.Logging.Log4Net;
using log4net.Config;
using MyGameServer.Handler;
using MyGameServer.Manager;
using MyGameServer.Model;

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

        //当一个客户端请求连接时
        protected override PeerBase CreatePeer(InitRequest initRequest)
        {
            log.Info("Client Connect!");
            return new ClientPeer(initRequest);
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

            log.Info("Setup completed!");

            InitHandler();
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
            LoginHandler loginhandler = new LoginHandler();
            DictRequestHandler.Add(loginhandler.OpCode,loginhandler);

            DefaultHandler defaultHandler = new DefaultHandler();
            DictRequestHandler.Add(defaultHandler.OpCode,defaultHandler);

            RegisterHandler registerHandler = new RegisterHandler();
            DictRequestHandler.Add(registerHandler.OpCode, registerHandler);

            SyncPositionHandler syncPositionHandler = new SyncPositionHandler();
            DictRequestHandler.Add(syncPositionHandler.OpCode, syncPositionHandler);
        }
    }
}
