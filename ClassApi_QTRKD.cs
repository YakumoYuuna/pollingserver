using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PollingServer
{
    public class ClassApi_QTRKD
    {
        public class FMATERIALID
        {
            public string FNumber { get; set; }
        }

        public class FUnitID
        {
            public string FNumber { get; set; }
        }

        public class FSTOCKID
        {
            public string FNumber { get; set; }
        }

        public class FSTOCKSTATUSID
        {
            public string FNumber { get; set; }
        }

        public class FBillTypeID
        {
            public string FNUMBER { get; set; }
        }

        public class FStockOrgId
        {
            public string FNumber { get; set; }
        }

        public class FSUPPLIERID
        {
            public string FNumber { get; set; }
        }

        public class FDEPTID
        {
            public string FNumber { get; set; }
        }

        public class FOwnerIdHead
        {
            public string FNumber { get; set; }
        }

        public class FEntity
        {
            public FMATERIALID FMATERIALID { get; set; }
            public FUnitID FUnitID { get; set; }
            public FSTOCKID FSTOCKID { get; set; }
            public FSTOCKSTATUSID FSTOCKSTATUSID { get; set; }
            public double FQty { get; set; }
        }

        public class Model
        {
            public FBillTypeID FBillTypeID { get; set; }
            public FStockOrgId FStockOrgId { get; set; }
            public string FStockDirect { get; set; }
            public string FDate { get; set; }
            public FSUPPLIERID FSUPPLIERID { get; set; }
            public FDEPTID FDEPTID { get; set; }
            public string FOwnerTypeIdHead { get; set; }
            public FOwnerIdHead FOwnerIdHead { get; set; }
            public List<FEntity> FEntity { get; set; }
        }

        public class RootObject
        {
            public Model Model { get; set; }
        }

    }
}
