using System;

using XYS.Common;
using XYS.Lis.Core;
using XYS.Lis.Util;

namespace XYS.Lis.Model
{
    public class ReportInfoElement : AbstractReportElement, IPatient
    {
        #region 私有常量字段
        private const ReportElementTag m_defaultElementTag = ReportElementTag.InfoElement;
        private static readonly string m_defaultEaxmSQL = @"select serialno,sampleno,s.CName as sampletypename,CAST(CONVERT(varchar(10), collectdate, 121) + ' ' + CONVERT(varchar(8), collecttime, 114) AS datetime) as collectdatetime,CAST(CONVERT(varchar(10), inceptdate, 121) + ' ' + CONVERT(varchar(8), incepttime, 114) AS datetime) as inceptdatetime,
                                                                             CAST(CONVERT(varchar(10), testdate, 121) + ' ' + CONVERT(varchar(8), testtime, 114) AS datetime) as testdatetime,CAST(CONVERT(varchar(10), checkdate, 121) + ' ' + CONVERT(varchar(8), checktime, 114) AS datetime) as checkdatetime,
                                                                             CAST(CONVERT(varchar(10), receivedate, 121) + ' ' + CONVERT(varchar(8), receivetime, 114) AS datetime) as receivedatetime,sendertime2 as secondecheckdatetime,paritemname,sectionno,r.sampletypeno,formmemo,formcomment,formcomment2,technician,checker,
                                                                             r.cname as patientname,patno as pid,id_number_patient as cid,genderno,age as agevalue,ageunitno,sicktypeno as clinictypeno,hospitalizedtimes as visittimes,d.cname as deptname,isnull(r.doctor,b.cname) as doctor,bed as bedno,zdy2 as clinicaldiagnosis,zdy5 as explanation
                                                                             from ReportForm as r left outer join SampleType as s on r.SampleTypeNo=s.SampleTypeNo left outer join department as d on r.DeptNo=d.DeptNo left outer join doctor as b ON r.doctor = CONVERT(varchar(20), b.doctorno)";
        #endregion

        #region 私有字段
        private string m_serialNo;
        private string m_sampleNo;
        private int m_sectionNo;
        private int m_sampleTypeNo;
        private string m_sampleTypeName;
        private DateTime m_collectDateTime;
        private DateTime m_inceptDateTime;
        private DateTime m_checkDateTime;
        private DateTime m_secondCheckDateTime;
        private DateTime m_receiveDateTime;
        //报告附注信息
        private DateTime m_testDateTime;
        private string m_parItemName;
        //备注、结论、解释等
        private string m_formMemo;
        private string m_formComment;
        private string m_formComment2;
        //检验者，审核者
        private string m_technician;
        private string m_checker;

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
        public ReportInfoElement()
            : base(m_defaultElementTag, m_defaultEaxmSQL)
        {
        }
        public ReportInfoElement(ReportElementTag elementTag, string sql)
            : base(elementTag, sql)
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

        #region 公共属性
        [Export()]
        [TableColumn(true)]
        public string SerialNo
        {
            get { return m_serialNo; }
            set { m_serialNo = value; }
        }

        [Export()]
        [TableColumn(true)]
        public string SampleNo
        {
            get { return m_sampleNo; }
            set { m_sampleNo = value; }
        }

        [Export()]
        [TableColumn(true)]
        public string SampleTypeName
        {
            get { return m_sampleTypeName; }
            set { m_sampleTypeName = value; }
        }

        [TableColumn(true)]
        public DateTime ReceiveDateTime
        {
            get { return m_receiveDateTime; }
            set { m_receiveDateTime = value; }
        }
        [TableColumn(true)]
        public DateTime CollectDateTime
        {
            get { return m_collectDateTime; }
            set { m_collectDateTime = value; }
        }
        [TableColumn(true)]
        public DateTime InceptDateTime
        {
            get { return m_inceptDateTime; }
            set { m_inceptDateTime = value; }
        }
        [TableColumn(true)]
        public DateTime TestDateTime
        {
            get { return m_testDateTime; }
            set { m_testDateTime = value; }
        }
        [TableColumn(true)]
        public DateTime CheckDateTime
        {
            get { return m_checkDateTime; }
            set { m_checkDateTime = value; }
        }
        [TableColumn(true)]
        public DateTime SecondeCheckDateTime
        {
            get { return m_secondCheckDateTime; }
            set { m_secondCheckDateTime = value; }
        }

        [TableColumn(true)]
        public string ParItemName
        {
            get { return m_parItemName; }
            set { m_parItemName = value; }
        }

        [Export()]
        [TableColumn(true)]
        public int SectionNo
        {
            get { return m_sectionNo; }
            set { m_sectionNo = value; }
        }

        [Export()]
        [TableColumn(true)]
        public int SampleTypeNo
        {
            get { return m_sampleTypeNo; }
            set { m_sampleTypeNo = value; }
        }

        [Export()]
        [TableColumn(true)]
        public string FormMemo
        {
            get { return m_formMemo; }
            set { m_formMemo = value; }
        }
        [Export()]
        [TableColumn(true)]
        public string FormComment
        {
            get { return m_formComment; }
            set { m_formComment = value; }
        }
        [Export()]
        [TableColumn(true)]
        public string FormComment2
        {
            get { return m_formComment2; }
            set { m_formComment2 = value; }
        }

        [Export()]
        [TableColumn(true)]
        public string Technician
        {
            get { return m_technician; }
            set { m_technician = value; }
        }

        [Export()]
        [TableColumn(true)]
        public string Checker
        {
            get { return m_checker; }
            set { m_checker = value; }
        }


        [Export()]
        [TableColumn(true)]
        public string PatientName
        {
            get { return this.m_patientName; }
            set { this.m_patientName = value; }
        }
        [Export()]
        [TableColumn(true)]
        public string PID
        {
            get { return this.m_pid; }
            set { this.m_pid = value; }
        }
        [Export()]
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

        [Export()]
        public GenderType Gender
        {
            get { return this.m_gender; }
            protected set { this.m_gender = value; }
        }

        public Age Ager
        {
            get { return this.m_age; }
            protected set { this.m_age = value; }
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
            protected set { this.m_ageUnit = value; }
        }

        [TableColumn(true)]
        public int ClinicTypeNo
        {
            get { return this.m_clinicTypeNo; }
            set { this.m_clinicTypeNo = value; }
        }

        [Export()]
        public ClinicType ClinicType
        {
            get { return this.m_clinicType; }
            protected set { this.m_clinicType = value; }
        }

        [Export()]
        [TableColumn(true)]
        public int VisitTimes
        {
            get { return this.m_visitTimes; }
            set { this.m_visitTimes = value; }
        }

        [Export()]
        [TableColumn(true)]
        public string DeptName
        {
            get { return this.m_deptName; }
            set { this.m_deptName = value; }
        }

        [Export()]
        [TableColumn(true)]
        public string Doctor
        {
            get { return this.m_doctor; }
            set { this.m_doctor = value; }
        }

        [Export()]
        [TableColumn(true)]
        public string BedNo
        {
            get { return this.m_bedNo; }
            set { this.m_bedNo = value; }
        }

        [Export()]
        [TableColumn(true)]
        public string ClinicalDiagnosis
        {
            get { return this.m_clinicalDiagnosis; }
            set { this.m_clinicalDiagnosis = value; }
        }

        [Export()]
        [TableColumn(true)]
        public string Explanation
        {
            get { return this.m_explanation; }
            set { this.m_explanation = value; }
        }
        #endregion

        #region 实现父类抽象方法
        protected override void Afterward()
        {
            this.ConvertGender();
            this.ConvertClinic();
            this.ConvertAge();
            this.ConvertCID();
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
