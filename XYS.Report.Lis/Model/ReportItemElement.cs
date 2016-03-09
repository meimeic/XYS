using XYS.Common;
namespace XYS.Report.Lis.Model
{
    [Export()]
    public class ReportItemElement : AbstractFillElement
    {
        #region 私有常量字段
        #endregion

        #region 私有字段
        private int m_itemNo;
        private int m_parItemNo;
        private string m_itemCName;
        private string m_itemEName;
        private double m_itemValue;
        private string m_itemDesc;
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
        public ReportItemElement()
            : this(null)
        {
        }
        public ReportItemElement(string sql)
            : base(sql)
        {
        }
        #endregion

        #region 公共属性
        [Export()]
        [Column(true)]
        public int ItemNo
        {
            get { return this.m_itemNo; }
            set { this.m_itemNo = value; }
        }

        [Column(true)]
        public int ParItemNo
        {
            get { return this.m_parItemNo; }
            set { this.m_parItemNo = value; }
        }

        [Export()]
        [Column(true)]
        public string ItemCName
        {
            get { return this.m_itemCName; }
            set { this.m_itemCName = value; }
        }

        [Export()]
        [Column(true)]
        public string ItemEName
        {
            get { return this.m_itemEName; }
            set { this.m_itemEName = value; }
        }

        [Column(true)]
        public double ItemValue
        {
            get { return this.m_itemValue; }
            set { this.m_itemValue = value; }
        }
        [Column(true)]
        public string ItemDesc
        {
            get { return this.m_itemDesc; }
            set { this.m_itemDesc = value; }
        }
        [Export()]
        public string ItemResult
        {
            get { return this.m_itemResult; }
            protected set { this.m_itemResult = value; }
        }

        [Export()]
        [Column(true)]
        public string ItemStandard
        {
            get { return this.m_itemStandard; }
            set { this.m_itemStandard = value; }
        }

        [Export()]
        [Column(true)]
        public string ResultStatus
        {
            get { return this.m_resultStatus; }
            set { this.m_resultStatus = value; }
        }

        [Export()]
        [Column(true)]
        public string ItemUnit
        {
            get { return this.m_itemUnit; }
            set { this.m_itemUnit = value; }
        }

        [Export()]
        [Column(true)]
        public string RefRange
        {
            get { return this.m_refRange; }
            set { this.m_refRange = value; }
        }

        [Export()]
        [Column(true)]
        public int DispOrder
        {
            get { return this.m_dispOrder; }
            set { this.m_dispOrder = value; }
        }

        [Column(true)]
        public int SecretGrade
        {
            get { return this.m_secretGrade; }
            set { this.m_secretGrade = value; }
        }

        [Column(true)]
        public int Prec
        {
            get { return this.m_prec; }
            set { this.m_prec = value; }
        }
        #endregion

        #region 方法
        //public override void AfterFill()
        //{
        //    if (this.ItemValue != 0.0D)
        //    {
        //        this.ItemResult = this.ItemValue.ToString();
        //    }
        //    else
        //    {
        //        this.ItemResult = this.ItemDesc;
        //    }
        //}
        #endregion

        #region 私有实例方法
        //private void AdjustItemResult()
        //{
        //    double temp;
        //    bool r = double.TryParse(this.ItemResult, out temp);
        //    if (r)
        //    {
        //        this.ItemResult = AdjustAccuracy(temp, this.Prec);
        //    }
        //}
        //public void AdjustItemStandard()
        //{
        //    if (this.Prec > 0)
        //    {
        //        string result;
        //        if (this.ItemStandard != null && !this.ItemStandard.Equals(""))
        //        {
        //            result = AdjustStr(this.ItemStandard, this.Prec);
        //            if (result != null)
        //            {
        //                this.ItemStandard = result;
        //            }
        //        }
        //    }
        //}
        //#endregion

        //#region 受保护的虚方法
        //protected virtual string AdjustStr(string s, int prec)
        //{
        //    double temp;
        //    bool r = double.TryParse(s, out temp);
        //    if (r)
        //    {
        //        return AdjustAccuracy(temp, prec);
        //    }
        //    else
        //    {
        //        return null;
        //    }
        //}
        //protected virtual string AdjustAccuracy(double d, int prec)
        //{
        //    string formatter = AccuracyFormat(prec);
        //    string result = d.ToString(formatter);
        //    return result;
        //}
        //protected virtual string AccuracyFormat(int prec)
        //{
        //    StringBuilder sb = new StringBuilder();
        //    sb.Append("0.");
        //    for (int i = 0; i < prec; i++)
        //    {
        //        sb.Append('0');
        //    }
        //    return sb.ToString();
        //}
        #endregion
    }
}
