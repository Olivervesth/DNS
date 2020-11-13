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
    class Display
    {
        /*This is the Display class basically just to prompt the user with the info i couldent find a need for 3 layers so there is just 2 */
        GetData data = new GetData();
        public void LocalPing()//2
        {
            // Ping's the local machine.
            Ping pingSender = new Ping();
            IPAddress address = IPAddress.Loopback;
            PingReply reply = pingSender.Send(address);

            if (reply.Status == IPStatus.Success)
            {
                Console.WriteLine("Address: {0}", reply.Address.ToString());
                Console.WriteLine("RoundTrip time: {0}", reply.RoundtripTime);
                Console.WriteLine("Time to live: {0}", reply.Options.Ttl);
                Console.WriteLine("Don't fragment: {0}", reply.Options.DontFragment);
                Console.WriteLine("Buffer size: {0}", reply.Buffer.Length);
            }
            else
            {
                Console.WriteLine(reply.Status);
            }
            Tracer();
        }
        public void DisplayDhcpServerAddresses()
        {
            Console.WriteLine("DHCP Servers");
            NetworkInterface[] adapters = NetworkInterface.GetAllNetworkInterfaces();
            foreach (NetworkInterface adapter in adapters)
            {
                IPInterfaceProperties adapteradapterProperties = adapter.GetIPProperties();
                IPAddressCollection addresses = adapteradapterProperties.DhcpServerAddresses;
                if (addresses.Count > 0)
                {
                    Console.WriteLine(adapter.Description);
                    foreach (IPAddress address in addresses)
                    {
                        Console.WriteLine("  Dhcp Address ............................ : {0}", address.ToString());
                    }
                    Console.WriteLine();
                }
            }
            DisplayHostInfo();
        }

        public void Tracer()//3
        {
            Console.WriteLine("Type in a address you wanna trace");
            string input = Console.ReadLine();
            Console.WriteLine("start");
            string t = data.GetHostnameFromIp(input);
            Console.WriteLine(t);
            Console.WriteLine("slut");
            string adr = data.GetIpFromHostname(t);
            Console.WriteLine("Weee " + adr);


            
            string a = data.Traceroute(input);
            Console.WriteLine("route*** " + a);

            DisplayDhcpServerAddresses();
        }
        public void DisplayHostInfo()
        {
            string[] alias = data.GetHostAlias();
            Console.WriteLine("Host name : " + data.GetHostInfo().HostName);
            Console.WriteLine("\nAliases : ");
            for (int index = 0; index < alias.Length; index++)
            {
                Console.WriteLine(alias[index]);
            }
            DisplayHostIpList();
        }
        public void DisplayHostIpList()
        {
            IPAddress[] address = data.GetHostIpAddressList();
            Console.WriteLine("\nIP address list : ");
            for (int index = 0; index < address.Length; index++)
            {
                Console.WriteLine(address[index]);
            }
            Console.WriteLine("Press enter to go again");
            Console.ReadKey();
        }
        public void Wiki()
        {
            IPAddress[] array = data.GetWiki();
            if(array != null)
            {
            foreach (IPAddress ip in array)
            {
                Console.WriteLine(ip.ToString());
            }
            }
            LocalPing();
        }
    }
}
