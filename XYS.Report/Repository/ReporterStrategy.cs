using System;
using System.Collections.Generic;

namespace XYS.Report.Repository
{
    public class ReporterStrategy
    {
        private string m_fillerName;
        private readonly string m_strategyName;
        private readonly List<string> m_handlerList;
        public ReporterStrategy(string name)
        {
            this.m_strategyName = name;
            this.m_handlerList = new List<string>(5);
        }
        public string StrategyName
        {
            get { return this.m_strategyName.ToLower(); }
        }
        public IList<string> HandlerList
        {
            get { return this.m_handlerList.AsReadOnly(); }
        }
        public string FillerName
        {
            get { return this.m_fillerName.ToLower(); }
            set { this.m_fillerName = value; }
        }
        public void AddHandler(string handlerName)
        {
            if (!string.IsNullOrEmpty(handlerName))
            {
                string s = handlerName.ToLower();
                if (!this.m_handlerList.Contains(s))
                {
                    this.m_handlerList.Add(s);
                }
            }
        }
    }
}
