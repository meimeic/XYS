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
        //public NVItem this[string key]
        //{
        //    get
        //    {
        //        lock (this)
        //        {
        //            return this.m_kvTable[key] as NVItem;
        //        }
        //    }
        //}
        //public ICollection AllNVItem
        //{
        //    get { return this.m_kvTable.Values; }
        //}
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
            if (value != null)
            {
                lock (this.m_kvTable)
                {
                    this.m_kvTable[key] = value;
                }
            }
        }
        #endregion

        //public class NVItem
        //{
        //    private string m_name;
        //    private string m_value;

        //    public NVItem()
        //    { 
        //    }
        //    public NVItem(string name, string value)
        //    {
        //        this.m_name = name;
        //        this.m_value = value;
        //    }
        //    public string Name
        //    {
        //        get { return this.m_name; }
        //        set { this.m_name = value; }
        //    }
        //    public string Value
        //    {
        //        get { return this.m_value; }
        //        set { this.m_value = value; }
        //    }
        //}
    }
}
