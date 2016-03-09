using System;
using System.Collections;
namespace XYS.Report.Lis.Core
{
    public class LisParItemMap
    {
        #region 字段
        private readonly Hashtable m_no2ParItemMap;
        private static readonly int DEFAULT_CAPACITY = 40;
        #endregion

        #region 构造函数
        public LisParItemMap()
            : this(DEFAULT_CAPACITY)
        { }
        public LisParItemMap(int c)
        {
            this.m_no2ParItemMap = new Hashtable(c);
        }
        #endregion

        #region 属性
        public LisParItem this[int itemNo]
        {
            get
            {
                lock (this)
                {
                    return this.m_no2ParItemMap[itemNo] as LisParItem;
                }
            }
        }
        public ICollection AllParItem
        {
            get
            {
                return this.m_no2ParItemMap.Values;
            }
        }
        public int Count
        {
            get { return this.m_no2ParItemMap.Count; }
        }
        #endregion

        #region 方法
        public void Clear()
        {
            this.m_no2ParItemMap.Clear();
        }
        public void Add(LisParItem item)
        {
            if (item == null)
            {
                throw new ArgumentNullException("paritem");
            }
            lock (this)
            {
                this.m_no2ParItemMap[item.ParItemNo] = item;
            }
        }
        #endregion
    }
}
