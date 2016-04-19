using System;
using System.Threading.Tasks;

namespace XYS.Report.Lis.IO.Mongo
{
    public class ReportStatusProjection : AbstractReportProjection
    {
        public ReportStatusProjection()
        { }

        public Guid ID { get; set; }
        public string ReportID { get; set; }
        public int Final { get; set; }
    }
}
