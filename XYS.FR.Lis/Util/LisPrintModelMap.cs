using System;
using System.Collections;

namespace XYS.FR.Lis.Util
{
    public class LisPrintModelMap
    {
        #region 字段
        private readonly Hashtable m_no2ModelMap;
        private static readonly int DEFAULT_CAPACITY = 20;
        #endregion

        #region
        public LisPrintModelMap()
            : this(DEFAULT_CAPACITY)
        { }
        public LisPrintModelMap(int c)
        {
            this.m_no2ModelMap = new Hashtable(c);
        }
        #endregion

        #region
        public LisPrintModel this[int no]
        {
            get
            {
                lock (this)
                {
                    return this.m_no2ModelMap[no] as LisPrintModel;
                }
            }
        }
        public ICollection AllPrintModel
        {
            get
            {
                return this.m_no2ModelMap.Values;
            }
        }
        public int Count
        {
            get { return this.m_no2ModelMap.Count; }
        }
        #endregion

        #region
        public void Clear()
        {
            this.m_no2ModelMap.Clear();
        }
        public void Add(LisPrintModel model)
        {
            if (model == null)
            {
                throw new ArgumentNullException("PrintModel");
            }
            lock (this)
            {
                this.m_no2ModelMap[model.No] = model;
            }
        }
        #endregion
    }
}
