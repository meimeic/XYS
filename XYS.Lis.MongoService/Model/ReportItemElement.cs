using System;

using XYS.Report;
using XYS.Common;

using MongoDB.Bson.Serialization.Attributes;
namespace XYS.Report.Lis.Model
{
    public class ReportItemElement : IFillElement, IComparable<ReportItemElement>
    {
        #region 私有字段
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
        public ReportItemElement()
        {
        }
        #endregion

        #region 公共属性
        [Column]
        [BsonElement("itemNo", Order = 1)]
        public int ItemNo
        {
            get { return this.m_itemNo; }
            set { this.m_itemNo = value; }
        }

        [Column]
        [BsonElement("superNo", Order = 2)]
        public int SuperNo
        {
            get { return this.m_superNo; }
            set { this.m_superNo = value; }
        }

        [Column]
        [BsonElement("cname", Order = 3)]
        public string CName
        {
            get { return this.m_cName; }
            set { this.m_cName = value; }
        }

        [Column]
        [BsonElement("ename", Order = 4)]
        public string EName
        {
            get { return this.m_eName; }
            set { this.m_eName = value; }
        }

        [Column]
        [BsonElement("result", Order = 5)]
        public string Result
        {
            get { return this.m_result; }
            set { this.m_result = value; }
        }

        [Column]
        [BsonElement("status", Order = 6)]
        public string Status
        {
            get { return this.m_status; }
            set { this.m_status = value; }
        }

        [Column]
        [BsonElement("unit", Order = 7)]
        public string Unit
        {
            get { return this.m_unit; }
            set { this.m_unit = value; }
        }

        [Column]
        [BsonElement("refRange", Order = 8)]
        public string RefRange
        {
            get { return this.m_refRange; }
            set { this.m_refRange = value; }
        }

        [Column]
        [BsonElement("order", Order = 9)]
        public int DispOrder
        {
            get { return this.m_order; }
            set { this.m_order = value; }
        }

        [Column]
        [BsonIgnore]
        public int SecretGrade
        {
            get { return this.m_secretGrade; }
            set { this.m_secretGrade = value; }
        }
        #endregion

        #region 实现比较方法
        public int CompareTo(ReportItemElement element)
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
