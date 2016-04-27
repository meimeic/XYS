using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

using XYS.Report.Lis;
using XYS.Report.Lis.Model;
using XYS.Report.Lis.IO.SQLServer;
namespace XYS.Report.WS
{
    public delegate void ApplyHandleComplete(LabApplyInfo applyInfo);
    public class ReportService
    {
        #region 私有字段
        private readonly Reporter m_reporter;
        private readonly LisReportPKDAL m_pkDAL;
        private readonly ConcurrentBag<ReportReportElement> m_reportBag;
        private event ApplyHandleComplete m_handleCompleteEvent;
        #endregion

        #region 构造函数
        public ReportService()
        {
            this.m_pkDAL = new LisReportPKDAL();
            this.m_reporter = new DefaultReporter();
            this.m_reportBag = new ConcurrentBag<ReportReportElement>();
        }
        #endregion

        #region 事件属性
        public event ApplyHandleComplete HandleCompleteEvent
        {
            add { this.m_handleCompleteEvent += value; }
            remove { this.m_handleCompleteEvent -= value; }
        }
        #endregion

        #region 实例属性
        public Reporter Reporter
        {
            get { return this.m_reporter; }
        }
        public ConcurrentBag<ReportReportElement> ReportBag
        {
            get { return this.m_reportBag; }
        }
        public LisReportPKDAL PKDAL
        {
            get { return this.m_pkDAL; }
        }
        #endregion

        #region
        public void HandleApplyInfo(LabApplyInfo applyInfo)
        {
            List<Apply> applyCollection = applyInfo.ApplyCollection;
            if (applyCollection != null && applyCollection.Count > 0)
            {
                List<LisReportPK> PKList = new List<LisReportPK>(applyCollection.Count);
                foreach (Apply app in applyCollection)
                {
                    //获取审核的报告单主键
                    if (app.ApplyStatus == 7)
                    {
                        InitPK(app.ApplyNo, PKList);
                    }
                }
                //处理
                if (PKList.Count > 0)
                {
                    //
                    ReportReportElement report = null;
                    foreach (LisReportPK pk in PKList)
                    {
                        report = new ReportReportElement();
                        this.ReportBag.Add(report);
                    }
                }
            }
            this.OnHandleComplete(applyInfo);
        }
        private void InitPK(string appNo,List<LisReportPK> PKList)
        {
            string where = "where serialno='" + appNo.Trim() + "'";
            this.InitReportPK(where, PKList);
        }
        #endregion

        #region 触发事件
        protected void OnHandleComplete(LabApplyInfo applyInfo)
        {
            ApplyHandleComplete handler = this.m_handleCompleteEvent;
            if (handler != null)
            {
                handler(applyInfo);
            }
        }
        #endregion
       
        #region 
        protected void InitReportPK(Require req, List<LisReportPK> PKList)
        {
            this.PKDAL.InitReportKey(req, PKList);
        }
        protected void InitReportPK(string where, List<LisReportPK> PKList)
        {
            this.PKDAL.InitReportKey(where, PKList);
        }
        #endregion
    }
}