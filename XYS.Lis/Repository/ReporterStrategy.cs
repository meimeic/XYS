using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XYS.Lis.Repository
{
    public class ReporterStrategy
    {
        private string m_strategyName;
        private string m_fillerName;
        private string m_exportName;
        private readonly List<string> m_handlerList;
        //private readonly List<string> m_fillerList;
        //private readonly List<string> m_exportList;
        public ReporterStrategy(string name)
        {
            this.m_strategyName = name;

            this.m_handlerList = new List<string>(7);
            //this.m_exportList=new List<string>(5);
            //this.m_fillerList = new List<string>(5);
        }
        public string StrategyName
        {
            get { return this.m_strategyName; }
        }

        public List<string> HandlerList
        {
            get { return this.m_handlerList; }
        }

        public string FillerName
        {
            get { return this.m_fillerName; }
            set { this.m_fillerName = value; }
        }
        public string ExportName
        {
            get { return this.m_exportName; }
            set { this.m_exportName = value; }
        }
        //public List<string> ExportList
        //{
        //    get { return this.m_exportList; }
        //}
        //public List<string> FillerList
        //{
        //    get { return this.m_fillerList; }
        //}

        public void AddHandler(string handlerName)
        {
            lock (this.m_handlerList)
            {
                if (!this.m_handlerList.Contains(handlerName))
                {
                    this.m_handlerList.Add(handlerName);
                }
            }
        }
        //public void AddExport(string exportName)
        //{
        //    lock (this.m_exportList)
        //    {
        //        if (!this.m_exportList.Contains(exportName))
        //        {
        //            this.m_exportList.Add(exportName);
        //        }
        //    }
        //}
        //public void AddFiller(string fillerName)
        //{
        //    lock (this.m_fillerList)
        //    {
        //        if (!this.m_fillerList.Contains(fillerName))
        //        {
        //            this.m_fillerList.Add(fillerName);
        //        }
        //    }
        //}
    }
}
