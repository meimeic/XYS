using System;
using System.Collections.Generic;
using XYS.Model;
using XYS.Lis.Core;
using XYS.Lis.Model;
namespace XYS.Lis.Model
{
    public class ReportReportElement:AbstractReportElement
    {
        #region 私有常量字段
        private static readonly ReportElementType m_defaultElementType = ReportElementType.ReportElement;
        private static readonly Type m_defaultCommonItemType = typeof(ReportCommonItemElement);
        private static readonly Type m_defaultGraphItemType = typeof(ReportGraphItemElement);
        private static readonly Type m_defaultCustomItemType = typeof(ReportCustomItemElement);
        private static readonly Type m_defaultPatientType = typeof(ReportPatientElement);
        private static readonly Type m_defaultExamType = typeof(ReportExamElement);
        #endregion

        #region 私有实例字段
        private int m_orderNo;
        private string m_reportTitle;
        private int m_printModelNo;
        private ILisReportElement m_patient;
        private ILisReportElement m_examInfo;
        private Type m_declareCommonItemType;
        private Type m_declareGraphItemType;
        private Type m_declareCustomItemType;
        private Type m_declarePatientType;
        private Type m_declareExamType;
        #endregion

        #region 私有只读字段
        private readonly List<ILisReportElement> m_commonItemList;
        private readonly List<ILisReportElement> m_graphItemList;
        private readonly List<ILisReportElement> m_customItemList;
        #endregion

        #region 公共构造函数
        public ReportReportElement()
            : base(m_defaultElementType,"")
        {
            this.m_patient = (ILisReportElement)m_defaultPatientType.Assembly.CreateInstance(m_defaultPatientType.FullName);
            this.m_examInfo = (ILisReportElement)m_defaultExamType.Assembly.CreateInstance(m_defaultExamType.FullName);
            this.m_commonItemList = new List<ILisReportElement>();
            this.m_graphItemList = new List<ILisReportElement>();
            this.m_customItemList = new List<ILisReportElement>();
        }
        #endregion

        #region 实例属性
        public ILisReportElement Patient
        {
            get { return this.m_patient; }
            //set { this.m_patient = value; }
        }
        public ILisReportElement ExamInfo
        {
            get { return this.m_examInfo; }
            //set { this.m_examInfo = value; }
        }
        public List<ILisReportElement> CommonItemList
        {
            get { return this.m_commonItemList; }
            //set { this.m_commonItemList = value; }
        }
        public List<ILisReportElement> GraphItemList
        {
            get { return this.m_graphItemList; }
            //set { this.m_graphItemList = value; }
        }
        public List<ILisReportElement> CustomItemList
        {
            get { return this.m_customItemList; }
            //set { this.m_customItemList = value; }
        }

        public Type CommonItemType
        {
            get 
            {
                if (this.m_declareCommonItemType != null)
                {
                    return this.m_declareCommonItemType;
                }
                else
                {
                    return m_defaultCommonItemType;
                }
            }
            set { this.m_declareCommonItemType = value; }
        }
        public Type GraphItemType
        {
            get 
            {
                if (this.m_declareGraphItemType != null)
                {
                    return this.m_declareGraphItemType;
                }
                else
                {
                    return m_defaultGraphItemType;
                }
            }
            set { this.m_declareGraphItemType = value; }
        }
        public Type CustomItem
        {
            get 
            {
                if (this.m_declareCustomItemType != null)
                {
                    return this.m_declareCustomItemType;
                }
                else
                {
                    return m_defaultCustomItemType;
                }
            }
            set { this.m_declareCustomItemType = value; }
        }
        public Type PatientType
        {
            get 
            {
                if (this.m_declarePatientType != null)
                {
                    return this.m_declarePatientType;
                }
                else
                {
                    return m_defaultPatientType;
                }
            }
            set { this.m_declarePatientType = value; }
        }
        public Type ExamType
        {
            get 
            {
                if (this.m_declareExamType != null)
                {
                    return this.m_declareExamType;
                }
                else
                {
                    return m_defaultExamType;
                }
            }
            set { this.m_declareExamType = value; }
        }
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
        private string ReportTitle
        {
            get { return this.m_reportTitle; }
            set { this.m_reportTitle = value; }
        }
        #endregion

        #region 公共方法

        public void Init()
        {

        }
        public void ReportClear()
        {

        }
        #endregion
    }
}
