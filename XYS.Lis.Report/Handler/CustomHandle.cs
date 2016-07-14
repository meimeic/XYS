using System;
using System.Collections.Generic;

using XYS.Report;
using XYS.Lis.Report;
using XYS.Lis.Report.Model;
namespace XYS.Lis.Report.Handler
{
    public class CustomHandle : HandleSkeleton
    {
        #region 构造函数
        public CustomHandle()
            : base()
        {
        }
        #endregion

        #region 实现父类抽象方法
        protected override bool HandleElement(IFillElement element, ReportPK RK)
        {
            throw new NotImplementedException();
        }

        protected override bool HandleElement(List<IFillElement> elements, ReportPK RK)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region custom内部处理逻辑
        #endregion
    }
}
