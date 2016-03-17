using System;
using System.Collections;
namespace XYS.Report.Lis.Model
{
    public class ReportKVElement : AbstractInnerElement
    {
        #region 字段
        private string m_name;
        private readonly Hashtable m_kvTable;
        private static readonly int DEFAULT_CAPACITY = 20;
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
        public int Count
        {
            get { return this.m_kvTable.Count; }
        }
        #endregion

        #region 方法
        public void Add(string key, object value)
        {
            if (!string.IsNullOrEmpty(key))
            {
                lock (this.m_kvTable)
                {
                    this.m_kvTable[key] = value;
                }
            }
        }
        #endregion
    }
}
