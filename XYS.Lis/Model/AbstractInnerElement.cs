using XYS.Lis.Core;
namespace XYS.Lis.Model
{
    public class AbstractInnerElement : IReportElement
    {
        #region 字段
        private readonly ReportElementTag m_elementTag;
        #endregion

        #region 构造函数
        protected AbstractInnerElement()
        {
            this.m_elementTag = ReportElementTag.Inner;
        }
        #endregion

        #region 实现接口
        public ReportElementTag ElementTag
        {
            get { return this.m_elementTag; }
        }
        
        public void After()
        {
        }
        #endregion
    }
}
