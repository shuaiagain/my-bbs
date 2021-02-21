using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseTest.ViewModel
{
    public class TestOne
    {
        public string name { get { return "abc"; } }

        private string myname = "abc";

        public string Name { get { return myname; } }
    }
}
