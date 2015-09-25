using XYS.Lis.Core;
using XYS.Model;
using XYS.Common;
namespace XYS.Lis.Model
{
    public abstract class AbstractPatientElement : ILisPatientElement,ILisReportElement
    {
        #region 私有字段
        private readonly ReportElementType m_elementType;
        private string m_patientName;
        private string m_pid;
        private string m_cid;
        private GenderType m_gender;
        private Age m_age;
        private ClinicType m_clinicType;
        private int m_visitTimes;
        private string m_deptName;
        private string m_doctor;
        private string m_clinicalDiagnosis;
        private string m_explanation;
        //private ReportKey m_reportKey;
        #endregion

        #region 受保护的构造方法
        protected AbstractPatientElement(ReportElementType elementType)
        {
            this.m_elementType = elementType;
        }
        #endregion

        #region 实现IPatientElement接口
        public string ClinicTypeName
        {
            get
            {
                switch (this.m_clinicType)
                {
                    case ClinicType.clinic:
                        return "门诊";
                    case ClinicType.hospital:
                        return "住院";
                    case ClinicType.other:
                        return "其他";
                    case ClinicType.none:
                        return "未知";
                    default:
                        return "未知";
                }
            }
        }
        public string GenderTypeName
        {
            get
            {
                switch (this.m_gender)
                {
                    case GenderType.female:
                        return "女";
                    case GenderType.male:
                        return "男";
                    case GenderType.other:
                        return "未知";
                    case GenderType.none:
                        return "未定";
                    default:
                        return "未知";
                }
            }
        }
        #endregion

        #region 实现IReportElement接口
        public ReportElementType ElementType
        {
            get { return m_elementType; }
        }
        //public ReportKey ReporterKey
        //{
        //    get { return this.m_reportKey; }
        //    set { this.m_reportKey = value; }
        //}
        #endregion

        #region 实现ILisReportElement
        public void AfterFill()
        {
            this.Afterward();
        }
        #endregion

        #region 公共属性
        public string PatientName
        {
            get { return this.m_patientName; }
            set { this.m_patientName = value; }
        }
        public string PID
        {
            get { return this.m_pid; }
            set { this.m_pid = value; }
        }
        public string CID
        {
            get { return this.m_cid; }
            set { this.m_cid = value; }
        }
        public GenderType Gender
        {
            get { return this.m_gender; }
            set { this.m_gender = value; }
        }
        public Age Ager
        {
            get { return this.m_age; }
            set { this.m_age = value; }
        }
        public ClinicType ClinicalType
        {
            get { return this.m_clinicType; }
            set { this.m_clinicType = value; }
        }
        public int VisitTimes
        {
            get { return this.m_visitTimes; }
            set { this.m_visitTimes = value; }
        }
        public string DeptName
        {
            get { return this.m_deptName; }
            set { this.m_deptName = value; }
        }
        public string Doctor
        {
            get { return this.m_doctor; }
            set { this.m_doctor = value; }
        }
        public string ClinicalDiagnosis
        {
            get { return this.m_clinicalDiagnosis; }
            set { this.m_clinicalDiagnosis = value; }
        }
        public string Explanation
        {
            get { return this.m_explanation; }
            set { this.m_explanation = value; }
        }
        #endregion

        #region 受保护的虚函数
        protected virtual void Afterward()
        { }
        #endregion

        #region  内部类
        public class Age
        {
            #region 内部类私有字段
            private int m_value;
            private AgeType m_unit;
            #endregion

            #region 共有构造方法
            public Age(int value, AgeType unit)
            {
                m_value = value;
                m_unit = unit;
            }
            #endregion

            #region 公共方法
            public override string ToString()
            {
                switch (this.m_unit)
                {
                    case AgeType.year:
                        return this.m_value.ToString() + " 岁";
                    case AgeType.month:
                        return this.m_value.ToString() + " 月";
                    case AgeType.day:
                        return this.m_value.ToString() + " 天";
                    default:
                        return this.m_value.ToString() + " 岁";
                }
            }
            #endregion
        }
        #endregion

    
    }
}
