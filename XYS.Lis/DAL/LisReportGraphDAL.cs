using System;
using System.Collections;
using XYS.Lis.Model;
namespace XYS.Lis.DAL
{
    public class LisReportGraphDAL:BasicDAL<ReportGraphItemElement>
    {
        protected override string GenderSql(Hashtable equalTable)
        {
            string sql = "select graphname,Graphjpg as graphimage from RFGraphData";
            return sql + this.GetSQLWhere(equalTable);
        }
    }
}
