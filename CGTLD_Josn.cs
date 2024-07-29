using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PollingServer
{
    class CGTLD_Josn
    {
        public class Model
        {
            public FBillTypeID FBillTypeID { get; set; }
            public string FDate { get; set; }
            public FStockOrgId FStockOrgId { get; set; }
            public FRequireOrgId FRequireOrgId { get; set; }
            public FPurchaseOrgId FPurchaseOrgId { get; set; }
            public FSupplierID FSupplierID { get; set; }
            public FACCEPTORID FACCEPTORID { get; set; }
            public FSettleId FSettleId { get; set; }
            public FCHARGEID FCHARGEID { get; set; }
            public string FOwnerTypeIdHead { get; set; }
            public FOwnerIdHead FOwnerIdHead { get; set; }
            public List<FPURMRBENTRY> FPURMRBENTRY { get; set; }
        }
        public class FBillTypeID
        {
            public string FNUMBER { get; set; }
        }

        public class FStockOrgId
        {
            public string FNumber { get; set; }
        }

        public class FRequireOrgId
        {
            public string FNumber { get; set; }
        }

        public class FPurchaseOrgId
        {
            public string FNumber { get; set; }
        }

        public class FSupplierID
        {
            public string FNumber { get; set; }
        }

        public class FACCEPTORID
        {
            public string FNumber { get; set; }
        }

        public class FSettleId
        {
            public string FNumber { get; set; }
        }

        public class FCHARGEID
        {
            public string FNumber { get; set; }
        }

        public class FOwnerIdHead
        {
            public string FNumber { get; set; }
        }

        public class FMATERIALID
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

        public class FPRICEUNITID
        {
            public string FNumber { get; set; }
        }

        public class FSTOCKID
        {
            public string FNumber { get; set; }
        }

        public class FOWNERID
        {
            public string FNumber { get; set; }
        }

        public class FPURMRBENTRYLink
        {
            public string FPURMRBENTRY_Link_FRuleId { get; set; }
            public string FPURMRBENTRY_Link_FSTableName { get; set; }
            public string FPURMRBENTRY_Link_FSBillId { get; set; }
            public string FPURMRBENTRY_Link_FSId { get; set; }
        }

        public class FPURMRBENTRY
        {
            public string FRowType { get; set; }
            public FMATERIALID FMATERIALID { get; set; }
            public FUnitID FUnitID { get; set; }
            public string FRMREALQTY { get; set; }
            public FLot FLot { get; set; }
            public string FSRCBILLTYPEID { get; set; }
            public string FSRCBillNo { get; set; }
            public FPRICEUNITID FPRICEUNITID { get; set; }
            public FSTOCKID FSTOCKID { get; set; }
            public string FGiveAway { get; set; }
            public string FOWNERTYPEID { get; set; }
            public FOWNERID FOWNERID { get; set; }
            public string FIsStock { get; set; }
            public List<FPURMRBENTRYLink> FPURMRBENTRY_Link { get; set; }
        }


        public class Root
        {
            public Model Model { get; set; }
        }

        public static Root Return_CGTLD_Json(string dataString)
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
                    FBillTypeID = new FBillTypeID { FNUMBER = "TLD01_SYS" },
                    FDate = keyValuePairsList[0]["FDate"].ToString(),
                    FStockOrgId = new FStockOrgId { FNumber = keyValuePairsList[0]["FStockOrgId"].ToString() },
                    FRequireOrgId = new FRequireOrgId { FNumber = keyValuePairsList[0]["FRequireOrgId"].ToString() },
                    FPurchaseOrgId = new FPurchaseOrgId { FNumber = keyValuePairsList[0]["FPurchaseOrgId"].ToString() },
                    FSupplierID = new FSupplierID { FNumber = keyValuePairsList[0]["FSupplierID"].ToString() },
                    FACCEPTORID = new FACCEPTORID { FNumber = keyValuePairsList[0]["FSupplierID"].ToString() },
                    FSettleId = new FSettleId { FNumber = keyValuePairsList[0]["FSettleId"].ToString() },
                    FCHARGEID = new FCHARGEID { FNumber = keyValuePairsList[0]["FCHARGEID"].ToString() },
                    FOwnerTypeIdHead = keyValuePairsList[0]["FOwnerTypeIdHead"].ToString(),
                    FOwnerIdHead = new FOwnerIdHead { FNumber = keyValuePairsList[0]["FOwnerIdHead"].ToString() },
                    FPURMRBENTRY = new List<FPURMRBENTRY>()
                }
            };

            //JArray fPURMRBENTRYArray = JArray.Parse(jsonObject["Model"]["FPURMRBENTRY"].ToString());
            foreach (var entry in keyValuePairsList)
            {
                FPURMRBENTRY fPURMRBENTRY = new FPURMRBENTRY
                {
                    FRowType = entry["FRowType"].ToString(),
                    FMATERIALID = new FMATERIALID { FNumber = entry["FMATERIALID"].ToString() },
                    FUnitID = new FUnitID { FNumber = entry["FUnitID"].ToString() },
                    FRMREALQTY = entry["FRMREALQTY"].ToString(),
                    FLot = new FLot { FNumber = entry["FLOT"].ToString() },
                    //STK_InStock
                    FSRCBILLTYPEID = "STK_InStock",
                    FSRCBillNo = entry["FBillNo"].ToString(),
                    FPRICEUNITID = new FPRICEUNITID { FNumber = entry["FPRICEUNITID"].ToString() },
                    FSTOCKID = new FSTOCKID { FNumber = entry["FSTOCKID"].ToString() },
                    FGiveAway = entry["FGiveAway"].ToString(),
                    FOWNERTYPEID = entry["FOWNERTYPEID"].ToString(),
                    FOWNERID = new FOWNERID { FNumber = entry["FOWNERID"].ToString() },
                    FIsStock = "false",
                    FPURMRBENTRY_Link = new List<FPURMRBENTRYLink>()
                };

                //JArray fPURMRBENTRYLinkArray = JArray.Parse(entry["FPURMRBENTRY_Link"].ToString());
                //foreach (JObject link in fPURMRBENTRYLinkArray)
                //{
                FPURMRBENTRYLink fPURMRBENTRYLink = new FPURMRBENTRYLink
                {
                    //STK_InStock-PUR_MRB
                    //T_STK_INSTOCKENTRY
                    FPURMRBENTRY_Link_FRuleId = "STK_InStock-PUR_MRB",
                    FPURMRBENTRY_Link_FSTableName = "T_STK_INSTOCKENTRY",
                    FPURMRBENTRY_Link_FSBillId = entry["FPURMRBENTRY_Link_FSBillId"].ToString(),
                    FPURMRBENTRY_Link_FSId = entry["FPURMRBENTRY_Link_FSId"].ToString()
                };

                fPURMRBENTRY.FPURMRBENTRY_Link.Add(fPURMRBENTRYLink);
                //}

                rootObject.Model.FPURMRBENTRY.Add(fPURMRBENTRY);
            }

            return rootObject;
        }
    }
}

