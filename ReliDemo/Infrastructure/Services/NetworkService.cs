using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.Runtime.InteropServices;

namespace ReliDemo.Infrastructure.Services
{
    public class NetworkService
    {
        [DllImport("iphlpapi.dll", ExactSpelling=true)]
        public static extern int SendARP( int DestIP, int SrcIP, [Out] byte[] pMacAddr, ref int PhyAddrLen );
        public string GetMacAddress(string sName)
        {
            string s = string.Empty;
            System.Net.IPHostEntry Tempaddr = null;
            Tempaddr = (System.Net.IPHostEntry)Dns.GetHostEntry(sName);
            System.Net.IPAddress[] TempAd = Tempaddr.AddressList;
            string[] Ipaddr = new string[3];
            foreach (IPAddress TempA in TempAd)
            {
                Ipaddr[1] = TempA.ToString();
                byte[] ab = new byte[6];
                int len = ab.Length;
                int intAddress = BitConverter.ToInt32(IPAddress.Parse(sName).GetAddressBytes(), 0);
                int r = SendARP(intAddress, 0, ab, ref len);
                string sMAC = BitConverter.ToString(ab, 0, 6);
                Ipaddr[2] = sMAC;
                s = sMAC;
            }
            return s;
        }

        public string GetUserIP(HttpRequestBase request)
        {
            string ipList = request.ServerVariables["HTTP_X_FORWARDED_FOR"];

            if (!string.IsNullOrEmpty(ipList))
            {
                return ipList.Split(',')[0];
            }

            return request.ServerVariables["REMOTE_ADDR"];
        }
    }
}