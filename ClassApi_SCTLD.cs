using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PollingServer
{
    class ClassApi_SCTLD
    {
        public class Model
        {
            public string FID { get; set; }
            public List<FEntityItem> FEntity { get; set; }
        }

        public class RootObject
        {
            public List<string> NeedUpDateFields { get; set; }
            public string IsDeleteEntry { get; set; }
            public Model Model { get; set; }
        }

        public class FEntityItem
        {
            public string FEntryID { get; set; }
            public string FAPPQty { get; set; }
            public string FQty { get; set; }
        }
    }
}
