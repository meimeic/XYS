using System.Collections;
using System.Collections.Generic;

using XYS.Lis.Core;
namespace XYS.Lis.DAL
{
    public class LisReportPatientDAL:BasicDAL<ReportPatientElement>
    {
        #region 实现BasicDAL抽象方法
        protected override string GenderSql(Hashtable equalTable)
        {
            string sql = @"select r.cname as patientname,patno as pid,id_number_patient as cid,genderno,age as agevalue,ageunitno,sicktypeno as clinictypeno,hospitalizedtimes as visittimes,d.cname as deptname,isnull(r.doctor,b.cname) as doctor,bed as bedno,zdy2 as clinicaldiagnosis,zdy5 as explanation
                                   from reportform as r left outer join department as d on r.DeptNo=d.DeptNo
                                   left outer join doctor as b ON r.doctor = CONVERT(varchar(20), b.doctorno)";
            return sql + this.GetSQLWhere(equalTable);
        }
        #endregion

        #region 重写BasicDAL虚方法
        protected override void AfterFill(ReportPatientElement t)
        {
            base.AfterFill(t);
            t.PID = t.PID.Trim();
        }
        #endregion
    }
}
