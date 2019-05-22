using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;

namespace Core.Common
{
    /// <summary>
    /// IpUtility
    /// </summary>
    public class IpUtility
    {
        /// <summary>
        /// 获取内网地址
        /// </summary>
        /// <returns></returns>
        public static string GetLocalIPv4()
        {
            foreach (NetworkInterface item in NetworkInterface.GetAllNetworkInterfaces())
            {
                if (item.OperationalStatus == OperationalStatus.Up)
                {
                    foreach (UnicastIPAddressInformation ip in item.GetIPProperties().UnicastAddresses)
                    {
                        if (ip.Address.AddressFamily == AddressFamily.InterNetwork)
                        {
                            if (!IPAddress.IsLoopback(ip.Address))
                            {
                                if (IsInternal(ip.Address))
                                {
                                    return ip.Address.ToString();
                                }
                            }
                        }
                    }
                }
            }
            return "";
        }

        /// <summary>
        /// 判断是否是内网地址
        /// </summary>
        /// <param name="toTest"></param>
        /// <returns></returns>
        public static bool IsInternal(IPAddress toTest)
        {
            byte[] bytes = toTest.GetAddressBytes();
            switch (bytes[0])
            {
                case 10:
                    return true;
                case 172:
                    return bytes[1] < 32 && bytes[1] >= 16;
                case 192:
                    return bytes[1] == 168;
                default:
                    return false;
            }
        }
    }
}
