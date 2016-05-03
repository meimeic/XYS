using System;

namespace XYS.Report.Lis.Persistent.Mongo
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
