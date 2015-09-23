using XYS.Model;
using System.Collections.Generic;
namespace XYS.Common
{
    /// <summary>
    /// 定义报告主键类，主键类用于将报告的各个部分关联起来
    /// </summary>
    /// <typeparam name="TElement"></typeparam>
    public class ReportIdentify<TElement>
        where TElement : IPKElement
    {
        private readonly HashSet<TElement> PKSet=new HashSet<TElement>();
        public void AddPKElement(TElement element)
        {
            this.PKSet.Add(element);
        }
        public HashSet<TElement> PKCollection
        {
            get { return this.PKSet; }
        }
    }
}
