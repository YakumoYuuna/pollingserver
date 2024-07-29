using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PollingServer
{
    public class ClassApi_CGTLD
    {
        public class FMATERIALID
        {
            public string FNumber { get; set; }
        }

        public class FPRICEUNITID
        {
            public string FNumber { get; set; }
        }

        public class FSTOCKID
        {
            public string FNumber { get; set; }
        }

        //public class FAUXPROPIDFF100001
        //{
        //    public string FNumber { get; set; }
        //}

        //public class FAuxPropID
        //{
        //    public FAUXPROPIDFF100001 FAUXPROPID__FF100001 { get; set; }
        //}

        public class FPURMRBENTRY
        {
            public FMATERIALID FMATERIALID { get; set; }
            public double FRMREALQTY { get; set; }
            public FPRICEUNITID FPRICEUNITID { get; set; }
            public FSTOCKID FSTOCKID { get; set; }
            //public FAuxPropID FAuxPropID { get; set; }
        }

        public class FBillTypeID
        {
            public string FNUMBER { get; set; }
        }

        public class FStockOrgId
        {
            public string FNumber { get; set; }
        }

        public class FSupplierID
        {
            public string FNumber { get; set; }
        }

        public class Model
        {
            public FBillTypeID FBillTypeID { get; set; }
            public string FDate { get; set; }
            public FStockOrgId FStockOrgId { get; set; }
            public FSupplierID FSupplierID { get; set; }
            public string FOwnerTypeIdHead { get; set; }
            public FStockOrgId FOwnerIdHead { get; set; }
            public List<FPURMRBENTRY> FPURMRBENTRY { get; set; }
        }

        public class Root
        {
            public Model Model { get; set; }
        }



    }
}
