using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XYS.Lis.Repository
{
    public class ReporterStrategy
    {
        private string m_fillerName;
        private string m_strategyName;
        private readonly List<string> m_handlerList;
        public ReporterStrategy(string name)
        {
            this.m_strategyName = name;
            this.m_handlerList = new List<string>(5);
        }
        public string StrategyName
        {
            get
            {
                if (this.m_strategyName != null)
                {
                    return this.m_strategyName.ToLower();
                }
                return null;
            }
        }
        public List<string> HandlerList
        {
            get { return this.m_handlerList; }
        }
        public string FillerName
        {
            get
            {
                if (this.m_fillerName != null)
                {
                    return this.m_fillerName.ToLower();
                }
                return null;
            }
            set { this.m_fillerName = value; }
        }
        public void AddHandler(string handlerName)
        {
            lock (this.m_handlerList)
            {
                if (!this.m_handlerList.Contains(handlerName.ToLower()))
                {
                    this.m_handlerList.Add(handlerName.ToLower());
                }
            }
        }
    }
}
