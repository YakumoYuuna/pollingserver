using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PollingServer
{
    class ZJDBD_Josn
    {
        public class FBillTypeID
        {
            public string FNUMBER { get; set; }
        }

        public class FSettleOrgId
        {
            public string FNumber { get; set; }
        }

        public class FSaleOrgId
        {
            public string FNumber { get; set; }
        }

        public class FStockOutOrgId
        {
            public string FNumber { get; set; }
        }

        public class FOwnerOutIdHead
        {
            public string FNumber { get; set; }
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

        public class FLot
        {
            public string FNumber { get; set; }
        }

        public class FSrcStockId
        {
            public string FNumber { get; set; }
        }

        public class FDestStockId
        {
            public string FNumber { get; set; }
        }

        public class FSrcStockStatusId
        {
            public string FNumber { get; set; }
        }

        public class FDestStockStatusId
        {
            public string FNumber { get; set; }
        }

        public class FOwnerOutId
        {
            public string FNumber { get; set; }
        }

        public class FOwnerId
        {
            public string FNumber { get; set; }
        }

        public class FBaseUnitId
        {
            public string FNumber { get; set; }
        }

        public class FKeeperId
        {
            public string FNumber { get; set; }
        }

        public class FKeeperOutId
        {
            public string FNumber { get; set; }
        }

        public class FDestLot
        {
            public string FNumber { get; set; }
        }

        public class FDestMaterialId
        {
            public string FNUMBER { get; set; }
        }

        public class FSaleUnitId
        {
            public string FNumber { get; set; }
        }

        public class FPriceUnitID
        {
            public string FNumber { get; set; }
        }

        public class FBillEntry
        {
            public string FRowType { get; set; }
            public FMaterialId FMaterialId { get; set; }
            public FUnitID FUnitID { get; set; }
            public string FQty { get; set; }
            public FLot FLot { get; set; }
            public FSrcStockId FSrcStockId { get; set; }
            public FDestStockId FDestStockId { get; set; }
            public FSrcStockStatusId FSrcStockStatusId { get; set; }
            public FDestStockStatusId FDestStockStatusId { get; set; }
            public string FBusinessDate { get; set; }
            public string FOwnerTypeOutId { get; set; }
            public FOwnerOutId FOwnerOutId { get; set; }
            public string FOwnerTypeId { get; set; }
            public FOwnerId FOwnerId { get; set; }
            public FBaseUnitId FBaseUnitId { get; set; }
            public string FBaseQty { get; set; }
            public string FISFREE { get; set; }
            public string FKeeperTypeId { get; set; }
            public FKeeperId FKeeperId { get; set; }
            public string FKeeperTypeOutId { get; set; }
            public FKeeperOutId FKeeperOutId { get; set; }
            public FDestLot FDestLot { get; set; }
            public FDestMaterialId FDestMaterialId { get; set; }
            public FSaleUnitId FSaleUnitId { get; set; }
            public string FSaleQty { get; set; }
            public string FSalBaseQty { get; set; }
            public FPriceUnitID FPriceUnitID { get; set; }
            public string FPriceQty { get; set; }
            public string FPriceBaseQty { get; set; }
            public string FTransReserveLink { get; set; }
            public string FCheckDelivery { get; set; }
        }

        public class Model
        {
            public FBillTypeID FBillTypeID { get; set; }
            public string FBizType { get; set; }
            public string FTransferDirect { get; set; }
            public string FTransferBizType { get; set; }
            public FSettleOrgId FSettleOrgId { get; set; }
            public FSaleOrgId FSaleOrgId { get; set; }
            public FStockOutOrgId FStockOutOrgId { get; set; }
            public string FOwnerTypeOutIdHead { get; set; }
            public FOwnerOutIdHead FOwnerOutIdHead { get; set; }
            public FStockOrgId FStockOrgId { get; set; }
            public string FOwnerTypeIdHead { get; set; }
            public FOwnerIdHead FOwnerIdHead { get; set; }
            public string FDate { get; set; }
            public List<FBillEntry> FBillEntry { get; set; }
        }

        public class Root
        {
            public Model Model { get; set; }
        }

        public static Root Return_ZJDBD_Json(string dataString)
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
                    FBizType = keyValuePairsList[0]["FBizType"].ToString(),
                    FTransferDirect = keyValuePairsList[0]["FTransferDirect"].ToString(),
                    FTransferBizType = keyValuePairsList[0]["FTransferBizType"].ToString(),
                    FSettleOrgId = new FSettleOrgId { FNumber = keyValuePairsList[0]["FSettleOrgId"].ToString() },
                    FSaleOrgId = new FSaleOrgId { FNumber = keyValuePairsList[0]["FSaleOrgId"].ToString() },
                    FStockOutOrgId = new FStockOutOrgId { FNumber = keyValuePairsList[0]["FStockOutOrgId"].ToString() },
                    FOwnerTypeOutIdHead = keyValuePairsList[0]["FOwnerTypeOutIdHead"].ToString(),
                    FOwnerOutIdHead = new FOwnerOutIdHead { FNumber = keyValuePairsList[0]["FOwnerOutIdHead"].ToString() },
                    FStockOrgId = new FStockOrgId { FNumber = keyValuePairsList[0]["FStockOrgId"].ToString() },
                    FOwnerTypeIdHead = keyValuePairsList[0]["FOwnerTypeIdHead"].ToString(),
                    FOwnerIdHead = new FOwnerIdHead { FNumber = keyValuePairsList[0]["FOwnerIdHead"].ToString() },
                    FDate = keyValuePairsList[0]["FDate"].ToString(),
                    FBillEntry = new List<FBillEntry>()
                }
            };

            //JArray fBillEntryArray = JArray.Parse(jsonObject["Model"]["FBillEntry"].ToString());
            foreach (var entry in keyValuePairsList)
            {
                FBillEntry fBillEntry = new FBillEntry
                {
                    FRowType = entry["FRowType"].ToString(),
                    FMaterialId = new FMaterialId { FNumber = entry["FMaterialId"].ToString() },
                    FUnitID = new FUnitID { FNumber = entry["FUnitID"].ToString() },
                    FQty = entry["FQty"].ToString(),
                    FLot = new FLot { FNumber = entry["FLot"].ToString() },
                    FSrcStockId = new FSrcStockId { FNumber = entry["FSrcStockId"].ToString() },
                    FDestStockId = new FDestStockId { FNumber = entry["FDestStockId"].ToString() },
                    FSrcStockStatusId = new FSrcStockStatusId { FNumber = entry["FSrcStockStatusId"].ToString() },
                    FDestStockStatusId = new FDestStockStatusId { FNumber = entry["FDestStockStatusId"].ToString() },
                    FBusinessDate = entry["FBusinessDate"].ToString(),
                    FOwnerTypeOutId = entry["FOwnerTypeOutId"].ToString(),
                    FOwnerOutId = new FOwnerOutId { FNumber = entry["FOwnerOutId"].ToString() },
                    FOwnerTypeId = entry["FOwnerTypeId"].ToString(),
                    FOwnerId = new FOwnerId { FNumber = entry["FOwnerId"].ToString() },
                    FBaseUnitId = new FBaseUnitId { FNumber = entry["FBaseUnitId"].ToString() },
                    FBaseQty = entry["FBaseQty"].ToString(),
                    FISFREE = entry["FISFREE"].ToString(),
                    FKeeperTypeId = entry["FKeeperTypeId"].ToString(),
                    FKeeperId = new FKeeperId { FNumber = entry["FKeeperId"].ToString() },
                    FKeeperTypeOutId = entry["FKeeperTypeOutId"].ToString(),
                    FKeeperOutId = new FKeeperOutId { FNumber = entry["FKeeperOutId"].ToString() },
                    FDestLot = new FDestLot { FNumber = entry["FDestLot"].ToString() },
                    FDestMaterialId = new FDestMaterialId { FNUMBER = entry["FDestMaterialId"].ToString() },
                    FSaleUnitId = new FSaleUnitId { FNumber = entry["FSaleUnitId"].ToString() },
                    FSaleQty = entry["FSaleQty"].ToString(),
                    FSalBaseQty = entry["FSalBaseQty"].ToString(),
                    FPriceUnitID = new FPriceUnitID { FNumber = entry["FPriceUnitID"].ToString() },
                    FPriceQty = entry["FPriceQty"].ToString(),
                    FPriceBaseQty = entry["FPriceBaseQty"].ToString(),
                    FTransReserveLink = entry["FTransReserveLink"].ToString(),
                    FCheckDelivery = entry["FCheckDelivery"].ToString()
                };
                rootObject.Model.FBillEntry.Add(fBillEntry);
            }

            return rootObject;
        }

    }
}
