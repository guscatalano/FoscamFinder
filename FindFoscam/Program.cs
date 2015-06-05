using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FindFoscam
{
    class Program
    {

        public static List<Camera> cameras = new List<Camera>();
        public static UdpClient udpclient = new UdpClient(10000);
        public static void DiscoverThread()
        {
            //UdpClient udpclient = new UdpClient();
            while (true)
            {
                try
                {
                    //This is the packet that discovers cameras
                    Byte[] buffer = null;//11
                    //4d:4f:5f:49:00:00:00:00:00:00:00:00:00:00:00:04:00:00:00:04:00:00:00:00:00:00:01
                    buffer = new byte[] { 
                0x4d, 0x4f, 0x5f, 0x49, 0x00, 0x00, 
                0x00, 0x00, 0x00, 0x00, 
                0x00, 0x00, 0x00, 0x00, 
                0x00, 0x04, 0x00, 0x00, 
                0x00, 0x04, 0x00, 0x00, 
                0x00, 0x00, 0x00, 0x00, 0x01 };

                    //Not sure what this packet is for, but it's sent... It's not needed.
                    Byte[] buffer2nd = new byte[] { 
                0xb4, 0x9a, 0x70, 0x4d, 0x00};



                    IPAddress multicastaddress = IPAddress.Parse("255.255.255.255");
                    IPEndPoint remoteep = new IPEndPoint(multicastaddress, 10000);
                    udpclient.EnableBroadcast = true;
                    udpclient.Send(buffer, buffer.Length, remoteep);
                    //udpclient.Send(buffer2nd, buffer2nd.Length, remoteep);

                    Thread.Sleep(10000);
                }
                catch (Exception e)
                {
                    Console.WriteLine("Discover: " + e.ToString());
                }
            }
        }

        public static void RecDiscover()
        {
            IPEndPoint recPoint = new IPEndPoint(IPAddress.Broadcast, 1);
            
            while (true)
            {
                try
                {
                    Byte[] receiveBytes = udpclient.Receive(ref recPoint);
                    if (Camera.IsCameraPacket(receiveBytes))
                    {
                        Camera cam = new Camera(receiveBytes, recPoint);
                        if (cameras.FirstOrDefault(x => x.ID == cam.ID) == null)
                        {
                            cameras.Add(cam);
                        }
                    }
                    Thread.Sleep(1000);
                }
                catch (Exception e)
                {
                    Console.WriteLine("Rec: " + e.ToString());
                }
            }
        }
        
        static void Main(string[] args)
        {
            
            Console.WriteLine("Press ENTER to start sending messages");
            Console.ReadLine();
            Thread t = new Thread(new ThreadStart(DiscoverThread));
            t.Start();

            Thread t2 = new Thread(new ThreadStart(RecDiscover));
            t2.Start();
            while (true)
            {
                Console.WriteLine("Press ENTER to print current list");
                Console.ReadLine();
                Console.WriteLine("~~~~~~~~~~~~~~~~~~START~~~~~~~~~~~~~~~~~~~~~~~");
                foreach (Camera cam in cameras)
                {
                    cam.PrintInfo();
                }
                Console.WriteLine("~~~~~~~~~~~~~~~~~~~END~~~~~~~~~~~~~~~~~~~~~~~~");
            }
            
            
            
            
            Console.WriteLine("All Done! Press ENTER to quit.");
            Console.ReadLine(); 
        }
    }
}
