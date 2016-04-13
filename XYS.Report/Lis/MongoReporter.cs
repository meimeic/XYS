using System;
using XYS.Report.Lis.Handler;

namespace XYS.Report.Lis
{
    public class MongoReporter:Reporter
    {
        public MongoReporter()
            : base()
        {
            this.InitReporter();
        }
        protected override void InitReporter()
        {
            base.InitReporter();
            IReportHandler handler = new ReportMongoHandler();
            AddHandler(handler);
        }
    }
}
