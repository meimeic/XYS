using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XYS.FR.Lis.Util
{
    public class LisItem
    {
        #region 字段
        private readonly int m_ItemNo;
        private string m_ItemName;
        private int m_printModelNo;
        private int m_orderNo;
        #endregion

        #region 构造函数
        public LisItem(int itemNo)
            : this(itemNo, null)
        {
        }
        public LisItem(int itemNo, string itemName)
        {
            this.m_ItemNo = itemNo;
            this.m_ItemName = itemName;
        }
        #endregion

        #region 属性
        public int ItemNo
        {
            get { return this.m_ItemNo; }
        }
        public string ItemName
        {
            get { return this.m_ItemName; }
            set { this.m_ItemName = value; }
        }
        public int PrintModelNo
        {
            get { return this.m_printModelNo; }
            set { this.m_printModelNo = value; }
        }
        public int OrderNo
        {
            get { return this.m_orderNo; }
            set { this.m_orderNo = value; }
        }
        #endregion
    }
}
