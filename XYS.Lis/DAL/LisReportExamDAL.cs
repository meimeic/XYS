using System;
using System.Collections;
using XYS.Lis.Core;

namespace XYS.Lis.DAL
{
    public class LisReportExamDAL:BasicDAL<ReportExamElement>
    {
        protected override string GenderSql(Hashtable equalTable)
        {
            string sql = @"select serialno,sampleno,s.CName as sampletypename,CAST(CONVERT(varchar(10), collectdate, 121) + ' ' + CONVERT(varchar(8), collecttime, 114) AS datetime) as collectdatetime,CAST(CONVERT(varchar(10), inceptdate, 121) + ' ' + CONVERT(varchar(8), incepttime, 114) AS datetime) as inceptdatetime,
                                   CAST(CONVERT(varchar(10), testdate, 121) + ' ' + CONVERT(varchar(8), testtime, 114) AS datetime) as testdatetime,CAST(CONVERT(varchar(10), checkdate, 121) + ' ' + CONVERT(varchar(8), checktime, 114) AS datetime) as checkdatetime,
                                   CAST(CONVERT(varchar(10), receivedate, 121) + ' ' + CONVERT(varchar(8), receivetime, 114) AS datetime) as receivedatetime,sendertime2 as secondcheckdatetime,paritemname,sectionno,r.sampletypeno,formmemo,formcomment,formcomment2,technician,checker
                                   from ReportForm as r left outer join SampleType as s on r.SampleTypeNo=s.SampleTypeNo";
            return sql + this.GetSQLWhere(equalTable);
        }
    }
}
