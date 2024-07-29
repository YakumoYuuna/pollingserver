using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace PollingServer
{
    public class SCLLD_Josn
    {
        public class Model
        {
            public FBillType FBillType { get; set; }
            public FStockOrgId FStockOrgId { get; set; }
            public FPrdOrgId FPrdOrgId { get; set; }
            public string FOwnerTypeId0 { get; set; }
            public string FWMSID { get; set; }
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
            public string FBaseStockActualQty { get; set; }
            public FMaterialId FMaterialId { get; set; }
            public FUnitID FUnitID { get; set; }
            public string FAppQty { get; set; }
            public string FActualQty { get; set; }
            public FStockId FStockId { get; set; }
            public FLot FLot { get; set; }
            public FStockStatusId FStockStatusId { get; set; }
            public string FProduceDate { get; set; }
            public string FMoBillNo { get; set; }
            public string FPPBOMENTRYID { get; set; }
            public string FMOENTRYID { get; set; }
            public string FOperId { get; set; }
            public string FOwnerTypeId { get; set; }
            public FStockUnitId FStockUnitId { get; set; }
            public string FStockAppQty { get; set; }
            public string FStockActualQty { get; set; }
            public string FMoId { get; set; }
            public string FMoEntrySeq { get; set; }
            public string FPPBomBillNo { get; set; }
            public FBaseUnitId FBaseUnitId { get; set; }
            public string FBaseAppQty { get; set; }
            public string FBaseActualQty { get; set; }
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

        public class FLot
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


        public static Root Return_SCLLD_Josn(string DataString)
        {

            // 反序列化为JArray
            JArray jsonArray = JArray.Parse(DataString);

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
                    FBillType = new FBillType { FNUMBER = "SCLLD01_SYS" },
                    FStockOrgId = new FStockOrgId { FNumber = keyValuePairsList[0]["FStockOrgId"].ToString() },
                    FPrdOrgId = new FPrdOrgId { FNumber = keyValuePairsList[0]["FPrdOrgId"].ToString() },
                    FOwnerTypeId0 = keyValuePairsList[0]["FOwnerTypeId"].ToString(),
                    FWMSID = keyValuePairsList[0]["FWMSID"].ToString(),
                    FEntity = new List<FEntity>()
                }
            };

            foreach (var dictionary in keyValuePairsList)
            {
                var fEntity = new FEntity
                {
                    FParentMaterialId = new FParentMaterialId { FNumber = dictionary["FParentMaterialId"].ToString() },
                    FBaseStockActualQty = dictionary["FBaseStockActualQty"].ToString(),
                    FMaterialId = new FMaterialId { FNumber = dictionary["FMaterialId"].ToString() },
                    FUnitID = new FUnitID { FNumber = dictionary["FUnitID"].ToString() },
                    FAppQty = dictionary["FAppQty"].ToString(),
                    FActualQty =dictionary["FActualQty"].ToString(),
                    FStockId = new FStockId { FNumber = dictionary["FStockId"].ToString() },
                    FLot = new FLot { FNumber = dictionary["FLot"].ToString() },
                    FStockStatusId = new FStockStatusId { FNumber = dictionary["FStockStatusId"].ToString() },
                    //FProduceDate = dictionary["FDate"].ToString(),
                    FMoBillNo = dictionary["FMoBillNo"].ToString(),
                    FPPBOMENTRYID = dictionary["FPPBomEntryId"].ToString(),
                    FMOENTRYID = dictionary["FMoEntryId"].ToString(),
                    FOperId = dictionary["FOperId"].ToString(),
                    FOwnerTypeId = dictionary["FOwnerTypeId"].ToString(),
                    FStockUnitId = new FStockUnitId { FNumber = dictionary["FStockUnitId"].ToString() },
                    FStockAppQty = dictionary["FStockAppQty"].ToString(),
                    FStockActualQty = dictionary["FStockActualQty"].ToString(),
                    FMoId = dictionary["FMoId"].ToString(),
                    FMoEntrySeq = dictionary["FMoEntrySeq"].ToString(),
                    FPPBomBillNo = dictionary["FPPBomBillNo"].ToString(),
                    FBaseUnitId = new FBaseUnitId { FNumber = dictionary["FBaseUnitId"].ToString() },
                    FBaseAppQty = dictionary["FBaseUnitId"].ToString(),
                    FBaseActualQty =dictionary["FBaseActualQty"].ToString(),
                    FOwnerId = new FOwnerId { FNumber = dictionary["FOwnerId"].ToString() },
                    FParentOwnerTypeId = dictionary["FParentOwnerTypeId"].ToString(),
                    FParentOwnerId = new FParentOwnerId { FNumber = dictionary["FParentOwnerId"].ToString() },
                    FEntity_Link = new List<FEntityLink>()
                };

                    var fEntityLink = new FEntityLink
                    {
                        FEntity_Link_FRuleId = "PRD_IssueMtrl2PickMtrl",
                        FEntity_Link_FSTableName = "T_PRD_PPBOMENTRY",
                        FEntity_Link_FSBillId = dictionary["FEntity_Link_FSBillId"].ToString(),
                        FEntity_Link_FSId = dictionary["FEntity_Link_FSId"].ToString()
                    };

                    fEntity.FEntity_Link.Add(fEntityLink);


                rootObject.Model.FEntity.Add(fEntity);
            }

            string jsonString = JsonConvert.SerializeObject(rootObject, Formatting.Indented);
        
            return rootObject;
        }

    }


    }
