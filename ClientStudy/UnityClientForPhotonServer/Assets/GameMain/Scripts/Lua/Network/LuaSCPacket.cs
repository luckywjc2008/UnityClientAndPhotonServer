using GameFramework.Network;
using System;

namespace StarForce
{
    public class LuaSCPacket : Packet
    {
        public override int Id
        {
            get
            {
                return Convert.ToInt32(Protocal.Message);
            }
        }

        public string protocalId;

        public byte[] dataBody;

        public override void Clear()
        {
            protocalId = null;
            dataBody = null;
        }
    }

}
