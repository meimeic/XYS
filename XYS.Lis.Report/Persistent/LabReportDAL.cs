using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

using XYS.Persistent;
namespace XYS.Lis.Report.Persistent
{
    public class LabReportDAL : ReportDAL
    {
        private readonly string m_connectionString;
        public LabReportDAL()
            : base()
        {
            this.m_connectionString = ConfigurationManager.ConnectionStrings["LabMSSQL"].ConnectionString;
        }

        protected override DataTable GetDataTable(string sql)
        {
            DataSet ds = this.Query(sql);
            if (ds != null)
            {
                return ds.Tables["dt"];
            }
            return null;
        }
        private DataSet Query(string SQLString)
        {
            using (SqlConnection con = new SqlConnection(this.m_connectionString))
            {
                try
                {
                    DataSet ds = new DataSet();
                    con.Open();
                    SqlDataAdapter da = new SqlDataAdapter(SQLString, con);
                    da.Fill(ds, "dt");
                    return ds;
                }
                catch (Exception e)
                {
                    LOG.Error("查询数据库失败", e);
                    return null;
                }
            }
        }
    }
}
