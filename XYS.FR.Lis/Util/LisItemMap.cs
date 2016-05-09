using System;
using System.Collections;

namespace XYS.FR.Lis.Util
{
    public class LisItemMap
    {
        #region 字段
        private readonly Hashtable m_no2ItemMap;
        private static readonly int DEFAULT_CAPACITY = 40;
        #endregion

        #region 构造函数
        public LisItemMap()
            : this(DEFAULT_CAPACITY)
        { }
        public LisItemMap(int c)
        {
            this.m_no2ItemMap = new Hashtable(c);
        }
        #endregion

        #region 属性
        public LisItem this[int itemNo]
        {
            get
            {
                lock (this)
                {
                    return this.m_no2ItemMap[itemNo] as LisItem;
                }
            }
        }
        public ICollection AllParItem
        {
            get
            {
                return this.m_no2ItemMap.Values;
            }
        }
        public int Count
        {
            get { return this.m_no2ItemMap.Count; }
        }
        #endregion

        #region 方法
        public void Clear()
        {
            this.m_no2ItemMap.Clear();
        }
        public void Add(LisItem item)
        {
            if (item == null)
            {
                throw new ArgumentNullException("paritem");
            }
            lock (this)
            {
                this.m_no2ItemMap[item.ItemNo] = item;
            }
        }
        #endregion
    }
}
