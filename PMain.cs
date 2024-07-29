using Kingdee.BOS.WebApi.Client;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static PollingServer.ClassApi_CGTLD;
using static PollingServer.ClassApi_QTCKD;

namespace PollingServer
{
    public partial class PMain : Form
    {
        public PMain()
        {
            InitializeComponent();

        }

        /// <summary>
        /// 配置信息
        /// </summary>
        public string ValidateLogin_id = ConfigurationManager.AppSettings["ValidateLogin_id"];
        public string ValidateLogin_name = ConfigurationManager.AppSettings["ValidateLogin_name"];
        public string ValidateLogin_pwd = ConfigurationManager.AppSettings["ValidateLogin_pwd"];
        public string SCLLD_RuleId = ConfigurationManager.AppSettings["SCLLD_RuleId"];
        public string SCBLD_RuleId = ConfigurationManager.AppSettings["SCBLD_RuleId"];
        public string SCTLD_RuleId = ConfigurationManager.AppSettings["SCTLD_RuleId"];
        public string K3CloudApi = ConfigurationManager.AppSettings["K3CloudApi"];


        private void PMain_Load(object sender, EventArgs e)
        {
            //string connStr1 = ConfigurationManager.AppSettings["test"];
            //string connStr = ConfigurationManager.ConnectionStrings["connecttest"].ConnectionString;

        }


        /// <summary>
        /// 生产退料单
        /// </summary>
        private static void SCTLD()
        {
            DataTable dt = new DataTable();
            dt = Class_Data.Data_List("MES", "PRD_FeedMtrl", false);
            string fid = "";   //下推出的生产退料单内码
            for (int i = 0; i <= dt.Rows.Count - 1; i++)
            {
                //为空直接跳出本次循环
                if (Convert.ToString(dt.Rows[i]["DATA"]) == "")
                { continue; }
                // 添加引号
                string resultData = SQLHelper.AddQuotes(Convert.ToString(dt.Rows[i]["DATA"]));
                List<List<object>> dataList = JsonConvert.DeserializeObject<List<List<object>>>(resultData);

                // 转换为 List<string[]>
                List<string[]> stringList = new List<string[]>();

                foreach (var item in dataList)
                {
                    string[] stringArray = item.ConvertAll(x => x.ToString()).ToArray();
                    stringList.Add(stringArray);
                }

                if (fid == "0") { continue; }
                else
                {
                    //SCTLD_XT(dt, i, stringList, ref fid);

                    //SCTLD_Update(dt, i, stringList, fid);

                    //SCTLD_Submit(dt, i, stringList, fid);

                    //SCTLD_Audit(dt, i, stringList, fid);
                }
            }
        }



        /// <summary>
        /// 生产退料单更新数据
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="i"></param>
        /// <param name="stringList"></param>
        /// <param name="fid"></param>
        private static void SCTLD_Update(DataTable dt, int i, List<string[]> stringList, string fid)
        {
            // 创建 RootObject 实例
            ClassApi_SCTLD.RootObject rootObject_SCTLD = new ClassApi_SCTLD.RootObject();

            // 赋值 NeedUpDateFields 和 IsDeleteEntry
            rootObject_SCTLD.NeedUpDateFields = new List<string> { "FEntity", "FAPPQty", "FQty" };
            rootObject_SCTLD.IsDeleteEntry = "false";

            // 赋值 Model
            rootObject_SCTLD.Model = new ClassApi_SCTLD.Model
            {
                FID = fid,
                FEntity = new List<ClassApi_SCTLD.FEntityItem>()
            };

            // 循环赋值 FEntity
            foreach (var list in stringList)
            {
                ClassApi_SCTLD.FEntityItem entityItem = new ClassApi_SCTLD.FEntityItem
                {
                    FEntryID = Class_Data.T_PRD_RETURNMTRLENTRY_FENTRYID(fid, list[2]),  //单体据内码
                    FAPPQty = list[3],  //申请数量
                    FQty = list[4],  //实退数量
                };

                rootObject_SCTLD.Model.FEntity.Add(entityItem);
            }
            // 将 Root 对象序列化为 JSON 字符串
            string jsonString = JsonConvert.SerializeObject(rootObject_SCTLD, Formatting.Indented);

            // 使用webapi引用组件Kingdee.BOS.WebApi.Client.dll
            K3CloudApiClient client = new K3CloudApiClient(ConfigurationManager.AppSettings["K3CloudApi"]);
            var loginResult = client.ValidateLogin(ConfigurationManager.AppSettings["ValidateLogin_id"], ConfigurationManager.AppSettings["ValidateLogin_name"], ConfigurationManager.AppSettings["ValidateLogin_pwd"], 2052);
            var resultType = JObject.Parse(loginResult)["LoginResultType"].Value<int>();
            //string webapijson = "";
            //登录结果类型等于1，代表登录成功
            if (resultType == 1)
            {
                var kdwebapijson = client.Save("PRD_ReturnMtrl", JsonConvert.SerializeObject(rootObject_SCTLD, Formatting.Indented));
                SQLHelper.Txt(kdwebapijson, "CC");
                //IsSuccess(dt, i, kdwebapijson);
            }
        }


        /// <summary>
        /// 生产领料单下推生产退料单
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="i"></param>
        /// <param name="stringList"></param>
        /// <param name="fid"></param>
        private static void SCTLD_XT(DataTable dt, int i, List<string[]> stringList, ref string fid)
        {
            // 创建 RootObject 实例
            ClassApi_Push.RootObject rootObject = new ClassApi_Push.RootObject();

            // 赋值 FBillTypeID
            rootObject = new ClassApi_Push.RootObject
            {
                Ids = Class_Data.T_PRD_PICKMTRL_FID(stringList[0][1]),  //生产领料单内码
                RuleId = ConfigurationManager.AppSettings["SCTLD_RuleId"]  //转换规则
            };

            // 将 Root 对象序列化为 JSON 字符串
            string jsonString = JsonConvert.SerializeObject(rootObject, Formatting.Indented);

            // 使用webapi引用组件Kingdee.BOS.WebApi.Client.dll
            K3CloudApiClient client = new K3CloudApiClient(ConfigurationManager.AppSettings["K3CloudApi"]);
            var loginResult = client.ValidateLogin(ConfigurationManager.AppSettings["ValidateLogin_id"], ConfigurationManager.AppSettings["ValidateLogin_name"], ConfigurationManager.AppSettings["ValidateLogin_pwd"], 2052);
            var resultType = JObject.Parse(loginResult)["LoginResultType"].Value<int>();
            //string webapijson = "";
            //登录结果类型等于1，代表登录成功
            if (resultType == 1)
            {
                var kdwebapijson = client.Push("PRD_PickMtrl", JsonConvert.SerializeObject(rootObject, Formatting.Indented));
                SQLHelper.Txt(kdwebapijson, "CC");
                IsSuccess_ref(dt, i, kdwebapijson, ref fid);
            }
        }



        /// <summary>
        /// 生产补料单
        /// </summary>
        private static void SCBLD()
        {
            DataTable dt = new DataTable();
            dt = Class_Data.Data_List("MES", "PRD_FeedMtrl", false);
            for (int i = 0; i <= dt.Rows.Count - 1; i++)
            {
                //为空直接跳出本次循环
                if (Convert.ToString(dt.Rows[i]["DATA"]) == "")
                { continue; }
                // 添加引号
                string resultData = SQLHelper.AddQuotes(Convert.ToString(dt.Rows[i]["DATA"]));
                List<List<object>> dataList = JsonConvert.DeserializeObject<List<List<object>>>(resultData);

                // 转换为 List<string[]>
                List<string[]> stringList = new List<string[]>();

                foreach (var item in dataList)
                {
                    string[] stringArray = item.ConvertAll(x => x.ToString()).ToArray();
                    stringList.Add(stringArray);
                }

                // 创建 RootObject 实例
                ClassApi_SCLLD.RootObject rootObject = new ClassApi_SCLLD.RootObject();

                // 赋值 NeedUpDateFields 和 IsDeleteEntry
                rootObject.NeedUpDateFields = new List<string> { "FEntity", "FLot", "FStockID" };
                rootObject.IsDeleteEntry = "false";
                // 赋值 Model
                rootObject.Model = new ClassApi_SCLLD.Model
                {
                    FID = Class_Data.T_PRD_PPBOMENTRY_FID(stringList[0][1], stringList[0][2]),  //生产用料清单内码
                    FEntity = new List<ClassApi_SCLLD.FEntityItem>()
                };
                // 循环赋值 FEntity
                foreach (var list in stringList)
                {
                    ClassApi_SCLLD.FEntityItem entityItem = new ClassApi_SCLLD.FEntityItem
                    {
                        FEntryID = Class_Data.T_PRD_PPBOMENTRY_FENTRYID(stringList[0][1], list[2]),  //单据体行内码
                        FLot = new ClassApi_SCLLD.FLot
                        {
                            FNumber = list[5]  //批号
                        },
                        FStockID = new ClassApi_SCLLD.FStockID
                        {
                            FNumber = list[6]  //仓库
                        }
                    };

                    rootObject.Model.FEntity.Add(entityItem);
                }

                // 将 Root 对象序列化为 JSON 字符串
                string jsonString = JsonConvert.SerializeObject(rootObject, Formatting.Indented);

                // 使用webapi引用组件Kingdee.BOS.WebApi.Client.dll
                K3CloudApiClient client = new K3CloudApiClient(ConfigurationManager.AppSettings["K3CloudApi"]);
                var loginResult = client.ValidateLogin(ConfigurationManager.AppSettings["ValidateLogin_id"], ConfigurationManager.AppSettings["ValidateLogin_name"], ConfigurationManager.AppSettings["ValidateLogin_pwd"], 2052);
                var resultType = JObject.Parse(loginResult)["LoginResultType"].Value<int>();
                string fid = "";   //下推出的生产领料单内码
                //string webapijson = "";
                //登录结果类型等于1，代表登录成功
                if (resultType == 1)
                {
                    var kdwebapijson = client.Save("PRD_PPBOM", JsonConvert.SerializeObject(rootObject, Formatting.Indented));
                    SQLHelper.Txt(kdwebapijson, "CC");

                    IsSuccess(dt, i, kdwebapijson);
                }
                if (fid == "0") { continue; }
                else
                {

                    //SCBLD_XT(dt, i, stringList, ref fid);

                    //SCBLD_Update(dt, i, stringList, fid);

                    //SCBLD_Submit(dt, i, stringList, fid);

                    //SCBLD_Audit(dt, i, stringList, fid);
                }
            }
        }

        /// <summary>
        /// 生产补料单更新数据
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="i"></param>
        /// <param name="stringList"></param>
        /// <param name="fid"></param>
        private static void SCBLD_Update(DataTable dt, int i, List<string[]> stringList, string fid)
        {
            // 创建 RootObject 实例
            ClassApi_SCBLD.RootObject rootObject_SCBLD = new ClassApi_SCBLD.RootObject();

            // 赋值 NeedUpDateFields 和 IsDeleteEntry
            rootObject_SCBLD.NeedUpDateFields = new List<string> { "FEntity", "FAppQty", "FActualQty" };
            rootObject_SCBLD.IsDeleteEntry = "false";

            // 赋值 Model
            rootObject_SCBLD.Model = new ClassApi_SCBLD.Model
            {
                FID = fid,
                FEntity = new List<ClassApi_SCBLD.FEntityItem>()
            };

            // 循环赋值 FEntity
            foreach (var list in stringList)
            {
                ClassApi_SCBLD.FEntityItem entityItem = new ClassApi_SCBLD.FEntityItem
                {
                    FEntryID = Class_Data.T_PRD_FEEDMTRLDATA_FENTRYID(fid, list[2]),  //单体据内码
                    FAppQty = list[3],  //申请数量
                    FActualQty = list[4],  //实发数量
                };

                rootObject_SCBLD.Model.FEntity.Add(entityItem);
            }
            // 将 Root 对象序列化为 JSON 字符串
            string jsonString = JsonConvert.SerializeObject(rootObject_SCBLD, Formatting.Indented);

            // 使用webapi引用组件Kingdee.BOS.WebApi.Client.dll
            K3CloudApiClient client = new K3CloudApiClient(ConfigurationManager.AppSettings["K3CloudApi"]);
            var loginResult = client.ValidateLogin(ConfigurationManager.AppSettings["ValidateLogin_id"], ConfigurationManager.AppSettings["ValidateLogin_name"], ConfigurationManager.AppSettings["ValidateLogin_pwd"], 2052);
            var resultType = JObject.Parse(loginResult)["LoginResultType"].Value<int>();
            //string webapijson = "";
            //登录结果类型等于1，代表登录成功
            if (resultType == 1)
            {
                var kdwebapijson = client.Save("PRD_FeedMtrl", JsonConvert.SerializeObject(rootObject_SCBLD, Formatting.Indented));
                SQLHelper.Txt(kdwebapijson, "CC");
                //IsSuccess(dt, i, kdwebapijson);
            }
        }




        /// <summary>
        /// 生产补料单提交操作
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="i"></param>
        /// <param name="stringList"></param>
        /// <param name="fid"></param>
        private static void SCBLD_Submit(DataTable dt, int i, List<string[]> stringList, string fid)
        {
            // 创建 RootObject 实例
            ClassApi_Submit.RootObject rootObject_Submit = new ClassApi_Submit.RootObject();
            // 赋值 FBillTypeID
            rootObject_Submit = new ClassApi_Submit.RootObject
            {
                Ids = fid,  //生产补料单内码
            };

            // 将 Root 对象序列化为 JSON 字符串
            string jsonString = JsonConvert.SerializeObject(rootObject_Submit, Formatting.Indented);

            // 使用webapi引用组件Kingdee.BOS.WebApi.Client.dll
            K3CloudApiClient client = new K3CloudApiClient(ConfigurationManager.AppSettings["K3CloudApi"]);
            var loginResult = client.ValidateLogin(ConfigurationManager.AppSettings["ValidateLogin_id"], ConfigurationManager.AppSettings["ValidateLogin_name"], ConfigurationManager.AppSettings["ValidateLogin_pwd"], 2052);
            var resultType = JObject.Parse(loginResult)["LoginResultType"].Value<int>();
            //string webapijson = "";
            //登录结果类型等于1，代表登录成功
            if (resultType == 1)
            {
                var kdwebapijson = client.Submit("PRD_FeedMtrl", JsonConvert.SerializeObject(rootObject_Submit, Formatting.Indented));
                SQLHelper.Txt(kdwebapijson, "CC");
                //IsSuccess_ref(dt, i, kdwebapijson, ref fid);
            }
        }

        /// <summary>
        /// 生产补料单审核操作
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="i"></param>
        /// <param name="stringList"></param>
        /// <param name="fid"></param>
        private static void SCBLD_Audit(DataTable dt, int i, List<string[]> stringList, string fid)
        {
            // 创建 RootObject 实例
            ClassApi_Audit.RootObject rootObject_Audit = new ClassApi_Audit.RootObject();
            // 赋值 FBillTypeID
            rootObject_Audit = new ClassApi_Audit.RootObject
            {
                Ids = fid,  //生产补料单内码
            };

            // 将 Root 对象序列化为 JSON 字符串
            string jsonString = JsonConvert.SerializeObject(rootObject_Audit, Formatting.Indented);

            // 使用webapi引用组件Kingdee.BOS.WebApi.Client.dll
            K3CloudApiClient client = new K3CloudApiClient(ConfigurationManager.AppSettings["K3CloudApi"]);
            var loginResult = client.ValidateLogin(ConfigurationManager.AppSettings["ValidateLogin_id"], ConfigurationManager.AppSettings["ValidateLogin_name"], ConfigurationManager.AppSettings["ValidateLogin_pwd"], 2052);
            var resultType = JObject.Parse(loginResult)["LoginResultType"].Value<int>();
            //string webapijson = "";
            //登录结果类型等于1，代表登录成功
            if (resultType == 1)
            {
                var kdwebapijson = client.Audit("PRD_FeedMtrl", JsonConvert.SerializeObject(rootObject_Audit, Formatting.Indented));
                SQLHelper.Txt(kdwebapijson, "CC");
                IsSuccess(dt, i, kdwebapijson);
            }
        }


        /// <summary>
        /// 下推生产补料单
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="i"></param>
        /// <param name="stringList"></param>
        private static void SCBLD_XT(DataTable dt, int i, List<string[]> stringList, ref string fid)
        {
            // 创建 RootObject 实例
            ClassApi_Push.RootObject rootObject = new ClassApi_Push.RootObject();

            // 赋值 FBillTypeID
            rootObject = new ClassApi_Push.RootObject
            {
                Ids = Class_Data.T_PRD_PPBOMENTRY_FID(stringList[0][1], stringList[0][2]),  //生产用料清单内码
                RuleId = ConfigurationManager.AppSettings["SCBLD_RuleId"]  //转换规则
            };

            // 将 Root 对象序列化为 JSON 字符串
            string jsonString = JsonConvert.SerializeObject(rootObject, Formatting.Indented);

            // 使用webapi引用组件Kingdee.BOS.WebApi.Client.dll
            K3CloudApiClient client = new K3CloudApiClient(ConfigurationManager.AppSettings["K3CloudApi"]);
            var loginResult = client.ValidateLogin(ConfigurationManager.AppSettings["ValidateLogin_id"], ConfigurationManager.AppSettings["ValidateLogin_name"], ConfigurationManager.AppSettings["ValidateLogin_pwd"], 2052);
            var resultType = JObject.Parse(loginResult)["LoginResultType"].Value<int>();
            //string webapijson = "";
            //登录结果类型等于1，代表登录成功
            if (resultType == 1)
            {
                var kdwebapijson = client.Push("PRD_PPBOM", JsonConvert.SerializeObject(rootObject, Formatting.Indented));
                SQLHelper.Txt(kdwebapijson, "CC");
                IsSuccess_ref(dt, i, kdwebapijson, ref fid);
            }
        }


        /// <summary>
        /// 生产领料单
        /// </summary>
        private static void SCLLD()
        {
            DataTable dt = new DataTable();
            dt = Class_Data.Data_List("MES", "PRD_PickMtrl", false);
            for (int i = 0; i <= dt.Rows.Count - 1; i++)
            {
                //为空直接跳出本次循环
                if (Convert.ToString(dt.Rows[i]["DATA"]) == "")
                { continue; }
                // 添加引号
                string resultData = SQLHelper.AddQuotes(Convert.ToString(dt.Rows[i]["DATA"]));
                List<List<object>> dataList = JsonConvert.DeserializeObject<List<List<object>>>(resultData);

                // 转换为 List<string[]>
                List<string[]> stringList = new List<string[]>();

                foreach (var item in dataList)
                {
                    string[] stringArray = item.ConvertAll(x => x.ToString()).ToArray();
                    stringList.Add(stringArray);
                }

                // 创建 RootObject 实例
                ClassApi_SCLLD.RootObject rootObject = new ClassApi_SCLLD.RootObject();

                // 赋值 NeedUpDateFields 和 IsDeleteEntry
                rootObject.NeedUpDateFields = new List<string> { "FEntity", "FLot", "FStockID" };
                rootObject.IsDeleteEntry = "false";
                // 赋值 Model
                rootObject.Model = new ClassApi_SCLLD.Model
                {
                    FID = Class_Data.T_PRD_PPBOMENTRY_FID(stringList[0][1], stringList[0][2]),  //生产用料清单内码
                    FEntity = new List<ClassApi_SCLLD.FEntityItem>()
                };
                // 循环赋值 FEntity
                foreach (var list in stringList)
                {
                    ClassApi_SCLLD.FEntityItem entityItem = new ClassApi_SCLLD.FEntityItem
                    {
                        FEntryID = Class_Data.T_PRD_PPBOMENTRY_FENTRYID(stringList[0][1], list[2]),  //单据体行内码
                        FLot = new ClassApi_SCLLD.FLot
                        {
                            FNumber = list[5]  //批号
                        },
                        FStockID = new ClassApi_SCLLD.FStockID
                        {
                            FNumber = list[6]  //仓库
                        }
                    };

                    rootObject.Model.FEntity.Add(entityItem);
                }

                // 将 Root 对象序列化为 JSON 字符串
                string jsonString = JsonConvert.SerializeObject(rootObject, Formatting.Indented);

                // 使用webapi引用组件Kingdee.BOS.WebApi.Client.dll
                K3CloudApiClient client = new K3CloudApiClient(ConfigurationManager.AppSettings["K3CloudApi"]);
                var loginResult = client.ValidateLogin(ConfigurationManager.AppSettings["ValidateLogin_id"], ConfigurationManager.AppSettings["ValidateLogin_name"], ConfigurationManager.AppSettings["ValidateLogin_pwd"], 2052);
                var resultType = JObject.Parse(loginResult)["LoginResultType"].Value<int>();
                string fid = "";   //下推出的生产领料单内码
                //string webapijson = "";
                //登录结果类型等于1，代表登录成功
                if (resultType == 1)
                {
                    var kdwebapijson = client.Save("PRD_PPBOM", JsonConvert.SerializeObject(rootObject, Formatting.Indented));
                    SQLHelper.Txt(kdwebapijson, "CC");
                    IsSuccess(dt, i, kdwebapijson);
                }
                if (fid == "0") { continue; }
                else
                {
                    //SCLLD_XT(dt, i, stringList, ref fid);

                    //SCLLD_Submit(dt, i, stringList, fid);

                    //SCLLD_Audit(dt, i, stringList, fid);
                }
            }
        }

        /// <summary>
        /// 下推生产领料单
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="i"></param>
        /// <param name="stringList"></param>
        private static void SCLLD_XT(DataTable dt, int i, List<string[]> stringList, ref string fid)
        {
            // 创建 RootObject 实例
            ClassApi_Push.RootObject rootObject = new ClassApi_Push.RootObject();

            // 赋值 FBillTypeID
            rootObject = new ClassApi_Push.RootObject
            {
                Ids = Class_Data.T_PRD_PPBOMENTRY_FID(stringList[0][1], stringList[0][2]),  //生产用料清单内码
                RuleId = ConfigurationManager.AppSettings["SCLLD_RuleId"]  //转换规则
            };

            // 将 Root 对象序列化为 JSON 字符串
            string jsonString = JsonConvert.SerializeObject(rootObject, Formatting.Indented);

            // 使用webapi引用组件Kingdee.BOS.WebApi.Client.dll
            K3CloudApiClient client = new K3CloudApiClient(ConfigurationManager.AppSettings["K3CloudApi"]);
            var loginResult = client.ValidateLogin(ConfigurationManager.AppSettings["ValidateLogin_id"], ConfigurationManager.AppSettings["ValidateLogin_name"], ConfigurationManager.AppSettings["ValidateLogin_pwd"], 2052);
            var resultType = JObject.Parse(loginResult)["LoginResultType"].Value<int>();
            //string webapijson = "";
            //登录结果类型等于1，代表登录成功
            if (resultType == 1)
            {
                var kdwebapijson = client.Push("PRD_PPBOM", JsonConvert.SerializeObject(rootObject, Formatting.Indented));
                SQLHelper.Txt(kdwebapijson, "CC");
                IsSuccess_ref(dt, i, kdwebapijson, ref fid);
            }
        }




        /// <summary>
        /// 其他入库单
        /// </summary>
        private static void QTRKD()
        {
            DataTable dt = new DataTable();
            dt = Class_Data.Data_List("MES", "STK_MISCELLANEOUS", false);
            for (int i = 0; i <= dt.Rows.Count - 1; i++)
            {
                //为空直接跳出本次循环
                if (Convert.ToString(dt.Rows[i]["DATA"]) == "")
                { continue; }
                // 添加引号
                string resultData = SQLHelper.AddQuotes(Convert.ToString(dt.Rows[i]["DATA"]));
                List<List<object>> dataList = JsonConvert.DeserializeObject<List<List<object>>>(resultData);

                // 转换为 List<string[]>
                List<string[]> stringList = new List<string[]>();

                foreach (var item in dataList)
                {
                    string[] stringArray = item.ConvertAll(x => x.ToString()).ToArray();
                    stringList.Add(stringArray);
                }

                // 创建 RootObject 实例
                ClassApi_QTRKD.RootObject rootObject = new ClassApi_QTRKD.RootObject();

                // 赋值 FBillTypeID
                rootObject.Model = new ClassApi_QTRKD.Model
                {
                    FBillTypeID = new ClassApi_QTRKD.FBillTypeID
                    {
                        FNUMBER = stringList[0][1]  //单据类型
                    },
                    FStockOrgId = new ClassApi_QTRKD.FStockOrgId
                    {
                        FNumber = stringList[0][2]  //库存组织
                    },
                    FStockDirect = stringList[0][3],  //库存方向
                    FDate = stringList[0][4],  //日期
                    FSUPPLIERID = new ClassApi_QTRKD.FSUPPLIERID
                    {
                        FNumber = stringList[0][5]  //供应商
                    },
                    FDEPTID = new ClassApi_QTRKD.FDEPTID
                    {
                        FNumber = stringList[0][6]  //部门
                    },
                    FOwnerTypeIdHead = stringList[0][7],  //货主类型
                    FOwnerIdHead = new ClassApi_QTRKD.FOwnerIdHead
                    {
                        FNumber = stringList[0][8]  //货主
                    },
                    FEntity = new List<ClassApi_QTRKD.FEntity>()
                };


                // 循环赋值 FEntity
                foreach (var list in stringList)
                {
                    ClassApi_QTRKD.FEntity entry = new ClassApi_QTRKD.FEntity
                    {
                        FMATERIALID = new ClassApi_QTRKD.FMATERIALID
                        {
                            FNumber = list[9]  //物料编码
                        },
                        FUnitID = new ClassApi_QTRKD.FUnitID
                        {
                            FNumber = list[10]  //单位
                        },
                        FSTOCKID = new ClassApi_QTRKD.FSTOCKID
                        {
                            FNumber = list[11]  //收货仓库
                        },
                        FSTOCKSTATUSID = new ClassApi_QTRKD.FSTOCKSTATUSID
                        {
                            FNumber = list[12]  //库存状态
                        },
                        FQty = Convert.ToDouble(list[13])  //实收数量
                    };

                    rootObject.Model.FEntity.Add(entry);
                }

                // 将 Root 对象序列化为 JSON 字符串
                string jsonString = JsonConvert.SerializeObject(rootObject, Formatting.Indented);

                // 使用webapi引用组件Kingdee.BOS.WebApi.Client.dll
                K3CloudApiClient client = new K3CloudApiClient(ConfigurationManager.AppSettings["K3CloudApi"]);
                var loginResult = client.ValidateLogin(ConfigurationManager.AppSettings["ValidateLogin_id"], ConfigurationManager.AppSettings["ValidateLogin_name"], ConfigurationManager.AppSettings["ValidateLogin_pwd"], 2052);
                var resultType = JObject.Parse(loginResult)["LoginResultType"].Value<int>();
                string fid = "";   //新增的其他入库单内码
                //string webapijson = "";
                //登录结果类型等于1，代表登录成功
                if (resultType == 1)
                {
                    var kdwebapijson = client.Save("STK_MISCELLANEOUS", JsonConvert.SerializeObject(rootObject, Formatting.Indented));
                    SQLHelper.Txt(kdwebapijson, "CC");
                    IsSuccess_ref(dt, i, kdwebapijson, ref fid);
                }
                if (fid == "0") { continue; }
                else
                {
                    //    QTRKD_Submit(dt, i, stringList, fid);

                    //    QTRKD_Audit(dt, i, stringList, fid);
                }
            }
        }

        /// <summary>
        /// 其他入库单提交操作
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="i"></param>
        /// <param name="stringList"></param>
        /// <param name="fid"></param>
        private static void QTRKD_Submit(DataTable dt, int i, List<string[]> stringList, string fid)
        {
            // 创建 RootObject 实例
            ClassApi_Submit.RootObject rootObject_Submit = new ClassApi_Submit.RootObject();
            // 赋值 FBillTypeID
            rootObject_Submit = new ClassApi_Submit.RootObject
            {
                Ids = fid,  //生产领料单内码
            };

            // 将 Root 对象序列化为 JSON 字符串
            string jsonString = JsonConvert.SerializeObject(rootObject_Submit, Formatting.Indented);

            // 使用webapi引用组件Kingdee.BOS.WebApi.Client.dll
            K3CloudApiClient client = new K3CloudApiClient(ConfigurationManager.AppSettings["K3CloudApi"]);
            var loginResult = client.ValidateLogin(ConfigurationManager.AppSettings["ValidateLogin_id"], ConfigurationManager.AppSettings["ValidateLogin_name"], ConfigurationManager.AppSettings["ValidateLogin_pwd"], 2052);
            var resultType = JObject.Parse(loginResult)["LoginResultType"].Value<int>();
            //string webapijson = "";
            //登录结果类型等于1，代表登录成功
            if (resultType == 1)
            {
                var kdwebapijson = client.Submit("STK_MISCELLANEOUS", JsonConvert.SerializeObject(rootObject_Submit, Formatting.Indented));
                SQLHelper.Txt(kdwebapijson, "CC");
                //IsSuccess_ref(dt, i, kdwebapijson, ref fid);
            }
        }

        /// <summary>
        /// 其他入库单审核操作
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="i"></param>
        /// <param name="stringList"></param>
        /// <param name="fid"></param>
        private static void QTRKD_Audit(DataTable dt, int i, List<string[]> stringList, string fid)
        {
            // 创建 RootObject 实例
            ClassApi_Audit.RootObject rootObject_Audit = new ClassApi_Audit.RootObject();
            // 赋值 FBillTypeID
            rootObject_Audit = new ClassApi_Audit.RootObject
            {
                Ids = fid,  //生产领料单内码
            };

            // 将 Root 对象序列化为 JSON 字符串
            string jsonString = JsonConvert.SerializeObject(rootObject_Audit, Formatting.Indented);

            // 使用webapi引用组件Kingdee.BOS.WebApi.Client.dll
            K3CloudApiClient client = new K3CloudApiClient(ConfigurationManager.AppSettings["K3CloudApi"]);
            var loginResult = client.ValidateLogin(ConfigurationManager.AppSettings["ValidateLogin_id"], ConfigurationManager.AppSettings["ValidateLogin_name"], ConfigurationManager.AppSettings["ValidateLogin_pwd"], 2052);
            var resultType = JObject.Parse(loginResult)["LoginResultType"].Value<int>();
            //string webapijson = "";
            //登录结果类型等于1，代表登录成功
            if (resultType == 1)
            {
                var kdwebapijson = client.Audit("STK_MISCELLANEOUS", JsonConvert.SerializeObject(rootObject_Audit, Formatting.Indented));
                SQLHelper.Txt(kdwebapijson, "CC");
                IsSuccess(dt, i, kdwebapijson);
            }
        }


        /// <summary>
        /// 判断api是否成功
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="i"></param>
        /// <param name="kdwebapijson"></param>
        private static void IsSuccess(DataTable dt, int i, string kdwebapijson)
        {
            // 使用 JObject 解析 JSON 字符串
            JObject jsonObject = JObject.Parse(kdwebapijson);

            // 访问 IsSuccess 属性并检查其值
            bool isSuccess = jsonObject["Result"]?["ResponseStatus"]?["IsSuccess"]?.Value<bool>() ?? false;

            if (isSuccess)
            {
                //UpdData(Convert.ToString(dt.Rows[i]["FBILLNO"]), "KingDee");
            }
            else
            {

            }
        }

        /// <summary>
        /// 判断api是否成功并回传值
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="i"></param>
        /// <param name="kdwebapijson"></param>
        private static void IsSuccess_ref(DataTable dt, int i, string kdwebapijson, ref string fid)
        {
            // 使用 JObject 解析 JSON 字符串
            JObject jsonObject = JObject.Parse(kdwebapijson);

            // 访问 IsSuccess 属性并检查其值
            bool isSuccess = jsonObject["Result"]?["ResponseStatus"]?["IsSuccess"]?.Value<bool>() ?? false;

            if (isSuccess)
            {
                UpdData(Convert.ToString(dt.Rows[i]["FBILLNO"]), "KingDee");
                // 获取 Result 下的 ResponseStatus 下的 id 的值
                fid = (string)jsonObject["Result"]["ResponseStatus"]["SuccessEntitys"][0]["Id"];
            }
            else
            {
                fid = "0";
            }
        }

        /// <summary>
        /// 其他出库单
        /// </summary>
        private static void QTCKD()
        {
            DataTable dt = new DataTable();
            dt = Class_Data.Data_List("MES", "STK_MisDelivery", false);

            for (int i = 0; i <= dt.Rows.Count - 1; i++)
            {
                //为空直接跳出本次循环
                if (Convert.ToString(dt.Rows[i]["DATA"]) == "")
                { continue; }
                // 添加引号
                string resultData = SQLHelper.AddQuotes(Convert.ToString(dt.Rows[i]["DATA"]));
                List<List<object>> dataList = JsonConvert.DeserializeObject<List<List<object>>>(resultData);

                // 转换为 List<string[]>
                List<string[]> stringList = new List<string[]>();

                foreach (var item in dataList)
                {
                    string[] stringArray = item.ConvertAll(x => x.ToString()).ToArray();
                    stringList.Add(stringArray);
                }

                // 创建 RootObject 实例
                ClassApi_QTCKD.RootObject rootObject = new ClassApi_QTCKD.RootObject();

                // 赋值 FBillTypeID
                rootObject.Model = new ClassApi_QTCKD.Model
                {
                    FBillTypeID = new ClassApi_QTCKD.FBillTypeID
                    {
                        FNUMBER = stringList[0][1]  //单据类型
                    },
                    FStockOrgId = new ClassApi_QTCKD.FStockOrgId
                    {
                        FNumber = stringList[0][2]  //库存组织
                    },
                    FStockDirect = stringList[0][3],  //库存方向
                    FDate = stringList[0][4],  //日期
                    FCustId = new FCustId
                    {
                        FNumber = stringList[0][5]  //客户
                    },
                    FDeptId = new FDeptId
                    {
                        FNumber = stringList[0][6]  //领料部门
                    },
                    FOwnerTypeIdHead = stringList[0][7],  //货主类型
                    FOwnerIdHead = new FOwnerIdHead
                    {
                        FNumber = stringList[0][8]  //货主
                    },
                    FEntity = new List<FEntity>()
                };

                // 循环赋值 FEntity
                foreach (var list in stringList)
                {
                    FEntity entry = new FEntity
                    {
                        FMaterialId = new FMaterialId
                        {
                            FNumber = list[9]  //物料编码
                        },
                        FUnitID = new FUnitID
                        {
                            FNumber = list[10]  //单位
                        },
                        FQty = Convert.ToDouble(list[11]),  //实发数量
                        FStockId = new FStockId
                        {
                            FNumber = list[12]  //发货仓库
                        },
                        FStockStatusId = new FStockStatusId
                        {
                            FNumber = list[13]  //库存状态
                        }
                    };

                    rootObject.Model.FEntity.Add(entry);
                }

                // 将 Root 对象序列化为 JSON 字符串
                string jsonString = JsonConvert.SerializeObject(rootObject, Formatting.Indented);

                // 使用webapi引用组件Kingdee.BOS.WebApi.Client.dll
                K3CloudApiClient client = new K3CloudApiClient(ConfigurationManager.AppSettings["K3CloudApi"]);
                var loginResult = client.ValidateLogin(ConfigurationManager.AppSettings["ValidateLogin_id"], ConfigurationManager.AppSettings["ValidateLogin_name"], ConfigurationManager.AppSettings["ValidateLogin_pwd"], 2052);
                var resultType = JObject.Parse(loginResult)["LoginResultType"].Value<int>();
                //string webapijson = "";
                string fid = "";   //新增的其他入库单内码
                //登录结果类型等于1，代表登录成功
                if (resultType == 1)
                {
                    var kdwebapijson = client.Save("STK_MisDelivery", JsonConvert.SerializeObject(rootObject, Formatting.Indented));
                    SQLHelper.Txt(kdwebapijson, "CC");
                    IsSuccess_ref(dt, i, kdwebapijson, ref fid);
                }
                if (fid == "0") { continue; }
                else
                {
                    //QTCKD_Submit(dt, i, stringList, fid);

                    //QTCKD_Audit(dt, i, stringList, fid);
                }
            }
        }

        /// <summary>
        /// 其他出库单提交操作
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="i"></param>
        /// <param name="stringList"></param>
        /// <param name="fid"></param>
        private static void QTCKD_Submit(DataTable dt, int i, List<string[]> stringList, string fid)
        {
            // 创建 RootObject 实例
            ClassApi_Submit.RootObject rootObject_Submit = new ClassApi_Submit.RootObject();
            // 赋值 FBillTypeID
            rootObject_Submit = new ClassApi_Submit.RootObject
            {
                Ids = fid,  //其他出库单内码
            };

            // 将 Root 对象序列化为 JSON 字符串
            string jsonString = JsonConvert.SerializeObject(rootObject_Submit, Formatting.Indented);

            // 使用webapi引用组件Kingdee.BOS.WebApi.Client.dll
            K3CloudApiClient client = new K3CloudApiClient(ConfigurationManager.AppSettings["K3CloudApi"]);
            var loginResult = client.ValidateLogin(ConfigurationManager.AppSettings["ValidateLogin_id"], ConfigurationManager.AppSettings["ValidateLogin_name"], ConfigurationManager.AppSettings["ValidateLogin_pwd"], 2052);
            var resultType = JObject.Parse(loginResult)["LoginResultType"].Value<int>();
            //string webapijson = "";
            //登录结果类型等于1，代表登录成功
            if (resultType == 1)
            {
                var kdwebapijson = client.Submit("STK_MisDelivery", JsonConvert.SerializeObject(rootObject_Submit, Formatting.Indented));
                SQLHelper.Txt(kdwebapijson, "CC");
                //IsSuccess_ref(dt, i, kdwebapijson, ref fid);
            }
        }

        /// <summary>
        /// 其他入库单审核操作
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="i"></param>
        /// <param name="stringList"></param>
        /// <param name="fid"></param>
        private static void QTCKD_Audit(DataTable dt, int i, List<string[]> stringList, string fid)
        {
            // 创建 RootObject 实例
            ClassApi_Audit.RootObject rootObject_Audit = new ClassApi_Audit.RootObject();
            // 赋值 FBillTypeID
            rootObject_Audit = new ClassApi_Audit.RootObject
            {
                Ids = fid,  //其他出库单内码
            };

            // 将 Root 对象序列化为 JSON 字符串
            string jsonString = JsonConvert.SerializeObject(rootObject_Audit, Formatting.Indented);

            // 使用webapi引用组件Kingdee.BOS.WebApi.Client.dll
            K3CloudApiClient client = new K3CloudApiClient(ConfigurationManager.AppSettings["K3CloudApi"]);
            var loginResult = client.ValidateLogin(ConfigurationManager.AppSettings["ValidateLogin_id"], ConfigurationManager.AppSettings["ValidateLogin_name"], ConfigurationManager.AppSettings["ValidateLogin_pwd"], 2052);
            var resultType = JObject.Parse(loginResult)["LoginResultType"].Value<int>();
            //string webapijson = "";
            //登录结果类型等于1，代表登录成功
            if (resultType == 1)
            {
                var kdwebapijson = client.Audit("STK_MisDelivery", JsonConvert.SerializeObject(rootObject_Audit, Formatting.Indented));
                SQLHelper.Txt(kdwebapijson, "CC");
                IsSuccess(dt, i, kdwebapijson);
            }
        }

        /// <summary>
        /// 采购退料单
        /// </summary>
        private static void CGTLD()
        {
            DataTable dt = new DataTable();
            dt = Class_Data.Data_List("MES", "PUR_MRB", false);
            //string fixedJsonData = FixJsonQuotes(Convert.ToString(dt.Rows[0]["DATA"]));

            for (int i = 0; i <= dt.Rows.Count - 1; i++)
            {
                //为空直接跳出本次循环
                if (Convert.ToString(dt.Rows[i]["DATA"]) == "")
                { continue; }
                // 添加引号
                //string resultData = SQLHelper.AddQuotes(Convert.ToString(dt.Rows[i]["DATA"]));
                //List<List<object>> dataList = JsonConvert.DeserializeObject<List<List<object>>>(resultData);

                // 添加引号
                //2024-5-23 由于数据格式改为标准Jobject格式，不需要再进行数据标准化操作，直接Jobject转List<List<object>>操作
                List<List<object>> dataList = JsonConvert.DeserializeObject<List<List<object>>>(Convert.ToString(dt.Rows[i]["DATA"]));

                // 转换为 List<string[]>
                List<string[]> stringList = new List<string[]>();

                foreach (var item in dataList)
                {
                    string[] stringArray = item.ConvertAll(x => x.ToString()).ToArray();
                    stringList.Add(stringArray);
                }

                // 创建一个 Root 对象并填充数据
                Root rootObject = new Root
                {
                    Model = new ClassApi_CGTLD.Model
                    {
                        FBillTypeID = new ClassApi_CGTLD.FBillTypeID
                        {
                            FNUMBER = stringList[0][1]  //单据类型
                        },
                        FDate = stringList[0][2],  //退料日期 
                        FStockOrgId = new ClassApi_CGTLD.FStockOrgId
                        {
                            FNumber = stringList[0][3]  //退料组织
                        },
                        FSupplierID = new FSupplierID
                        {
                            FNumber = stringList[0][4]  //供应商
                        },
                        FOwnerTypeIdHead = stringList[0][5], //货主类型
                        FOwnerIdHead = new ClassApi_CGTLD.FStockOrgId
                        {
                            FNumber = stringList[0][6]  //货主
                        },
                        FPURMRBENTRY = new List<FPURMRBENTRY>()
                    }
                };

                // 循环添加 FPURMRBENTRY 数据
                foreach (var list in stringList)
                {
                    FPURMRBENTRY entry = new FPURMRBENTRY
                    {
                        FMATERIALID = new FMATERIALID
                        {
                            FNumber = list[7]  //物料编码    
                        },
                        FRMREALQTY = Convert.ToDouble(list[8]),  //实退数量
                        FPRICEUNITID = new FPRICEUNITID
                        {
                            FNumber = list[9]  //计价单位
                        },
                        FSTOCKID = new FSTOCKID
                        {
                            FNumber = list[10]  //仓库
                        }
                        //FAuxPropID = new FAuxPropID
                        //{
                        //    FAUXPROPID__FF100001 = new FAUXPROPIDFF100001
                        //    {
                        //        FNumber = list[11] != "0" ? list[11] : null
                        //    }
                        //}
                    };

                    rootObject.Model.FPURMRBENTRY.Add(entry);
                }

                // 将 Root 对象序列化为 JSON 字符串
                string jsonString = JsonConvert.SerializeObject(rootObject, Formatting.Indented);

                // 使用webapi引用组件Kingdee.BOS.WebApi.Client.dll
                K3CloudApiClient client = new K3CloudApiClient(ConfigurationManager.AppSettings["K3CloudApi"]);
                var loginResult = client.ValidateLogin(ConfigurationManager.AppSettings["ValidateLogin_id"], ConfigurationManager.AppSettings["ValidateLogin_name"], ConfigurationManager.AppSettings["ValidateLogin_pwd"], 2052);
                var resultType = JObject.Parse(loginResult)["LoginResultType"].Value<int>();
                string fid = "";   //新增的采购退料单内码
                //string webapijson = "";
                //登录结果类型等于1，代表登录成功
                if (resultType == 1)
                {
                    var kdwebapijson = client.Save("PUR_MRB", JsonConvert.SerializeObject(rootObject, Formatting.Indented));
                    SQLHelper.Txt(kdwebapijson, "CC");
                    IsSuccess_ref(dt, i, kdwebapijson, ref fid);
                }
                if (fid == "0") { continue; }
                else
                {
                    //CGTLD_Submit(dt, i, stringList, fid);

                    //CGTLD_Audit(dt, i, stringList, fid);
                }
            }
        }

        /// <summary>
        /// 采购退料单提交操作
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="i"></param>
        /// <param name="stringList"></param>
        /// <param name="fid"></param>
        private static void CGTLD_Submit(DataTable dt, int i, List<string[]> stringList, string fid)
        {
            // 创建 RootObject 实例
            ClassApi_Submit.RootObject rootObject_Submit = new ClassApi_Submit.RootObject();
            // 赋值 FBillTypeID
            rootObject_Submit = new ClassApi_Submit.RootObject
            {
                Ids = fid,  //采购退料单内码
            };

            // 将 Root 对象序列化为 JSON 字符串
            string jsonString = JsonConvert.SerializeObject(rootObject_Submit, Formatting.Indented);

            // 使用webapi引用组件Kingdee.BOS.WebApi.Client.dll
            K3CloudApiClient client = new K3CloudApiClient(ConfigurationManager.AppSettings["K3CloudApi"]);
            var loginResult = client.ValidateLogin(ConfigurationManager.AppSettings["ValidateLogin_id"], ConfigurationManager.AppSettings["ValidateLogin_name"], ConfigurationManager.AppSettings["ValidateLogin_pwd"], 2052);
            var resultType = JObject.Parse(loginResult)["LoginResultType"].Value<int>();
            //string webapijson = "";
            //登录结果类型等于1，代表登录成功
            if (resultType == 1)
            {
                var kdwebapijson = client.Submit("PUR_MRB", JsonConvert.SerializeObject(rootObject_Submit, Formatting.Indented));
                SQLHelper.Txt(kdwebapijson, "CC");
                //IsSuccess_ref(dt, i, kdwebapijson, ref fid);
            }
        }

        /// <summary>
        /// 采购退料单审核操作
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="i"></param>
        /// <param name="stringList"></param>
        /// <param name="fid"></param>
        private static void CGTLD_Audit(DataTable dt, int i, List<string[]> stringList, string fid)
        {
            // 创建 RootObject 实例
            ClassApi_Audit.RootObject rootObject_Audit = new ClassApi_Audit.RootObject();
            // 赋值 FBillTypeID
            rootObject_Audit = new ClassApi_Audit.RootObject
            {
                Ids = fid,  //采购退料单内码
            };

            // 将 Root 对象序列化为 JSON 字符串
            string jsonString = JsonConvert.SerializeObject(rootObject_Audit, Formatting.Indented);

            // 使用webapi引用组件Kingdee.BOS.WebApi.Client.dll
            K3CloudApiClient client = new K3CloudApiClient(ConfigurationManager.AppSettings["K3CloudApi"]);
            var loginResult = client.ValidateLogin(ConfigurationManager.AppSettings["ValidateLogin_id"], ConfigurationManager.AppSettings["ValidateLogin_name"], ConfigurationManager.AppSettings["ValidateLogin_pwd"], 2052);
            var resultType = JObject.Parse(loginResult)["LoginResultType"].Value<int>();
            //string webapijson = "";
            //登录结果类型等于1，代表登录成功
            if (resultType == 1)
            {
                var kdwebapijson = client.Audit("PUR_MRB", JsonConvert.SerializeObject(rootObject_Audit, Formatting.Indented));
                SQLHelper.Txt(kdwebapijson, "CC");
                IsSuccess(dt, i, kdwebapijson);
            }
        }




        private void Class_Timer_Tick(object sender, EventArgs e)
        {
            DateTime dtNow = DateTime.Now;
            if (dtNow.Hour == 23)
            {

                Do_SCLLD(); //生产领料单

                Do_SCTLD(); //生产退料单

                Do_CGRKD(); //采购入库单

                Do_CGTLD(); //采购退料单

                Do_XSCKD(); //销售出库单

                Do_QTRKD(); //其他入库单

                Do_QTCKD(); //其他出库单

                Do_PYD(); //盘盈单

                Do_PKD(); //盘亏单

                Do_ZJDBD();//直接调拨单
            }

        }



        /// <summary>
        /// 生产领料单
        /// </summary>
        private static void Do_SCLLD()
        {
            DataTable dt = new DataTable();
            dt = Class_Data.Data_List("WMS", "PRD_PickMtrl", false);
            for (int i = 0; i <= dt.Rows.Count - 1; i++)
            {
                try
                {
                    //为空直接跳出本次循环
                    if (Convert.ToString(dt.Rows[i]["DATA"]) == "")
                    { continue; }


                    SCLLD_Josn.Root root_SCLLD = SCLLD_Josn.Return_SCLLD_Josn(Convert.ToString(dt.Rows[i]["DATA"]));
                    string jsonString = JsonConvert.SerializeObject(root_SCLLD, Formatting.Indented);

                    // 使用webapi引用组件Kingdee.BOS.WebApi.Client.dll
                    K3CloudApiClient client = new K3CloudApiClient(ConfigurationManager.AppSettings["K3CloudApi"]);
                    var loginResult = client.ValidateLogin(ConfigurationManager.AppSettings["ValidateLogin_id"], ConfigurationManager.AppSettings["ValidateLogin_name"], ConfigurationManager.AppSettings["ValidateLogin_pwd"], 2052);
                    var resultType = JObject.Parse(loginResult)["LoginResultType"].Value<int>();
                    string fid = "";   //保存的生产领料单内码
                                       //string webapijson = "";
                                       ////测试用数据
                                       //resultType = 1;

                    //登录结果类型等于1，代表登录成功
                    if (resultType == 1)
                    {
                        var kdwebapijson = client.Save("PRD_PickMtrl", JsonConvert.SerializeObject(root_SCLLD, Formatting.Indented));
                        ////测试用数据
                        //kdwebapijson = "{\"Result\":{\"ResponseStatus\":{\"IsSuccess\":true,\"Errors\":[],\"SuccessEntitys\":[{\"Id\":100538,\"Number\":\"SOUT00000472\",\"DIndex\":0}],\"SuccessMessages\":[],\"MsgCode\":0},\"Id\":100538,\"Number\":\"SOUT00000472\",\"NeedReturnData\":[{}]}}";
                        SQLHelper.Txt(kdwebapijson, "CC");
                        IsSuccess_ref(dt, i, kdwebapijson, ref fid);
                    }
                    if (fid == "0") { continue; }
                    else
                    {
                        SCLLD_Submit(fid);

                        SCLLD_Audit(dt, i, fid);
                    }
                }
                catch (Exception ex)
                {
                    //异常
                    SQLHelper.Txt("生产领料单" + ex.Message.ToString(), "CC");
                }
            }
        }

        /// <summary>
        /// 生产领料单提交操作
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="i"></param>
        /// <param name="stringList"></param>
        /// <param name="fid"></param>
        private static void SCLLD_Submit(string fid)
        {
            // 创建 RootObject 实例
            ClassApi_Submit.RootObject rootObject_Submit = new ClassApi_Submit.RootObject();
            // 赋值 FBillTypeID
            rootObject_Submit = new ClassApi_Submit.RootObject
            {
                Ids = fid,  //生产领料单内码
            };

            // 将 Root 对象序列化为 JSON 字符串
            string jsonString = JsonConvert.SerializeObject(rootObject_Submit, Formatting.Indented);

            // 使用webapi引用组件Kingdee.BOS.WebApi.Client.dll
            K3CloudApiClient client = new K3CloudApiClient(ConfigurationManager.AppSettings["K3CloudApi"]);
            var loginResult = client.ValidateLogin(ConfigurationManager.AppSettings["ValidateLogin_id"], ConfigurationManager.AppSettings["ValidateLogin_name"], ConfigurationManager.AppSettings["ValidateLogin_pwd"], 2052);
            var resultType = JObject.Parse(loginResult)["LoginResultType"].Value<int>();
            //string webapijson = "";
            //登录结果类型等于1，代表登录成功
            if (resultType == 1)
            {
                var kdwebapijson = client.Submit("PRD_PickMtrl", JsonConvert.SerializeObject(rootObject_Submit, Formatting.Indented));
                SQLHelper.Txt(kdwebapijson, "CC");
                //IsSuccess_ref(dt, i, kdwebapijson, ref fid);
            }
        }

        /// <summary>
        /// 生产领料单审核操作
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="i"></param>
        /// <param name="stringList"></param>
        /// <param name="fid"></param>
        private static void SCLLD_Audit(DataTable dt, int i, string fid)
        {
            // 创建 RootObject 实例
            ClassApi_Audit.RootObject rootObject_Audit = new ClassApi_Audit.RootObject();
            // 赋值 FBillTypeID
            rootObject_Audit = new ClassApi_Audit.RootObject
            {
                Ids = fid,  //生产领料单内码
            };

            // 将 Root 对象序列化为 JSON 字符串
            string jsonString = JsonConvert.SerializeObject(rootObject_Audit, Formatting.Indented);

            // 使用webapi引用组件Kingdee.BOS.WebApi.Client.dll
            K3CloudApiClient client = new K3CloudApiClient(ConfigurationManager.AppSettings["K3CloudApi"]);
            var loginResult = client.ValidateLogin(ConfigurationManager.AppSettings["ValidateLogin_id"], ConfigurationManager.AppSettings["ValidateLogin_name"], ConfigurationManager.AppSettings["ValidateLogin_pwd"], 2052);
            var resultType = JObject.Parse(loginResult)["LoginResultType"].Value<int>();
            //string webapijson = "";
            //登录结果类型等于1，代表登录成功
            if (resultType == 1)
            {
                var kdwebapijson = client.Audit("PRD_PickMtrl", JsonConvert.SerializeObject(rootObject_Audit, Formatting.Indented));
                SQLHelper.Txt(kdwebapijson, "CC");
                IsSuccess(dt, i, kdwebapijson);
            }
        }


        /// <summary>
        /// 生产退料单
        /// </summary>
        private static void Do_SCTLD()
        {
            DataTable dt = new DataTable();
            dt = Class_Data.Data_List("WMS", "PRD_ReturnMtrl", false);
            string fid = "";   //保存的生产退料单内码
            for (int i = 0; i <= dt.Rows.Count - 1; i++)
            {
                try
                {
                    //为空直接跳出本次循环
                    if (Convert.ToString(dt.Rows[i]["DATA"]) == "")
                    { continue; }
                    SCTLD_Josn.Root root_SCTLD = SCTLD_Josn.Return_SCTLD_Json(Convert.ToString(dt.Rows[i]["DATA"]));
                    string jsonString = JsonConvert.SerializeObject(root_SCTLD, Formatting.Indented);

                    // 使用webapi引用组件Kingdee.BOS.WebApi.Client.dll
                    K3CloudApiClient client = new K3CloudApiClient(ConfigurationManager.AppSettings["K3CloudApi"]);
                    var loginResult = client.ValidateLogin(ConfigurationManager.AppSettings["ValidateLogin_id"], ConfigurationManager.AppSettings["ValidateLogin_name"], ConfigurationManager.AppSettings["ValidateLogin_pwd"], 2052);
                    var resultType = JObject.Parse(loginResult)["LoginResultType"].Value<int>();
                    //string fid = "";   //下推出的生产领料单内码
                    //string webapijson = "";
                    //登录结果类型等于1，代表登录成功
                    if (resultType == 1)
                    {
                        var kdwebapijson = client.Save("PRD_ReturnMtrl", JsonConvert.SerializeObject(root_SCTLD, Formatting.Indented));
                        SQLHelper.Txt(kdwebapijson, "CC");
                        IsSuccess_ref(dt, i, kdwebapijson, ref fid);
                    }
                    if (fid == "0") { continue; }
                    else
                    {

                        SCTLD_Submit(fid);

                        SCTLD_Audit(dt, i, fid);
                    }
                }
                catch (Exception ex)
                {
                    //异常
                    SQLHelper.Txt("生产退料单" + ex.Message.ToString(), "CC");
                }
            }
        }

        /// <summary>
        /// 生产退料单提交操作
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="i"></param>
        /// <param name="stringList"></param>
        /// <param name="fid"></param>
        private static void SCTLD_Submit(string fid)
        {
            // 创建 RootObject 实例
            ClassApi_Submit.RootObject rootObject_Submit = new ClassApi_Submit.RootObject();
            // 赋值 FBillTypeID
            rootObject_Submit = new ClassApi_Submit.RootObject
            {
                Ids = fid,  //生产退料单内码
            };

            // 将 Root 对象序列化为 JSON 字符串
            string jsonString = JsonConvert.SerializeObject(rootObject_Submit, Formatting.Indented);

            // 使用webapi引用组件Kingdee.BOS.WebApi.Client.dll
            K3CloudApiClient client = new K3CloudApiClient(ConfigurationManager.AppSettings["K3CloudApi"]);
            var loginResult = client.ValidateLogin(ConfigurationManager.AppSettings["ValidateLogin_id"], ConfigurationManager.AppSettings["ValidateLogin_name"], ConfigurationManager.AppSettings["ValidateLogin_pwd"], 2052);
            var resultType = JObject.Parse(loginResult)["LoginResultType"].Value<int>();
            //string webapijson = "";
            //登录结果类型等于1，代表登录成功
            if (resultType == 1)
            {
                var kdwebapijson = client.Submit("PRD_ReturnMtrl", JsonConvert.SerializeObject(rootObject_Submit, Formatting.Indented));
                SQLHelper.Txt(kdwebapijson, "CC");
                //IsSuccess_ref(dt, i, kdwebapijson, ref fid);
            }
        }

        /// <summary>
        /// 生产退料单审核操作
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="i"></param>
        /// <param name="stringList"></param>
        /// <param name="fid"></param>
        private static void SCTLD_Audit(DataTable dt, int i, string fid)
        {
            // 创建 RootObject 实例
            ClassApi_Audit.RootObject rootObject_Audit = new ClassApi_Audit.RootObject();
            // 赋值 FBillTypeID
            rootObject_Audit = new ClassApi_Audit.RootObject
            {
                Ids = fid,  //生产退料单内码
            };

            // 将 Root 对象序列化为 JSON 字符串
            string jsonString = JsonConvert.SerializeObject(rootObject_Audit, Formatting.Indented);

            // 使用webapi引用组件Kingdee.BOS.WebApi.Client.dll
            K3CloudApiClient client = new K3CloudApiClient(ConfigurationManager.AppSettings["K3CloudApi"]);
            var loginResult = client.ValidateLogin(ConfigurationManager.AppSettings["ValidateLogin_id"], ConfigurationManager.AppSettings["ValidateLogin_name"], ConfigurationManager.AppSettings["ValidateLogin_pwd"], 2052);
            var resultType = JObject.Parse(loginResult)["LoginResultType"].Value<int>();
            //string webapijson = "";
            //登录结果类型等于1，代表登录成功
            if (resultType == 1)
            {
                var kdwebapijson = client.Audit("PRD_ReturnMtrl", JsonConvert.SerializeObject(rootObject_Audit, Formatting.Indented));
                SQLHelper.Txt(kdwebapijson, "CC");
                IsSuccess(dt, i, kdwebapijson);
            }
        }



        /// <summary>
        /// 采购入库单
        /// </summary>
        private static void Do_CGRKD()
        {
            DataTable dt = new DataTable();
            dt = Class_Data.Data_List("WMS", "STK_InStock", false);
            string fid = "";   //保存的采购入库单内码
            for (int i = 0; i <= dt.Rows.Count - 1; i++)
            {
                try
                {
                    //为空直接跳出本次循环
                    if (Convert.ToString(dt.Rows[i]["DATA"]) == "")
                    { continue; }
                    CGRKD_Josn.Root root_CGRKD = CGRKD_Josn.Return_CGRKD_Json(Convert.ToString(dt.Rows[i]["DATA"]));
                    string jsonString = JsonConvert.SerializeObject(root_CGRKD, Formatting.Indented);

                    // 使用webapi引用组件Kingdee.BOS.WebApi.Client.dll
                    K3CloudApiClient client = new K3CloudApiClient(ConfigurationManager.AppSettings["K3CloudApi"]);
                    var loginResult = client.ValidateLogin(ConfigurationManager.AppSettings["ValidateLogin_id"], ConfigurationManager.AppSettings["ValidateLogin_name"], ConfigurationManager.AppSettings["ValidateLogin_pwd"], 2052);
                    var resultType = JObject.Parse(loginResult)["LoginResultType"].Value<int>();
                    //string fid = "";   //下推出的采购入库单内码
                    ////测试
                    //resultType = 1;
                    //登录结果类型等于1，代表登录成功
                    if (resultType == 1)
                    {
                        var kdwebapijson = client.Save("STK_InStock", JsonConvert.SerializeObject(root_CGRKD, Formatting.Indented));
                        ////测试
                        //kdwebapijson = "{\"Result\":{\"ResponseStatus\":{\"IsSuccess\":true,\"Errors\":[],\"SuccessEntitys\":[{\"Id\":102493,\"Number\":\"CGRK02474\",\"DIndex\":0}],\"SuccessMessages\":[],\"MsgCode\":0},\"Id\":102493,\"Number\":\"CGRK02474\",\"NeedReturnData\":[{}]}}";
                        SQLHelper.Txt(kdwebapijson, "CC");
                        IsSuccess_ref(dt, i, kdwebapijson, ref fid);
                    }
                    if (fid == "0") { continue; }
                    else
                    {

                        CGRKD_Submit(fid);

                        CGRKD_Audit(dt, i, fid);
                    }
                }
                catch (Exception ex)
                {
                    //异常
                    SQLHelper.Txt("采购入库单" + ex.Message.ToString(), "CC");
                }
            }
        }

        /// <summary>
        /// 采购入库单提交操作
        /// </summary>
        /// <param name="fid"></param>
        private static void CGRKD_Submit(string fid)
        {
            // 创建 RootObject 实例
            ClassApi_Submit.RootObject rootObject_Submit = new ClassApi_Submit.RootObject();
            // 赋值 FBillTypeID
            rootObject_Submit = new ClassApi_Submit.RootObject
            {
                Ids = fid,  //采购入库单内码
            };

            // 将 Root 对象序列化为 JSON 字符串
            string jsonString = JsonConvert.SerializeObject(rootObject_Submit, Formatting.Indented);

            // 使用webapi引用组件Kingdee.BOS.WebApi.Client.dll
            K3CloudApiClient client = new K3CloudApiClient(ConfigurationManager.AppSettings["K3CloudApi"]);
            var loginResult = client.ValidateLogin(ConfigurationManager.AppSettings["ValidateLogin_id"], ConfigurationManager.AppSettings["ValidateLogin_name"], ConfigurationManager.AppSettings["ValidateLogin_pwd"], 2052);
            var resultType = JObject.Parse(loginResult)["LoginResultType"].Value<int>();
            //string webapijson = "";
            //登录结果类型等于1，代表登录成功
            if (resultType == 1)
            {
                var kdwebapijson = client.Submit("STK_InStock", JsonConvert.SerializeObject(rootObject_Submit, Formatting.Indented));
                SQLHelper.Txt(kdwebapijson, "CC");
                //IsSuccess_ref(dt, i, kdwebapijson, ref fid);
            }
        }

        /// <summary>
        /// 采购入库单审核操作
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="i"></param>
        /// <param name="fid"></param>
        private static void CGRKD_Audit(DataTable dt, int i, string fid)
        {
            // 创建 RootObject 实例
            ClassApi_Audit.RootObject rootObject_Audit = new ClassApi_Audit.RootObject();
            // 赋值 FBillTypeID
            rootObject_Audit = new ClassApi_Audit.RootObject
            {
                Ids = fid,  //采购入库单内码
            };

            // 将 Root 对象序列化为 JSON 字符串
            string jsonString = JsonConvert.SerializeObject(rootObject_Audit, Formatting.Indented);

            // 使用webapi引用组件Kingdee.BOS.WebApi.Client.dll
            K3CloudApiClient client = new K3CloudApiClient(ConfigurationManager.AppSettings["K3CloudApi"]);
            var loginResult = client.ValidateLogin(ConfigurationManager.AppSettings["ValidateLogin_id"], ConfigurationManager.AppSettings["ValidateLogin_name"], ConfigurationManager.AppSettings["ValidateLogin_pwd"], 2052);
            var resultType = JObject.Parse(loginResult)["LoginResultType"].Value<int>();
            //string webapijson = "";
            //登录结果类型等于1，代表登录成功
            if (resultType == 1)
            {
                var kdwebapijson = client.Audit("STK_InStock", JsonConvert.SerializeObject(rootObject_Audit, Formatting.Indented));
                SQLHelper.Txt(kdwebapijson, "CC");
                IsSuccess(dt, i, kdwebapijson);
            }
        }



        /// <summary>
        /// 采购退料单
        /// </summary>
        private static void Do_CGTLD()
        {
            DataTable dt = new DataTable();
            dt = Class_Data.Data_List("WMS", "PUR_MRB", false);
            string fid = "";   //保存的采购退料单的内码
            for (int i = 0; i <= dt.Rows.Count - 1; i++)
            {
                try
                {
                    //为空直接跳出本次循环
                    if (Convert.ToString(dt.Rows[i]["DATA"]) == "")
                    { continue; }
                    CGTLD_Josn.Root root_CGTLD = CGTLD_Josn.Return_CGTLD_Json(Convert.ToString(dt.Rows[i]["DATA"]));
                    string jsonString = JsonConvert.SerializeObject(root_CGTLD, Formatting.Indented);

                    // 使用webapi引用组件Kingdee.BOS.WebApi.Client.dll
                    K3CloudApiClient client = new K3CloudApiClient(ConfigurationManager.AppSettings["K3CloudApi"]);
                    var loginResult = client.ValidateLogin(ConfigurationManager.AppSettings["ValidateLogin_id"], ConfigurationManager.AppSettings["ValidateLogin_name"], ConfigurationManager.AppSettings["ValidateLogin_pwd"], 2052);
                    var resultType = JObject.Parse(loginResult)["LoginResultType"].Value<int>();
                    //string fid = "";   //新增的采购退料单内码
                    ////测试
                    //resultType = 1;
                    //登录结果类型等于1，代表登录成功
                    if (resultType == 1)
                    {
                        var kdwebapijson = client.Save("PUR_MRB", JsonConvert.SerializeObject(root_CGTLD, Formatting.Indented));
                        ////测试
                        //kdwebapijson = "{\"Result\":{\"ResponseStatus\":{\"IsSuccess\":true,\"Errors\":[],\"SuccessEntitys\":[{\"Id\":100049,\"Number\":\"CGTL000047\",\"DIndex\":0}],\"SuccessMessages\":[],\"MsgCode\":0},\"Id\":100049,\"Number\":\"CGTL000047\",\"NeedReturnData\":[{}]}}";
                        SQLHelper.Txt(kdwebapijson, "CC");
                        IsSuccess_ref(dt, i, kdwebapijson, ref fid);
                    }
                    if (fid == "0") { continue; }
                    else
                    {

                        CGTLD_Submit(fid);

                        CGTLD_Audit(dt, i, fid);
                    }
                }
                catch (Exception ex)
                {
                    //异常
                    SQLHelper.Txt("采购退料单" + ex.Message.ToString(), "CC");
                }
            }
        }

        /// <summary>
        /// 采购退料单提交操作
        /// </summary>
        /// <param name="fid"></param>
        private static void CGTLD_Submit(string fid)
        {
            // 创建 RootObject 实例
            ClassApi_Submit.RootObject rootObject_Submit = new ClassApi_Submit.RootObject();
            // 赋值 FBillTypeID
            rootObject_Submit = new ClassApi_Submit.RootObject
            {
                Ids = fid,  //采购退料单内码
            };

            // 将 Root 对象序列化为 JSON 字符串
            string jsonString = JsonConvert.SerializeObject(rootObject_Submit, Formatting.Indented);

            // 使用webapi引用组件Kingdee.BOS.WebApi.Client.dll
            K3CloudApiClient client = new K3CloudApiClient(ConfigurationManager.AppSettings["K3CloudApi"]);
            var loginResult = client.ValidateLogin(ConfigurationManager.AppSettings["ValidateLogin_id"], ConfigurationManager.AppSettings["ValidateLogin_name"], ConfigurationManager.AppSettings["ValidateLogin_pwd"], 2052);
            var resultType = JObject.Parse(loginResult)["LoginResultType"].Value<int>();
            //string webapijson = "";
            //登录结果类型等于1，代表登录成功
            if (resultType == 1)
            {
                var kdwebapijson = client.Submit("PUR_MRB", JsonConvert.SerializeObject(rootObject_Submit, Formatting.Indented));
                SQLHelper.Txt(kdwebapijson, "CC");
                //IsSuccess_ref(dt, i, kdwebapijson, ref fid);
            }
        }

        /// <summary>
        /// 采购退料单审核操作
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="i"></param>
        /// <param name="fid"></param>
        private static void CGTLD_Audit(DataTable dt, int i, string fid)
        {
            // 创建 RootObject 实例
            ClassApi_Audit.RootObject rootObject_Audit = new ClassApi_Audit.RootObject();
            // 赋值 FBillTypeID
            rootObject_Audit = new ClassApi_Audit.RootObject
            {
                Ids = fid,  //采购退料单内码
            };

            // 将 Root 对象序列化为 JSON 字符串
            string jsonString = JsonConvert.SerializeObject(rootObject_Audit, Formatting.Indented);

            // 使用webapi引用组件Kingdee.BOS.WebApi.Client.dll
            K3CloudApiClient client = new K3CloudApiClient(ConfigurationManager.AppSettings["K3CloudApi"]);
            var loginResult = client.ValidateLogin(ConfigurationManager.AppSettings["ValidateLogin_id"], ConfigurationManager.AppSettings["ValidateLogin_name"], ConfigurationManager.AppSettings["ValidateLogin_pwd"], 2052);
            var resultType = JObject.Parse(loginResult)["LoginResultType"].Value<int>();
            //string webapijson = "";
            //登录结果类型等于1，代表登录成功
            if (resultType == 1)
            {
                var kdwebapijson = client.Audit("PUR_MRB", JsonConvert.SerializeObject(rootObject_Audit, Formatting.Indented));
                SQLHelper.Txt(kdwebapijson, "CC");
                IsSuccess(dt, i, kdwebapijson);
            }
        }


        /// <summary>
        /// 销售出库单
        /// </summary>
        private static void Do_XSCKD()
        {
            DataTable dt = new DataTable();
            dt = Class_Data.Data_List("WMS", "SAL_OUTSTOCK", false);
            string fid = "";   //保存的销售出库单内码
            for (int i = 0; i <= dt.Rows.Count - 1; i++)
            {
                try
                {
                    //为空直接跳出本次循环
                    if (Convert.ToString(dt.Rows[i]["DATA"]) == "")
                    { continue; }
                    XSCKD_Josn.Root root_XSCKD = XSCKD_Josn.Return_XSCKD_Json(Convert.ToString(dt.Rows[i]["DATA"]));
                    string jsonString = JsonConvert.SerializeObject(root_XSCKD, Formatting.Indented);

                    // 使用webapi引用组件Kingdee.BOS.WebApi.Client.dll
                    K3CloudApiClient client = new K3CloudApiClient(ConfigurationManager.AppSettings["K3CloudApi"]);
                    var loginResult = client.ValidateLogin(ConfigurationManager.AppSettings["ValidateLogin_id"], ConfigurationManager.AppSettings["ValidateLogin_name"], ConfigurationManager.AppSettings["ValidateLogin_pwd"], 2052);
                    var resultType = JObject.Parse(loginResult)["LoginResultType"].Value<int>();
                    //string fid = "";   //新增的采购退料单内码
                    //string webapijson = "";
                    //登录结果类型等于1，代表登录成功
                    if (resultType == 1)
                    {
                        var kdwebapijson = client.Save("SAL_OUTSTOCK", JsonConvert.SerializeObject(root_XSCKD, Formatting.Indented));
                        SQLHelper.Txt(kdwebapijson, "CC");
                        IsSuccess_ref(dt, i, kdwebapijson, ref fid);
                    }
                    if (fid == "0") { continue; }
                    else
                    {
                        XSCKD_Submit(fid);

                        XSCKD_Audit(dt, i, fid);
                    }
                }
                catch (Exception ex)
                {
                    //异常
                    SQLHelper.Txt("采购退料单" + ex.Message.ToString(), "CC");
                }
            }
        }

        /// <summary>
        /// 销售出库单提交操作
        /// </summary>
        /// <param name="fid"></param>
        private static void XSCKD_Submit(string fid)
        {
            // 创建 RootObject 实例
            ClassApi_Submit.RootObject rootObject_Submit = new ClassApi_Submit.RootObject();
            // 赋值 FBillTypeID
            rootObject_Submit = new ClassApi_Submit.RootObject
            {
                Ids = fid,  //销售出库单内码
            };

            // 将 Root 对象序列化为 JSON 字符串
            string jsonString = JsonConvert.SerializeObject(rootObject_Submit, Formatting.Indented);

            // 使用webapi引用组件Kingdee.BOS.WebApi.Client.dll
            K3CloudApiClient client = new K3CloudApiClient(ConfigurationManager.AppSettings["K3CloudApi"]);
            var loginResult = client.ValidateLogin(ConfigurationManager.AppSettings["ValidateLogin_id"], ConfigurationManager.AppSettings["ValidateLogin_name"], ConfigurationManager.AppSettings["ValidateLogin_pwd"], 2052);
            var resultType = JObject.Parse(loginResult)["LoginResultType"].Value<int>();
            //string webapijson = "";
            //登录结果类型等于1，代表登录成功
            if (resultType == 1)
            {
                var kdwebapijson = client.Submit("SAL_OUTSTOCK", JsonConvert.SerializeObject(rootObject_Submit, Formatting.Indented));
                SQLHelper.Txt(kdwebapijson, "CC");
                //IsSuccess_ref(dt, i, kdwebapijson, ref fid);
            }
        }

        /// <summary>
        /// 销售出库单审核操作
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="i"></param>
        /// <param name="fid"></param>
        private static void XSCKD_Audit(DataTable dt, int i, string fid)
        {
            // 创建 RootObject 实例
            ClassApi_Audit.RootObject rootObject_Audit = new ClassApi_Audit.RootObject();
            // 赋值 FBillTypeID
            rootObject_Audit = new ClassApi_Audit.RootObject
            {
                Ids = fid,  //销售出库单内码
            };

            // 将 Root 对象序列化为 JSON 字符串
            string jsonString = JsonConvert.SerializeObject(rootObject_Audit, Formatting.Indented);

            // 使用webapi引用组件Kingdee.BOS.WebApi.Client.dll
            K3CloudApiClient client = new K3CloudApiClient(ConfigurationManager.AppSettings["K3CloudApi"]);
            var loginResult = client.ValidateLogin(ConfigurationManager.AppSettings["ValidateLogin_id"], ConfigurationManager.AppSettings["ValidateLogin_name"], ConfigurationManager.AppSettings["ValidateLogin_pwd"], 2052);
            var resultType = JObject.Parse(loginResult)["LoginResultType"].Value<int>();
            //string webapijson = "";
            //登录结果类型等于1，代表登录成功
            if (resultType == 1)
            {
                var kdwebapijson = client.Audit("SAL_OUTSTOCK", JsonConvert.SerializeObject(rootObject_Audit, Formatting.Indented));
                SQLHelper.Txt(kdwebapijson, "CC");
                IsSuccess(dt, i, kdwebapijson);
            }
        }



        /// <summary>
        /// 其他入库单
        /// </summary>
        private static void Do_QTRKD()
        {
            DataTable dt = new DataTable();
            dt = Class_Data.Data_List("WMS", "STK_MISCELLANEOUS", false);
            for (int i = 0; i <= dt.Rows.Count - 1; i++)
            {
                try
                {
                    //为空直接跳出本次循环
                    if (Convert.ToString(dt.Rows[i]["DATA"]) == "")
                    { continue; }
                    QTRKD_Josn.Root root_QTRKD = QTRKD_Josn.Return_QTRKD_Json(Convert.ToString(dt.Rows[i]["DATA"]));
                    string jsonString = JsonConvert.SerializeObject(root_QTRKD, Formatting.Indented);

                    // 使用webapi引用组件Kingdee.BOS.WebApi.Client.dll
                    K3CloudApiClient client = new K3CloudApiClient(ConfigurationManager.AppSettings["K3CloudApi"]);
                    var loginResult = client.ValidateLogin(ConfigurationManager.AppSettings["ValidateLogin_id"], ConfigurationManager.AppSettings["ValidateLogin_name"], ConfigurationManager.AppSettings["ValidateLogin_pwd"], 2052);
                    var resultType = JObject.Parse(loginResult)["LoginResultType"].Value<int>();
                    string fid = "";   //新增的其他入库单内码
                                       //string webapijson = "";
                                       //登录结果类型等于1，代表登录成功
                    if (resultType == 1)
                    {
                        var kdwebapijson = client.Save("STK_MISCELLANEOUS", JsonConvert.SerializeObject(root_QTRKD, Formatting.Indented));
                        SQLHelper.Txt(kdwebapijson, "CC");
                        IsSuccess_ref(dt, i, kdwebapijson, ref fid);
                    }
                    if (fid == "0") { continue; }
                    else
                    {
                        QTRKD_Submit(fid);

                        QTRKD_Audit(dt, i, fid);
                    }
                }
                catch (Exception ex)
                {
                    //异常
                    SQLHelper.Txt("其他入库单" + ex.Message.ToString(), "CC");
                }
            }
        }


        /// <summary>
        /// 其他入库单提交操作
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="i"></param>
        /// <param name="stringList"></param>
        /// <param name="fid"></param>
        private static void QTRKD_Submit(string fid)
        {
            // 创建 RootObject 实例
            ClassApi_Submit.RootObject rootObject_Submit = new ClassApi_Submit.RootObject();
            // 赋值 FBillTypeID
            rootObject_Submit = new ClassApi_Submit.RootObject
            {
                Ids = fid,  //其他入库单内码
            };

            // 将 Root 对象序列化为 JSON 字符串
            string jsonString = JsonConvert.SerializeObject(rootObject_Submit, Formatting.Indented);

            // 使用webapi引用组件Kingdee.BOS.WebApi.Client.dll
            K3CloudApiClient client = new K3CloudApiClient(ConfigurationManager.AppSettings["K3CloudApi"]);
            var loginResult = client.ValidateLogin(ConfigurationManager.AppSettings["ValidateLogin_id"], ConfigurationManager.AppSettings["ValidateLogin_name"], ConfigurationManager.AppSettings["ValidateLogin_pwd"], 2052);
            var resultType = JObject.Parse(loginResult)["LoginResultType"].Value<int>();
            //string webapijson = "";
            //登录结果类型等于1，代表登录成功
            if (resultType == 1)
            {
                var kdwebapijson = client.Submit("STK_MISCELLANEOUS", JsonConvert.SerializeObject(rootObject_Submit, Formatting.Indented));
                SQLHelper.Txt(kdwebapijson, "CC");
                //IsSuccess_ref(dt, i, kdwebapijson, ref fid);
            }
        }

        /// <summary>
        /// 其他入库单审核操作
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="i"></param>
        /// <param name="stringList"></param>
        /// <param name="fid"></param>
        private static void QTRKD_Audit(DataTable dt, int i, string fid)
        {
            // 创建 RootObject 实例
            ClassApi_Audit.RootObject rootObject_Audit = new ClassApi_Audit.RootObject();
            // 赋值 FBillTypeID
            rootObject_Audit = new ClassApi_Audit.RootObject
            {
                Ids = fid,  //其他入库单内码
            };

            // 将 Root 对象序列化为 JSON 字符串
            string jsonString = JsonConvert.SerializeObject(rootObject_Audit, Formatting.Indented);

            // 使用webapi引用组件Kingdee.BOS.WebApi.Client.dll
            K3CloudApiClient client = new K3CloudApiClient(ConfigurationManager.AppSettings["K3CloudApi"]);
            var loginResult = client.ValidateLogin(ConfigurationManager.AppSettings["ValidateLogin_id"], ConfigurationManager.AppSettings["ValidateLogin_name"], ConfigurationManager.AppSettings["ValidateLogin_pwd"], 2052);
            var resultType = JObject.Parse(loginResult)["LoginResultType"].Value<int>();
            //string webapijson = "";
            //登录结果类型等于1，代表登录成功
            if (resultType == 1)
            {
                var kdwebapijson = client.Audit("STK_MISCELLANEOUS", JsonConvert.SerializeObject(rootObject_Audit, Formatting.Indented));
                SQLHelper.Txt(kdwebapijson, "CC");
                IsSuccess(dt, i, kdwebapijson);
            }
        }


        /// <summary>
        /// 其他出库单
        /// </summary>
        private static void Do_QTCKD()
        {
            DataTable dt = new DataTable();
            dt = Class_Data.Data_List("WMS", "STK_MisDelivery", false);

            for (int i = 0; i <= dt.Rows.Count - 1; i++)
            {
                try
                {
                    //为空直接跳出本次循环
                    if (Convert.ToString(dt.Rows[i]["DATA"]) == "")
                    { continue; }
                    QTCKD_Josn.Root root_QTCKD = QTCKD_Josn.Return_QTCKD_Json(Convert.ToString(dt.Rows[i]["DATA"]));
                    string jsonString = JsonConvert.SerializeObject(root_QTCKD, Formatting.Indented);

                    // 使用webapi引用组件Kingdee.BOS.WebApi.Client.dll
                    K3CloudApiClient client = new K3CloudApiClient(ConfigurationManager.AppSettings["K3CloudApi"]);
                    var loginResult = client.ValidateLogin(ConfigurationManager.AppSettings["ValidateLogin_id"], ConfigurationManager.AppSettings["ValidateLogin_name"], ConfigurationManager.AppSettings["ValidateLogin_pwd"], 2052);
                    var resultType = JObject.Parse(loginResult)["LoginResultType"].Value<int>();
                    //string webapijson = "";
                    string fid = "";   //新增的其他出库单内码
                                       //登录结果类型等于1，代表登录成功
                    if (resultType == 1)
                    {
                        var kdwebapijson = client.Save("STK_MisDelivery", JsonConvert.SerializeObject(root_QTCKD, Formatting.Indented));
                        SQLHelper.Txt(kdwebapijson, "CC");
                        IsSuccess_ref(dt, i, kdwebapijson, ref fid);
                    }
                    if (fid == "0") { continue; }
                    else
                    {
                        QTCKD_Submit(fid);

                        QTCKD_Audit(dt, i, fid);
                    }
                }
                catch (Exception ex)
                {
                    //异常
                    SQLHelper.Txt("其他出库单" + ex.Message.ToString(), "CC");
                }
            }
        }


        /// <summary>
        /// 其他出库单提交操作
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="i"></param>
        /// <param name="stringList"></param>
        /// <param name="fid"></param>
        private static void QTCKD_Submit(string fid)
        {
            // 创建 RootObject 实例
            ClassApi_Submit.RootObject rootObject_Submit = new ClassApi_Submit.RootObject();
            // 赋值 FBillTypeID
            rootObject_Submit = new ClassApi_Submit.RootObject
            {
                Ids = fid,  //其他出库单内码
            };

            // 将 Root 对象序列化为 JSON 字符串
            string jsonString = JsonConvert.SerializeObject(rootObject_Submit, Formatting.Indented);

            // 使用webapi引用组件Kingdee.BOS.WebApi.Client.dll
            K3CloudApiClient client = new K3CloudApiClient(ConfigurationManager.AppSettings["K3CloudApi"]);
            var loginResult = client.ValidateLogin(ConfigurationManager.AppSettings["ValidateLogin_id"], ConfigurationManager.AppSettings["ValidateLogin_name"], ConfigurationManager.AppSettings["ValidateLogin_pwd"], 2052);
            var resultType = JObject.Parse(loginResult)["LoginResultType"].Value<int>();
            //string webapijson = "";
            //登录结果类型等于1，代表登录成功
            if (resultType == 1)
            {
                var kdwebapijson = client.Submit("STK_MisDelivery", JsonConvert.SerializeObject(rootObject_Submit, Formatting.Indented));
                SQLHelper.Txt(kdwebapijson, "CC");
                //IsSuccess_ref(dt, i, kdwebapijson, ref fid);
            }
        }

        /// <summary>
        /// 其他出库单审核操作
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="i"></param>
        /// <param name="stringList"></param>
        /// <param name="fid"></param>
        private static void QTCKD_Audit(DataTable dt, int i, string fid)
        {
            // 创建 RootObject 实例
            ClassApi_Audit.RootObject rootObject_Audit = new ClassApi_Audit.RootObject();
            // 赋值 FBillTypeID
            rootObject_Audit = new ClassApi_Audit.RootObject
            {
                Ids = fid,  //其他出库单内码
            };

            // 将 Root 对象序列化为 JSON 字符串
            string jsonString = JsonConvert.SerializeObject(rootObject_Audit, Formatting.Indented);

            // 使用webapi引用组件Kingdee.BOS.WebApi.Client.dll
            K3CloudApiClient client = new K3CloudApiClient(ConfigurationManager.AppSettings["K3CloudApi"]);
            var loginResult = client.ValidateLogin(ConfigurationManager.AppSettings["ValidateLogin_id"], ConfigurationManager.AppSettings["ValidateLogin_name"], ConfigurationManager.AppSettings["ValidateLogin_pwd"], 2052);
            var resultType = JObject.Parse(loginResult)["LoginResultType"].Value<int>();
            //string webapijson = "";
            //登录结果类型等于1，代表登录成功
            if (resultType == 1)
            {
                var kdwebapijson = client.Audit("STK_MisDelivery", JsonConvert.SerializeObject(rootObject_Audit, Formatting.Indented));
                SQLHelper.Txt(kdwebapijson, "CC");
                IsSuccess(dt, i, kdwebapijson);
            }
        }




        /// <summary>
        /// 盘盈单
        /// </summary>
        private static void Do_PYD()
        {
            DataTable dt = new DataTable();
            dt = Class_Data.Data_List("WMS", "STK_StockCountGain", false);

            for (int i = 0; i <= dt.Rows.Count - 1; i++)
            {
                try
                {
                    //为空直接跳出本次循环
                    if (Convert.ToString(dt.Rows[i]["DATA"]) == "")
                    { continue; }
                    PYD_Josn.Root root_PYD = PYD_Josn.Return_PYD_Json(Convert.ToString(dt.Rows[i]["DATA"]));
                    string jsonString = JsonConvert.SerializeObject(root_PYD, Formatting.Indented);

                    // 使用webapi引用组件Kingdee.BOS.WebApi.Client.dll
                    K3CloudApiClient client = new K3CloudApiClient(ConfigurationManager.AppSettings["K3CloudApi"]);
                    var loginResult = client.ValidateLogin(ConfigurationManager.AppSettings["ValidateLogin_id"], ConfigurationManager.AppSettings["ValidateLogin_name"], ConfigurationManager.AppSettings["ValidateLogin_pwd"], 2052);
                    var resultType = JObject.Parse(loginResult)["LoginResultType"].Value<int>();
                    //string webapijson = "";
                    string fid = "";   //新增的盘盈单内码
                                       //登录结果类型等于1，代表登录成功
                    if (resultType == 1)
                    {
                        var kdwebapijson = client.Save("STK_StockCountGain", JsonConvert.SerializeObject(root_PYD, Formatting.Indented));
                        SQLHelper.Txt(kdwebapijson, "CC");
                        IsSuccess_ref(dt, i, kdwebapijson, ref fid);
                    }
                    if (fid == "0") { continue; }
                    else
                    {
                        PYD_Submit(fid);

                        PYD_Audit(dt, i, fid);
                    }
                }
                catch (Exception ex)
                {
                    //异常
                    SQLHelper.Txt("盘盈单" + ex.Message.ToString(), "CC");
                }
            }
        }


        /// <summary>
        /// 盘盈单提交操作
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="i"></param>
        /// <param name="stringList"></param>
        /// <param name="fid"></param>
        private static void PYD_Submit(string fid)
        {
            // 创建 RootObject 实例
            ClassApi_Submit.RootObject rootObject_Submit = new ClassApi_Submit.RootObject();
            // 赋值 FBillTypeID
            rootObject_Submit = new ClassApi_Submit.RootObject
            {
                Ids = fid,  //盘盈单内码
            };

            // 将 Root 对象序列化为 JSON 字符串
            string jsonString = JsonConvert.SerializeObject(rootObject_Submit, Formatting.Indented);

            // 使用webapi引用组件Kingdee.BOS.WebApi.Client.dll
            K3CloudApiClient client = new K3CloudApiClient(ConfigurationManager.AppSettings["K3CloudApi"]);
            var loginResult = client.ValidateLogin(ConfigurationManager.AppSettings["ValidateLogin_id"], ConfigurationManager.AppSettings["ValidateLogin_name"], ConfigurationManager.AppSettings["ValidateLogin_pwd"], 2052);
            var resultType = JObject.Parse(loginResult)["LoginResultType"].Value<int>();
            //string webapijson = "";
            //登录结果类型等于1，代表登录成功
            if (resultType == 1)
            {
                var kdwebapijson = client.Submit("STK_StockCountGain", JsonConvert.SerializeObject(rootObject_Submit, Formatting.Indented));
                SQLHelper.Txt(kdwebapijson, "CC");
                //IsSuccess_ref(dt, i, kdwebapijson, ref fid);
            }
        }

        /// <summary>
        /// 盘盈单审核操作
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="i"></param>
        /// <param name="stringList"></param>
        /// <param name="fid"></param>
        private static void PYD_Audit(DataTable dt, int i, string fid)
        {
            // 创建 RootObject 实例
            ClassApi_Audit.RootObject rootObject_Audit = new ClassApi_Audit.RootObject();
            // 赋值 FBillTypeID
            rootObject_Audit = new ClassApi_Audit.RootObject
            {
                Ids = fid,  //盘盈单内码
            };

            // 将 Root 对象序列化为 JSON 字符串
            string jsonString = JsonConvert.SerializeObject(rootObject_Audit, Formatting.Indented);

            // 使用webapi引用组件Kingdee.BOS.WebApi.Client.dll
            K3CloudApiClient client = new K3CloudApiClient(ConfigurationManager.AppSettings["K3CloudApi"]);
            var loginResult = client.ValidateLogin(ConfigurationManager.AppSettings["ValidateLogin_id"], ConfigurationManager.AppSettings["ValidateLogin_name"], ConfigurationManager.AppSettings["ValidateLogin_pwd"], 2052);
            var resultType = JObject.Parse(loginResult)["LoginResultType"].Value<int>();
            //string webapijson = "";
            //登录结果类型等于1，代表登录成功
            if (resultType == 1)
            {
                var kdwebapijson = client.Audit("STK_StockCountGain", JsonConvert.SerializeObject(rootObject_Audit, Formatting.Indented));
                SQLHelper.Txt(kdwebapijson, "CC");
                IsSuccess(dt, i, kdwebapijson);
            }
        }



        /// <summary>
        /// 直接调拨单
        /// </summary>
        private static void Do_ZJDBD()
        {
            DataTable dt = new DataTable();
            dt = Class_Data.Data_List("WMS", "STK_TransferDirect", false);

            for (int i = 0; i <= dt.Rows.Count - 1; i++)
            {
                try
                {
                    //为空直接跳出本次循环
                    if (Convert.ToString(dt.Rows[i]["DATA"]) == "")
                    { continue; }
                    ZJDBD_Josn.Root root_ZJDBD = ZJDBD_Josn.Return_ZJDBD_Json(Convert.ToString(dt.Rows[i]["DATA"]));
                    string jsonString = JsonConvert.SerializeObject(root_ZJDBD, Formatting.Indented);

                    // 使用webapi引用组件Kingdee.BOS.WebApi.Client.dll
                    K3CloudApiClient client = new K3CloudApiClient(ConfigurationManager.AppSettings["K3CloudApi"]);
                    var loginResult = client.ValidateLogin(ConfigurationManager.AppSettings["ValidateLogin_id"], ConfigurationManager.AppSettings["ValidateLogin_name"], ConfigurationManager.AppSettings["ValidateLogin_pwd"], 2052);
                    var resultType = JObject.Parse(loginResult)["LoginResultType"].Value<int>();
                    //string webapijson = "";
                    string fid = "";   //新增的直接调拨单内码
                                       //登录结果类型等于1，代表登录成功
                    if (resultType == 1)
                    {
                        var kdwebapijson = client.Save("STK_TransferDirect", JsonConvert.SerializeObject(root_ZJDBD, Formatting.Indented));
                        SQLHelper.Txt(kdwebapijson, "CC");
                        IsSuccess_ref(dt, i, kdwebapijson, ref fid);
                    }
                    if (fid == "0") { continue; }
                    else
                    {
                        ZJDBD_Submit(fid);

                        ZJDBD_Audit(dt, i, fid);
                    }
                }
                catch (Exception ex)
                {
                    //异常
                    SQLHelper.Txt("直接调拨单" + ex.Message.ToString(), "CC");
                }
            }
        }


        /// <summary>
        /// 直接调拨单单提交操作
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="i"></param>
        /// <param name="stringList"></param>
        /// <param name="fid"></param>
        private static void ZJDBD_Submit(string fid)
        {
            // 创建 RootObject 实例
            ClassApi_Submit.RootObject rootObject_Submit = new ClassApi_Submit.RootObject();
            // 赋值 FBillTypeID
            rootObject_Submit = new ClassApi_Submit.RootObject
            {
                Ids = fid,  //直接调拨单单内码
            };

            // 将 Root 对象序列化为 JSON 字符串
            string jsonString = JsonConvert.SerializeObject(rootObject_Submit, Formatting.Indented);

            // 使用webapi引用组件Kingdee.BOS.WebApi.Client.dll
            K3CloudApiClient client = new K3CloudApiClient(ConfigurationManager.AppSettings["K3CloudApi"]);
            var loginResult = client.ValidateLogin(ConfigurationManager.AppSettings["ValidateLogin_id"], ConfigurationManager.AppSettings["ValidateLogin_name"], ConfigurationManager.AppSettings["ValidateLogin_pwd"], 2052);
            var resultType = JObject.Parse(loginResult)["LoginResultType"].Value<int>();
            //string webapijson = "";
            //登录结果类型等于1，代表登录成功
            if (resultType == 1)
            {
                var kdwebapijson = client.Submit("STK_TransferDirect", JsonConvert.SerializeObject(rootObject_Submit, Formatting.Indented));
                SQLHelper.Txt(kdwebapijson, "CC");
                //IsSuccess_ref(dt, i, kdwebapijson, ref fid);
            }
        }

        /// <summary>
        /// 直接调拨单单审核操作
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="i"></param>
        /// <param name="stringList"></param>
        /// <param name="fid"></param>
        private static void ZJDBD_Audit(DataTable dt, int i, string fid)
        {
            // 创建 RootObject 实例
            ClassApi_Audit.RootObject rootObject_Audit = new ClassApi_Audit.RootObject();
            // 赋值 FBillTypeID
            rootObject_Audit = new ClassApi_Audit.RootObject
            {
                Ids = fid,  //直接调拨单内码
            };

            // 将 Root 对象序列化为 JSON 字符串
            string jsonString = JsonConvert.SerializeObject(rootObject_Audit, Formatting.Indented);

            // 使用webapi引用组件Kingdee.BOS.WebApi.Client.dll
            K3CloudApiClient client = new K3CloudApiClient(ConfigurationManager.AppSettings["K3CloudApi"]);
            var loginResult = client.ValidateLogin(ConfigurationManager.AppSettings["ValidateLogin_id"], ConfigurationManager.AppSettings["ValidateLogin_name"], ConfigurationManager.AppSettings["ValidateLogin_pwd"], 2052);
            var resultType = JObject.Parse(loginResult)["LoginResultType"].Value<int>();
            //string webapijson = "";
            //登录结果类型等于1，代表登录成功
            if (resultType == 1)
            {
                var kdwebapijson = client.Audit("STK_TransferDirect", JsonConvert.SerializeObject(rootObject_Audit, Formatting.Indented));
                SQLHelper.Txt(kdwebapijson, "CC");
                IsSuccess(dt, i, kdwebapijson);
            }
        }






        /// <summary>
        /// 盘亏单
        /// </summary>
        private static void Do_PKD()
        {
            DataTable dt = new DataTable();
            dt = Class_Data.Data_List("WMS", "STK_StockCountLoss", false);

            for (int i = 0; i <= dt.Rows.Count - 1; i++)
            {
                try
                {
                    //为空直接跳出本次循环
                    if (Convert.ToString(dt.Rows[i]["DATA"]) == "")
                    { continue; }
                    PKD_Josn.Root root_PKD = PKD_Josn.Return_PKD_Json(Convert.ToString(dt.Rows[i]["DATA"]));
                    string jsonString = JsonConvert.SerializeObject(root_PKD, Formatting.Indented);

                    // 使用webapi引用组件Kingdee.BOS.WebApi.Client.dll
                    K3CloudApiClient client = new K3CloudApiClient(ConfigurationManager.AppSettings["K3CloudApi"]);
                    var loginResult = client.ValidateLogin(ConfigurationManager.AppSettings["ValidateLogin_id"], ConfigurationManager.AppSettings["ValidateLogin_name"], ConfigurationManager.AppSettings["ValidateLogin_pwd"], 2052);
                    var resultType = JObject.Parse(loginResult)["LoginResultType"].Value<int>();
                    //string webapijson = "";
                    string fid = "";   //新增的盘亏单内码
                                       //登录结果类型等于1，代表登录成功
                    if (resultType == 1)
                    {
                        var kdwebapijson = client.Save("STK_StockCountLoss", JsonConvert.SerializeObject(root_PKD, Formatting.Indented));
                        SQLHelper.Txt(kdwebapijson, "CC");
                        IsSuccess_ref(dt, i, kdwebapijson, ref fid);
                    }
                    if (fid == "0") { continue; }
                    else
                    {
                        PKD_Submit(fid);

                        PKD_Audit(dt, i, fid);
                    }
                }
                catch (Exception ex)
                {
                    //异常
                    SQLHelper.Txt("盘亏单" + ex.Message.ToString(), "CC");
                }
            }
        }


        /// <summary>
        /// 盘亏单提交操作
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="i"></param>
        /// <param name="stringList"></param>
        /// <param name="fid"></param>
        private static void PKD_Submit(string fid)
        {
            // 创建 RootObject 实例
            ClassApi_Submit.RootObject rootObject_Submit = new ClassApi_Submit.RootObject();
            // 赋值 FBillTypeID
            rootObject_Submit = new ClassApi_Submit.RootObject
            {
                Ids = fid,  //盘亏单内码
            };

            // 将 Root 对象序列化为 JSON 字符串
            string jsonString = JsonConvert.SerializeObject(rootObject_Submit, Formatting.Indented);

            // 使用webapi引用组件Kingdee.BOS.WebApi.Client.dll
            K3CloudApiClient client = new K3CloudApiClient(ConfigurationManager.AppSettings["K3CloudApi"]);
            var loginResult = client.ValidateLogin(ConfigurationManager.AppSettings["ValidateLogin_id"], ConfigurationManager.AppSettings["ValidateLogin_name"], ConfigurationManager.AppSettings["ValidateLogin_pwd"], 2052);
            var resultType = JObject.Parse(loginResult)["LoginResultType"].Value<int>();
            //string webapijson = "";
            //登录结果类型等于1，代表登录成功
            if (resultType == 1)
            {
                var kdwebapijson = client.Submit("STK_StockCountLoss", JsonConvert.SerializeObject(rootObject_Submit, Formatting.Indented));
                SQLHelper.Txt(kdwebapijson, "CC");
                //IsSuccess_ref(dt, i, kdwebapijson, ref fid);
            }
        }

        /// <summary>
        /// 盘亏单审核操作
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="i"></param>
        /// <param name="stringList"></param>
        /// <param name="fid"></param>
        private static void PKD_Audit(DataTable dt, int i, string fid)
        {
            // 创建 RootObject 实例
            ClassApi_Audit.RootObject rootObject_Audit = new ClassApi_Audit.RootObject();
            // 赋值 FBillTypeID
            rootObject_Audit = new ClassApi_Audit.RootObject
            {
                Ids = fid,  //盘亏单内码
            };

            // 将 Root 对象序列化为 JSON 字符串
            string jsonString = JsonConvert.SerializeObject(rootObject_Audit, Formatting.Indented);

            // 使用webapi引用组件Kingdee.BOS.WebApi.Client.dll
            K3CloudApiClient client = new K3CloudApiClient(ConfigurationManager.AppSettings["K3CloudApi"]);
            var loginResult = client.ValidateLogin(ConfigurationManager.AppSettings["ValidateLogin_id"], ConfigurationManager.AppSettings["ValidateLogin_name"], ConfigurationManager.AppSettings["ValidateLogin_pwd"], 2052);
            var resultType = JObject.Parse(loginResult)["LoginResultType"].Value<int>();
            //string webapijson = "";
            //登录结果类型等于1，代表登录成功
            if (resultType == 1)
            {
                var kdwebapijson = client.Audit("STK_StockCountLoss", JsonConvert.SerializeObject(rootObject_Audit, Formatting.Indented));
                SQLHelper.Txt(kdwebapijson, "CC");
                IsSuccess(dt, i, kdwebapijson);
            }
        }















        /// <summary>
        /// 用于执行完反写入数据后,更新中间表当前数据状态
        /// </summary>
        /// <param name="Fnumber"></param>
        /// <param name="Updateperson"></param>
        private static void UpdData(string FGUID, string Updateperson)
        {
            Class_Data.Update_FBILLNO(FGUID);  //更新数据查询次数
            Class_Data.Update_Data(FGUID, Updateperson, DateTime.Now);  //更新数据
        }

        private void Class_Button_Click(object sender, EventArgs e)
        {
            if (Class_Button.Text == "点击此处启动")
            {
                Class_Button.Text = "启动中...";
                Class_Timer.Enabled = true;

            }
            else
            {
                if (Class_Button.Text == "启动中...")
                {
                    Class_Button.Text = "点击此处启动";
                    Class_Timer.Enabled = false;
                }
            }


        }

        private void 关于ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult MsgBoxResult;//设置对话框的返回值
            MsgBoxResult = System.Windows.Forms.MessageBox.Show("本次操作用于直接执行一次中间表中数据的ERP保存操作，请确认是否继续执行？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2);//定义对话框的按钮式样
            if (MsgBoxResult.ToString() == "Yes")//如果对话框的返回值是YES
            {
                Do_SCLLD(); //生产领料单

                Do_SCTLD(); //生产退料单

                Do_CGRKD(); //采购入库单

                Do_CGTLD(); //采购退料单

                Do_XSCKD(); //销售出库单

                Do_QTRKD(); //其他入库单

                Do_QTCKD(); //其他出库单

                Do_PYD(); //盘盈单

                Do_PKD(); //盘亏单

                Do_ZJDBD();//直接调拨单
            }
            if (MsgBoxResult.ToString() == "No")//如果对话框的返回值是NO
            {
                
            }


        }

        private void PMain_SizeChanged(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)
            {
                this.Hide();
                this.notifyIcon1.Visible = true;
            }

        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            this.Visible = true;
            this.WindowState = FormWindowState.Normal;
            this.notifyIcon1.Visible = false;

        }
    }
}
