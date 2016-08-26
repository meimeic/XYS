using System;

using XYS.Common;
using XYS.Report;
namespace XYS.Lis.Report.Model
{
    public class GSItem : IFillElement
    {
        private string m_reportID;
        private int m_itemNo;
        private string m_cName;
        private int m_bloodNum;
        private double m_bloodValue;
        private int m_marrowNum;
        private double m_marrowValue;

        public GSItem()
        { }

        [Column]
        public int ItemNo
        {
            get { return this.m_itemNo; }
            set { this.m_itemNo = value; }
        }
        [Column]
        public string CName
        {
            get { return this.m_cName; }
            set { this.m_cName = value; }
        }
        [Column]
        public int BloodNum
        {
            get { return this.m_bloodNum; }
            set { this.m_bloodNum = value; }
        }
        [Column]
        public double BloodValue
        {
            get { return this.m_bloodValue; }
            set { this.m_bloodValue = value; }
        }
        [Column]
        public int MarrowNum
        {
            get { return this.m_marrowNum; }
            set { this.m_marrowNum = value; }
        }
        [Column]
        public double MarrowValue
        {
            get { return this.m_marrowValue; }
            set { this.m_marrowValue = value; }
        }
        public string ReportID
        {
            get { return this.m_reportID; }
            set { this.m_reportID = value; }
        }
    }
}
