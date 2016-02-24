using System;
using System.Collections;

using XYS.Lis.Core;
namespace XYS.Lis.Export.Model
{
    public class ReportImage : IExportElement
    {
        private static readonly string m_defaultName = "ReportImage";
        private string m_name;
        private readonly Hashtable m_imageTable;

        public ReportImage()
        {
            this.m_name = m_defaultName;
            this.m_imageTable = new Hashtable(3);
        }

        public string Name
        {
            get { return this.m_name; }
            set { this.m_name = value; }
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
