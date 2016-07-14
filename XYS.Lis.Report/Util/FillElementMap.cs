using System;
using System.Collections;

using XYS.Util;
namespace XYS.Lis.Report.Util
{
    public class FillElementMap
    {
        #region 字段
        private static readonly int m_capacity = 4;
        private readonly Hashtable m_name2ElementMap;
        #endregion

        #region 构造函数
        public FillElementMap()
            : this(m_capacity)
        {
        }
        public FillElementMap(int c)
        {
            this.m_name2ElementMap = SystemInfo.CreateCaseInsensitiveHashtable(c);
        }
        #endregion

        #region 属性
        public int Count
        {
            get { return this.m_name2ElementMap.Count; }
        }
        public FillElement this[string name]
        {
            get
            {
                lock (this)
                {
                    return this.m_name2ElementMap[name] as FillElement;
                }
            }
        }
        public ICollection AllElements
        {
            get { return this.m_name2ElementMap.Values; }
        }
        #endregion

        #region 方法
        public void Add(FillElement element)
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