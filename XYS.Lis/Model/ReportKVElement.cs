using System;
using System.Collections;

using XYS.Common;
using XYS.Lis.Core;

namespace XYS.Lis.Model
{
    public class ReportKVElement : AbstractInnerElement
    {
        #region 字段
        private string m_name;
        private readonly Hashtable m_kvTable;
        private static readonly int DEFAULT_CAPACITY = 4;
        #endregion

        #region 构造函数
        public ReportKVElement()
            : base()
        {
            this.m_kvTable = new Hashtable(DEFAULT_CAPACITY);
        }
        #endregion

        #region  属性
        public string Name
        {
            get { return this.m_name; }
            set { this.m_name = value; }
        }
        public Hashtable KVTable
        {
            get { return this.m_kvTable; }
        }
        #endregion
    }
}
