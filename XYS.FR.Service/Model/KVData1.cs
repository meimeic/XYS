using System;

using XYS.Report;
using XYS.Common;
namespace XYS.FR.Service.Model
{
    public class KVData1 : IExportElement
    {
        private string m_key;
        private string m_value;

        public KVData1()
        {
        }

        [Export]
        public string Key
        {
            get { return this.m_key; }
            set { this.m_key = value; }
        }
        [Export]
        public string Value
        {
            get { return this.m_value; }
            set { this.m_value = value; }
        }
    }
}