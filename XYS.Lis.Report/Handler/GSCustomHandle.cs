﻿using System;
using System.Collections.Generic;

using XYS.Report;
using XYS.Lis.Report.Persistent;
namespace XYS.Lis.Report.Handler
{
    public class GSCustomHandle : HandleSkeleton
    {

        public GSCustomHandle(LabReportDAL dal)
            : base(dal)
        { }

        protected override bool InnerHandle(IFillElement element, IReportKey RK)
        {
            element.ReportID = RK.ID;
            return true;
        }

        protected override bool InnerHandle(List<IFillElement> elements, IReportKey RK)
        {
            if (IsExist(elements))
            {
                LOG.Info("骨髓自定义项集合处理");
                bool result = false;
                for (int i = elements.Count - 1; i >= 0; i--)
                {
                    result = this.InnerHandle(elements[i], RK);
                    if (!result)
                    {
                        elements.RemoveAt(i);
                    }
                }
            }
            return true;
        }
    }
}
