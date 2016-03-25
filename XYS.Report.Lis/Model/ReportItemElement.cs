using XYS.Common;
using XYS.Report.Lis.Core;

using MongoDB.Bson.Serialization.Attributes;
namespace XYS.Report.Lis.Model
{
    public class ReportItemElement : ILisReportElement
    {
        #region 私有字段
        private int m_itemNo;
        private int m_parItemNo;
        private string m_itemCName;
        private string m_itemEName;
        private string m_itemResult;
        private string m_resultStatus;
        private string m_itemUnit;
        private string m_refRange;
        private int m_dispOrder;
        private int m_secretGrade;
        #endregion

        #region 公共构造方法
        public ReportItemElement()
        {
        }
        #endregion

        #region 公共属性
        [Column]
        public int ItemNo
        {
            get { return this.m_itemNo; }
            set { this.m_itemNo = value; }
        }

        [Column]
        public int ParItemNo
        {
            get { return this.m_parItemNo; }
            set { this.m_parItemNo = value; }
        }

        [Column]
        public string ItemCName
        {
            get { return this.m_itemCName; }
            set { this.m_itemCName = value; }
        }

        [Column]
        public string ItemEName
        {
            get { return this.m_itemEName; }
            set { this.m_itemEName = value; }
        }

        [Column]
        public string ItemResult
        {
            get { return this.m_itemResult; }
            set { this.m_itemResult = value; }
        }

        [Column]
        public string ResultStatus
        {
            get { return this.m_resultStatus; }
            set { this.m_resultStatus = value; }
        }

        [Column]
        public string ItemUnit
        {
            get { return this.m_itemUnit; }
            set { this.m_itemUnit = value; }
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
            get { return this.m_dispOrder; }
            set { this.m_dispOrder = value; }
        }

        [Column]
        [BsonIgnore]
        public int SecretGrade
        {
            get { return this.m_secretGrade; }
            set { this.m_secretGrade = value; }
        }
        #endregion
    }
}
