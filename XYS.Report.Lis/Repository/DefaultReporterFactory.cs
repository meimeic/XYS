using System;

using XYS.Report.Core;
namespace XYS.Report.Repository.Lis
{
    public class DefaultReporterFactory : IReporterFactory
    {
        #region
        public DefaultReporterFactory()
        {
        }
        #endregion

        #region 实现IReporterFactory接口
        public LisReporter CreateReporter(IReporterRepository repository, ReporterKey key)
        {
            if (key == null)
            {
                return new DefaultReporter();
            }
            return new ReporterImpl(key.CallerName, key.StrategyName);
        }
        #endregion

        #region 内部类
        public sealed class ReporterImpl : LisReporter
        {
            public ReporterImpl(string callerName, string strategyName)
                : base(callerName, strategyName)
            {
            }
        }
        #endregion
    }
}
