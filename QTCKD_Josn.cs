using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PollingServer
{
    class QTCKD_Josn
    {
        public class FBillTypeID
        {
            public string FNUMBER { get; set; }
        }

        public class FStockOrgId
        {
            public string FNumber { get; set; }
        }

        public class FPickOrgId
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

        public class FPickerId
        {
            public string FStaffNumber { get; set; }
        }

        public class FOwnerIdHead
        {
            public string FNumber { get; set; }
        }

        public class FMaterialId
        {
            public string FNumber { get; set; }
        }

        public class FUnitID
        {
            public string FNumber { get; set; }
        }

        public class FBaseUnitId
        {
            public string FNumber { get; set; }
        }

        public class FStockId
        {
            public string FNumber { get; set; }
        }

        public class FLot
        {
            public string FNumber { get; set; }
        }

        public class FOwnerId
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
            public string FQty { get; set; }
            public FBaseUnitId FBaseUnitId { get; set; }
            public FStockId FStockId { get; set; }
            public FLot FLot { get; set; }
            public string FOwnerTypeId { get; set; }
            public FOwnerId FOwnerId { get; set; }
            public FStockStatusId FStockStatusId { get; set; }
        }

        public class Model
        {
            public FBillTypeID FBillTypeID { get; set; }
            public FStockOrgId FStockOrgId { get; set; }
            public FPickOrgId FPickOrgId { get; set; }
            public string FStockDirect { get; set; }
            public string FDate { get; set; }
            public FCustId FCustId { get; set; }
            public FDeptId FDeptId { get; set; }
            public FPickerId FPickerId { get; set; }
            public string FOwnerTypeIdHead { get; set; }
            public FOwnerIdHead FOwnerIdHead { get; set; }
            public List<FEntity> FEntity { get; set; }
        }

        public class Root
        {
            public Model Model { get; set; }
        }

        public static Root Return_QTCKD_Json(string dataString)
        {
            // 反序列化为JArray
            JArray jsonArray = JArray.Parse(dataString);

            // 创建键值对数组列表
            List<Dictionary<string, object>> keyValuePairsList = new List<Dictionary<string, object>>();

            foreach (JObject entry in jsonArray)
            {
                Dictionary<string, object> keyValuePairs = new Dictionary<string, object>();

                foreach (var property in entry.Properties())
                {
                    keyValuePairs.Add(property.Name, property.Value);
                }

                keyValuePairsList.Add(keyValuePairs);
            }

            // 创建Model对象并进行赋值
            Root rootObject = new Root
            {
                Model = new Model
                {
                    FBillTypeID = new FBillTypeID { FNUMBER = "QTCKD01_SYS" },
                    FStockOrgId = new FStockOrgId { FNumber = keyValuePairsList[0]["FStockOrgId"].ToString() },
                    FPickOrgId = new FPickOrgId { FNumber = keyValuePairsList[0]["FPickOrgId"].ToString() },
                    FStockDirect = keyValuePairsList[0]["FStockDirect"].ToString(),
                    FDate = keyValuePairsList[0]["FDate"].ToString(),
                    //FCustId = new FCustId { FNumber = keyValuePairsList[0]["FCustId"].ToString() },
                    FDeptId = new FDeptId { FNumber = keyValuePairsList[0]["FDeptId"].ToString() },
                    //FPickerId = new FPickerId { FStaffNumber = keyValuePairsList[0]["FPickerId"].ToString() },
                    FOwnerTypeIdHead = keyValuePairsList[0]["FOwnerTypeIdHead"].ToString(),
                    FOwnerIdHead = new FOwnerIdHead { FNumber = keyValuePairsList[0]["FOwnerIdHead"].ToString() },
                    FEntity = new List<FEntity>()
                }
            };

            //JArray fEntityArray = JArray.Parse(jsonObject["Model"]["FEntity"].ToString());
            foreach (var entity in keyValuePairsList)
            {
                FEntity fEntity = new FEntity
                {
                    FMaterialId = new FMaterialId { FNumber = entity["FMaterialId"].ToString() },
                    FUnitID = new FUnitID { FNumber = entity["FUnitID"].ToString() },
                    FQty = entity["FQty"].ToString(),
                    FBaseUnitId = new FBaseUnitId { FNumber = entity["FBaseUnitId"].ToString() },
                    FStockId = new FStockId { FNumber = entity["FStockId"].ToString() },
                    FLot = new FLot { FNumber = entity["FLot"].ToString() },
                    FOwnerTypeId = entity["FOwnerTypeId"].ToString(),
                    FOwnerId = new FOwnerId { FNumber = entity["FOwnerId"].ToString() },
                    FStockStatusId = new FStockStatusId { FNumber = entity["FStockStatusId"].ToString() }
                };

                rootObject.Model.FEntity.Add(fEntity);
            }

            return rootObject;
        }
    }
}
