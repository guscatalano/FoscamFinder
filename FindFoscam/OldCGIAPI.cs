using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FindFoscam
{
    //Follows this PDF: IP Camera CGI v1.21.pdf written by Maverick Gao in 2007
    public class OldCGIAPI : ICameraAPI
    {
        public OldCGIAPI(Camera cam) : base(cam)
        {

        }

        public override string GetAPIName()
        {
            return "OldGCIAPI - 1.21";
        }

        public override bool Login(string user, string password)
        {
            throw new NotImplementedException();
        }
    }
}
