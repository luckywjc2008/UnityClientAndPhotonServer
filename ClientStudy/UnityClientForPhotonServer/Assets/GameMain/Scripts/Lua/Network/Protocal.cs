using System.Runtime.InteropServices;
using System;

namespace StarForce
{
    public class Protocal
    {
        /// <summary>
        /// ����
        /// </summary>
        public const string HeartBeat = "01";
        /// <summary>
        /// ���ӷ�����
        /// </summary>
        public const string Connect = "02";
        /// <summary>
        /// �쳣����
        /// </summary>
        public const string Exception = "03";
        /// <summary>
        /// ��������   
        /// </summary>
        public const string Disconnect = "04";

        /// <summary>
        /// ������Ϣ
        /// </summary>
        public const string Message = "05";

        /// <summary>
        /// ���Ӵ���
        /// </summary>
        public const string NetError = "06";
    }

    //�����ں�
    struct TCP_Info
    {
        public byte cbDataKind;                            //��������
        public byte cbCheckCode;                       //Ч���ֶ�
        public ushort wPacketSize;                       //���ݴ�С
    };

    //��������
    struct TCP_Command
    {
        public ushort wMainCmdID;                            //��������
        public ushort wSubCmdID;                         //��������
    };

    //�����ͷ
    struct TCP_Head
    {
        public byte cbDataKind;                            //��������
        public byte cbCheckCode;                       //Ч���ֶ�
        public ushort wPacketSize;                       //���ݴ�С

        public ushort wMainCmdID;                            //��������
        public ushort wSubCmdID;                         //��������
    };

    struct Packet_Error
    {
        public ushort wMainCmdID;                   //��������
        public ushort wSubCmdID;                    //��������
        public string errorMsg;                     //������Ϣ����
    }

    struct Net_Error
    {
        public ushort errorCode;                    //����������
        public int SocketErrorCode;                 //socket������
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 100)]
        public byte[] errorMsg;                     //������Ϣ����
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