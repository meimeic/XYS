using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace XYS.Lis.Report.Util
{
    public class PUser
    {
        #region 静态字段
        private static readonly Hashtable User2UrlMap;
        private static readonly Hashtable UserImageMap;
        private static readonly string ImageServer;
        private static readonly string ConnectionString;
        #endregion

        #region 构造函数
        static PUser()
        {
            User2UrlMap = new Hashtable(30);
            UserImageMap = new Hashtable(50);
            ImageServer = ConfigurationManager.AppSettings["LabImageServer"].ToString();
            ConnectionString = ConfigurationManager.ConnectionStrings["LabMSSQL"].ConnectionString;

            InitUserUrlMap();
            InitUserImageMap();
        }
        #endregion

        #region  公共方法
        public static byte[] GetUserImage(string userName)
        {
            return UserImageMap[userName] as byte[];
        }
        public static string GetUserUrl(string name)
        {
            //string path = User2ImageMap[name] as string;
            //if (!string.IsNullOrEmpty(path))
            //{
            //    return ImageServer + path;
            //}
            //return null;
            return ImageServer + "/user/" + name + ".jpg";
        }
        #endregion

        #region 初始化
        private static void InitUserImageMap()
        {
            string sql = "select cname,userimage from PUser where userimage is not null";
            DataTable dt = GetDataTable(sql);
            UserImageMap.Clear();
            foreach (DataRow dr in dt.Rows)
            {
                UserImageMap.Add(dr["cname"].ToString(), (byte[])dr["userimage"]);
            }
        }
        private static DataTable GetDataTable(string sql)
        {
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                DataSet ds = new DataSet();
                try
                {
                    con.Open();
                    SqlDataAdapter da = new SqlDataAdapter(sql, con);
                    da.Fill(ds, "dt");
                    return ds.Tables["dt"];
                }
                catch (Exception e)
                {
                    throw new Exception(e.Message);
                }
            }
        }
        private static void InitUserUrlMap()
        {
        }
        #endregion
    }
}