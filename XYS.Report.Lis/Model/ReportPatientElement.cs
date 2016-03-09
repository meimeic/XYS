using XYS.Model;
using XYS.Common;
namespace XYS.Report.Lis.Model
{
    [Export()]
    public class ReportPatientElement : AbstractFillElement, IPatientElement
    {
        #region 私有常量字段
        #endregion

        #region 私有字段
        private string m_patientName;
        private string m_pid;
        private string m_cid;

        private string m_genderName;

        private string m_ageStr;

        private string m_clinicName;

        private int m_visitTimes;
        private string m_deptName;
        private string m_doctor;
        private string m_bedNo;
        private string m_clinicalDiagnosis;
        private string m_explanation;
        #endregion

        #region 构造函数
        public ReportPatientElement()
            : this(null)
        {
        }
        public ReportPatientElement(string sql)
            : base(sql)
        {
        }
        #endregion

        #region 实现IPatient接口
        [Export()]
        [Column(true)]
        public string PatientName
        {
            get { return this.m_patientName; }
            set { this.m_patientName = value; }
        }
        [Export()]
        [Column(true)]
        public string GenderName
        {
            get { return this.m_genderName; }
            set { this.m_genderName = value; }
        }
        [Export()]
        [Column(true)]
        public string AgeStr
        {
            get { return this.m_ageStr; }
            set { this.m_ageStr = value; }
        }
        [Export()]
        [Column(true)]
        public string PatientID
        {
            get { return this.m_pid; }
            set { this.m_pid = value; }
        }
        [Export()]
        [Column(true)]
        public string ClinicName
        {
            get { return this.m_clinicName; }
            set { this.m_clinicName = value; }
        }
        #endregion

        #region 方法
        #endregion

        #region 公共属性
        [Export()]
        [Column(true)]
        public string CID
        {
            get { return this.m_cid; }
            set { this.m_cid = value; }
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
        public string BedNo
        {
            get { return this.m_bedNo; }
            set { this.m_bedNo = value; }
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
        //private void ConvertGender()
        //{
        //    //性别转换
        //    if (this.GenderNo > 4 || this.GenderNo <= 0)
        //    {
        //        this.Gender = GenderType.none;
        //    }
        //    else
        //    {
        //        this.Gender = (GenderType)this.GenderNo;
        //    }
        //}
        //private void ConvertClinic()
        //{
        //    //就诊类型转换
        //    if (this.ClinicTypeNo == 1 || this.ClinicTypeNo == 4)
        //    {
        //        //住院
        //        this.ClinicType = ClinicType.hospital;
        //    }
        //    else if (this.ClinicTypeNo == 2 || this.ClinicTypeNo == 3)
        //    {
        //        this.ClinicType = ClinicType.clinic;
        //    }
        //    else if (this.ClinicTypeNo != 0)
        //    {
        //        this.ClinicType = ClinicType.other;
        //    }
        //    else
        //    {
        //        this.ClinicType = ClinicType.none;
        //    }
        //}
        //private void ConvertAge()
        //{
        //    //年龄单位转换
        //    if (this.AgeUnitNo > 4 || this.AgeUnitNo <= 0)
        //    {
        //        this.AgeUnit = AgeType.none;
        //    }
        //    else
        //    {
        //        this.AgeUnit = (AgeType)this.AgeUnitNo;
        //    }
        //    Age age = new Age(this.AgeValue, this.AgeUnit);
        //    this.Ager = age;
        //}
        //private void ConvertCID()
        //{
        //    if (this.CID != null)
        //    {
        //        this.CID = this.CID.Trim();
        //    }
        //}
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
