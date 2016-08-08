using System;

using XYS.Report;
using XYS.Common;

namespace XYS.Model.Lab
{
    public class ItemElement : IFillElement, IComparable<ItemElement>
    {
        #region 私有字段
        private string m_reportID;
        private int m_itemNo;
        private int m_superNo;
        private string m_cName;
        private string m_eName;
        private string m_result;
        private string m_status;
        private string m_unit;
        private string m_refRange;
        private int m_order;
        private int m_secretGrade;
        #endregion

        #region 公共构造方法
        public ItemElement()
        {
        }
        #endregion

        #region 公共属性
        public string ReportID
        {
            get { return this.m_reportID; }
            set { this.m_reportID = value; }
        }

        [Column]
        public int ItemNo
        {
            get { return this.m_itemNo; }
            set { this.m_itemNo = value; }
        }

        [Column]
        public int SuperNo
        {
            get { return this.m_superNo; }
            set { this.m_superNo = value; }
        }

        [Column]
        public string CName
        {
            get { return this.m_cName; }
            set { this.m_cName = value; }
        }

        [Column]
        public string EName
        {
            get { return this.m_eName; }
            set { this.m_eName = value; }
        }

        [Column]
        public string Result
        {
            get { return this.m_result; }
            set { this.m_result = value; }
        }

        [Column]
        public string Status
        {
            get { return this.m_status; }
            set { this.m_status = value; }
        }

        [Column]
        public string Unit
        {
            get { return this.m_unit; }
            set { this.m_unit = value; }
        }

        [Column]
        public string RefRange
        {
            get { return this.m_refRange; }
            set { this.m_refRange = value; }
        }

        [Column]
        public int DispOrder
        {
            get { return this.m_order; }
            set { this.m_order = value; }
        }

        [Column]
        public int SecretGrade
        {
            get { return this.m_secretGrade; }
            set { this.m_secretGrade = value; }
        }
        #endregion

        #region 实现比较方法
        public int CompareTo(ItemElement element)
        {
            if (element == null)
            {
                return 1;
            }
            else
            {
                return this.DispOrder - element.DispOrder;
            }
        }
        #endregion
    }
}