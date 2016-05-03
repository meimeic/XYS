using System;

namespace XYS.Report.Lis.Persistent.Mongo
{
    public class ReportInfoProjection : AbstractReportProjection
    {
        public ReportInfoProjection()
        { }

        public Guid ID { get; set; }
        public string ReportName { get; set; }
        public DateTime ReportDateTime { get; set; }
    }
}
