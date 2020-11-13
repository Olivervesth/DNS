using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace DNS
{
    class GetData
    {
        bool internetconnection = true;
        public string GetIpFromHostname(string Hostname)
        {
            string ip = "";
            try
            {
                IPHostEntry ipHostEntry = Dns.GetHostEntry(Hostname);//Apparently GetHostEntry is the new syntax
                if (ipHostEntry.AddressList.Length > 0)
                {
                    //ip = ipHostEntry.AddressList[0].Address.ToString();
                    ip = ipHostEntry.AddressList[0].ToString();
                }
                else
                {
                    ip = "No information found.";
                }
            }
            catch (SocketException)
            {
                ip = "Unable to perform lookup - a socket error occured.";
                return ip;
            }
            catch (SecurityException)
            {
                ip = "Unable to perform lookup - permission denied.";
                return ip;
            }
            catch (Exception)
            {
                ip = "An unspecified error occured.";
                return ip;
            }

            return ip;
        }

        public string GetHostnameFromIp(string Ip)
        {
            string hostname = "";
            try
            {
                IPHostEntry ipHostEntry = Dns.GetHostEntry(Ip);
                hostname = ipHostEntry.HostName;
            }
            catch (FormatException)
            {
                hostname = "Please specify a valid IP address.";
                return hostname;
            }
            catch (SocketException)
            {
                hostname = "Unable to perform lookup - a socket error occured.";
                return hostname;
            }
            catch (SecurityException)
            {
                hostname = "Unable to perform lookup - permission denied.";
                return hostname;
            }
            catch (Exception)
            {
                hostname = "An unspecified error occured.";
                return hostname;
            }

            return hostname;
        }
        public string[] GetHostAlias()
        {
            // Get the alias names of the addresses in the IP address list.
            IPHostEntry hostinfo = GetHostInfo();

            string[] alias = hostinfo.Aliases;


            return alias;
        }
        public IPHostEntry GetHostInfo()
        {
            //WIN-M69SG2Q0732.test.local
            //ZBC-RG01203MKC
            //string hostName = "ZBC-RG01203MKC";
            IPHostEntry hostInfo = Dns.GetHostEntry(Dns.GetHostName());//This is much more dynamic than hardcoding specific hostname this way there is no reason to change this part of the code in the feature
            return hostInfo;
        }
        public IPAddress[] GetHostIpAddressList()
        {
            // Get the IP address list that resolves to the host names contained in the 
            // Alias property.
            IPAddress[] address = GetHostInfo().AddressList;

            return address;
        }

        public IPAddress[] GetWiki()//I found use in this by using it as a internet connection checker this is our fail safe 
        {
            try
            {
                IPAddress[] array = Dns.GetHostAddresses("en.wikipedia.org");
                internetconnection = true;
                return array;
            }
            catch (Exception)
            {
                internetconnection = false;
                return null;
            }
            
        }
        public string Traceroute(string ipAddressOrHostName)//Tracing addresses 
        {
            if(internetconnection == false)//Checks of we have gotten told there is no connection
            {
                return "No internet connection";
            }
            IPAddress ipAddress = null;
            try
            {
                 ipAddress = Dns.GetHostEntry(ipAddressOrHostName).AddressList[0];
            }
            catch (Exception e)
            {

                return $"Something was wrong {e.Message}";
            }
                StringBuilder traceResults = new StringBuilder();
          


            using (Ping pingSender = new Ping())
            {

                PingOptions pingOptions = new PingOptions();
                Stopwatch stopWatch = new Stopwatch();
                byte[] bytes = new byte[32];

                pingOptions.DontFragment = true;
                pingOptions.Ttl = 1;
                int maxHops = 30;

                traceResults.AppendLine(
                    string.Format(
                        "Tracing route to {0} over a maximum of {1} hops:",
                        ipAddress,
                        maxHops));

                traceResults.AppendLine();

                for (int i = 1; i < maxHops + 1; i++)
                {
                    stopWatch.Reset();
                    stopWatch.Start();

                    PingReply pingReply = pingSender.Send(
                        ipAddress,
                        5000,
                        new byte[32], pingOptions);

                    stopWatch.Stop();

                    traceResults.AppendLine(
                        string.Format("{0}\t{1} ms\t{2}",
                        i,
                        stopWatch.ElapsedMilliseconds,
                        pingReply.Address));



                    if (pingReply.Status == IPStatus.Success)
                    {
                        traceResults.AppendLine();
                        traceResults.AppendLine("Trace complete."); break;
                    }

                    pingOptions.Ttl++;

                }
            }
            return traceResults.ToString();
        }
    }
}
