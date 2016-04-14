using System;

using XYS.Report.Lis.Model;
using XYS.Report.Lis.Persistence.Mongo;

namespace XYS.Report.Lis.Persistence
{
    public class ReportMongo
    {
        #region 字段
        #endregion

        #region 构造函数

        public ReportMongo()
        {
        }
        #endregion

        #region 实现父类抽象方法
        protected override HandlerResult OperateReport(ReportReportElement report)
        {
            MongoService.Insert(report);
            return new HandlerResult();
        }
        #endregion
    }
}
