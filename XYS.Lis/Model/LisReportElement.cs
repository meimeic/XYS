using XYS.Lis.Core;
namespace XYS.Lis.Model
{
    public abstract class LisReportElement:ILisReportElement
    {
        #region 私有字段
        private readonly string m_reportElementName;
        private readonly long m_reportElementValue;
        #endregion

        #region 受保护的构造函数
        protected LisReportElement(string name,long value)
        {
            m_reportElementName = name;
            m_reportElementValue = value;
        }
        #endregion
        
        #region IReportElement实现
        public string ReportElementName
        {
            get { return m_reportElementName; }
        }

        public long ReportElementValue
        {
            get { return m_reportElementValue; }
        }
        #endregion
    }
}
