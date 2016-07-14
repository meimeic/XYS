using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

using log4net;

using XYS.Util;
using XYS.Report;
using XYS.Lis.Report.Model;
using XYS.Lis.Report.Persistent;
namespace XYS.Lis.Report
{
    public class ReportService
    {
        #region 静态变量
        private static ReportPKDAL PKDAL;
        private static ILog LOG = LogManager.GetLogger("LisReportHandle");
        #endregion

        #region 构造函数
        static ReportService()
        {
            //初始化handleTable
            PKDAL = new ReportPKDAL();
        }
        public ReportService()
        {
            this.Init();
        }
        #endregion

        #region 公共方法
        public void InitReportPK(Require req, List<ReportPK> PKList)
        {
            PKDAL.InitReportKey(req, PKList);
        }
        public void InitReportPK(string where, List<ReportPK> PKList)
        {
            PKDAL.InitReportKey(where, PKList);
        }
        #endregion

        #region 辅助方法
        #endregion

        #region 触发事件
        #endregion

        #region 私有方法
        private void Init()
        {
        }
        private void LogError(ReportReportElement report)
        {
            LOG.Error(report.HandleResult.Message, report.HandleResult.Exception);
        }
        #endregion

        #region 
        protected void HandleReport(LabReport report)
        {
            if (report.ReportPK == null)
            {
                //错误
            }
            else
            {
            }
        }
        #endregion
    }
}
