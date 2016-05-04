using System;
using System.Collections.Generic;

using XYS.Report.Lis.Persistent;
namespace XYS.Report.Lis
{
    public class ReportPKService
    {
        #region 只读字段
        private readonly ReportPKDAL m_PKDAL;
        #endregion

        #region 构造函数
        public ReportPKService()
        {
            this.m_PKDAL = new ReportPKDAL();
        }
        #endregion

        #region 受保护的属性
        protected ReportPKDAL PKDAL
        {
            get { return this.m_PKDAL; }
        }
        #endregion

        #region 公共方法
        public void InitReportPK(Require req, List<ReportPK> PKList)
        {
            this.PKDAL.InitReportKey(req, PKList);
        }
        public void InitReportPK(string where, List<ReportPK> PKList)
        {
            this.PKDAL.InitReportKey(where, PKList);
        }
        #endregion
    }
}
