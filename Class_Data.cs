using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace PollingServer
{
    public class Class_Data
    {
        /// <summary>
        /// 向中间表写入数据
        /// </summary>
        /// <param name="FBILLNO">数据编号</param>
        /// <param name="SOURCESYSTEM">来源系统</param>
        /// <param name="FORMID">单据类型</param>
        /// <param name="NUMBER">单据编号</param>
        /// <param name="DATA">单据数据</param>
        /// <param name="ADDPERSON">新增人</param>
        /// <param name="ADDDATE">新增时间</param>
        /// <param name="UPDATEPERSON">数据更新人</param>
        /// <param name="UPDATEDATE">更新时间</param>
        /// <param name="PROORDERNUM">生产订单编号</param>
        /// <param name="PROORDERLINENUM">生产订单行号</param>
        /// <param name="FIELD">字段中文</param>
        /// <param name="EFIELD">字段英文</param>
        /// <param name="DATATYPE">数据类型</param>
        /// <param name="tran"></param>
        /// <returns></returns>
        public static int Add_XXDV_t_Intermediate(string FBILLNO, string SOURCESYSTEM, string FORMID, string NUMBER, string DATA, string ADDPERSON, DateTime ADDDATE, string UPDATEPERSON, DateTime UPDATEDATE, string PROORDERNUM, string PROORDERLINENUM, string FIELD, string EFIELD, string DATATYPE, SqlTransaction tran)
        {
            int i = 0;//返回结果
            string sql = "insert into XXDV_t_Intermediate(FBILLNO,FDOCUMENTSTATUS,SOURCESYSTEM,FORMID,NUMBER,DATA,ADDPERSON,ADDDATE,UPDATEPERSON,UPDATEDATE,READE,ISHANDLEE,PROORDERNUM,PROORDERLINENUM,FIELD,EFIELD,DATATYPE) " +
                "values('" + FBILLNO + "','1','" + SOURCESYSTEM + "','" + FORMID + "','" + NUMBER + "','" + DATA + "','" + ADDPERSON + "','" + ADDDATE + "','" + UPDATEPERSON + "','" + UPDATEDATE + "','0','0','" + PROORDERNUM + "','" + PROORDERLINENUM + "','" + FIELD + "','" + EFIELD + "','" + DATATYPE + "')";
            i += SQLHelper.ExecuteNonQuery(tran, (CommandType)1, sql);
            return i;
        }

        /// <summary>
        /// 获取最新单据数据集合
        /// </summary>
        /// <param name="SOURCESYSTEM"></param>
        /// <param name="FORMID"></param>
        /// <param name="ISHANDLEE"></param>
        /// <returns></returns>
        public static DataTable Data_List(string SOURCESYSTEM, string FORMID, Boolean ISHANDLEE)
        {
            string sql = "SELECT FBILLNO,SOURCESYSTEM,FORMID,NUMBER,DATA,ADDPERSON,ADDDATE,UPDATEPERSON,UPDATEDATE,READE,ISHANDLEE,FIELD,EFIELD,DATATYPE FROM XXDV_t_Intermediate t1 WHERE (t1.ADDDATE) IN ( SELECT MAX(ADDDATE) FROM XXDV_t_Intermediate t2 WHERE t1.NUMBER = t2.NUMBER  and t2.SOURCESYSTEM='" + SOURCESYSTEM + "' and t2.FORMID='" + FORMID + "'  and t2.ISHANDLEE='" + ISHANDLEE + "'  GROUP BY NUMBER )  and t1.SOURCESYSTEM='" + SOURCESYSTEM + "' and t1.FORMID='" + FORMID + "'  and t1.ISHANDLEE='" + ISHANDLEE + "'";
            DataTable dt = new DataTable();
            dt = SQLHelper.ExecuteDataTable(sql);
            return dt;
        }


        /// <summary>
        /// 使用生产用料清单代号和物料编码查询生产用料清单内码
        /// </summary>
        /// <param name="FBillNo"></param>
        /// <param name="FNumber"></param>
        /// <returns></returns>
        public static string T_PRD_PPBOMENTRY_FID(string FBillNo, string FNumber)
        {
            string sql = "select T_PRD_PPBOMENTRY.FID, T_PRD_PPBOMENTRY.FENTRYID from T_PRD_PPBOMENTRY, T_PRD_PPBOMENTRY_C, T_PRD_PPBOM, T_BD_MATERIAL where T_PRD_PPBOMENTRY.FENTRYID = T_PRD_PPBOMENTRY_C.FENTRYID  and T_PRD_PPBOM.FID = T_PRD_PPBOMENTRY.FID and T_BD_MATERIAL.FMATERIALID = T_PRD_PPBOMENTRY.FMATERIALID and T_PRD_PPBOM.FBillNo = '"+ FBillNo + "' and T_BD_MATERIAL.FNumber = '"+ FNumber + "'";
            DataTable dt = new DataTable();
            dt = SQLHelper.ExecuteDataTable(sql);
            string FID = dt.Rows[0][0].ToString();
            return FID;
        }

        /// <summary>
        /// 使用生产领料单代号查询生产领料单内码
        /// </summary>
        /// <param name="FNumber"></param>
        /// <returns></returns>
        public static string T_PRD_PICKMTRL_FID(string FNumber)
        {
            string sql = "select FID from T_PRD_PICKMTRL where FBillNo='"+ FNumber + "'";
            DataTable dt = new DataTable();
            dt = SQLHelper.ExecuteDataTable(sql);
            string FID = dt.Rows[0]["FID"].ToString();
            return FID;
        }

        /// <summary>
        /// 使用生产用料清单代号和物料编码查询生产用料清单单据体行内码
        /// </summary>
        /// <param name="FBillNo"></param>
        /// <param name="FNumber"></param>
        /// <returns></returns>
        public static string T_PRD_PPBOMENTRY_FENTRYID(string FBillNo, string FNumber)
        {
            string sql = "select T_PRD_PPBOMENTRY.FID, T_PRD_PPBOMENTRY.FENTRYID from T_PRD_PPBOMENTRY, T_PRD_PPBOMENTRY_C, T_PRD_PPBOM, T_BD_MATERIAL where T_PRD_PPBOMENTRY.FENTRYID = T_PRD_PPBOMENTRY_C.FENTRYID  and T_PRD_PPBOM.FID = T_PRD_PPBOMENTRY.FID and T_BD_MATERIAL.FMATERIALID = T_PRD_PPBOMENTRY.FMATERIALID and T_PRD_PPBOM.FBillNo = '" + FBillNo + "' and T_BD_MATERIAL.FNumber = '" + FNumber + "'";
            DataTable dt = new DataTable();
            dt = SQLHelper.ExecuteDataTable(sql);
            string FENTRYID = dt.Rows[0][1].ToString();
            return FENTRYID;
        }


        /// <summary>
        /// 使用生产补料单内码和物料编码查询生产补料单清单单据体行内码
        /// </summary>
        /// <param name="FID"></param>
        /// <param name="FNumber"></param>
        /// <returns></returns>
        public static string T_PRD_FEEDMTRLDATA_FENTRYID(string FID, string FNumber)
        {
            string sql = "select T_PRD_FEEDMTRLDATA.FENTRYID from T_PRD_FEEDMTRLDATA, T_BD_MATERIAL  where FID='"+ FID + "'and T_BD_MATERIAL.FMATERIALID = T_PRD_FEEDMTRLDATA.FMATERIALID and T_BD_MATERIAL.FNumber='"+ FNumber + "'";
            DataTable dt = new DataTable();
            dt = SQLHelper.ExecuteDataTable(sql);
            string FENTRYID = dt.Rows[0]["FENTRYID"].ToString();
            return FENTRYID;
        }

        /// <summary>
        /// 使用生产退料单内码和物料编码查询生产退料单清单单据体行内码
        /// </summary>
        /// <param name="FID"></param>
        /// <param name="FNumber"></param>
        /// <returns></returns>
        public static string T_PRD_RETURNMTRLENTRY_FENTRYID(string FID, string FNumber)
        {
            string sql = "select T_PRD_RETURNMTRLENTRY.FENTRYID from T_PRD_RETURNMTRLENTRY ,T_BD_MATERIAL where T_BD_MATERIAL.FMATERIALID = T_PRD_RETURNMTRLENTRY.FMATERIALID and fid='"+ FID + "' and T_BD_MATERIAL.FNumber='"+ FNumber + "'";
            DataTable dt = new DataTable();
            dt = SQLHelper.ExecuteDataTable(sql);
            string FENTRYID = dt.Rows[0]["FENTRYID"].ToString();
            return FENTRYID;
        }

        /// <summary>
        /// 更新数据查询次数
        /// </summary>
        /// <param name="FBILLNO"></param>
        /// <returns></returns>
        public static int Update_FBILLNO(string FBILLNO)
        {
            int i = 0;//返回结果
            string sql = "update  XXDV_t_Intermediate set READE=READE+1 where FBILLNO='" + FBILLNO + "'";
            i += SQLHelper.ExecuteNonQuery(sql);
            return i;
        }

        /// <summary>
        /// 更新数据是否更新,记录更新人更新时间
        /// </summary>
        /// <param name="FBILLNO"></param>
        /// <param name="UPDATEPERSON"></param>
        /// <param name="UPDATEDATE"></param>
        /// <returns></returns>
        public static int Update_Data(string FBILLNO, string UPDATEPERSON, DateTime UPDATEDATE)
        {
            int i = 0;//返回结果
            string sql = "update  XXDV_t_Intermediate set ISHANDLEE='true',UPDATEPERSON='" + UPDATEPERSON + "',UPDATEDATE='" + UPDATEDATE + "' where FBILLNO='" + FBILLNO + "'";
            i += SQLHelper.ExecuteNonQuery(sql);
            return i;
        }

        public static string returntable(string a, string b, object d)
        {
            ReturnData retData = new ReturnData();
            retData.code = a;
            retData.msg = b;
            retData.data = d;
            return JsonConvert.SerializeObject(retData);
        }

        public class ReturnData
        {
            public string code { get; set; }
            public string msg { get; set; }
            public object data { get; set; }
            public string userid { get; set; }
            public string stationid { get; set; }
            public FileStream file { get; set; }
            public string filename { get; set; }
            public HttpResponseMessage response { get; set; }
        }
    }
}
