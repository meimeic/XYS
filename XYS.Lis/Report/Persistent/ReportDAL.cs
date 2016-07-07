using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

using XYS.Persistent;
namespace XYS.Lis.Report.Persistent
{
    public class ReportDAL : ReportCommonDAL
    {
        private readonly string m_connectionString;
        public ReportDAL()
            : base()
        {
            this.m_connectionString = ConfigurationManager.ConnectionStrings["LisReport"].ConnectionString;
        }
        protected override DataTable GetDataTable(string sql)
        {
            DataTable dt = null;
            if (!string.IsNullOrEmpty(sql))
            {
                dt = this.Query(sql).Tables["dt"];
            }
            return dt;
        }
        private DataSet Query(string SQLString)
        {
            using (SqlConnection con = new SqlConnection(this.m_connectionString))
            {
                DataSet ds = new DataSet();
                try
                {
                    con.Open();
                    SqlDataAdapter da = new SqlDataAdapter(SQLString, con);
                    da.Fill(ds, "dt");
                }
                catch (SqlException e)
                {
                    throw new Exception(e.Message);
                }
                return ds;
            }
        }
    }
}
