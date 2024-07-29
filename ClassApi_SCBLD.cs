using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PollingServer
{
    class ClassApi_SCBLD
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
            public string FAppQty { get; set; }
            public string FActualQty { get; set; }
        }
    }
}
