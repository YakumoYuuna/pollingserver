using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PollingServer
{
    class ClassApi_SCLLD
    {
        public class FLot
        {
            public string FNumber { get; set; }
        }

        public class FStockID
        {
            public string FNumber { get; set; }
        }

        public class FEntityItem
        {
            public string FEntryID { get; set; }
            public FLot FLot { get; set; }
            public FStockID FStockID { get; set; }
        }

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

    }
}
