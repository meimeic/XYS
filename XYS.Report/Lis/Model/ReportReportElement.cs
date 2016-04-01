using System;
using System.Collections;
using System.Collections.Generic;

using XYS.Util;
using XYS.Model;
using XYS.Report;
using XYS.Common;

using MongoDB.Bson.Serialization.Options;
using MongoDB.Bson.Serialization.Attributes;
namespace XYS.Report.Lis.Model
{
    public class ReportReportElement : AbstractSubFillElement, IReportElement, IPatientElement
    {
        #region 私有实例字段
        private LisReportPK m_reportPK;

        private string m_reportID;

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
        //报告名
        private string m_reportTitle;
        private string m_parItemName;
        //附加备注
        private int m_remarkFlag;
        private string m_remark;
        //时间信息
        private DateTime m_receiveDateTime;
        private DateTime m_collectDateTime;
        private DateTime m_inceptDateTime;
        private DateTime m_testDateTime;
        private DateTime m_checkDateTime;
        private DateTime m_secondCheckDateTime;
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

        private Dictionary<string, string> m_reportImageMap;
        private List<ReportItemElement> m_reportItemCollection;
        private List<ReportCustomElement> m_reportCustomCollection;

        private readonly Hashtable m_reportItemTable;
        #endregion

        #region 公共构造函数
        public ReportReportElement()
        {
            this.m_remarkFlag = 0;
            this.m_reportImageMap = null;
            this.m_reportCustomCollection = null;
            this.m_reportPK = new LisReportPK();
            this.m_reportItemCollection = new List<ReportItemElement>(20);
            this.m_reportItemTable = SystemInfo.CreateCaseInsensitiveHashtable(3);
        }
        #endregion

        #region 实现IReportElement接口
        public IReportKey PK
        {
            get { return this.m_reportPK; }
        }
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
        [BsonId]
        [Column]
        public string ReportID
        {
            get { return this.m_reportID; }
            set { this.m_reportID = value; }
        }

        [Column]
        public int SectionNo
        {
            get { return this.m_sectionNo; }
            set { this.m_sectionNo = value; }
        }
        [Column]
        public string SerialNo
        {
            get { return this.m_serialNo; }
            set { this.m_serialNo = value; }
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
        public string FormMemo
        {
            get { return this.m_formMemo; }
            set { this.m_formMemo = value; }
        }
        [Column]
        public string FormComment
        {
            get { return this.m_formComment; }
            set { this.m_formComment = value; }
        }
        [Column]
        public string FormComment2
        {
            get { return this.m_formComment2; }
            set { this.m_formComment2 = value; }
        }

        [Column]
        public string Technician
        {
            get { return this.m_technician; }
            set { this.m_technician = value; }
        }
        [Column]
        public string Checker
        {
            get { return this.m_checker; }
            set { this.m_checker = value; }
        }

        public string ReportTitle
        {
            get { return this.m_reportTitle; }
            set { this.m_reportTitle = value; }
        }
        [Column]
        public string ParItemName
        {
            get { return this.m_parItemName; }
            set { this.m_parItemName = value; }
        }

        //备注标识 0表示没有备注 1 表示备注已设置  2 表示备注未设置
        [BsonIgnore]
        public int RemarkFlag
        {
            get { return this.m_remarkFlag; }
            set { this.m_remarkFlag = value; }
        }
        public string Remark
        {
            get { return this.m_remark; }
            set { this.m_remark = value; }
        }

        [Column]
        public DateTime ReceiveDateTime
        {
            get { return this.m_receiveDateTime; }
            set { this.m_receiveDateTime = value; }
        }
        [Column]
        public DateTime CollectDateTime
        {
            get { return this.m_collectDateTime; }
            set { this.m_collectDateTime = value; }
        }
        [Column]
        public DateTime InceptDateTime
        {
            get { return m_inceptDateTime; }
            set { m_inceptDateTime = value; }
        }
        [Column]
        public DateTime TestDateTime
        {
            get { return m_testDateTime; }
            set { m_testDateTime = value; }
        }
        [Column]
        public DateTime CheckDateTime
        {
            get { return m_checkDateTime; }
            set { m_checkDateTime = value; }
        }
        [Column]
        public DateTime SecondeCheckDateTime
        {
            get { return m_secondCheckDateTime; }
            set { m_secondCheckDateTime = value; }
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

        [BsonDictionaryOptions(DictionaryRepresentation.Document)]
        public Dictionary<string, string> ReportImageMap
        {
            get { return this.m_reportImageMap; }
            set { this.m_reportImageMap = value; }
        }

        public List<ReportItemElement> ReportItemCollection
        {
            get { return this.m_reportItemCollection; }
            set { this.m_reportItemCollection = value; }
        }
        #endregion

        #region 公共方法
        public void Init()
        {
        }
        public void Clear()
        {
            this.m_reportItemTable.Clear();
            this.ReportTitle = "";
            this.RemarkFlag = 0;
            this.Remark = "";
        }
        public void CreateImageMap()
        {
            this.m_reportImageMap = new Dictionary<string, string>();
        }
        public void RemoveReportItem(Type type)
        {
            if (type != null)
            {
                if (this.m_reportItemTable.ContainsKey(type))
                {
                    lock (this.m_reportItemTable)
                    {
                        this.m_reportItemTable.Remove(type);
                    }
                }
            }
        }
        public List<AbstractSubFillElement> GetReportItem(Type type)
        {
            if (type != null)
            {
                List<AbstractSubFillElement> result = this.m_reportItemTable[type] as List<AbstractSubFillElement>;
                if (result == null)
                {
                    result = new List<AbstractSubFillElement>(10);
                    lock (this.m_reportItemTable)
                    {
                        this.m_reportItemTable[type] = result;
                    }
                }
                return result;
            }
            return null;
        }
        #endregion
    }
}