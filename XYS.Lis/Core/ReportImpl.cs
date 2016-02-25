using System;
using System.Collections;
using System.Collections.Generic;

using XYS.Common;
using XYS.Lis;
using XYS.Lis.DAL;
using XYS.Lis.Export.Model;
namespace XYS.Lis.Core
{
    public class ReportImpl : ReporterWrapperImpl, IReport
    {
        #region
        private LisReporterKeyDAL m_reportKeyDAL;
        #endregion

        #region 构造函数
        public ReportImpl(IReporter reporter)
            : base(reporter)
        {
            this.m_reportKeyDAL = new LisReporterKeyDAL();
        }
        #endregion

        #region 属性
        public LisReporterKeyDAL ReportKeyDAL
        {
            get { return this.m_reportKeyDAL; }
            set { this.m_reportKeyDAL = value; }
        }
        #endregion

        #region 实现IReport接口
        //public void InitReport(ReportKey key, ReportReportElement report)
        //{
        //    this.Reporter.FillReportElement(report, key);
        //}
        //public void InitReport(LisSearchRequire require, ReportReportElement report)
        //{
        //    ReportKey key = GetReportKey(require);
        //    this.InitReport(key, report);
        //}

        //public void InitReports(LisSearchRequire require, List<IReportElement> reportList)
        //{
        //    List<ReportKey> keyList = GetReportKeyList(require);
        //    this.InitReports(keyList, reportList);
        //}
        //public void InitReports(List<ReportKey> keyList, List<IReportElement> reportList)
        //{
        //    ReportReportElement rre;
        //    if (keyList != null && keyList.Count > 0)
        //    {
        //        foreach (ReportKey key in keyList)
        //        {
        //            rre = new ReportReportElement();
        //            InitReport(key, rre);
        //            reportList.Add(rre);
        //        }
        //    }
        //}

        //public bool OperateReport(ReportReportElement report)
        //{
        //    return this.Reporter.Option(report);
        //}

        //public bool OperateReport(List<IReportElement> reportList)
        //{
        //    //return this.Reporter.Option(reportList,ReportElementTag.Report);
        //    return true;
        //}

        //public void HandleReport(LisSearchRequire require, ReportReportElement report)
        //{
        //    this.InitReport(require, report);
        //    this.OperateReport(report);
        //}

        //public void HandleReports(List<ReportKey> keyList, List<IReportElement> reportList)
        //{
        //    this.InitReports(keyList, reportList);
        //    this.OperateReport(reportList);
        //}

        //public void HandleReports(LisSearchRequire require, List<IReportElement> reportList)
        //{
        //    List<ReportKey> keyList = GetReportKeyList(require);
        //    this.HandleReports(keyList, reportList);
        //}

        public void InitReport(ReportReport export, ReportKey key)
        {
            if (key != null)
            {
                this.Reporter.InitExport(export, key);
            }
        }
        public void InitReport(ReportReport export, LisRequire require)
        {
            ReportKey key = this.GetReportKey(require);
            InitReport(export, key);
        }

        public void InitReports(List<ReportReport> exportList, List<ReportKey> keyList)
        {
            this.Reporter.InitExport(exportList, keyList);
        }
        public void InitReports(List<ReportReport> exportList, LisRequire require)
        {
            List<ReportKey> keyList = GetReportKeyList(require);
            InitReports(exportList, keyList);
        }
        #endregion

        #region
        protected virtual List<ReportKey> GetReportKeyList(LisRequire require)
        {
            return this.ReportKeyDAL.GetReportKey(require);
        }
        protected virtual ReportKey GetReportKey(LisRequire require)
        {
            List<ReportKey> result = this.ReportKeyDAL.GetReportKey(require);
            if (result.Count > 0)
            {
                return result[0];
            }
            return null;
        }
        #endregion
    }
}