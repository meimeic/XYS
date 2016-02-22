using System;
using System.Collections;

using XYS.Lis.Core;
namespace XYS.Lis.Export.Model
{
    public class ReportGraph : IExportElement
    {
        private readonly string m_name;
        private readonly Hashtable m_imageTable;

        public ReportGraph()
        { }

        public string Name
        {
            get { return this.m_name; }
        }
        public Hashtable ImageTable
        {
            get { return this.m_imageTable; }
        }

        public void Add(string key, object value)
        {
            lock (this.m_imageTable)
            {
                this.m_imageTable[key] = value;
            }
        }
    }
}
