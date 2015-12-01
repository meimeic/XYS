using System;

namespace XYS.Lis.Repository.Hierarchy
{
    public class DefaultReporter : Reporter
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
