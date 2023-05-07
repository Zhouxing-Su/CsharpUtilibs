using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;


namespace IDeal.Szx.CsharpUtilibs.Sys.Network {
    public class IP {
        public static IPAddress[] GetLocalIP() { return Dns.GetHostEntry(Dns.GetHostName()).AddressList; }
        public static IEnumerable<IPAddress> GetLocalIP(AddressFamily addressFamily) {
            return GetLocalIP().Where((ip) => (ip.AddressFamily == addressFamily));
        }
        public static IEnumerable<IPAddress> GetLocalIPv4() { return GetLocalIP(AddressFamily.InterNetwork); }
        public static IEnumerable<IPAddress> GetLocalIPv6() { return GetLocalIP(AddressFamily.InterNetworkV6); }
    }
}
