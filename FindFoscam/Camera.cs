using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace FindFoscam
{
    class Camera
    {
        public bool Valid
        {
            get;
            internal set;
        }
        public Exception Error
        {
            get;
            internal set;
        }
        public string ID {
            get;
            internal set;
        }

        public string Name {
            get;
            internal set;
        }

        public IPEndPoint SourceEP
        {
            get;
            internal set;
        }

        public IPAddress IP {
            get;
            internal set;
        }

        public IPAddress Mask {
            get;
            internal set;
        }

        public IPAddress Gateway {
            get;
            internal set;
        }

        public IPAddress DNS {
            get;
            internal set;
        }

        public string SysVersion {
            get;
            internal set;
        }

        public string AppVersion {
            get;
            internal set;
        }

        public int Port {
            get;
            internal set;
        }

        public bool DHCPEnabled {
            get;
            internal set;
        }

        public static bool IsCameraPacket(Byte[] receiveBytes)
        {
            return !(receiveBytes.Count() < 65);
        }
        public Camera(Byte[] receiveBytes, IPEndPoint source)
        {

            try
            {
                Valid = true;
                Error = null;
                SourceEP = source;
                Byte[] camID = new Byte[13];
                int startIndex = 23;
                int currentIndex = startIndex;
                Array.Copy(receiveBytes, currentIndex, camID, 0, 13);
                ID = Encoding.Default.GetString(camID);

                currentIndex += 13;
                Byte[] camname = new Byte[21];
                Array.Copy(receiveBytes, currentIndex, camname, 0, 21);
                Name = Encoding.Default.GetString(camname);

                currentIndex += 21;
                Byte[] ipb = new Byte[4];
                Array.Copy(receiveBytes, currentIndex, ipb, 0, 4);
                int ip1 = BitConverter.ToInt32(ipb, 0);
                if (ip1 < 0)
                {
                    Array.Reverse(ipb);
                    ip1 = BitConverter.ToInt32(ipb, 0);
                }
                IP = new IPAddress(ip1);

                currentIndex += 4;
                Byte[] maskB = new Byte[4];
                Array.Copy(receiveBytes, currentIndex, maskB, 0, 4);
                int mask1 = BitConverter.ToInt32(maskB, 0);
                if (mask1 < 0)
                {
                    Array.Reverse(maskB);
                    mask1 = BitConverter.ToInt32(maskB, 0);
                }
                Mask = new IPAddress(mask1);

                currentIndex += 4;
                Byte[] gw = new Byte[4];
                Array.Copy(receiveBytes, currentIndex, gw, 0, 4);
                int gw1 = BitConverter.ToInt32(gw, 0);
                if (gw1 < 0)
                {
                    Array.Reverse(gw);
                    gw1 = BitConverter.ToInt32(gw, 0);
                }
                Gateway = new IPAddress(gw1);

                currentIndex += 4;
                Byte[] dnsB = new Byte[4];
                Array.Copy(receiveBytes, currentIndex, dnsB, 0, 4);
                int dns1 = BitConverter.ToInt32(dnsB, 0);
                if (dns1 < 0)
                {
                    Array.Reverse(dnsB);
                    dns1 = BitConverter.ToInt32(dnsB, 0);
                }

                DNS = new IPAddress(dns1);


                currentIndex += 4;
                Byte[] reserve = new Byte[4];
                Array.Copy(receiveBytes, currentIndex, reserve, 0, 4);

                currentIndex += 4;
                Byte[] sys = new Byte[4];
                Array.Copy(receiveBytes, currentIndex, sys, 0, 4);
                SysVersion = sys[0] + "." + sys[1] + "." + sys[2] + "." + sys[3];

                currentIndex += 4;
                Byte[] app = new Byte[4];
                Array.Copy(receiveBytes, currentIndex, app, 0, 4);
                AppVersion = app[0] + "." + app[1] + "." + app[2] + "." + app[3];

                currentIndex += 4;
                Byte[] portB = new Byte[2];
                Array.Copy(receiveBytes, currentIndex, portB, 0, 2);
                Array.Reverse(portB);
                Port = BitConverter.ToInt16(portB, 0);

                currentIndex += 2;
                Byte[] dhcp = new Byte[1];
                Array.Copy(receiveBytes, currentIndex, dhcp, 0, 1);
                DHCPEnabled = BitConverter.ToBoolean(dhcp, 0);

            }
            catch (Exception e)
            {
                Valid = false;
                Error = e;
            }
        
        }

        public void PrintInfo()
        {
            Console.WriteLine("==================================");
            Console.WriteLine("Cam ID: " + ID);
            Console.WriteLine("Cam Name: " + Name);
            Console.WriteLine("IP: " + IP);
            Console.WriteLine("Mask: " + Mask);
            Console.WriteLine("GW: " + Gateway);
            Console.WriteLine("DNS: " + DNS);
            Console.WriteLine("Sys: " + SysVersion);
            Console.WriteLine("App: " + AppVersion);
            Console.WriteLine("Port: " + Port);
            Console.WriteLine("DHCP: " + DHCPEnabled);
            Console.WriteLine("Valid: " + Valid);
            if (!Valid)
            {
                Console.WriteLine("Error: " + Error.ToString());
            }
            Console.WriteLine("==================================");
        }
    }
}
