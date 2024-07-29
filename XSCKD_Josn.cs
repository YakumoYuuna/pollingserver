using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PollingServer
{
    class XSCKD_Josn
    {
        public class Model
        {
            public FBillTypeID FBillTypeID { get; set; }
            public string FDate { get; set; }
            public FSaleOrgId FSaleOrgId { get; set; }
            public FCustomerID FCustomerID { get; set; }
            public FReceiverID FReceiverID { get; set; }
            public FStockOrgId FStockOrgId { get; set; }
            public FSettleID FSettleID { get; set; }
            public FPayerID FPayerID { get; set; }
            public string FOwnerTypeIdHead { get; set; }
            public List<FEntity> FEntity { get; set; }
        }
        public class FBillTypeID
        {
            public string FNUMBER { get; set; }
        }

        public class FSaleOrgId
        {
            public string FNumber { get; set; }
        }

        public class FCustomerID
        {
            public string FNumber { get; set; }
        }

        public class FReceiverID
        {
            public string FNumber { get; set; }
        }

        public class FStockOrgId
        {
            public string FNumber { get; set; }
        }

        public class FSettleID
        {
            public string FNumber { get; set; }
        }

        public class FPayerID
        {
            public string FNumber { get; set; }
        }

        public class FOwnerID
        {
            public string FNumber { get; set; }
        }

        public class FMaterialID
        {
            public string FNumber { get; set; }
        }

        public class FUnitID
        {
            public string FNumber { get; set; }
        }

        public class FStockID
        {
            public string FNumber { get; set; }
        }

        public class FStockStatusID
        {
            public string FNumber { get; set; }
        }

        public class FSalUnitID
        {
            public string FNumber { get; set; }
        }

        public class FLot
        {
            public string FNumber { get; set; }
        }

        public class FEntityLink
        {
            public string FEntity_Link_FRuleId { get; set; }
            public string FEntity_Link_FSTableName { get; set; }
            public string FEntity_Link_FSBillId { get; set; }
            public string FEntity_Link_FSId { get; set; }
        }

        public class FEntity
        {
            public string FRowType { get; set; }
            public FMaterialID FMaterialID { get; set; }
            public FUnitID FUnitID { get; set; }
            public string FRealQty { get; set; }
            public string FIsFree { get; set; }
            public string FOwnerTypeID { get; set; }
            public FOwnerID FOwnerID { get; set; }
            public string FEntryTaxRate { get; set; }
            public FStockID FStockID { get; set; }
            public FStockStatusID FStockStatusID { get; set; }
            public FSalUnitID FSalUnitID { get; set; }
            public string FSALUNITQTY { get; set; }
            public string FSALBASEQTY { get; set; }
            public string FPRICEBASEQTY { get; set; }
            public FLot FLot { get; set; }
            public List<FEntityLink> FEntity_Link { get; set; }
        }



        public class Root
        {
            public Model Model { get; set; }
        }

        public static Root Return_XSCKD_Json(string dataString)
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
                    FBillTypeID = new FBillTypeID { FNUMBER = "XSCKD01_SYS" },
                    FDate = keyValuePairsList[0]["FDate"].ToString(),
                    FSaleOrgId = new FSaleOrgId { FNumber = keyValuePairsList[0]["FSaleOrgId"].ToString() },
                    FCustomerID = new FCustomerID { FNumber = keyValuePairsList[0]["FCustomerID"].ToString() },
                    FReceiverID = new FReceiverID { FNumber = keyValuePairsList[0]["FReceiverID"].ToString() },
                    FStockOrgId = new FStockOrgId { FNumber = keyValuePairsList[0]["FStockOrgId"].ToString() },
                    FSettleID = new FSettleID { FNumber = keyValuePairsList[0]["FSettleID"].ToString() },
                    FPayerID = new FPayerID { FNumber = keyValuePairsList[0]["FPayerID"].ToString() },
                    FOwnerTypeIdHead = keyValuePairsList[0]["FOwnerTypeIdHead"].ToString(),
                    FEntity = new List<FEntity>()
                }
            };

            //JArray fEntityArray = JArray.Parse(jsonObject["Model"]["FEntity"].ToString());
            foreach (var entity in keyValuePairsList)
            {
                FEntity fEntity = new FEntity
                {
                    FRowType = "Standard",
                    FMaterialID = new FMaterialID { FNumber = entity["FMaterialId"].ToString() },
                    FUnitID = new FUnitID { FNumber = entity["FUnitID"].ToString() },
                    FRealQty = entity["FRealQty"].ToString(),
                    FIsFree = entity["FIsFree"].ToString(),
                    FOwnerTypeID = entity["FOwnerTypeID"].ToString(),
                    FOwnerID = new FOwnerID { FNumber = entity["FOwnerID"].ToString() },
                    FEntryTaxRate = entity["FEntryTaxRate"].ToString(),
                    FStockID = new FStockID { FNumber = entity["FStockID"].ToString() },
                    FStockStatusID = new FStockStatusID { FNumber = entity["FStockStatusID"].ToString() },
                    FSalUnitID = new FSalUnitID { FNumber = entity["FSalUnitID"].ToString() },
                    FSALUNITQTY = entity["FSALUNITQTY"].ToString(),
                    FSALBASEQTY = entity["FSALBASEQTY"].ToString(),
                    FPRICEBASEQTY = entity["FPRICEBASEQTY"].ToString(),
                    FLot = new FLot { FNumber = entity["Flot"].ToString() },
                    FEntity_Link = new List<FEntityLink>()
                };

                //JArray fEntityLinkArray = JArray.Parse(entity["FEntity_Link"].ToString());
                //foreach (JObject link in fEntityLinkArray)
                //{
                FEntityLink fEntityLink = new FEntityLink
                {
                    FEntity_Link_FRuleId = "DeliveryNotice-OutStock",
                    FEntity_Link_FSTableName = "T_SAL_DELIVERYNOTICEENTRY",
                    FEntity_Link_FSBillId = entity["FEntity_Link_FSBillId"].ToString(),
                    FEntity_Link_FSId = entity["FEntity_Link_FSId"].ToString()
                };

                fEntity.FEntity_Link.Add(fEntityLink);
                //}

                rootObject.Model.FEntity.Add(fEntity);
            }

            return rootObject;
        }
    }
}
