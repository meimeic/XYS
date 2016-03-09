using System;
namespace XYS.Report.Lis.Repository
{
    class DefaultReporter : Reporter
    {
        #region
        private static readonly string m_strategyName = "DefaultStrategy";
        #endregion

        #region
        public DefaultReporter()
            : base("DefaultReporter", m_strategyName)
        {
        }
        #endregion
    }
}
