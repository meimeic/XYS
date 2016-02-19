using System;
using System.Collections;
using XYS.Lis.Core;
namespace XYS.Lis.DAL
{
    public class LisReportGraphDAL:BasicDAL<ReportGraphElement>
    {
        protected override string GenderSql(Hashtable equalTable)
        {
            string sql = "select graphname,Graphjpg as graphimage from RFGraphData";
            return sql + this.GetSQLWhere(equalTable);
        }
    }
}
