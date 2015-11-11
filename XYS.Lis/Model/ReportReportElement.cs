using System;
using System.Collections.Generic;
using System.Collections;
using XYS.Model;
using XYS.Lis.Core;
using XYS.Lis.Model;
namespace XYS.Lis.Model
{
    public class ReportReportElement:AbstractReportElement
    {

        #region 私有常量字段
        private static readonly ReportElementTag m_defaultElementTag = ReportElementTag.ReportElement;
        //private static readonly Type m_defaultCommonItemType = typeof(ReportItemElement);
        //private static readonly Type m_defaultGraphItemType = typeof(ReportGraphElement);
        //private static readonly Type m_defaultCustomItemType = typeof(ReportCustomElement);
        //private static readonly Type m_defaultPatientType = typeof(ReportPatientElement);
        //private static readonly Type m_defaultExamType = typeof(ReportExamElement);
        #endregion

        #region 私有实例字段
        private int m_orderNo;
        private string m_reportTitle;
        private int m_printModelNo;
        private string m_remark;
        private List<int> m_parItemList;
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
        #endregion

        #region 私有只读字段
        private readonly Hashtable m_itemTable;
        private readonly Hashtable m_reportItemTable;
        #endregion

        #region 公共构造函数
        public ReportReportElement()
            : base(m_defaultElementTag,"")
        {
            //this.m_patient = (ILisReportElement)m_defaultPatientType.Assembly.CreateInstance(m_defaultPatientType.FullName);
            //this.m_examInfo = (ILisReportElement)m_defaultExamType.Assembly.CreateInstance(m_defaultExamType.FullName);
            //this.m_examList = new List<ILisReportElement>();
            //this.m_patientList = new List<ILisReportElement>();
            //this.m_commonItemList = new List<ILisReportElement>();
            //this.m_graphItemList = new List<ILisReportElement>();
            //this.m_customItemList = new List<ILisReportElement>();
            this.m_parItemList = new List<int>(5);
            this.m_itemTable = new Hashtable(5);
            this.m_reportItemTable = new Hashtable(5);
        }
        #endregion

        #region 实例属性
        //public List<ILisReportElement> PatientList
        //{
        //    get { return this.m_patientList; }
        //}
        //public List<ILisReportElement> ExamList
        //{
        //    get { return this.m_examList; }
        //}
        //public List<ILisReportElement> CommonItemList
        //{
        //    get { return this.m_commonItemList; }
        //}
        //public List<ILisReportElement> GraphItemList
        //{
        //    get { return this.m_graphItemList; }
        //    //set { this.m_graphItemList = value; }
        //}
        //public List<ILisReportElement> CustomItemList
        //{
        //    get { return this.m_customItemList; }
        //    //set { this.m_customItemList = value; }
        //}
        //type属性
        //public Type CommonItemType
        //{
        //    get 
        //    {
        //        if (this.m_declareCommonItemType != null)
        //        {
        //            return this.m_declareCommonItemType;
        //        }
        //        else
        //        {
        //            return m_defaultCommonItemType;
        //        }
        //    }
        //    set { this.m_declareCommonItemType = value; }
        //}
        //public Type GraphItemType
        //{
        //    get 
        //    {
        //        if (this.m_declareGraphItemType != null)
        //        {
        //            return this.m_declareGraphItemType;
        //        }
        //        else
        //        {
        //            return m_defaultGraphItemType;
        //        }
        //    }
        //    set { this.m_declareGraphItemType = value; }
        //}
        //public Type CustomItem
        //{
        //    get 
        //    {
        //        if (this.m_declareCustomItemType != null)
        //        {
        //            return this.m_declareCustomItemType;
        //        }
        //        else
        //        {
        //            return m_defaultCustomItemType;
        //        }
        //    }
        //    set { this.m_declareCustomItemType = value; }
        //}
        //public Type PatientType
        //{
        //    get 
        //    {
        //        if (this.m_declarePatientType != null)
        //        {
        //            return this.m_declarePatientType;
        //        }
        //        else
        //        {
        //            return m_defaultPatientType;
        //        }
        //    }
        //    set { this.m_declarePatientType = value; }
        //}
        //public Type ExamType
        //{
        //    get 
        //    {
        //        if (this.m_declareExamType != null)
        //        {
        //            return this.m_declareExamType;
        //        }
        //        else
        //        {
        //            return m_defaultExamType;
        //        }
        //    }
        //    set { this.m_declareExamType = value; }
        //}
        public List<int> ParItemList
        {
            get { return this.m_parItemList; }
        }
        public Hashtable ItemTable
        {
            get { return this.m_itemTable; }
        }
        public Hashtable ReportItemTable
        {
            get { return this.m_reportItemTable; }
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
        public string ReportTitle
        {
            get { return this.m_reportTitle; }
            set { this.m_reportTitle = value; }
        }
        public string Remark
        {
            get { return this.m_remark; }
            set { this.m_remark = value; }
        }
        #endregion

        #region 公共方法

        public void Init()
        {

        }
        public void ReportClear()
        {
        }
        public void AddTableItem(ReportElementTag elementTag,Hashtable table)
        {
            lock (this.m_itemTable)
            {
                if (table != null)
                {
                    this.m_itemTable[elementTag] = table;
                }
            }
        }
        public void AddItem(ReportElementTag elementTag,ILisReportElement element)
        {
            lock (this.m_itemTable)
            {
                if (element != null)
                {
                    this.m_itemTable[elementTag] = element;
                }
            }
        }
        public Hashtable GetItem(ReportElementTag elementTag)
        {
            return this.m_itemTable[elementTag] as Hashtable;
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
    }
}
