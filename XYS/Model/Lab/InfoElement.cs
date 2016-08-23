using System;

using XYS.Model;
using XYS.Report;
using XYS.Common;

namespace XYS.Model.Lab
{
    [Serializable]
    public class InfoElement : IPatientElement, IFillElement
    {
        #region 私有字段
        //
        private string m_reportID;
        //患者信息
        private string m_patientName;
        private string m_genderName;
        private string m_ageStr;
        private string m_cid;

        private string m_pid;
        private string m_clinicName;
        private int m_visitTimes;
        private string m_deptName;
        private string m_doctor;
        private string m_bedNo;
        private string m_clinicalDiagnosis;
        private string m_explanation;


        private int m_sectionNo;
        private string m_serialNo;

        private string m_sampleNo;
        private int m_sampleTypeNo;
        private string m_sampleTypeName;

        //备注、结论、解释等
        private string m_memo;
        private string m_comment;
        private string m_description;

        private string m_technician;
        private byte[] m_technicianImage;
        private string m_technicianUrl;

        private string m_checker;
        private byte[] m_checkerImage;
        private string m_checkerUrl;

        private string m_reportContent;

        //时间信息
        private DateTime m_receiveTime;
        private DateTime m_collectTime;
        private DateTime m_inceptTime;
        private DateTime m_testTime;
        private DateTime m_checkTime;
        private DateTime m_finalTime;
        #endregion

        #region 公共构造函数
        public InfoElement()
        { }
        #endregion

        #region 实现IPatientElement接口
        [Column]
        public string PatientName
        {
            get { return this.m_patientName; }
            set { this.m_patientName = value; }
        }
        [Column]
        public string GenderName
        {
            get { return this.m_genderName; }
            set { this.m_genderName = value; }
        }
        [Column]
        public string AgeStr
        {
            get { return this.m_ageStr; }
            set { this.m_ageStr = value; }
        }
        [Column]
        public string PatientID
        {
            get { return this.m_pid; }
            set { this.m_pid = value; }
        }
        [Column]
        public string ClinicName
        {
            get { return this.m_clinicName; }
            set { this.m_clinicName = value; }
        }
        #endregion

        #region 实例属性
        public string ReportID
        {
            get { return this.m_reportID; }
            set { this.m_reportID = value; }
        }

        [Column]
        public string CID
        {
            get { return this.m_cid; }
            set { this.m_cid = value; }
        }
        [Column]
        public int VisitTimes
        {
            get { return this.m_visitTimes; }
            set { this.m_visitTimes = value; }
        }
        [Column]
        public string BedNo
        {
            get { return this.m_bedNo; }
            set { this.m_bedNo = value; }
        }
        [Column]
        public string DeptName
        {
            get { return this.m_deptName; }
            set { this.m_deptName = value; }
        }
        [Column]
        public string Doctor
        {
            get { return this.m_doctor; }
            set { this.m_doctor = value; }
        }
        [Column]
        public string ClinicalDiagnosis
        {
            get { return this.m_clinicalDiagnosis; }
            set { this.m_clinicalDiagnosis = value; }
        }
        [Column]
        public string Explanation
        {
            get { return this.m_explanation; }
            set { this.m_explanation = value; }
        }
        [Column]
        public string SerialNo
        {
            get { return this.m_serialNo; }
            set { this.m_serialNo = value; }
        }

        [Column]
        public int SectionNo
        {
            get { return this.m_sectionNo; }
            set { this.m_sectionNo = value; }
        }
        [Column]
        public string SampleNo
        {
            get { return this.m_sampleNo; }
            set { this.m_sampleNo = value; }
        }
        [Column]
        public int SampleTypeNo
        {
            get { return this.m_sampleTypeNo; }
            set { this.m_sampleTypeNo = value; }
        }
        [Column]
        public string SampleTypeName
        {
            get { return this.m_sampleTypeName; }
            set { this.m_sampleTypeName = value; }
        }

        [Column]
        public DateTime ReceiveTime
        {
            get { return this.m_receiveTime; }
            set { this.m_receiveTime = value; }
        }
        [Column]
        public DateTime CollectTime
        {
            get { return this.m_collectTime; }
            set { this.m_collectTime = value; }
        }
        [Column]
        public DateTime InceptTime
        {
            get { return m_inceptTime; }
            set { m_inceptTime = value; }
        }
        [Column]
        public DateTime TestTime
        {
            get { return m_testTime; }
            set { m_testTime = value; }
        }
        [Column]
        public DateTime CheckTime
        {
            get { return m_checkTime; }
            set { m_checkTime = value; }
        }
        [Column]
        public DateTime FinalTime
        {
            get { return m_finalTime; }
            set { m_finalTime = value; }
        }

        [Column]
        public string Technician
        {
            get { return this.m_technician; }
            set { this.m_technician = value; }
        }
        public byte[] TechnicianImage
        {
            get { return this.m_technicianImage; }
            set { this.m_technicianImage = value; }
        }
        public string TechnicianUrl
        {
            get { return this.m_technicianUrl; }
            set { this.m_technicianUrl = value; }
        }
        [Column]
        public string Checker
        {
            get { return this.m_checker; }
            set { this.m_checker = value; }
        }
        public byte[] CheckerImage
        {
            get { return this.m_checkerImage; }
            set { this.m_checkerImage = value; }
        }
        public string CheckerUrl
        {
            get { return this.m_checkerUrl; }
            set { this.m_checkerUrl = value; }
        }
        [Column]
        public string Memo
        {
            get { return this.m_memo; }
            set { this.m_memo = value; }
        }
        [Column]
        public string Comment
        {
            get { return this.m_comment; }
            set { this.m_comment = value; }
        }
        [Column]
        public string Description
        {
            get { return this.m_description; }
            set { this.m_description = value; }
        }
        [Column]
        public string ReportContent
        {
            get { return this.m_reportContent; }
            set { this.m_reportContent = value; }
        }
        #endregion
    }
}
