using XYS.Common;
using XYS.Lis.Core;
namespace XYS.Lis.Model
{
    public class ReportPatientElement :AbstractReportElement,IPatient
    {
        #region 私有常量字段
        private static readonly string m_defaultPatientSQL = @"select r.cname as patientname,patno as pid,id_number_patient as cid,genderno,age as agevalue,ageunitno,sicktypeno as clinictypeno,hospitalizedtimes as visittimes,d.cname as deptname,isnull(r.doctor,b.cname) as doctor,bed as bedno,zdy2 as clinicaldiagnosis,zdy5 as explanation
                                                                    from reportform as r left outer join department as d on r.DeptNo=d.DeptNo
                                                                    left outer join doctor as b ON r.doctor = CONVERT(varchar(20), b.doctorno)";
        #endregion

        #region 私有字段
        #endregion

        #region 公共属性
        #endregion

        #region 构造函数
        public ReportPatientElement()
            : this(m_defaultPatientSQL)
        {
        }
        
        public ReportPatientElement(string sql)
            : base(sql)
        {
        }
        #endregion
        
        #region 实现父类抽象方法
        protected override void Afterward()
        {
            throw new System.NotImplementedException();
        }
        #endregion
        
        #region 实现ipatient接口属性
        public string ClinicName
        {
            get { throw new System.NotImplementedException(); }
        }
        public string GenderName
        {
            get { throw new System.NotImplementedException(); }
        }
        #endregion
    }
}
