using System;
using System.Collections;

namespace XYS.Report.Core
{
    public class LisParItemMap
    {
        #region
        private readonly Hashtable m_no2ParItemMap;
        private static readonly int DEFAULT_CAPACITY = 60;
        #endregion

        #region
        public LisParItemMap()
            : this(DEFAULT_CAPACITY)
        { }
        public LisParItemMap(int c)
        {
            this.m_no2ParItemMap = new Hashtable(c);
        }
        #endregion

        #region
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

        #region
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
