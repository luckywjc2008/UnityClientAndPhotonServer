using System.Runtime.InteropServices;
using System;

namespace StarForce
{
    public class Protocal
    {
        /// <summary>
        /// 心跳
        /// </summary>
        public const string HeartBeat = "01";
        /// <summary>
        /// 连接服务器
        /// </summary>
        public const string Connect = "02";
        /// <summary>
        /// 异常掉线
        /// </summary>
        public const string Exception = "03";
        /// <summary>
        /// 正常断线   
        /// </summary>
        public const string Disconnect = "04";

        /// <summary>
        /// 数据消息
        /// </summary>
        public const string Message = "05";

        /// <summary>
        /// 链接错误
        /// </summary>
        public const string NetError = "06";
    }

    //网络内核
    struct TCP_Info
    {
        public byte cbDataKind;                            //数据类型
        public byte cbCheckCode;                       //效验字段
        public ushort wPacketSize;                       //数据大小
    };

    //网络命令
    struct TCP_Command
    {
        public ushort wMainCmdID;                            //主命令码
        public ushort wSubCmdID;                         //子命令码
    };

    //网络包头
    struct TCP_Head
    {
        public byte cbDataKind;                            //数据类型
        public byte cbCheckCode;                       //效验字段
        public ushort wPacketSize;                       //数据大小

        public ushort wMainCmdID;                            //主命令码
        public ushort wSubCmdID;                         //子命令码
    };

    struct Packet_Error
    {
        public ushort wMainCmdID;                   //主命令码
        public ushort wSubCmdID;                    //子命令码
        public string errorMsg;                     //错误消息文字
    }

    struct Net_Error
    {
        public ushort errorCode;                    //操作错误码
        public int SocketErrorCode;                 //socket错误码
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 100)]
        public byte[] errorMsg;                     //错误消息文字
    }

    public struct Intermediate_Packet_Info
    {
        public byte[] data;
        public string networkChannelName;
    }

    struct NetworkChannelStatus
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
        public byte[] localIPAddress;
        public int localPort;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
        public byte[] remoteIPAddress;
        public int remotePort;
        public bool connectStatus;
    }
}