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

        }
    }
}