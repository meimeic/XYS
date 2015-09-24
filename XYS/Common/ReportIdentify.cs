using XYS.Model;
using System.Collections.Generic;
namespace XYS.Common
{
    /// <summary>
    /// 定义报告主键类，主键类用于将报告的各个部分关联起来
    /// </summary>
    /// <typeparam name="TElement"></typeparam>
    public abstract class ReportIdentify<TElement>
        where TElement : IPKElement
    {
        private readonly HashSet<TElement> m_PKSet=new HashSet<TElement>();
        private IReportElement m_reportElement;
        public void AddPKElement(TElement element)
        {
            this.m_PKSet.Add(element);
        }
        public HashSet<TElement> PKCollection
        {
            get { return this.m_PKSet; }
        }
        public IReportElement ReportElement
        {
            get { return this.m_reportElement; }
            set { this.m_reportElement = value; }
        }
    }
}
