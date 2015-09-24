using XYS.Lis.Core;
using XYS.Model;
namespace XYS.Lis.Model
{
   public abstract class AbstractPatientElement:IReportPatientElement
   {
       #region 私有字段
       private readonly ReportElementType m_elementType;
       private string m_patientName;
       private string m_pid;
       private string m_cid;
       private GenderType m_gender;
       private Age m_age;
       private ClinicType m_sickType;
       private int m_visitTimes;
       private string m_deptName;
       private string m_doctor;
       private string m_clinicalDiagnosis;
       private string m_explanation;
       #endregion

       #region 受保护的构造方法
       protected AbstractPatientElement(ReportElementType elementType)
       {
           this.m_elementType = elementType;
       }
       #endregion

       #region 实现IPatientElement接口
       public string SickTypeName
       {
           get
           {
               switch (this.m_sickType)
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
        public ClinicType SickType
        {
            get { return this.m_sickType; }
            set { this.m_sickType = value; }
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
        private string Explanation
        {
            get { return this.m_explanation; }
            set { this.m_explanation = value; }
        }
        #endregion

        #region  内部类
        internal class Age
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
