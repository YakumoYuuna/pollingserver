using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PollingServer
{
    class PKD_Josn
    {
        public class FBillTypeID
        {
            public string FNUMBER { get; set; }
        }

        public class FStockOrgId
        {
            public string FNumber { get; set; }
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

        public class FStockId
        {
            public string FNumber { get; set; }
        }

        public class FLot
        {
            public string FNumber { get; set; }
        }

        public class FStockStatusId
        {
            public string FNumber { get; set; }
        }

        public class FOwnerId
        {
            public string FNumber { get; set; }
        }

        public class FKeeperId
        {
            public string FNumber { get; set; }
        }

        public class FBillEntry
        {
            public FMaterialId FMaterialId { get; set; }
            public FUnitID FUnitID { get; set; }
            public string FCountQty { get; set; }
            public FStockId FStockId { get; set; }
            public FLot FLot { get; set; }
            public FStockStatusId FStockStatusId { get; set; }
            public string FOwnerTypeId { get; set; }
            public FOwnerId FOwnerId { get; set; }
            public string FKeeperTypeId { get; set; }
            public FKeeperId FKeeperId { get; set; }
            public string FBaseCountQty { get; set; }
        }

        public class Model
        {
            public FBillTypeID FBillTypeID { get; set; }
            public string FDate { get; set; }
            public FStockOrgId FStockOrgId { get; set; }
            public string FOwnerTypeIdHead { get; set; }
            public FOwnerIdHead FOwnerIdHead { get; set; }
            public List<FBillEntry> FBillEntry { get; set; }
        }

        public class Root
        {
            public Model Model { get; set; }
        }

        public static Root Return_PKD_Json(string dataString)
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
                    FBillTypeID = new FBillTypeID { FNUMBER = keyValuePairsList[0]["FBillTypeID"].ToString() },
                    FDate = keyValuePairsList[0]["FDate"].ToString(),
                    FStockOrgId = new FStockOrgId { FNumber = keyValuePairsList[0]["FStockOrgId"].ToString() },
                    FOwnerTypeIdHead = keyValuePairsList[0]["FOwnerTypeIdHead"].ToString(),
                    FOwnerIdHead = new FOwnerIdHead { FNumber = keyValuePairsList[0]["FOwnerIdHead"].ToString() },
                    FBillEntry = new List<FBillEntry>()
                }
            };

            //JArray fBillEntryArray = JArray.Parse(jsonObject["Model"]["FBillEntry"].ToString());
            foreach (var entry in keyValuePairsList)
            {
                FBillEntry fBillEntry = new FBillEntry
                {
                    FMaterialId = new FMaterialId { FNumber = entry["FMaterialId"].ToString() },
                    FUnitID = new FUnitID { FNumber = entry["FUnitID"].ToString() },
                    FCountQty = entry["FCountQty"].ToString(),
                    FStockId = new FStockId { FNumber = entry["FStockId"].ToString() },
                    FLot = new FLot { FNumber = entry["FLot"].ToString() },
                    FStockStatusId = new FStockStatusId { FNumber = entry["FStockStatusId"].ToString() },
                    FOwnerTypeId = entry["FOwnerTypeId"].ToString(),
                    FOwnerId = new FOwnerId { FNumber = entry["FOwnerid"].ToString() },
                    FKeeperTypeId = entry["FKeeperTypeId"].ToString(),
                    FKeeperId = new FKeeperId { FNumber = entry["FKeeperId"].ToString() },
                    FBaseCountQty = entry["FBaseCountQty"].ToString()
                };
                rootObject.Model.FBillEntry.Add(fBillEntry);
            }

            return rootObject;
        }
    }
}
