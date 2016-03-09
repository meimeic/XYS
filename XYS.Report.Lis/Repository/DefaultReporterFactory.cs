using System;

using XYS.Common;
using XYS.Repository;
namespace XYS.Report.Lis.Repository
{
    public class DefaultReporterFactory : IReporterFactory
    {
        #region
        public DefaultReporterFactory()
        {
        }
        #endregion

        #region 实现IReporterFactory接口
        public Reporter CreateReporter(IReporterRepository repository, ReporterKey key)
        {
            if (key == null)
            {
                return new DefaultReporter();
            }
            return new ReporterImpl(key.CallerName, key.StrategyName);
        }
        #endregion

        #region 内部类
        public sealed class ReporterImpl : Reporter
        {
            public ReporterImpl(string callerName, string strategyName)
                : base(callerName, strategyName)
            {
            }
        }
        #endregion
    }
}
