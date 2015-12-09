using System;
using System.Collections.Generic;

using XYS.Model;
using XYS.Lis.Core;
using XYS.Lis.Model;
using XYS.Lis.Service.Models.Report;
namespace XYS.Lis.Service.Common
{
    public class ReportCommon
    {
        #region
        static IReport report = ReportManager.GetReporter(typeof(ReportCommon));
        private volatile static ReportCommon m_report = new ReportCommon();
        private static readonly object locker = new object();
        private IModelConvert m_convert;
        #endregion

        #region
        private ReportCommon()
        {
            this.m_convert = new ModelConvert();
        }
        #endregion
        public static ReportCommon ReportOperate
        {
            get
            {
                if (m_report == null)
                {
                    lock (locker)
                    {
                        if (m_report == null)
                        {
                            m_report = new ReportCommon();
                        }
                    }
                }
                return m_report;
            }
        }
        //
        public ReportReportElement GetLisReport(LisSearchRequire require)
        {
            ReportReportElement rre = new ReportReportElement();
            report.HandleReport(require, rre);
            return rre;
        }
        public void SetLisReportList(List<ILisReportElement> lisReportList, LisSearchRequire require)
        {
            report.HandleReports(require, lisReportList);
        }
        //
        public ReportReport GetReport(LisSearchRequire require)
        {
            ReportReport rr = new ReportReport();
            ReportReportElement rre = GetLisReport(require);
            ConvertReport(rre, rr);
            return rr;
        }
        public void SetReportList(List<IReportModel> reportList, LisSearchRequire require)
        {
            List<ILisReportElement> lisReportList = new List<ILisReportElement>(100);
            SetLisReportList(lisReportList, require);
            ConvertReportList(lisReportList, reportList);
        }

        public void SetReportListByPID(List<IReportModel> reportList, string pid)
        {
            LisSearchRequire require = new LisSearchRequire(10, 365);
            require.EqualFields.Add("patno", pid);
            SetReportList(reportList, require);
        }
        public void SetReportListBySerailNo(List<IReportModel> reportList, string serialNo)
        {
            LisSearchRequire require = new LisSearchRequire(10, 365);
            require.EqualFields.Add("serialno", serialNo);
            SetReportList(reportList, require);
        }

        protected void ConvertReport(ReportReportElement rre, ReportReport rr)
        {
            if (this.m_convert != null)
            {
                this.m_convert.Convert2Export(rre, rr);
            }
        }
        protected void ConvertReportList(List<ILisReportElement> rreList, List<IReportModel> rrList)
        {
            if (this.m_convert != null)
            {
                this.m_convert.Convert2Export(rreList, rrList, ReportElementTag.ReportElement);
            }
        }
    }
}