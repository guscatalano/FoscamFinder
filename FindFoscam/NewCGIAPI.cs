using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace FindFoscam
{
    //Follows PDF: IPCamera CGI user Guide-3518 Ver 1.0.10 written by Xiao Jinsheng in 2012
    public class NewCGIAPI : ICameraAPI
    {

        public string RootURL;
        public NewCGIAPI(Camera cam) : base(cam)
        {
            RootURL = "http://" + camera.IP + ":" + camera.Port;
        }

        public override string GetAPIName()
        {
            return "NewCGIAPI - 1.0.10";
        }

        public bool CheckResultCode(XmlDocument result, int IdealState = 0)
        {
            if (!result.DocumentElement.Name.Equals("CGI_Result"))
            {
                return false;
            }
            XmlNodeList list = result.DocumentElement.GetElementsByTagName("result");
            if(list.Count != 1)
            {
                return false;
            }
            int state = Int32.Parse(list[0].InnerText);
            return (state == IdealState);
        }

        public XmlDocument MakeRequest(string parameters)
        {
            string url = RootURL + "/cgi-bin/CGIProxy.fcgi?" + WebUtility.UrlEncode(parameters);

            HttpClient client = new HttpClient();
            Task<string> t = client.GetStringAsync(url);
            t.Wait();
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(t.Result);
            if (!doc.DocumentElement.Name.Equals("CGI_Result"))
            {
                return null;
            }
            return doc;
        }

        public override bool Login(string user, string password)
        {
            XmlDocument ret = MakeRequest("cmd=logIn&usrName=" + user + "&pwd=" + password);
            bool r = CheckResultCode(ret);
            
            return true;
        }
    }
}
