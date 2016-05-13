using System;

using XYS.Model;
using XYS.Common;

using MongoDB.Bson.Serialization.Options;
using MongoDB.Bson.Serialization.Attributes;
namespace XYS.Report.Lis.Model
{
    public class ReportInfoElement : IPatientElement, IFillElement
    {
        #region 私有字段
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
        private string m_formMemo;
        private string m_formComment;
        private string m_formComment2;

        private string m_technician;
        private string m_checker;
        private string m_parItemName;

        //时间信息
        private DateTime m_receiveDateTime;
        private DateTime m_collectDateTime;
        private DateTime m_inceptDateTime;
        private DateTime m_testDateTime;
        private DateTime m_checkDateTime;
        private DateTime m_secondCheckDateTime;
        #endregion

        #region 公共构造函数
        public ReportInfoElement()
        { }
        #endregion

        #region 实现IPatientElement接口
        [Column]
        [BsonElement(Order = 1)]
        public string PatientName
        {
            get { return this.m_patientName; }
            set { this.m_patientName = value; }
        }
        [Column]
        [BsonElement(Order = 2)]
        public string GenderName
        {
            get { return this.m_genderName; }
            set { this.m_genderName = value; }
        }
        [Column]
        [BsonElement(Order = 3)]
        public string AgeStr
        {
            get { return this.m_ageStr; }
            set { this.m_ageStr = value; }
        }
        [Column]
        [BsonElement(Order = 5)]
        public string PatientID
        {
            get { return this.m_pid; }
            set { this.m_pid = value; }
        }
        [Column]
        [BsonElement(Order = 6)]
        public string ClinicName
        {
            get { return this.m_clinicName; }
            set { this.m_clinicName = value; }
        }
        #endregion

        #region
        [Column]
        [BsonElement(Order = 4)]
        public string CID
        {
            get { return this.m_cid; }
            set { this.m_cid = value; }
        }
        [Column]
        [BsonElement(Order = 7)]
        public int VisitTimes
        {
            get { return this.m_visitTimes; }
            set { this.m_visitTimes = value; }
        }
        [Column]
        [BsonElement(Order = 8)]
        public string BedNo
        {
            get { return this.m_bedNo; }
            set { this.m_bedNo = value; }
        }
        [Column]
        [BsonElement(Order = 9)]
        public string DeptName
        {
            get { return this.m_deptName; }
            set { this.m_deptName = value; }
        }
        [Column]
        [BsonElement(Order = 10)]
        public string Doctor
        {
            get { return this.m_doctor; }
            set { this.m_doctor = value; }
        }
        [Column]
        [BsonElement(Order = 11)]
        public string ClinicalDiagnosis
        {
            get { return this.m_clinicalDiagnosis; }
            set { this.m_clinicalDiagnosis = value; }
        }
        [Column]
        [BsonElement(Order = 12)]
        public string Explanation
        {
            get { return this.m_explanation; }
            set { this.m_explanation = value; }
        }
        [Column]
        [BsonElement(Order = 14)]
        public string SerialNo
        {
            get { return this.m_serialNo; }
            set { this.m_serialNo = value; }
        }

        [Column]
        [BsonElement(Order = 15)]
        public int SectionNo
        {
            get { return this.m_sectionNo; }
            set { this.m_sectionNo = value; }
        }
        [Column]
        [BsonElement(Order = 16)]
        public string SampleNo
        {
            get { return this.m_sampleNo; }
            set { this.m_sampleNo = value; }
        }
        [Column]
        [BsonElement(Order = 17)]
        public int SampleTypeNo
        {
            get { return this.m_sampleTypeNo; }
            set { this.m_sampleTypeNo = value; }
        }
        [Column]
        [BsonElement(Order = 18)]
        public string SampleTypeName
        {
            get { return this.m_sampleTypeName; }
            set { this.m_sampleTypeName = value; }
        }

        [Column]
        [BsonElement(Order = 19)]
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime ReceiveDateTime
        {
            get { return this.m_receiveDateTime; }
            set { this.m_receiveDateTime = value; }
        }
        [Column]
        [BsonElement(Order = 20)]
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime CollectDateTime
        {
            get { return this.m_collectDateTime; }
            set { this.m_collectDateTime = value; }
        }
        [Column]
        [BsonElement(Order = 21)]
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime InceptDateTime
        {
            get { return m_inceptDateTime; }
            set { m_inceptDateTime = value; }
        }
        [Column]
        [BsonElement(Order = 22)]
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime TestDateTime
        {
            get { return m_testDateTime; }
            set { m_testDateTime = value; }
        }
        [Column]
        [BsonElement(Order = 23)]
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime CheckDateTime
        {
            get { return m_checkDateTime; }
            set { m_checkDateTime = value; }
        }
        [Column]
        [BsonElement(Order = 24)]
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime SecondCheckDateTime
        {
            get { return m_secondCheckDateTime; }
            set { m_secondCheckDateTime = value; }
        }

        [Column]
        [BsonElement(Order = 26)]
        public string Technician
        {
            get { return this.m_technician; }
            set { this.m_technician = value; }
        }
        [Column]
        [BsonElement(Order = 27)]
        public string Checker
        {
            get { return this.m_checker; }
            set { this.m_checker = value; }
        }
        [Column]
        [BsonElement(Order = 28)]
        public string FormMemo
        {
            get { return this.m_formMemo; }
            set { this.m_formMemo = value; }
        }
        [Column]
        [BsonElement(Order = 29)]
        public string FormComment
        {
            get { return this.m_formComment; }
            set { this.m_formComment = value; }
        }
        [Column]
        [BsonElement(Order = 30)]
        public string FormComment2
        {
            get { return this.m_formComment2; }
            set { this.m_formComment2 = value; }
        }
        [Column]
        [BsonElement(Order = 31)]
        public string ParItemName
        {
            get { return this.m_parItemName; }
            set { this.m_parItemName = value; }
        }
        #endregion
    }
}
