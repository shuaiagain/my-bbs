using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Configuration;

namespace BBS2018.Bussiness.Utils
{
    public class ConnectionUtil
    {
        public static readonly string connBBS = ConfigurationManager.ConnectionStrings["connBBS"].Name;
    }
}
