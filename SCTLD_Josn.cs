using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace PollingServer
{
    class SCTLD_Josn
    {
        public class Model
        {
            public FBillType FBillType { get; set; }
            public string FDate { get; set; }
            public FStockOrgId FStockOrgId { get; set; }
            public FPrdOrgId FPrdOrgId { get; set; }
            public string FOwnerTypeId0 { get; set; }
            public List<FEntity> FEntity { get; set; }
        }

        public class FBillType
        {
            public string FNUMBER { get; set; }
        }

        public class FStockOrgId
        {
            public string FNumber { get; set; }
        }

        public class FPrdOrgId
        {
            public string FNumber { get; set; }
        }

        public class FEntity
        {
            public FParentMaterialId FParentMaterialId { get; set; }
            public FMaterialId FMaterialId { get; set; }
            public FUnitID FUnitID { get; set; }
            public string FAPPQty { get; set; }
            public string FQty { get; set; }
            public string FReturnType { get; set; }
            public string FReserveType { get; set; }
            public FStockStatusId FStockStatusId { get; set; }
            public FStockId FStockId { get; set; }
            public string FIsUpdateQty { get; set; }
            public string FBASESTOCKQTY { get; set; }
            public FStockUnitId FStockUnitId { get; set; }
            public string FStockQty { get; set; }
            public FBaseUnitId FBaseUnitId { get; set; }
            public string FBaseQty { get; set; }
            public string FMoBillNo { get; set; }
            public string FMOENTRYID { get; set; }
            public string FPPBOMENTRYID { get; set; }
            public string FOperId { get; set; }
            public FLot FLot { get; set; }
            public string FMoId { get; set; }
            public string FMoEntrySeq { get; set; }
            public string FPPBomBillNo { get; set; }
            public string FSrcBillType { get; set; }
            public string FOwnerTypeId { get; set; }
            public FOwnerId FOwnerId { get; set; }
            public string FParentOwnerTypeId { get; set; }
            public FParentOwnerId FParentOwnerId { get; set; }
            public List<FEntityLink> FEntity_Link { get; set; }
        }

        public class FParentMaterialId
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

        public class FStockStatusId
        {
            public string FNumber { get; set; }
        }

        public class FStockUnitId
        {
            public string FNumber { get; set; }
        }

        public class FBaseUnitId
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

        public class FParentOwnerId
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

        public class Root
        {
            public Model Model { get; set; }
        }


        public static Root Return_SCTLD_Json(string dataString)
        {

            // 反序列化为JArray
            JArray jsonArray = JArray.Parse(dataString);

            // 创建键值对数组列表
            List<Dictionary<string, object>> keyValuePairsList = new List<Dictionary<string, object>>();

            foreach (JObject jsonObject in jsonArray)
            {
                Dictionary<string, object> keyValuePairs = new Dictionary<string, object>();

                foreach (var property in jsonObject.Properties())
                {
                    keyValuePairs.Add(property.Name, property.Value);
                }

                keyValuePairsList.Add(keyValuePairs);
            }

            Root rootObject = new Root
            {
                Model = new Model
                {
                    FBillType = new FBillType { FNUMBER = "SCTLD01_SYS" },
                    FDate = keyValuePairsList[0]["FDate"].ToString(),
                    FStockOrgId = new FStockOrgId { FNumber = keyValuePairsList[0]["FStockOrgId"].ToString() },
                    FPrdOrgId = new FPrdOrgId { FNumber = keyValuePairsList[0]["FPrdOrgId"].ToString() },
                    FOwnerTypeId0 = keyValuePairsList[0]["FOwnerTypeId0"].ToString(),
                    FEntity = new List<FEntity>()
                }
            };

            foreach (var entity in keyValuePairsList)
            {
                var fEntity = new FEntity
                {
                    FParentMaterialId = new FParentMaterialId { FNumber = entity["FParentMaterialId"].ToString() },
                    FMaterialId = new FMaterialId { FNumber = entity["FMaterialId"].ToString() },
                    FUnitID = new FUnitID { FNumber = entity["FUnitID"].ToString() },
                    FAPPQty = entity["FAPPQty"].ToString(),
                    FQty = entity["FQty"].ToString(),
                    FReturnType = entity["FReturnType"].ToString(),
                    //FReserveType = entity["FReserveType"].ToString(),
                    FStockStatusId = new FStockStatusId { FNumber = entity["FStockStatusId"].ToString() },
                    FStockId = new FStockId { FNumber = entity["FStockId"].ToString() },
                    //FIsUpdateQty = entity["FIsUpdateQty"].ToString(),
                    FBASESTOCKQTY = entity["FBASESTOCKQTY"].ToString(),
                    FStockUnitId = new FStockUnitId { FNumber = entity["FStockUnitId"].ToString() },
                    FStockQty = entity["FStockQty"].ToString(),
                    FBaseUnitId = new FBaseUnitId { FNumber = entity["FBaseUnitId"].ToString() },
                    FBaseQty = entity["FBaseQty"].ToString(),
                    FMoBillNo = entity["FMoBillNo"].ToString(),
                    FMOENTRYID = entity["FMoEntryId"].ToString(),
                    FPPBOMENTRYID = entity["FPPBomEntryId"].ToString(),
                    FOperId = entity["FOperId"].ToString(),
                    FLot = new FLot { FNumber = entity["FLot"].ToString() },
                    FMoId = entity["FMoId"].ToString(),
                    FMoEntrySeq = entity["FMoEntrySeq"].ToString(),
                    FPPBomBillNo = entity["FPPBomBillNo"].ToString(),
                    //FSrcBillType = "PRD_PPBOM",
                    FSrcBillType = "PRD_PickMtrl",
                    FOwnerTypeId = entity["FOwnerTypeId"].ToString(),
                    FOwnerId = new FOwnerId { FNumber = entity["FOwnerId"].ToString() },
                    FParentOwnerTypeId = entity["FParentOwnerTypeId"].ToString(),
                    FParentOwnerId = new FParentOwnerId { FNumber = entity["FParentOwnerId"].ToString() },
                    FEntity_Link = new List<FEntityLink>()
                };

                //foreach (var link in entity["FEntity_Link"])
                //{
                var fEntityLink = new FEntityLink
                {
                    //PRD_PICKMTRL2RETURNMTRL
                    //T_PRD_PICKMTRLDATA
                    FEntity_Link_FRuleId = "PRD_PICK2RETURN_BACKFLUSH",
                    FEntity_Link_FSTableName = "T_PRD_PICKMTRLDATA",
                    FEntity_Link_FSBillId = entity["FEntity_Link_FSBillId"].ToString(),
                    FEntity_Link_FSId = entity["FEntity_Link_FSId"].ToString()
                };

                fEntity.FEntity_Link.Add(fEntityLink);
                //}

                rootObject.Model.FEntity.Add(fEntity);
            }

            string jsonString = JsonConvert.SerializeObject(rootObject, Formatting.Indented);
            Console.WriteLine(jsonString);

            return rootObject;
        }
    }


}