using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PollingServer
{
    class PYD_Josn
    {
        public class FBillTypeID
        {
            public string FNUMBER { get; set; }
        }

        public class FStockOrgId
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

        public class FOwnerid
        {
            public string FNumber { get; set; }
        }

        public class FStockStatusId
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
            public string FBusinessDate { get; set; }
            public string FOwnerTypeId { get; set; }
            public FOwnerid FOwnerid { get; set; }
            public FStockStatusId FStockStatusId { get; set; }
        }

        public class Model
        {
            public FBillTypeID FBillTypeID { get; set; }
            public FStockOrgId FStockOrgId { get; set; }
            public string FOwnerTypeIdHead { get; set; }
            public FOwnerIdHead FOwnerIdHead { get; set; }
            public string FDate { get; set; }
            public FDeptId FDeptId { get; set; }
            public List<FBillEntry> FBillEntry { get; set; }
        }

        public class Root
        {
            public Model Model { get; set; }
        }

        public static Root Return_PYD_Json(string dataString)
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
                    FStockOrgId = new FStockOrgId { FNumber = keyValuePairsList[0]["FStockOrgId"].ToString() },
                    FOwnerTypeIdHead = keyValuePairsList[0]["FOwnerTypeIdHead"].ToString(),
                    FOwnerIdHead = new FOwnerIdHead { FNumber = keyValuePairsList[0]["FOwnerIdHead"].ToString() },
                    FDate = keyValuePairsList[0]["FDate"].ToString(),
                    FDeptId = new FDeptId { FNumber = keyValuePairsList[0]["FDeptId"].ToString() },
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
                    FBusinessDate = entry["FBusinessDate"].ToString(),
                    FOwnerTypeId = entry["FOwnerTypeId"].ToString(),
                    FOwnerid = new FOwnerid { FNumber = entry["FOwnerid"].ToString() },
                    FStockStatusId = new FStockStatusId { FNumber = entry["FStockStatusId"].ToString() }
                };

                rootObject.Model.FBillEntry.Add(fBillEntry);
            }

            return rootObject;
        }
    }
}
