using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

using XYS.Model;
using XYS.Common;
using XYS.Lis.Core;
using XYS.Lis.Model;
using XYS.Lis.Util;

namespace XYS.Lis.Model
{
    public class ReportReportElement : AbstractReportElement
    {

        #region 私有常量字段
        private static readonly ReportElementTag m_defaultElementTag = ReportElementTag.ReportElement;
        #endregion

        #region 私有实例字段
        private int m_orderNo;
        private int m_printModelNo;

        private string m_reportTitle;
        private string m_remark;
        private bool m_remarkFlag;
        private string m_parItemName;

        private byte[] m_technicianImage;
        private byte[] m_checkerImage;

        private string m_collectDateTime;
        private string m_inceptDateTime;
        private string m_testDateTime;
        private string m_checkDateTime;
        private string m_secondCheckDateTime;

        private int m_sectionNo;
        private ClinicType m_clinicType;

        private readonly List<int> m_parItemList;
        private readonly Hashtable m_reportItemTable;
        #endregion

        #region 公共构造函数
        public ReportReportElement()
            : base(m_defaultElementTag, "")
        {
            this.m_parItemList = new List<int>(5);
            this.m_reportItemTable = new Hashtable(5);
            this.m_remarkFlag = false;
        }
        #endregion

        #region 实例属性
        public int OrderNo
        {
            get { return this.m_orderNo; }
            set { this.m_orderNo = value; }
        }
        public int PrintModelNo
        {
            get { return this.m_printModelNo; }
            set { this.m_printModelNo = value; }
        }
        public bool RemarkFlag
        {
            get { return this.m_remarkFlag; }
            set { this.m_remarkFlag = value; }
        }

        [Export()]
        public string CollectDateTime
        {
            get { return this.m_collectDateTime; }
            set { this.m_collectDateTime = value; }
        }
        
        [Export()]
        public string InceptDateTime
        {
            get { return m_inceptDateTime; }
            set { m_inceptDateTime = value; }
        }
        
        [Export()]
        public string TestDateTime
        {
            get { return m_testDateTime; }
            set { m_testDateTime = value; }
        }
        
        [Export()]
        public string CheckDateTime
        {
            get { return m_checkDateTime; }
            set { m_checkDateTime = value; }
        }
        
        [Export()]
        public string SecondeCheckDateTime
        {
            get { return m_secondCheckDateTime; }
            set { m_secondCheckDateTime = value; }
        }

        [Export()]
        public string ReportTitle
        {
            get { return this.m_reportTitle; }
            set { this.m_reportTitle = value; }
        }

        [Export()]
        public string Remark
        {
            get { return this.m_remark; }
            set { this.m_remark = value; }
        }

        [Export()]
        public string ParItemName
        {
            get { return this.m_parItemName; }
            set { this.m_parItemName = value; }
        }

        [Export()]
        public byte[] TechnicianImage
        {
            get { return this.m_technicianImage; }
            set { this.m_technicianImage = value; }
        }
        [Export()]
        public byte[] CheckerImage
        {
            get { return this.m_checkerImage; }
            set { this.m_checkerImage = value; }
        }

        public int SectionNo
        {
            get { return this.m_sectionNo; }
            set { this.m_sectionNo = value; }
        }
        public ClinicType ClinicType
        {
            get { return this.m_clinicType; }
            set { this.m_clinicType = value; }
        }
        public List<int> ParItemList
        {
            get { return this.m_parItemList; }
        }
        public Hashtable ReportItemTable
        {
            get { return this.m_reportItemTable; }
        }
        #endregion

        #region 实现父类抽象方法
        protected override void Afterward()
        {

        }
        #endregion

        #region 公共方法
        public void Init()
        {

        }
        public void ReportClear()
        {
            this.ParItemList.Clear();
            this.ReportItemTable.Clear();
            this.OrderNo = 0;
            this.PrintModelNo = -1;
            this.ReportTitle = "";
            this.Remark = "";
        }
        public List<ILisReportElement> GetReportItem(ReportElementTag elementTag)
        {
            List<ILisReportElement> result = this.m_reportItemTable[elementTag] as List<ILisReportElement>;
            if (result == null)
            {
                result = new List<ILisReportElement>(10);
                lock (this.m_reportItemTable)
                {
                    this.m_reportItemTable[elementTag] = result;
                }
            }
            return result;
        }
        #endregion

        #region
        //private Type m_declareCommonItemType;
        //private Type m_declareGraphItemType;
        //private Type m_declareCustomItemType;
        //private Type m_declarePatientType;
        //private Type m_declareExamType;
        //private ILisReportElement m_patient;
        //private ILisReportElement m_examInfo;
        //private ReportFKImpl m_reportFK;
        //private readonly List<ILisReportElement> m_patientList;
        //private readonly List<ILisReportElement> m_examList;
        //private readonly List<ILisReportElement> m_commonItemList;
        //private readonly List<ILisReportElement> m_graphItemList;
        //private readonly List<ILisReportElement> m_customItemList;
        //private static readonly Type m_defaultCommonItemType = typeof(ReportItemElement);
        //private static readonly Type m_defaultGraphItemType = typeof(ReportGraphElement);
        //private static readonly Type m_defaultCustomItemType = typeof(ReportCustomElement);
        //private static readonly Type m_defaultPatientType = typeof(ReportPatientElement);
        //private static readonly Type m_defaultExamType = typeof(ReportExamElement);
        #endregion
    }
}
