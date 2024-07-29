using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PollingServer
{
    class QTRKD_Josn
    {
        public class FBillTypeID
        {
            public string FNUMBER { get; set; }
        }

        public class FStockOrgId
        {
            public string FNumber { get; set; }
        }

        public class FSUPPLIERID
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

        public class FSTOCKID
        {
            public string FNumber { get; set; }
        }

        public class FSTOCKSTATUSID
        {
            public string FNumber { get; set; }
        }

        public class FLOT
        {
            public string FNumber { get; set; }
        }

        public class FOWNERID
        {
            public string FNumber { get; set; }
        }

        public class FEntity
        {
            public FMATERIALID FMATERIALID { get; set; }
            public FUnitID FUnitID { get; set; }
            public FSTOCKID FSTOCKID { get; set; }
            public FSTOCKSTATUSID FSTOCKSTATUSID { get; set; }
            public FLOT FLOT { get; set; }
            public string FQty { get; set; }
            public string FOWNERTYPEID { get; set; }
            public FOWNERID FOWNERID { get; set; }
        }

        public class Model
        {
            public FBillTypeID FBillTypeID { get; set; }
            public FStockOrgId FStockOrgId { get; set; }
            public string FStockDirect { get; set; }
            public string FDate { get; set; }
            public FSUPPLIERID FSUPPLIERID { get; set; }
            public string FOwnerTypeIdHead { get; set; }
            public FOwnerIdHead FOwnerIdHead { get; set; }
            public List<FEntity> FEntity { get; set; }
        }

        public class Root
        {
            public Model Model { get; set; }
        }

        public static Root Return_QTRKD_Json(string dataString)
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
                    FBillTypeID = new FBillTypeID { FNUMBER = "QTRKD01_SYS" },
                    FStockOrgId = new FStockOrgId { FNumber = keyValuePairsList[0]["FStockOrgId"].ToString() },
                    FStockDirect = "GENERAL",
                    FDate = keyValuePairsList[0]["FDate"].ToString(),
                    FSUPPLIERID = new FSUPPLIERID { FNumber = keyValuePairsList[0]["FSUPPLIERID"].ToString() },
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
                    FMATERIALID = new FMATERIALID { FNumber = entity["FMATERIALID"].ToString() },
                    FUnitID = new FUnitID { FNumber = entity["FUnitID"].ToString() },
                    FSTOCKID = new FSTOCKID { FNumber = entity["FSTOCKID"].ToString() },
                    FSTOCKSTATUSID = new FSTOCKSTATUSID { FNumber = entity["FSTOCKSTATUSID"].ToString() },
                    FLOT = new FLOT { FNumber = entity["FLot"].ToString() },
                    FQty = entity["FQTY"].ToString(),
                    FOWNERTYPEID = entity["FOWNERTYPEID"].ToString(),
                    FOWNERID = new FOWNERID { FNumber = entity["FOWNERID"].ToString() }
                };

                rootObject.Model.FEntity.Add(fEntity);
            }

            return rootObject;
        }

    }
}
