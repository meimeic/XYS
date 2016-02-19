using XYS.Common;
using XYS.Lis.Core;
namespace XYS.Lis.Model
{
    [Table()]
    [Export()]
    public class ReportPatientElement : AbstractReportElement, IPatient
    {
        #region 私有常量字段
        private static readonly ReportElementTag m_defaultElementTag = ReportElementTag.Patient;
        #endregion

        #region 私有字段
        private string m_patientName;
        private string m_pid;
        private string m_cid;

        private int m_genderNo;
        private GenderType m_gender;

        private Age m_age;
        private int m_ageValue;
        private int m_ageUnitNo;
        private AgeType m_ageUnit;

        private int m_clinicTypeNo;
        private ClinicType m_clinicType;

        private int m_visitTimes;
        private string m_deptName;
        private string m_doctor;
        private string m_bedNo;
        private string m_clinicalDiagnosis;
        private string m_explanation;
        #endregion

        #region 构造函数
        public ReportPatientElement()
            : base(m_defaultElementTag)
        {
        }
        public ReportPatientElement(string sql)
            : base(m_defaultElementTag, sql)
        {
        }
        #endregion

        #region 实现IPatient接口
        [Export()]
        public string ClinicName
        {
            get
            {
                switch (this.ClinicType)
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
        [Export()]
        public string GenderName
        {
            get
            {
                switch (this.Gender)
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

        #region 实现父类抽象方法
        public override void AfterFill()
        {
            this.ConvertGender();
            this.ConvertClinic();
            this.ConvertAge();
            this.ConvertCID();
        }
        #endregion

        #region 公共属性

        [Export()]
        [Column(true)]
        public string PatientName
        {
            get { return this.m_patientName; }
            set { this.m_patientName = value; }
        }
        [Export()]
        [Column(true)]
        public string PID
        {
            get { return this.m_pid; }
            set { this.m_pid = value; }
        }
        [Export()]
        [Column(true)]
        public string CID
        {
            get { return this.m_cid; }
            set { this.m_cid = value; }
        }

        [Column(true)]
        public int GenderNo
        {
            get { return this.m_genderNo; }
            set { this.m_genderNo = value; }
        }

        public GenderType Gender
        {
            get { return this.m_gender; }
            protected set { this.m_gender = value; }
        }

        [Export()]
        public string AgeStr
        {
            get
            {
                if (this.Ager == null)
                {
                    return "";
                }
                return this.Ager.ToString();
            }
        }
        public Age Ager
        {
            get { return this.m_age; }
            protected set { this.m_age = value; }
        }
        [Column(true)]
        public int AgeValue
        {
            get { return this.m_ageValue; }
            set { this.m_ageValue = value; }
        }
        [Column(true)]
        public int AgeUnitNo
        {
            get { return this.m_ageUnitNo; }
            set { this.m_ageUnitNo = value; }
        }
        public AgeType AgeUnit
        {
            get { return this.m_ageUnit; }
            protected set { this.m_ageUnit = value; }
        }

        [Column(true)]
        public int ClinicTypeNo
        {
            get { return this.m_clinicTypeNo; }
            set { this.m_clinicTypeNo = value; }
        }
        public ClinicType ClinicType
        {
            get { return this.m_clinicType; }
            protected set { this.m_clinicType = value; }
        }

        [Export()]
        [Column(true)]
        public int VisitTimes
        {
            get { return this.m_visitTimes; }
            set { this.m_visitTimes = value; }
        }

        [Export()]
        [Column(true)]
        public string DeptName
        {
            get { return this.m_deptName; }
            set { this.m_deptName = value; }
        }

        [Export()]
        [Column(true)]
        public string Doctor
        {
            get { return this.m_doctor; }
            set { this.m_doctor = value; }
        }

        [Export()]
        [Column(true)]
        public string BedNo
        {
            get { return this.m_bedNo; }
            set { this.m_bedNo = value; }
        }

        [Export()]
        [Column(true)]
        public string ClinicalDiagnosis
        {
            get { return this.m_clinicalDiagnosis; }
            set { this.m_clinicalDiagnosis = value; }
        }

        [Export()]
        [Column(true)]
        public string Explanation
        {
            get { return this.m_explanation; }
            set { this.m_explanation = value; }
        }
        #endregion

        #region 私有方法
        private void ConvertGender()
        {
            //性别转换
            if (this.GenderNo > 4 || this.GenderNo <= 0)
            {
                this.Gender = GenderType.none;
            }
            else
            {
                this.Gender = (GenderType)this.GenderNo;
            }
        }
        private void ConvertClinic()
        {
            //就诊类型转换
            if (this.ClinicTypeNo == 1 || this.ClinicTypeNo == 4)
            {
                //住院
                this.ClinicType = ClinicType.hospital;
            }
            else if (this.ClinicTypeNo == 2 || this.ClinicTypeNo == 3)
            {
                this.ClinicType = ClinicType.clinic;
            }
            else if (this.ClinicTypeNo != 0)
            {
                this.ClinicType = ClinicType.other;
            }
            else
            {
                this.ClinicType = ClinicType.none;
            }
        }
        private void ConvertAge()
        {
            //年龄单位转换
            if (this.AgeUnitNo > 4 || this.AgeUnitNo <= 0)
            {
                this.AgeUnit = AgeType.none;
            }
            else
            {
                this.AgeUnit = (AgeType)this.AgeUnitNo;
            }
            Age age = new Age(this.AgeValue, this.AgeUnit);
            this.Ager = age;
        }
        private void ConvertCID()
        {
            if (this.CID != null)
            {
                this.CID = this.CID.Trim();
            }
        }
        #endregion

        #region  Age内部类
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
                    case AgeType.hours:
                        return this.m_value.ToString() + " 小时";
                    case AgeType.none:
                        return this.m_value.ToString() + " 年龄单位不明";
                    default:
                        return this.m_value.ToString() + " 岁";
                }
            }
            #endregion
        }
        #endregion
    }
}
