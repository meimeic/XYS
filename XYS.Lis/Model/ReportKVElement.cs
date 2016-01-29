using System;
using System.Collections;

using XYS.Common;
using XYS.Lis.Core;

namespace XYS.Lis.Model
{
    public class ReportKVElement : AbstractReportElement
    {
        private string m_name;
        private readonly Hashtable m_kvTable;
        private static readonly ReportElementTag m_defaultElementTag = ReportElementTag.KVElement;

        public ReportKVElement()
            : this(m_defaultElementTag, "")
        {
        }
        public ReportKVElement(ReportElementTag elementTag, string sql)
            : base(elementTag, sql)
        {
            this.m_kvTable = new Hashtable(4);
        }

        [Export()]
        public string Name
        {
            get { return this.m_name; }
            set { this.m_name = value; }
        }
        public Hashtable KVTable
        {
            get { return this.m_kvTable; }
        }

        #region 实现父类抽象方法
        protected override void Afterward()
        {
        }
        #endregion
    }
}
