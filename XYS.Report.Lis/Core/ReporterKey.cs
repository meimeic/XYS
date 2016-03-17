using System;
namespace XYS.Report.Lis.Core
{
    public class ReporterKey
    {
        #region 字段
        private static readonly string DefaultStrategy = "DefaultStrategy";
        private readonly string m_callerName;
        private readonly string m_strategyName;
        private readonly int m_hashCache;
        #endregion

        #region 构造函数
        public ReporterKey(string callerName)
            : this(callerName, DefaultStrategy)
        {
        }
        public ReporterKey(string callerName, string strategyName)
        {
            if (!string.IsNullOrEmpty(callerName) && !string.IsNullOrEmpty(strategyName))
            {
                this.m_callerName = string.Intern(callerName);
                this.m_strategyName = string.Intern(strategyName);
                this.m_hashCache = this.m_callerName.GetHashCode() + this.m_strategyName.GetHashCode();
            }
            else
            {
                throw new ArgumentNullException("callerName or strategyName is empty!");
            }
        }
        #endregion

        #region 实例属性
        public string CallerName
        {
            get { return this.m_callerName; }
        }
        public string StrategyName
        {
            get
            {
                return this.m_strategyName.ToLower();
            }
        }
        #endregion

        #region 重写object方法
        public override int GetHashCode()
        {
            return this.m_hashCache;
        }
        public override bool Equals(object obj)
        {
            if (((object)this) == obj)
            {
                return true;
            }
            ReporterKey rk = obj as ReporterKey;
            if (rk != null)
            {
                return (((object)m_callerName) == ((object)rk.m_callerName) && ((object)m_strategyName) == ((object)rk.m_strategyName));
            }
            return false;
        }
        #endregion
    }
}
