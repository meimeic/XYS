using System;
using System.Collections;

using XYS.Util;
namespace XYS.Report.Lis.Util
{
    public class SubElementMap
    {
        #region 字段
        private static readonly int m_capacity = 4;
        private readonly Hashtable m_name2ElementMap;
        #endregion

        #region 构造函数
        public SubElementMap()
            : this(m_capacity)
        {
        }
        public SubElementMap(int c)
        {
            this.m_name2ElementMap = SystemInfo.CreateCaseInsensitiveHashtable(c);
        }
        #endregion

        #region 属性
        public int Count
        {
            get { return this.m_name2ElementMap.Count; }
        }
        public SubElement this[string name]
        {
            get
            {
                lock (this)
                {
                    return this.m_name2ElementMap[name] as SubElement;
                }
            }
        }
        public ICollection AllElements
        {
            get { return this.m_name2ElementMap.Values; }
        }
        #endregion

        #region 方法
        public void Add(SubElement element)
        {
            if (element == null)
            {
                throw new ArgumentNullException("liselement");
            }
            lock (this)
            {
                this.m_name2ElementMap[element.Name] = element;
            }
        }
        public void Clear()
        {
            this.m_name2ElementMap.Clear();
        }
        #endregion
    }
}