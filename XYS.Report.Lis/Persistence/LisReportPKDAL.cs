﻿using System;
using System.Text;
using System.Data;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;

using XYS.Report.Lis;
using XYS.Report.Lis.Core;
namespace XYS.Report.Lis.Persistence
{
    public class LisReportPKDAL
    {
        public List<LisReportPK> GetReportKey(Require require)
        {
            LisReportPK temp;
            List<LisReportPK> result = new List<LisReportPK>();
            string sql = GetSQLString(require);
            DataTable dt = GetDataTable(sql);
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    temp = new LisReportPK();
                    SetReportKey(dr, temp);
                    result.Add(temp);
                }
            }
            return result;
        }
        protected string GetSQLString(Require require)
        {
            StringBuilder sb = new StringBuilder();
            string temp;
            sb.Append("select top ");
            sb.Append(require.MaxRecord.ToString());
            sb.Append(" receivedate,sectionno,testtypeno,sampleno from reportform where patno is not null and patno<>'' and ");
            //相等条件
            temp = GetWhereStr(require.EqualFields, 1);
            if (!temp.Equals(""))
            {
                sb.Append(temp);
            }
            //不等条件
            temp = GetWhereStr(require.NoEqualFields, 2);
            if (!temp.Equals(""))
            {
                sb.Append(" and ");
                sb.Append(temp);
            }
            //like 条件
            temp = GetWhereStr(require.LikeFields, 3);
            if (!temp.Equals(""))
            {
                sb.Append(" and ");
                sb.Append(temp);
            }
            //时间限制
            if (require.DateLimit)
            {
                sb.Append(" and checkdate>'" + require.StartDateTime.AddDays(-1).ToString("yyyy-MM-dd"));
                sb.Append("' and checkdate<'" + require.EndDateTime.AddDays(1).ToString("yyyy-MM-dd"));
                sb.Append("'");
            }
            return sb.ToString();
        }
        protected string GetWhereStr(Dictionary<string, object> dic, int flag)
        {
            string equalFilter = "";
            switch (flag)
            {
                case 1:
                    equalFilter = "=";
                    break;
                case 2:
                    equalFilter = "!=";
                    break;
                case 3:
                    equalFilter = " like ";
                    break;
                default:
                    break;
            }
            //
            StringBuilder sb = new StringBuilder();
            if (IsExist(dic))
            {
                foreach (KeyValuePair<string, object> item in dic)
                {
                    //int
                    if (item.Value.GetType().FullName == "System.Int32")
                    {
                        sb.Append(item.Key);
                        sb.Append(equalFilter);
                        sb.Append(item.Value);
                    }
                    //datetime
                    else if (item.Value.GetType().FullName == "System.DateTime")
                    {
                        DateTime dt = (DateTime)item.Value;
                        sb.Append(item.Key);
                        sb.Append(equalFilter);
                        sb.Append("'");
                        sb.Append(dt.Date.ToString("yyyy-MM-dd"));
                        sb.Append("'");
                    }
                    //其他类型
                    else
                    {
                        sb.Append(item.Key);
                        sb.Append(equalFilter);
                        sb.Append("'");
                        sb.Append(item.Value.ToString());
                        sb.Append("'");
                    }
                    sb.Append(" and ");
                }
                sb.Remove(sb.Length - 5, 5);
                return sb.ToString();
            }
            else
            {
                return "";
            }
        }
        protected void SetReportKey(DataRow dr, LisReportPK PK)
        {
            PK.SampleNo = dr["sampleno"].ToString();
            PK.ReceiveDate = (DateTime)dr["receivedate"];
            PK.SectionNo = Convert.ToInt32(dr["sectionno"]);
            PK.TestTypeNo = Convert.ToInt32(dr["testtypeno"]);
        }
        protected DataTable GetDataTable(string sql)
        {
            DataTable dt = DbHelperSQL.Query(sql).Tables["dt"];
            return dt;
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