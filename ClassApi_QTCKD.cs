using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PollingServer
{
   public  class ClassApi_QTCKD
    {
        public class FMaterialId
        {
            public string FNumber { get; set; }
        }

        public class FUnitID
        {
            public string FNumber { get; set; }
        }

        public class FStockId
        {
            public string FNumber { get; set; }
        }

        public class FStockStatusId
        {
            public string FNumber { get; set; }
        }

        public class FEntity
        {
            public FMaterialId FMaterialId { get; set; }
            public FUnitID FUnitID { get; set; }
            public double FQty { get; set; }
            public FStockId FStockId { get; set; }
            public FStockStatusId FStockStatusId { get; set; }
        }

        public class FBillTypeID
        {
            public string FNUMBER { get; set; }
        }

        public class FStockOrgId
        {
            public string FNumber { get; set; }
        }

        public class FCustId
        {
            public string FNumber { get; set; }
        }

        public class FDeptId
        {
            public string FNumber { get; set; }
        }

        public class FOwnerIdHead
        {
            public string FNumber { get; set; }
        }

        public class Model
        {
            public FBillTypeID FBillTypeID { get; set; }
            public FStockOrgId FStockOrgId { get; set; }
            public string FStockDirect { get; set; }
            public string FDate { get; set; }
            public FCustId FCustId { get; set; }
            public FDeptId FDeptId { get; set; }
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
