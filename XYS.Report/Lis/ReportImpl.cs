using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using XYS.Report.Lis.Model;
using XYS.Report.Lis.IO.SQLServer;
namespace XYS.Report.Lis
{
    public class ReportImpl : ReportWrapper, IReport
    {
        #region 字段
        private LisReportPKDAL m_pkDAL;
        #endregion

        #region 构造函数
        public ReportImpl(Reporter reporter)
            : base(reporter)
        {
            this.m_pkDAL = new LisReportPKDAL();
        }
        #endregion

        #region 属性
        public LisReportPKDAL PKDAL
        {
            get { return this.m_pkDAL; }
        }
        #endregion

        #region 实现ireport接口
        public void Operate(ReportReportElement report, Action<ReportReportElement> callbak = null)
        {
            this.Reporter.OperateReport(report, callbak);
        }
        public void Operate(List<ReportReportElement> reportList, Action<ReportReportElement> callbak = null)
        {
            //if (reportList != null && reportList.Count > 0)
            //{
            //    foreach (ReportReportElement report in reportList)
            //    {
            //        this.Operate(report, callbak);
            //    }
            //}

            ///测试代码
            //this.Reporter.OperateReport(reportList);
            this.Reporter.OperateReportAsync(reportList);
        }
        
        public List<ReportReportElement> GetReports(Require req)
        {
            ReportReportElement rre = null;
            List<LisReportPK> PKList = new List<LisReportPK>();
            List<ReportReportElement> reportList = new List<ReportReportElement>();
            InitReportPK(req, PKList);
            if (PKList.Count > 0)
            {
                foreach (LisReportPK pk in PKList)
                {
                    rre = new ReportReportElement();
                    rre.LisPK = pk;
                    reportList.Add(rre);
                }
            }
            return reportList;
        }
        public List<ReportReportElement> GetReports(string where)
        {
            ReportReportElement rre = null;
            List<LisReportPK> PKList = new List<LisReportPK>();
            List<ReportReportElement> reportList = new List<ReportReportElement>();
            InitReportPK(where, PKList);
            if (PKList.Count > 0)
            {
                foreach (LisReportPK pk in PKList)
                {
                    rre = new ReportReportElement();
                    rre.LisPK = pk;
                    reportList.Add(rre);
                }
            }
            return reportList;
        }

        public List<LisReportPK> GetReportPK(Require req)
        {
            List<LisReportPK> PKList = new List<LisReportPK>();
            InitReportPK(req, PKList);
            return PKList;
        }
        public List<LisReportPK> GetReportPK(string where)
        {
            List<LisReportPK> PKList = new List<LisReportPK>();
            InitReportPK(where, PKList);
            return PKList;
        }
        #endregion

        #region 私有方法
        private void InitReportPK(Require req, List<LisReportPK> PKList)
        {
            this.PKDAL.InitReportKey(req, PKList);
        }
        private void InitReportPK(string where, List<LisReportPK> PKList)
        {
            this.PKDAL.InitReportKey(where, PKList);
        }
        #endregion
    }
}
