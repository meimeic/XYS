using System;
using System.Text;
using System.Data;
using System.Reflection;
using System.Collections;
using System.Configuration;
using System.Data.SqlClient;
using System.Collections.Generic;

using log4net;

using XYS.Report;
using XYS.Lis.Report;
namespace XYS.Lis.Report.Persistent
{
    public class LabPKDAL
    {
        private readonly string m_connectionString;
        private static readonly ILog LOG = LogManager.GetLogger("LabReport");
        public LabPKDAL()
        {
            this.m_connectionString = ConfigurationManager.ConnectionStrings["LabMSSQL"].ConnectionString;
        }
        public void InitKey(Require require, List<LabPK> PKList)
        {
            LabPK pk = null;
            string sql = GetSQLString(require.ToString());

            DataTable dt = GetDataTable(sql);
            if (dt == null)
            {
                LOG.Error("获取主键集合失败");
                return;
            }
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    pk = new LabPK();
                    SetKey(dr, pk);
                    pk.Configured = true;
                    PKList.Add(pk);
                }
            }
        }
        public void InitKey(string where, List<LabPK> PKList)
        {
            LabPK pk = null;
            string sql = GetSQLString(where);
            LOG.Info("查询主键SQL语句:" + sql);
            DataTable dt = GetDataTable(sql);
            if (dt == null)
            {
                LOG.Error("获取主键集合失败");
                return;
            }
            if (dt.Rows.Count > 0)
            {
                LOG.Info("填充主键集合");
                foreach (DataRow dr in dt.Rows)
                {
                    pk = new LabPK();
                    if (SetKey(dr, pk))
                    {
                        pk.Configured = true;
                        PKList.Add(pk);
                    }
                    else
                    {
                        LOG.Warn("主键集合数据填充有错误项");
                    }
                }
            }
        }
        //基因配型特殊代码处理
        public void InitKey(LabPK PK, List<LabPK> PKList)
        {
            LOG.Info("利用通用主键获取基因配型特殊主键");
            string sql = "select patno,paritemname from reportform" + PK.KeyWhere();
            DataTable dt = GetDataTable(sql);
            if (dt.Rows.Count > 0)
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(" where");
                sb.Append(" receivedate='");
                sb.Append(PK.ReceiveDate.ToString("yyyy-MM-dd"));
                sb.Append("' and sectionno=");
                sb.Append(PK.SectionNo);
                sb.Append(" and testtypeno=");
                sb.Append(PK.TestTypeNo);
                sb.Append(" and patno='");
                sb.Append(dt.Rows[0]["patno"].ToString());
                sb.Append("' and paritemname='");
                sb.Append(dt.Rows[0]["paritemname"].ToString());
                sb.Append("'");
                string where = sb.ToString();
                LOG.Info("获取基因配型特殊主键的where语句" + where);
                this.InitKey(where, PKList);
            }
        }
        protected bool SetKey(DataRow dr, LabPK PK)
        {
            try
            {
                PK.SampleNo = dr["sampleno"].ToString();
                PK.ReceiveDate = (DateTime)dr["receivedate"];
                PK.SectionNo = Convert.ToInt32(dr["sectionno"]);
                PK.TestTypeNo = Convert.ToInt32(dr["testtypeno"]);
                return true;
            }
            catch (Exception ex)
            {
                LOG.Error("填充主键数据异常", ex);
                return false;
            }
        }
        protected string GetSQLString(string where)
        {
            return "select receivedate,sectionno,testtypeno,sampleno from reportform " + where;
        }
        protected DataTable GetDataTable(string sql)
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
                DataSet ds = new DataSet();
                try
                {
                    con.Open();
                    SqlDataAdapter da = new SqlDataAdapter(SQLString, con);
                    da.Fill(ds, "dt");
                    return ds;
                }
                catch (SqlException e)
                {
                    LOG.Error("查询语句" + SQLString + "执行异常", e);
                    return null;
                }
            }
        }
        private bool IsExist(Dictionary<string, object> dic)
        {
            if (dic != null && dic.Count > 0)
            {
                return true;
            }
            return false;
        }
    }
}