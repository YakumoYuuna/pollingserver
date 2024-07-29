using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PollingServer
{
    public class SQLHelper
    {
        private static readonly string connStr = ConfigurationManager.ConnectionStrings["connecttest"].ConnectionString;
        private static readonly string strPATH = ConfigurationManager.AppSettings["PATH"];
        /// <summary>
        ///  //1.执行增、删、改的方法：ExecuteNonQuery
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public static int ExecuteNonQuery(string sql)
        {
            using (SqlConnection con = new SqlConnection(connStr))
            {
                using (SqlCommand cmd = new SqlCommand(sql, con))
                {
                    con.Open();
                    return cmd.ExecuteNonQuery();
                }
            }
        }

        /// <summary>
        /// 执行增加二进制数据方法
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="byData"></param>
        /// <returns></returns>
        public static int ExecuteReaderBinary(string sql, Byte[] byData)
        {
            using (SqlConnection con = new SqlConnection(connStr))
            {
                using (SqlCommand cmd = new SqlCommand(sql, con))
                {
                    con.Open();
                    cmd.Parameters.Add("@file", SqlDbType.Binary, byData.Length);
                    cmd.Parameters["@file"].Value = byData;
                    return cmd.ExecuteNonQuery();
                }
            }
        }

        /// <summary>
        /// 包含事务的增删改查方法 （commandType默认为1）
        /// </summary>
        /// <param name="transaction"></param>
        /// <param name="commandType"></param>
        /// <param name="commandText"></param>
        /// <param name="commandParameters"></param>
        /// <returns></returns>
        public static int ExecuteNonQuery_Binary(SqlTransaction transaction, CommandType commandType, string commandText, Byte[] byData, params SqlParameter[] commandParameters)
        {
            if (transaction == null) throw new ArgumentNullException("transaction");
            if (transaction != null && transaction.Connection == null) throw new ArgumentException("The transaction was rollbacked or commited, please provide an open transaction.", "transaction");//1
                                                                                                                                                                                                     // 预处理
            SqlCommand cmd = new SqlCommand();
            cmd.Parameters.Add("@file", SqlDbType.Binary, byData.Length);
            cmd.Parameters["@file"].Value = byData;
            bool mustCloseConnection = false;
            PrepareCommand(cmd, transaction.Connection, transaction, commandType, commandText, commandParameters, out mustCloseConnection);//2
                                                                                                                                           // 执行
            int retval = cmd.ExecuteNonQuery();//3
                                               // 清除参数集,以便再次使用.
            cmd.Parameters.Clear();//4
            return retval;//5
        }

        /// <summary>
        /// //2.封装一个执行返回单个对象的方法：ExecuteScalar()
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="pms"></param>
        /// <returns></returns>
        public static object ExecuteScalar(string sql, params SqlParameter[] pms)
        {
            using (SqlConnection con = new SqlConnection(connStr))
            {
                using (SqlCommand cmd = new SqlCommand(sql, con))
                {
                    if (pms != null)
                    {
                        cmd.Parameters.AddRange(pms);
                    }
                    con.Open();
                    return cmd.ExecuteScalar();
                }
            }
        }

        /// <summary>
        /// //3.执行查询多行多列的数据的方法：ExecuteReader
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="pms"></param>
        /// <returns></returns>
        public static SqlDataReader ExecuteReader(string sql, params SqlParameter[] pms)
        {
            SqlConnection con = new SqlConnection(connStr);
            using (SqlCommand cmd = new SqlCommand(sql, con))
            {
                if (pms != null)
                {
                    cmd.Parameters.AddRange(pms);
                }
                try
                {
                    con.Open();
                    return cmd.ExecuteReader(CommandBehavior.CloseConnection);
                }
                catch (Exception)
                {
                    con.Close();
                    con.Dispose();
                    throw;
                }
            }
        }

        /// <summary>
        /// //4.执行返回DataTable的方法
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="pms"></param>
        /// <returns></returns>
        public static DataTable ExecuteDataTable(string sql, params SqlParameter[] pms)
        {
            DataTable dt = new DataTable();
            using (SqlDataAdapter adapter = new SqlDataAdapter(sql, connStr))
            {
                if (pms != null)
                {
                    adapter.SelectCommand.Parameters.AddRange(pms);
                }
                adapter.Fill(dt);
            }
            return dt;
        }

        /// <summary>
        /// 事务
        /// </summary>
        /// <returns></returns>
        public static SqlTransaction BeginTransaction()
        {
            SqlConnection connection = new SqlConnection(connStr);
            connection.Open();
            SqlTransaction tran = connection.BeginTransaction();
            return tran;
        }

        /// <summary>
        /// 包含事务的增删改查方法 （commandType默认为1）
        /// </summary>
        /// <param name="transaction"></param>
        /// <param name="commandType"></param>
        /// <param name="commandText"></param>
        /// <param name="commandParameters"></param>
        /// <returns></returns>
        public static int ExecuteNonQuery(SqlTransaction transaction, CommandType commandType, string commandText, params SqlParameter[] commandParameters)
        {
            if (transaction == null) throw new ArgumentNullException("transaction");
            if (transaction != null && transaction.Connection == null) throw new ArgumentException("The transaction was rollbacked or commited, please provide an open transaction.", "transaction");//1
                                                                                                                                                                                                     // 预处理
            SqlCommand cmd = new SqlCommand();
            bool mustCloseConnection = false;
            PrepareCommand(cmd, transaction.Connection, transaction, commandType, commandText, commandParameters, out mustCloseConnection);//2
                                                                                                                                           // 执行
            int retval = cmd.ExecuteNonQuery();//3
                                               // 清除参数集,以便再次使用.
            cmd.Parameters.Clear();//4
            return retval;//5
        }

        /// <summary>
        /// 将SqlParameter参数数组(参数值)分配给SqlCommand命令.
        /// 这个方法将给任何一个参数分配DBNull.Value;
        /// 该操作将阻止默认值的使用.
        /// </summary>
        /// <param name="command">命令名</param>
        /// <param name="commandParameters">SqlParameters数组</param>
        private static void AttachParameters(SqlCommand command, SqlParameter[] commandParameters)
        {
            if (command == null) throw new ArgumentNullException("command");
            if (commandParameters != null)
            {
                foreach (SqlParameter p in commandParameters)
                {
                    if (p != null)
                    {
                        // 检查未分配值的输出参数,将其分配以DBNull.Value.
                        if ((p.Direction == ParameterDirection.InputOutput || p.Direction == ParameterDirection.Input) &&
                            (p.Value == null))
                        {
                            p.Value = DBNull.Value;
                        }
                        command.Parameters.Add(p);
                    }
                }
            }
        }

        /// <summary>                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                             
        /// 预处理用户提供的命令,数据库连接/事务/命令类型/参数
        /// </summary>
        /// <param name="command">要处理的SqlCommand</param>
        /// <param name="connection">数据库连接</param>
        /// <param name="transaction">一个有效的事务或者是null值</param>
        /// <param name="commandType">命令类型 (存储过程,命令文本, 其它.)</param>
        /// <param name="commandText">存储过程名或都T-SQL命令文本</param>
        /// <param name="commandParameters">和命令相关联的SqlParameter参数数组,如果没有参数为'null'</param>
        /// <param name="mustCloseConnection"><c>true</c> 如果连接是打开的,则为true,其它情况下为false.</param>
        private static void PrepareCommand(SqlCommand command, SqlConnection connection, SqlTransaction transaction, CommandType commandType, string commandText, SqlParameter[] commandParameters, out bool mustCloseConnection)
        {
            if (command == null) throw new ArgumentNullException("command");
            if (commandText == null || commandText.Length == 0) throw new ArgumentNullException("commandText");

            // If the provided connection is not open, we will open it
            if (connection.State != ConnectionState.Open)
            {
                mustCloseConnection = true;
                connection.Open();
            }
            else
            {
                mustCloseConnection = false;
            }

            // 给命令分配一个数据库连接.
            command.Connection = connection;

            // 设置命令文本(存储过程名或SQL语句)
            command.CommandText = commandText;

            // 分配事务
            if (transaction != null)
            {
                if (transaction.Connection == null) throw new ArgumentException("The transaction was rollbacked or commited, please provide an open transaction.", "transaction");
                command.Transaction = transaction;
            }

            // 设置命令类型.
            command.CommandType = commandType;

            // 分配命令参数
            if (commandParameters != null)
            {
                AttachParameters(command, commandParameters);
            }
            return;
        }

        /// <summary>
        /// 执行使用不同数据库连接返回DataTable的方法
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="connStr"></param>
        /// <param name="pms"></param>
        /// <returns></returns>
        public static DataTable ExecuteDataTable(string sql, string connStr, params SqlParameter[] pms)
        {
            DataTable dt = new DataTable();
            using (SqlDataAdapter adapter = new SqlDataAdapter(sql, connStr))
            {
                if (pms != null)
                {
                    adapter.SelectCommand.Parameters.AddRange(pms);
                }
                adapter.Fill(dt);
            }
            return dt;
        }

        //public static string RootPath_MVC
        //{
        //    get { return System.Web.HttpContext.Current.Server.MapPath("~"); }
        //}
        //create Directory
        public static bool CreateDirectoryIfNotExist(string filePath)
        {
            if (!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);
            }
            return true;
        }

        public static object IsSuccess_Msg_Data_HttpCode(bool isSuccess, string msg, dynamic data, HttpStatusCode httpCode = HttpStatusCode.OK)
        {
            return new { isSuccess = isSuccess, msg = msg, httpCode = httpCode, data = data };
        }
        public static object Success_Msg_Data_DCount_HttpCode(string msg, dynamic data = null, int dataCount = 0, HttpStatusCode httpCode = HttpStatusCode.OK)
        {
            return new { isSuccess = true, msg = msg, httpCode = httpCode, data = data, dataCount = dataCount };
        }
        public static object Error_Msg_Ecode_Elevel_HttpCode(string msg, int errorCode = 0, int errorLevel = 0, HttpStatusCode httpCode = HttpStatusCode.InternalServerError)
        {
            return new { isSuccess = false, msg = msg, httpCode = httpCode, errorCode = errorCode, errorLevel = errorLevel };
        }

        private static string CmdPath = @"C:\Windows\System32\cmd.exe";

        /// <summary>
        /// 执行cmd命令
        /// 多命令请使用批处理命令连接符：
        /// <![CDATA[
        /// &:同时执行两个命令
        /// |:将上一个命令的输出,作为下一个命令的输入
        /// &&：当&&前的命令成功时,才执行&&后的命令
        /// ||：当||前的命令失败时,才执行||后的命令]]>
        /// 其他请百度
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="output"></param>
        public static void RunCmd(string cmd, out string output)
        {
            cmd = cmd.Trim().TrimEnd('&') + "&exit";//说明：不管命令是否成功均执行exit命令，否则当调用ReadToEnd()方法时，会处于假死状态
            using (Process p = new Process())
            {
                p.StartInfo.FileName = CmdPath;
                p.StartInfo.UseShellExecute = false;        //是否使用操作系统shell启动
                p.StartInfo.RedirectStandardInput = true;   //接受来自调用程序的输入信息
                p.StartInfo.RedirectStandardOutput = true;  //由调用程序获取输出信息
                p.StartInfo.RedirectStandardError = true;   //重定向标准错误输出
                p.StartInfo.CreateNoWindow = true;          //不显示程序窗口
                p.Start();//启动程序

                //向cmd窗口写入命令
                p.StandardInput.WriteLine(cmd);
                p.StandardInput.AutoFlush = true;

                //获取cmd窗口的输出信息
                output = p.StandardOutput.ReadToEnd();
                p.WaitForExit();//等待程序执行完退出进程
                p.Close();
            }
        }

        /// <summary>
        /// 字符串规范化
        /// </summary>
        /// <param name="inputData"></param>
        /// <returns></returns>
       public  static string AddQuotes(string inputData)
        {
            // 按逗号拆分字符串
            string[] elements = inputData.Split(',');

            // 遍历每个元素，在非符号的元素周围添加引号
            for (int i = 0; i < elements.Length; i++)
            {
                // 去除可能包含的引号等字符
                string cleanedElement = elements[i].Replace("\"", "").Trim();

                // 判断是否为符号
                if (cleanedElement != "")
                {
                    // 判断元素中是否以 [[ 开头，如果是，将引号添加在 [[ 的后面
                    if (cleanedElement.StartsWith("[["))
                    {
                        elements[i] = $"[[\"{cleanedElement.Substring(2)}\"";
                    }
                    // 判断元素中是否以 ]] 结尾，如果是，将引号添加在 ]] 的前面
                    else if (cleanedElement.EndsWith("]]"))
                    {
                        elements[i] = $"\"{cleanedElement.Substring(0, cleanedElement.Length - 2)}\"]]";
                    }
                    // 判断元素中是否以 [ 开头，如果是，将引号添加在 [ 的后面
                    else if (cleanedElement.StartsWith("["))
                    {
                        elements[i] = $"[\"{cleanedElement.Substring(1)}\"";
                    }
                    // 判断元素中是否以 ] 结尾，如果是，将引号添加在 ] 的前面
                    else if (cleanedElement.EndsWith("]"))
                    {
                        elements[i] = $"\"{cleanedElement.Substring(0, cleanedElement.Length - 1)}\"]";
                    }
                    else
                    {
                        // 否则直接添加引号
                        elements[i] = $"\"{cleanedElement}\"";
                    }
                }
            }

            // 重新组合字符串
            string resultData = string.Join(",", elements);

            return resultData;
        }

        /// <summary>
        /// 保存txt文档
        /// </summary>
        /// <param name="zhi">保存内容</param>
        /// <param name="tongdao">通道</param>
        public static void Txt(string txt, string tongdao)
        {
            string path = strPATH;//日志文件路径&配置到Config文件中直接获取
            string filename = DateTime.Now.ToString("yyyy-MM-dd") + ".txt";//文件名
            string year = DateTime.Now.ToString("yyyy-MM");//年月日文件夹
            string passageway = tongdao;//通道文件夹


            //判断log文件路径是否存在，不存在则创建文件夹 
            if (!System.IO.Directory.Exists(path))
            {
                System.IO.Directory.CreateDirectory(path);//不存在就创建目录 
            }

            path += passageway + "\\";
            //判断通道文件夹是否存在，不存在则创建文件夹 
            if (!System.IO.Directory.Exists(path))
            {
                System.IO.Directory.CreateDirectory(path);//不存在就创建目录 
            }

            path += year + "\\";
            //判断年月文件夹是否存在，不存在则创建文件夹 
            if (!System.IO.Directory.Exists(path))
            {
                System.IO.Directory.CreateDirectory(path);//不存在就创建目录 
            }

            //拼接完整文件路径
            path += filename;
            if (!File.Exists(path))
            {
                //文件不存在，新建文件
                FileStream fs = new FileStream(path, FileMode.OpenOrCreate);
                StreamWriter sw = new StreamWriter(fs);
                sw.Close();
            }

            using (FileStream fs = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Write))
            {
                using (StreamWriter sw = new StreamWriter(fs))
                {
                    sw.BaseStream.Seek(0, SeekOrigin.End);
                    //sw.Write("操作时间：" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "  内容：{0}\r\r\r", txt, DateTime.Now);
                    sw.Write("操作时间：" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "  内容：\r\n{0}\r\n\r\n\r\n\r\n", txt);
                    sw.Flush();
                }
            }
        }
    }
}
