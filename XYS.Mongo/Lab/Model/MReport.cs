using System;
using System.Collections.Generic;

using MongoDB.Bson;
using MongoDB.Bson.Serialization.Options;
using MongoDB.Bson.Serialization.Attributes;
namespace XYS.Mongo.Lab.Model
{
    public class MReport
    {
        #region
        private ObjectId m_id;
        private int m_activeFlag;
        private string m_reportID;
        private string m_reportTitle;
        private DateTime m_createTime;
        private int m_reportNo;
        #endregion

        #region 基本信息
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
        private string m_technicianUrl;
        private string m_checker;
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
        private List<Item> m_itemCollection;
        private List<Custom> m_customCollection;
        private Dictionary<string, string> m_imageMap;

        public MReport()
        {
            this.m_itemCollection = new List<Item>(30);
            this.m_customCollection = new List<Custom>(3);
            this.m_imageMap = new Dictionary<string, string>(4);
        }


        [BsonId]
        public ObjectId ID
        {
            get { return this.m_id; }
            set { this.m_id = value; }
        }
        [BsonElement("reportID", Order = 1)]
        public string ReportID
        {
            get { return this.m_reportID; }
            set { this.m_reportID = value; }
        }
        [BsonElement("activeFlag", Order = 2)]
        public int ActiveFlag
        {
            get { return m_activeFlag; }
            set { m_activeFlag = value; }
        }
        [BsonElement("reportTitle", Order = 3)]
        public string ReportTitle
        {
            get { return this.m_reportTitle; }
            set { this.m_reportTitle = value; }
        }
        [BsonElement("createTime", Order = 4)]
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime CreateTime
        {
            get
            {
                if (this.m_createTime == default(DateTime))
                {
                    this.m_createTime = DateTime.Now;
                }
                return this.m_createTime;
            }
            set { this.m_createTime = value; }
        }
        [BsonElement("reportNo", Order = 6)]
        public int ReportNo
        {
            get { return this.m_reportNo; }
            set { this.m_reportNo = value; }
        }

        [BsonElement("patientName", Order = 8)]
        public string PatientName
        {
            get { return this.m_patientName; }
            set { this.m_patientName = value; }
        }
        [BsonElement("genderName", Order = 9)]
        public string GenderName
        {
            get { return this.m_genderName; }
            set { this.m_genderName = value; }
        }
        [BsonElement("ageStr", Order = 10)]
        public string AgeStr
        {
            get { return this.m_ageStr; }
            set { this.m_ageStr = value; }
        }
        [BsonElement("cid", Order = 11)]
        public string CID
        {
            get { return this.m_cid; }
            set { this.m_cid = value; }
        }
        [BsonElement("pid", Order = 12)]
        public string PatientID
        {
            get { return this.m_pid; }
            set { this.m_pid = value; }
        }
        [BsonElement("clinicName", Order = 13)]
        public string ClinicName
        {
            get { return this.m_clinicName; }
            set { this.m_clinicName = value; }
        }
        [BsonElement("visitTimes", Order = 14)]
        public int VisitTimes
        {
            get { return this.m_visitTimes; }
            set { this.m_visitTimes = value; }
        }
        [BsonElement("bedNo", Order = 15)]
        public string BedNo
        {
            get { return this.m_bedNo; }
            set { this.m_bedNo = value; }
        }
        [BsonElement("deptName", Order = 16)]
        public string DeptName
        {
            get { return this.m_deptName; }
            set { this.m_deptName = value; }
        }
        [BsonElement("doctor", Order = 17)]
        public string Doctor
        {
            get { return this.m_doctor; }
            set { this.m_doctor = value; }
        }
        [BsonElement("clinicalDiagnosis", Order = 18)]
        public string ClinicalDiagnosis
        {
            get { return this.m_clinicalDiagnosis; }
            set { this.m_clinicalDiagnosis = value; }
        }
        [BsonElement("explanation", Order = 19)]
        public string Explanation
        {
            get { return this.m_explanation; }
            set { this.m_explanation = value; }
        }
        [BsonElement("serialNo", Order = 20)]
        public string SerialNo
        {
            get { return this.m_serialNo; }
            set { this.m_serialNo = value; }
        }
        [BsonElement("sectionNo", Order = 21)]
        public int SectionNo
        {
            get { return this.m_sectionNo; }
            set { this.m_sectionNo = value; }
        }
        [BsonElement("sampleNo", Order = 22)]
        public string SampleNo
        {
            get { return this.m_sampleNo; }
            set { this.m_sampleNo = value; }
        }
        [BsonElement("sampleTypeNo", Order = 23)]
        public int SampleTypeNo
        {
            get { return this.m_sampleTypeNo; }
            set { this.m_sampleTypeNo = value; }
        }
        [BsonElement("sampleTypeName", Order = 24)]
        public string SampleTypeName
        {
            get { return this.m_sampleTypeName; }
            set { this.m_sampleTypeName = value; }
        }
        [BsonElement("receiveTime", Order = 25)]
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime ReceiveTime
        {
            get { return this.m_receiveTime; }
            set { this.m_receiveTime = value; }
        }
        [BsonElement("collectTime", Order = 26)]
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime CollectTime
        {
            get { return this.m_collectTime; }
            set { this.m_collectTime = value; }
        }
        [BsonElement("inceptTime", Order = 27)]
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime InceptTime
        {
            get { return m_inceptTime; }
            set { m_inceptTime = value; }
        }
        [BsonElement("testTime", Order = 28)]
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime TestTime
        {
            get { return m_testTime; }
            set { m_testTime = value; }
        }
        [BsonElement("checkTime", Order = 29)]
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime CheckTime
        {
            get { return m_checkTime; }
            set { m_checkTime = value; }
        }
        [BsonElement("finalTime", Order = 30)]
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime FinalTime
        {
            get { return m_finalTime; }
            set { m_finalTime = value; }
        }
        [BsonElement("technician", Order = 31)]
        public string Technician
        {
            get { return this.m_technician; }
            set { this.m_technician = value; }
        }
        [BsonElement("technicianUrl", Order = 32)]
        public string TechnicianUrl
        {
            get { return this.m_technicianUrl; }
            set { this.m_technicianUrl = value; }
        }
        [BsonElement("checker", Order = 33)]
        public string Checker
        {
            get { return this.m_checker; }
            set { this.m_checker = value; }
        }
        [BsonElement("checkerUrl", Order = 34)]
        public string CheckerUrl
        {
            get { return this.m_checkerUrl; }
            set { this.m_checkerUrl = value; }
        }
        [BsonElement("memo", Order = 35)]
        public string Memo
        {
            get { return this.m_memo; }
            set { this.m_memo = value; }
        }
        [BsonElement("comment", Order = 36)]
        public string Comment
        {
            get { return this.m_comment; }
            set { this.m_comment = value; }
        }
        [BsonElement("description", Order = 37)]
        public string Description
        {
            get { return this.m_description; }
            set { this.m_description = value; }
        }
        [BsonElement("reportContent", Order = 38)]
        public string ReportContent
        {
            get { return this.m_reportContent; }
            set { this.m_reportContent = value; }
        }


        [BsonElement("items", Order = 39)]
        public List<Item> ItemCollection
        {
            get { return this.m_itemCollection; }
            set { this.m_itemCollection = value; }
        }
        
        [BsonElement("customs", Order = 40)]
        public List<Custom> CustomCollection
        {
            get { return this.m_customCollection; }
            set { this.m_customCollection = value; }
        }
        
        [BsonElement("images", Order = 41)]
        [BsonDictionaryOptions(DictionaryRepresentation.Document)]
        public Dictionary<string, string> ImageMap
        {
            get { return this.m_imageMap; }
            set { this.m_imageMap = value; }
        }
    }
}