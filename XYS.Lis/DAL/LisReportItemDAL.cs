using System.Collections;
using XYS.Lis.Core;

namespace XYS.Lis.DAL
{
    public class LisReportItemDAL:BasicDAL<ReportItemElement>
    {
        protected override string GenderSql(Hashtable equalTable)
        {
            string sql = @"select r.itemno as itemno,paritemno,t.cname as itemcname,t.ename as itemename, ISNULL(r.reportdesc, '') + ISNULL(CONVERT(VARCHAR(50), r.reportvalue), '') as itemresult,resultstatus,ISNULL(r.unit,t.unit) as itemunit,refrange,disporder,prec,secretgrade
                                   from ReportItem as r left outer join TestItem as t on r.ItemNo=t.ItemNo";
            return sql + GetSQLWhere(equalTable);
        }
    }
}
