using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PollingServer
{
    class ClassApi_Push
    {
        public class Model
        {
            public string Ids { get; set; }
            public string RuleId { get; set; }
        }

        public class RootObject
        {
            public string Ids { get; set; }
            public string RuleId { get; set; }
        }
    }
}
