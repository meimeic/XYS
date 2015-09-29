using XYS.Model;
using System.Text;
using XYS.Common;
namespace XYS.Lis.Model
{
    public class ReportCommonItemElement : AbstractReportElement
    {
        #region 私有常量字段
        private const ReportElementType m_defaultElementType = ReportElementType.ItemElement;
        private const string m_defaultItemSQL = @"select r.itemno as itemno,paritemno,t.cname as itemcname,t.ename as itemename, ISNULL(r.reportdesc, '') + ISNULL(CONVERT(VARCHAR(50), r.reportvalue), '') as itemresult,resultstatus,ISNULL(r.unit,t.unit) as itemunit,refrange,disporder,prec,secretgrade
                                                                            from ReportItem as r left outer join TestItem as t on r.ItemNo=t.ItemNo";
        #endregion

        #region 私有字段
        private int m_itemNo;
        private int m_parItemNo;
        private string m_itemCName;
        private string m_itemEName;
        private string m_itemResult;
        private string m_itemStandard;
        private string m_resultStatus;
        private string m_itemUnit;
        private string m_refRange;
        private int m_dispOrder;
        private int m_secretGrade;
        private int m_prec;
        #endregion

        #region 公共构造方法
        public ReportCommonItemElement()
            : base(m_defaultElementType,m_defaultItemSQL)
        { }
        public ReportCommonItemElement(ReportElementType elementType,string sql)
            : base(elementType,sql)
        { }
        #endregion

        #region 公共属性
        [TableColumn(true)]
        public int ItemNo
        {
            get { return this.m_itemNo; }
            set { this.m_itemNo = value; }
        }
        [TableColumn(true)]
        public int ParItemNo
        {
            get { return this.m_parItemNo; }
            set { this.m_parItemNo = value; }
        }
        [TableColumn(true)]
        public string ItemCName
        {
            get { return this.m_itemCName; }
            set { this.m_itemCName = value; }
        }
        [TableColumn(true)]
        public string ItemEName
        {
            get { return this.m_itemEName; }
            set { this.m_itemEName = value; }
        }
        [TableColumn(true)]
        public string ItemResult
        {
            get { return this.m_itemResult; }
            set { this.m_itemResult = value; }
        }
        public string ItemStandard
        {
            get { return this.m_itemStandard; }
            set { this.m_itemStandard = value; }
        }
        [TableColumn(true)]
        public string ResultStatus
        {
            get { return this.m_resultStatus; }
            set { this.m_resultStatus = value; }
        }
        [TableColumn(true)]
        public string ItemUnit
        {
            get { return this.m_itemUnit; }
            set { this.m_itemUnit = value; }
        }
        [TableColumn(true)]
        public string RefRange
        {
            get { return this.m_refRange; }
            set { this.m_refRange = value; }
        }
        [TableColumn(true)]
        public int DispOrder
        {
            get { return this.m_dispOrder; }
            set { this.m_dispOrder = value; }
        }
         [TableColumn(true)]
        public int SecretGrade
        {
            get { return this.m_secretGrade; }
            set { this.m_secretGrade = value; }
        }
        [TableColumn(true)]
        public int Prec
        {
            get { return this.m_prec; }
            set { this.m_prec = value; }
        }
        #endregion

        #region 重写父类方法
        protected override void Afterward()
        {
            if (this.m_prec > 0)
            {
                AdjustItemResult();
                AdjustItemStandard();
            }
        }
        #endregion

        #region 私有实例方法
        private void AdjustItemResult()
        {
            double temp;
            bool r = double.TryParse(this.m_itemResult, out temp);
            if (r)
            {
                this.m_itemResult = AdjustAccuracy(temp, this.m_prec);
            }
        }
        private void AdjustItemStandard()
        {
            double temp;
            bool r = double.TryParse(this.m_itemStandard, out temp);
            if (r)
            {
                this.m_itemStandard = AdjustAccuracy(temp, this.m_prec);
            }
        }
        #endregion

        #region 受保护的虚方法
        protected virtual void AdjustStr(ref string s, int prec)
        {
            double temp;
            bool r = double.TryParse(s, out temp);
            if (r)
            {
                s = AdjustAccuracy(temp, prec);
            }
        }
        protected virtual string AdjustAccuracy(double d, int prec)
        {
            string formatter = AccuracyFormat(prec);
            string result = d.ToString(formatter);
            return result;
        }
        protected virtual string AccuracyFormat(int prec)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("0.");
            for (int i = 0; i < prec; i++)
            {
                sb.Append('0');
            }
            return sb.ToString();
        }
        #endregion
    }
}
