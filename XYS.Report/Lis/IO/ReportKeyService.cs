using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using XYS.Report.Lis.Model;
using XYS.Report.Lis.IO.SQLServer;
namespace XYS.Report.Lis.IO
{
    public class ReportKeyService
    {
        #region 私有字段
        private readonly LisReportPKDAL m_pkDAL;
        #endregion

        #region 构造函数
        public ReportKeyService()
        {
            this.m_pkDAL = new LisReportPKDAL();
        }
        #endregion
        
        #region 实例属性
        public LisReportPKDAL PKDAL
        {
            get { return this.m_pkDAL; }
        }
        #endregion

        #region 公共方法
        public void InitReportPK(Require req, List<LisReportPK> PKList)
        {
            this.PKDAL.InitReportKey(req, PKList);
        }
        public void InitReportPK(string where, List<LisReportPK> PKList)
        {
            this.PKDAL.InitReportKey(where, PKList);
        }
        #endregion
    }
}
