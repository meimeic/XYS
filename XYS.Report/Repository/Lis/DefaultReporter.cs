using System;

namespace XYS.Report.Repository.Lis
{
    class DefaultReporter : LisReporter
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
