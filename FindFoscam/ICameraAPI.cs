using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace FindFoscam
{
    public abstract class ICameraAPI
    {

        public Camera camera
        {
            get;
            internal set;
        }
        public ICameraAPI(Camera cam)
        {
            this.camera = cam;
        }

        public static ICameraAPI GetAPI(Camera cam)
        {
            try
            {
                string root = "http://" + cam.IP + ":" + cam.Port;
                string url = root + "/cgi-bin/CGIProxy.fcgi";

                HttpClient client = new HttpClient();
                Task<string> t = client.GetStringAsync(url);
                t.Wait();
                return new NewCGIAPI(cam);
            }
            catch (Exception)
            {
                return new OldCGIAPI(cam);
            }
        }

        public abstract string GetAPIName();

        public abstract bool Login(string user, string password);
    }
}
