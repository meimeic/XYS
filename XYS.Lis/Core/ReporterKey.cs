﻿using System;
namespace XYS.Lis.Core
{
    public class ReporterKey
    {
        #region
        private static readonly string DefaultStrategy = "DefaultStrategy";
        private readonly string m_name;
        private readonly string m_strategyName;
        private readonly int m_hashCache;
        #endregion

        #region
        public ReporterKey(string name)
        {
            if (name == null || name.Equals(""))
            {
                throw new ArgumentNullException("reporterName");
            }
            this.m_name = string.Intern(name);
            this.m_strategyName = string.Intern(DefaultStrategy.ToLower());
            this.m_hashCache = this.m_name.GetHashCode() + this.m_strategyName.GetHashCode();
        }
        public ReporterKey(string name, string strategyName)
            : this(name)
        {
            if (strategyName != null && !strategyName.Equals(""))
            {
                this.m_strategyName = string.Intern(strategyName.ToLower());
                this.m_hashCache = this.m_name.GetHashCode() + this.m_strategyName.GetHashCode();
            }
            else
            {
                throw new ArgumentNullException("strategyName");
            }
        }
        #endregion

        #region
        public string ReporterName
        {
            get { return this.m_name; }
        }
        public string StrategyName
        {
            get { return this.m_strategyName; }
        }
        #endregion

        #region
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
                return (((object)m_name) == ((object)rk.m_name)&&((object)m_strategyName)==((object)rk.m_strategyName));
            }
            return false;
        }
        #endregion

    }
}
