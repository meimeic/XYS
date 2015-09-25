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
        private const ReportElementType m_defaultElementType = ReportElementType.ReportElement;
        #endregion

        #region 私有实例字段
        private ILisPatientElement m_patient;
        private ILisReportElement m_examInfo;
        private List<ILisReportElement> m_commonItemList;
        private List<ILisReportElement> m_graphItemList;
        private List<ILisReportElement> m_customItemList;
        #endregion

        #region 公共构造函数
        public ReportReportElement()
            : base(m_defaultElementType)
        {
            this.m_patient = new ReportPatientElement();
            this.m_examInfo = new ReportExamElement();
            this.m_commonItemList = new List<ILisReportElement>();
            this.m_graphItemList = new List<ILisReportElement>();
            this.m_customItemList = new List<ILisReportElement>();
        }
        public ReportReportElement(ReportElementType elementType)
            : base(elementType)
        {
        }
        #endregion

        #region 实例属性
        public ILisPatientElement Patient
        {
            get { return this.m_patient; }
            set { this.m_patient = value; }
        }
        public ILisReportElement ExamInfo
        {
            get { return this.m_examInfo; }
            set { this.m_examInfo = value; }
        }
        public List<ILisReportElement> CommonItemList
        {
            get { return this.m_commonItemList; }
            set { this.m_commonItemList = value; }
        }
        public List<ILisReportElement> GraphItemList
        {
            get { return this.m_graphItemList; }
            set { this.m_graphItemList = value; }
        }
        public List<ILisReportElement> CustomItemList
        {
            get { return this.m_customItemList; }
            set { this.m_customItemList = value; }
        }
        #endregion

    }
}
