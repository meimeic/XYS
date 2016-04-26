using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace XYS.Report.WS
{
    public delegate void ApplyHandleComplete(LabApplyInfo applyInfo);
    public class ReportService
    {
        private ApplyHandleComplete m_handleComplete;
        public ReportService()
        {
        }

        public void HandleApply(LabApplyInfo applyInfo)
        {
            List<Apply> applyCollection = applyInfo.ApplyCollection;
            if (applyCollection != null && applyCollection.Count > 0)
            {
                foreach (Apply app in applyCollection)
                {
                    //处理审核的报告单
                    if (app.ApplyStatus == 7)
                    {
 
                    }
                }
            }
        }
    }
}