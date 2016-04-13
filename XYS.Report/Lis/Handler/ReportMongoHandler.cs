using System;

using XYS.Report.Lis.Model;
using XYS.Report.Lis.Persistence.Mongo;

namespace XYS.Report.Lis.Handler
{
    public class ReportMongoHandler : ReportHandlerSkeleton
    {
        #region 字段
        private static readonly string m_defaultHandlerName = "ReportMongoHandler";
        #endregion

        #region 构造函数
        public ReportMongoHandler()
            : this(m_defaultHandlerName)
        {
        }
        public ReportMongoHandler(string handlerName)
            : base(handlerName)
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
