using System;
using System.Text;
using System.Data;
using System.Collections;
using System.Collections.Generic;

using XYS.Lis;
using XYS.Common;
using XYS.Utility.DB;

namespace XYS.Lis.DAL
{
   public class LisReporterKeyDAL
    {
       public List<ReportKey> GetReportKey(LisSearchRequire require)
       {
           ReportKey temp;
           List<ReportKey> result = new List<ReportKey>();
           string sql = GetSQLString(require);
           DataTable dt = GetDataTable(sql);
           if (dt.Rows.Count > 0)
           {
               foreach (DataRow dr in dt.Rows)
               {
                   temp = SetReportKey(dr);
                   result.Add(temp);
               }
           }
           return result;
       }
       protected string GetSQLString(LisSearchRequire require)
       {
           StringBuilder sb = new StringBuilder();
           string temp;
           sb.Append("select top ");
           sb.Append(require.MaxRecord.ToString());
           sb.Append(" receivedate,sectionno,testtypeno,sampleno from reportform where patno is not null and patno<>'' and ");
           temp = GetWhereStr(require.EqualFields, 1);
           if (!temp.Equals(""))
           {
               sb.Append(temp);
           }
           temp = GetWhereStr(require.NoEqualFields, 2);
           if (!temp.Equals(""))
           {
               sb.Append(" and ");
               sb.Append(temp);
           }
           temp = GetWhereStr(require.LikeFields, 3);
           if (!temp.Equals(""))
           {
               sb.Append(" and ");
               sb.Append(temp);
           }
           return sb.ToString();
       }
       protected string GetWhereStr(Dictionary<string,object> dic,int flag)
       {
           string equalFilter="";
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
           StringBuilder sb = new StringBuilder();
           if (dic.Count > 0)
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

       protected ReportKey SetReportKey(DataRow dr)
       {
           KeyColumn temp;
           LisDBKeyImpl key = new LisDBKeyImpl();
           foreach (DataColumn dc in dr.Table.Columns)
           {
               temp = new KeyColumn(dc.ColumnName, dr[dc]);
               key.AddKey(temp);
           }
           //temp = new KeyColumn("sectionno", dr["sectionno"]);
           //key.AddKey(temp);
           //temp = new KeyColumn("testtypeno", dr["testtypeno"]);
           //key.AddKey(temp);
           //temp = new KeyColumn("sampleno", dr["sampleno"]);
           //key.AddKey(temp);
           //temp = new KeyColumn("receivedate", dr["receivedate"]);
           //key.AddKey(temp);
           return key;
       }
       protected DataTable GetDataTable(string sql)
       {
           DataTable dt = DbHelperSQL.Query(sql).Tables["dt"];
           return dt;
       }

       //protected string GetLikeStr(Dictionary<string, string> dic)
       //{
       //    StringBuilder sb = new StringBuilder();
       //    if (dic.Count > 0)
       //    {
       //        foreach (KeyValuePair<string, string> item in dic)
       //        {
       //            sb.Append(item.Key);
       //            sb.Append(" like '");

       //        }
       //    }
       //    else
       //    {
       //        return "";
       //    }
       //}
    }
}
