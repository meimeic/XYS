﻿using System;
using System.Text;
using System.Data;
using System.Reflection;
using System.Collections;
using System.Configuration;
using System.Data.SqlClient;
using System.Collections.Generic;

using XYS.Report;
namespace XYS.Lis.Report.Persistent
{
    public class LabPKDAL
    {
        private readonly string m_connectionString;
        public LabPKDAL()
        {
            this.m_connectionString = ConfigurationManager.ConnectionStrings["LabMSSQL"].ConnectionString;
        }

        public void InitKey(Require require, LabPK PK)
        {
            string sql = GetSQLString(require.ToString());
            DataTable dt = GetDataTable(sql);
            if (dt != null && dt.Rows.Count > 0)
            {
                SetKey(dt.Rows[0], PK);
                PK.Configured = true;
            }
        }
        public void InitKey(Require require, List<LabPK> PKList)
        {
            LabPK temp;
            string sql = GetSQLString(require.ToString());

            DataTable dt = GetDataTable(sql);
            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    temp = new LabPK();
                    SetKey(dr, temp);
                    temp.Configured = true;
                    PKList.Add(temp);
                }
            }
        }
        public void InitKey(string where, List<LabPK> PKList)
        {
            LabPK temp = null;
            string sql = GetSQLString(where);
            DataTable dt = GetDataTable(sql);
            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    temp = new LabPK();
                    SetKey(dr, temp);
                    temp.Configured = true;
                    PKList.Add(temp);
                }
            }
        }
        //基因配型特殊代码处理
        public void InitKey(LabPK PK, List<LabPK> PKList)
        {
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
                this.InitKey(where, PKList);
            }
        }
        protected void SetKey(DataRow dr, LabPK PK)
        {
            PK.SampleNo = dr["sampleno"].ToString();
            PK.ReceiveDate = (DateTime)dr["receivedate"];
            PK.SectionNo = Convert.ToInt32(dr["sectionno"]);
            PK.TestTypeNo = Convert.ToInt32(dr["testtypeno"]);
        }
        protected string GetSQLString(string where)
        {
            return "select receivedate,sectionno,testtypeno,sampleno from reportform " + where;
        }
        protected DataTable GetDataTable(string sql)
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
