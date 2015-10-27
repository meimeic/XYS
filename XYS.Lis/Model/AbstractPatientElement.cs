using XYS.Lis.Core;
using XYS.Model;
using XYS.Common;
namespace XYS.Lis.Model
{
    public abstract class AbstractPatientElement : ILisPatientElement, ILisReportElement
    {

        #region 私有字段
        private readonly ReportElementTag m_elementTag;
        private readonly string m_searchSQL;
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
        //private ReportKey m_reportKey;
        #endregion

        #region 受保护的构造方法
        protected AbstractPatientElement(ReportElementTag elementTag, string sql)
        {
            this.m_elementTag = elementTag;
            this.m_searchSQL = sql;
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
        public ReportElementTag ElementTag
        {
            get { return m_elementTag; }
        }
        //public ReportKey ReporterKey
        //{
        //    get { return this.m_reportKey; }
        //    set { this.m_reportKey = value; }
        //}
        #endregion

        #region 实现ILisReportElement
        public string SearchSQL
        {
            get { return this.m_searchSQL; }
        }
        public void AfterFill()
        {
            this.Afterward();
        }
        #endregion

        #region 公共属性
        [TableColumn(true)]
        public string PatientName
        {
            get { return this.m_patientName; }
            set { this.m_patientName = value; }
        }
        [TableColumn(true)]
        public string PID
        {
            get { return this.m_pid; }
            set { this.m_pid = value; }
        }
        [TableColumn(true)]
        public string CID
        {
            get { return this.m_cid; }
            set { this.m_cid = value; }
        }
        [TableColumn(true)]
        public int GenderNo
        {
            get { return this.m_genderNo; }
            set { this.m_genderNo = value; }
        }
        public GenderType Gender
        {
            get { return this.m_gender; }
            //set { this.m_gender = value; }
        }
        public Age Ager
        {
            get { return this.m_age; }
        }
        [TableColumn(true)]
        public int AgeValue
        {
            get { return this.m_ageValue; }
            set { this.m_ageValue = value; }
        }
        [TableColumn(true)]
        public int AgeUnitNo
        {
            get { return this.m_ageUnitNo; }
            set { this.m_ageUnitNo = value; }
        }
        public AgeType AgeUnit
        {
            get { return this.m_ageUnit; }
            //  set { this.m_ageUnit = value; }
        }
        [TableColumn(true)]
        public int clinicTypeNo
        {
            get { return this.m_clinicTypeNo; }
            set { this.m_clinicTypeNo = value; }
        }
        public ClinicType ClinicalType
        {
            get { return this.m_clinicType; }
            //set { this.m_clinicType = value; }
        }
        [TableColumn(true)]
        public int VisitTimes
        {
            get { return this.m_visitTimes; }
            set { this.m_visitTimes = value; }
        }
        [TableColumn(true)]
        public string DeptName
        {
            get { return this.m_deptName; }
            set { this.m_deptName = value; }
        }
        [TableColumn(true)]
        public string Doctor
        {
            get { return this.m_doctor; }
            set { this.m_doctor = value; }
        }
        [TableColumn(true)]
        public string BedNo
        {
            get { return this.m_bedNo; }
            set { this.m_bedNo = value; }
        }
        [TableColumn(true)]
        public string ClinicalDiagnosis
        {
            get { return this.m_clinicalDiagnosis; }
            set { this.m_clinicalDiagnosis = value; }
        }
        [TableColumn(true)]
        public string Explanation
        {
            get { return this.m_explanation; }
            set { this.m_explanation = value; }
        }
        #endregion

        #region 受保护的虚函数
        protected virtual void Afterward()
        {
            //性别转换
            if (this.m_genderNo > 4 || this.m_genderNo <= 0)
            {
                this.m_gender = GenderType.none;
            }
            else
            {
                this.m_gender = (GenderType)this.m_genderNo;
            }
            //就诊类型转换
            if (this.m_clinicTypeNo == 1 || this.m_clinicTypeNo == 4)
            {
                //住院
                this.m_clinicType = ClinicType.hospital;
            }
            else if (this.m_clinicTypeNo == 2 || this.m_clinicTypeNo == 3)
            {
                this.m_clinicType = ClinicType.clinic;
            }
            else if (this.m_clinicTypeNo != 0)
            {
                this.m_clinicType = ClinicType.other;
            }
            else
            {
                this.m_clinicType = ClinicType.none;
            }
            //年龄单位转换
            if (this.m_ageUnitNo > 4 || this.m_ageUnitNo <= 0)
            {
                this.m_ageUnit = AgeType.none;
            }
            else
            {
                this.m_ageUnit = (AgeType)this.m_ageUnitNo;
            }
            Age age = new Age(this.m_ageValue, this.m_ageUnit);
            this.m_age = age;
        }
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
                    case AgeType.hours:
                        return this.m_value.ToString() + "小时";
                    case AgeType.none:
                        return this.m_value.ToString() + "年龄单位不明";
                    default:
                        return this.m_value.ToString() + " 岁";
                }
            }
            #endregion
        }
        #endregion
    }
}
