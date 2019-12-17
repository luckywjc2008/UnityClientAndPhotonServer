/*
 * Author:      NOW
 * CreateTime:  1/17/2018 11:25:50 AM
 * Description:
 * 
*/
using GameFramework;
using System;
using System.Net.Sockets;
using System.Runtime.InteropServices;

namespace UnityGameFramework.Runtime
{
    public sealed partial class NetworkComponent : GameFrameworkComponent
    {
        #region 自动转换IPV4为IPV6

#if UNITY_IPHONE && !UNITY_EDITOR
	[DllImport("__Internal")]
	private static extern string getIPv6(string mHost, string mPort);  
#endif

        private string GetIPv6(string mHost, string mPort)
        {
#if UNITY_IPHONE && !UNITY_EDITOR
		    string mIPv6 = getIPv6(mHost, mPort);
		    return mIPv6;
#else
            return mHost + "&&ipv4";
#endif
        }

        public void GetIPType(string serverIp, string serverPorts, out string newServerIp, out AddressFamily mIPType)
        {
            mIPType = AddressFamily.InterNetwork;
            newServerIp = serverIp;
            try
            {
                string mIPv6 = GetIPv6(serverIp, serverPorts);
                if (!string.IsNullOrEmpty(mIPv6))
                {
                    string[] m_StrTemp = System.Text.RegularExpressions.Regex.Split(mIPv6, "&&");
                    if (m_StrTemp != null && m_StrTemp.Length >= 2)
                    {
                        string IPType = m_StrTemp[1];
                        if (IPType == "ipv6")
                        {
                            newServerIp = m_StrTemp[0];
                            mIPType = AddressFamily.InterNetworkV6;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Log.Error("GetIPv6 error:" + e);
            }
        }

        #endregion
    }
}