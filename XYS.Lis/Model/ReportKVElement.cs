using System;
using System.Collections;

using XYS.Common;
using XYS.Lis.Core;

namespace XYS.Lis.Model
{
    public class ReportKVElement : AbstractInnerElement
    {
        private string m_name;
        private readonly Hashtable m_kvTable;
        private static readonly int DEFAULT_CAPACITY = 4;
        public ReportKVElement()
            : base()
        {
            this.m_kvTable = new Hashtable(DEFAULT_CAPACITY);
        }
        public string Name
        {
            get { return this.m_name; }
            set { this.m_name = value; }
        }
        public Hashtable KVTable
        {
            get { return this.m_kvTable; }
        }
    }
}
