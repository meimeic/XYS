using System;
using System.IO;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Xml.Serialization;

using XYS.Report.Lis;
using XYS.Report.Lis.Model;
using XYS.Report.Lis.IO.SQLServer;
namespace XYS.Report.WS
{
    public class ReportService
    {
        #region
        private static readonly ReportService ServiceInstance;
        #endregion

        #region 私有字段
        private readonly Reporter m_reporter;
        private readonly XmlSerializer m_serializer;
        private readonly LisReportPKDAL m_pkDAL;
        private readonly ConcurrentBag<ReportReportElement> m_reportBag;
        #endregion

        #region 构造函数
        static ReportService()
        {
            ServiceInstance = new ReportService();
        }
        private ReportService()
        {
            this.m_pkDAL = new LisReportPKDAL();
            this.m_reporter = new DefaultReporter();
            this.m_serializer = new XmlSerializer(typeof(LabApplyInfo));
            this.m_reportBag = new ConcurrentBag<ReportReportElement>();
        }
        #endregion

        #region 静态属性
        public static ReportService ReporterService
        {
            get { return ServiceInstance; }
        }
        #endregion

        #region 实例属性
        public Reporter Reporter
        {
            get { return this.m_reporter; }
        }
        public XmlSerializer Serializer
        {
            get { return this.m_serializer; }
        }
        public LisReportPKDAL PKDAL
        {
            get { return this.m_pkDAL; }
        }
        public ConcurrentBag<ReportReportElement> ReportBag
        {
            get { return this.m_reportBag; }
        }
        #endregion

        #region 生产者方法
        public void Deserialize(string applyInfo)
        {
            if (applyInfo != null)
            {
                StringReader reader = new StringReader(applyInfo);
                try
                {
                    LabApplyInfo info = (LabApplyInfo)this.Serializer.Deserialize(reader);
                    this.HandleApplyInfo(info);
                }
                catch (InvalidOperationException ex)
                {
                }
            }
            else
            {
            }
        }
        protected void HandleApplyInfo(LabApplyInfo applyInfo)
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
                        this.InitReportPK(app.ApplyNo, PKList);
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
        }
        private void InitReportPK(string appNo, List<LisReportPK> PKList)
        {
            string where = "where serialno='" + appNo.Trim() + "'";
            this.D_InitReportPK(where, PKList);
        }
        #endregion

        #region 消费者方法
        #endregion

        #region 数据持久层处理
        private void D_InitReportPK(Require req, List<LisReportPK> PKList)
        {
            this.PKDAL.InitReportKey(req, PKList);
        }
        private void D_InitReportPK(string where, List<LisReportPK> PKList)
        {
            this.PKDAL.InitReportKey(where, PKList);
        }
        #endregion
    }
}