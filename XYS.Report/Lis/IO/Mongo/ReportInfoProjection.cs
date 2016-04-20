using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XYS.Report.Lis.IO.Mongo
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
