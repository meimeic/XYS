using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XYS.Lis.Core
{
    public class ParItemMap
    {
        #region
        private Hashtable m_mapNo2ParItem;
        #endregion

        #region
        public ParItemMap()
        {
            this.m_mapNo2ParItem = new Hashtable(50);
        }
        #endregion

        #region
        public ParItem this[int itemNo]
        {
            get 
            {
                lock (this)
                {
                    return this.m_mapNo2ParItem[itemNo] as ParItem;
                }
            }
        }
        public ParItemCollection AllParItem
        {
            get
            {
                lock (this)
                {
                    return new ParItemCollection(this.m_mapNo2ParItem.Values);
                }
            }
        }
        public int Count
        {
            get { return this.m_mapNo2ParItem.Count; }
        }
        #endregion

        #region
        public void Clear()
        {
            this.m_mapNo2ParItem.Clear();
        }
        public void Add(ParItem item)
        {
            if (item == null)
            {
                throw new ArgumentNullException("paritem");
            }
            lock (this)
            {
                this.m_mapNo2ParItem.Add(item.ParItemNo,item);
            }
        }
        #endregion
    }
}
