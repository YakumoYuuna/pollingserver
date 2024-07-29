using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PollingServer
{
    class CGRKD_Josn
    {
        public class Model
        {
            public FBillTypeID FBillTypeID { get; set; }
            public string FDate { get; set; }
            public string FWMSID { get; set; }
            public FStockOrgId FStockOrgId { get; set; }
            public FDemandOrgId FDemandOrgId { get; set; }
            public FPurchaseOrgId FPurchaseOrgId { get; set; }
            public FSupplierId FSupplierId { get; set; }
            public FSupplyId FSupplyId { get; set; }
            public FSettleId FSettleId { get; set; }
            public FChargeId FChargeId { get; set; }
            public string FOwnerTypeIdHead { get; set; }
            public FOwnerIdHead FOwnerIdHead { get; set; }
            public List<FInStockEntry> FInStockEntry { get; set; }
        }

        public class FBillTypeID
        {
            public string FNUMBER { get; set; }
        }

        public class FStockOrgId
        {
            public string FNumber { get; set; }
        }

        public class FDemandOrgId
        {
            public string FNumber { get; set; }
        }

        public class FPurchaseOrgId
        {
            public string FNumber { get; set; }
        }

        public class FSupplierId
        {
            public string FNumber { get; set; }
        }

        public class FSupplyId
        {
            public string FNumber { get; set; }
        }

        public class FSettleId
        {
            public string FNumber { get; set; }
        }

        public class FChargeId
        {
            public string FNumber { get; set; }
        }

        public class FOwnerIdHead
        {
            public string FNumber { get; set; }
        }

        public class FInStockEntry
        {
            public string FRowType { get; set; }
            public FMaterialId FMaterialId { get; set; }
            public FLot FLot { get; set; }
            public string FSRCBILLTYPEID { get; set; }
            public string FSRCBillNo { get; set; }
            public FUnitID FUnitID { get; set; }
            public string FRealQty { get; set; }
            public FPriceUnitID FPriceUnitID { get; set; }
            public FStockId FStockId { get; set; }
            public FStockStatusId FStockStatusId { get; set; }
            public string FGiveAway { get; set; }
            public string FOWNERTYPEID { get; set; }
            public FOWNERID FOWNERID { get; set; }
            public string FCheckInComing { get; set; }
            public string FPriceBaseQty { get; set; }
            public FRemainInStockUnitId FRemainInStockUnitId { get; set; }
            public List<FInStockEntryLink> FInStockEntry_Link { get; set; }
        }

        public class FMaterialId
        {
            public string FNumber { get; set; }
        }

        public class FLot
        {
            public string FNumber { get; set; }
        }

        public class FUnitID
        {
            public string FNumber { get; set; }
        }

        public class FPriceUnitID
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

        public class FOWNERID
        {
            public string FNumber { get; set; }
        }

        public class FRemainInStockUnitId
        {
            public string FNumber { get; set; }
        }

        public class FInStockEntryLink
        {
            public string FInStockEntry_Link_FRuleId { get; set; }
            public string FInStockEntry_Link_FSTableName { get; set; }
            public string FInStockEntry_Link_FSBillId { get; set; }
            public string FInStockEntry_Link_FSId { get; set; }
        }

        public class Root
        {
            public Model Model { get; set; }
        }


        public static Root Return_CGRKD_Json(string dataString)
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
                    FBillTypeID = new FBillTypeID { FNUMBER = "RKD01_SYS" },
                    FDate = keyValuePairsList[0]["FDate"].ToString(),
                    FWMSID= keyValuePairsList[0]["FWMSID"].ToString(),
                    FStockOrgId = new FStockOrgId { FNumber = keyValuePairsList[0]["FStockOrgId"].ToString() },
                    FDemandOrgId = new FDemandOrgId { FNumber = keyValuePairsList[0]["FDemandOrgId"].ToString() },
                    FPurchaseOrgId = new FPurchaseOrgId { FNumber = keyValuePairsList[0]["FPurchaseOrgId"].ToString() },
                    FSupplierId = new FSupplierId { FNumber = keyValuePairsList[0]["FSupplierId"].ToString() },
                    FSupplyId = new FSupplyId { FNumber = keyValuePairsList[0]["FSupplyId"].ToString() },
                    FSettleId = new FSettleId { FNumber = keyValuePairsList[0]["FSettleId"].ToString() },
                    FChargeId = new FChargeId { FNumber = keyValuePairsList[0]["FChargeId"].ToString() },
                    FOwnerTypeIdHead = keyValuePairsList[0]["FOwnerTypeIdHead"].ToString(),
                    FOwnerIdHead = new FOwnerIdHead { FNumber = keyValuePairsList[0]["FOwnerIdHead"].ToString() },
                    FInStockEntry = new List<FInStockEntry>()
                }
            };

            foreach (var keyValuePairs in keyValuePairsList)
            {
                var fInStockEntry = new FInStockEntry
                {
                    FRowType = keyValuePairs["FRowType"].ToString(),
                    FMaterialId = new FMaterialId { FNumber = keyValuePairs["FMaterialId"].ToString() },
                    FLot = new FLot { FNumber = keyValuePairs["FLOT"].ToString() },
                    FSRCBILLTYPEID = "PUR_ReceiveBill",
                    FSRCBillNo = keyValuePairs["FBillNo"].ToString(),
                    FUnitID = new FUnitID { FNumber = keyValuePairs["FUnitID"].ToString() },
                    FRealQty = keyValuePairs["FRealQty"].ToString(),
                    FPriceUnitID = new FPriceUnitID { FNumber = keyValuePairs["FPriceUnitID"].ToString() },
                    FStockId = new FStockId { FNumber = keyValuePairs["FStockId"].ToString() },
                    FStockStatusId = new FStockStatusId { FNumber = keyValuePairs["FStockStatusId"].ToString() },
                    FGiveAway = keyValuePairs["FGiveAway"].ToString(),
                    FOWNERTYPEID = keyValuePairs["FOWNERTYPEID"].ToString(),
                    FOWNERID = new FOWNERID { FNumber = keyValuePairs["FOwnerIdHead"].ToString() },
                    FCheckInComing = keyValuePairs["FCheckInComing"].ToString(),
                    FPriceBaseQty = keyValuePairs["FPriceBaseQty"].ToString(),
                    FRemainInStockUnitId = new FRemainInStockUnitId { FNumber = keyValuePairs["FRemainInStockUnitId"].ToString() },
                    FInStockEntry_Link = new List<FInStockEntryLink>()
                };

                //JArray fInStockEntryLinks = JArray.Parse(keyValuePairs["FInStockEntry_Link"].ToString());
                //foreach (JObject link in fInStockEntryLinks)
                //{
                var fInStockEntryLink = new FInStockEntryLink
                {
                    FInStockEntry_Link_FRuleId = "PUR_ReceiveBill-STK_InStock",
                    FInStockEntry_Link_FSTableName = "T_PUR_ReceiveEntry",
                    FInStockEntry_Link_FSBillId = keyValuePairs["FInStockEntry_Link_FSBillId"].ToString(),
                    FInStockEntry_Link_FSId = keyValuePairs["FInStockEntry_Link_FSId"].ToString()
                };

                fInStockEntry.FInStockEntry_Link.Add(fInStockEntryLink);
                //}

                rootObject.Model.FInStockEntry.Add(fInStockEntry);
            }

            return rootObject;
        }

    }
}
