using System;
using System.Collections.Generic;

using Newtonsoft.Json;

using XYS.Model;
using XYS.Lis.Core;

namespace XYS.Lis.Model.Export
{
    public class ReporterInfo : ILisExportElement
    {
        private static readonly ReportElementTag Default_Tag = ReportElementTag.InfoElement;

        private int m_sectionNo;
        private string m_serialNo;
        private string m_sampleNo;
        private int m_sampleTypeNo;
        private string m_sampleTypeName;
        private string m_formMemo;
        private string m_formComment;
        private string m_formComment2;
        private string m_technician;
        private string m_checker;

        private string m_patientName;
        private string m_cid;
        private string m_ageStr;
        private GenderType m_gender;
        private string m_genderName;
        private string m_pid;
        private ClinicType m_clinicType;
        private string m_clinicName;
        private int m_visitTimes;
        private string m_deptName;
        private string m_doctor;
        private string m_bedNo;
        private string m_clinicalDiagnosis;
        private string m_explanation;

        [JsonIgnore]
        public ReportElementTag ElementTag
        {
            get { return Default_Tag; }
        }

        public ReporterInfo()
        {

        }

        public int SectionNo
        {
            get { return m_sectionNo; }
            set { m_sectionNo = value; }
        }
        public string SerialNo
        {
            get { return m_serialNo; }
            set { m_serialNo = value; }
        }
        public string SampleNo
        {
            get { return m_sampleNo; }
            set { m_sampleNo = value; }
        }
        public int SampleTypeNo
        {
            get { return m_sampleTypeNo; }
            set { m_sampleTypeNo = value; }
        }
        public string SampleTypeName
        {
            get { return m_sampleTypeName; }
            set { m_sampleTypeName = value; }
        }
        public string FormMemo
        {
            get { return m_formMemo; }
            set { m_formMemo = value; }
        }
        public string FormComment
        {
            get { return m_formComment; }
            set { m_formComment = value; }
        }
        public string FormComment2
        {
            get { return m_formComment2; }
            set { m_formComment2 = value; }
        }
        public string Technician
        {
            get { return m_technician; }
            set { m_technician = value; }
        }
        public string Checker
        {
            get { return m_checker; }
            set { m_checker = value; }
        }

        public string PatientName
        {
            get { return this.m_patientName; }
            set { this.m_patientName = value; }
        }
        public string CID
        {
            get { return this.m_cid; }
            set { this.m_cid = value; }
        }
        public string AgeStr
        {
            get { return this.m_ageStr; }
            set { this.m_ageStr = value; }
        }
        public GenderType Gender
        {
            get { return this.m_gender; }
            set { this.m_gender = value; }
        }
        public string GenderName
        {
            get { return this.m_genderName; }
            set { this.m_genderName = value; }
        }
        public string PID
        {
            get { return this.m_pid; }
            set { this.m_pid = value; }
        }
        public ClinicType ClinicType
        {
            get { return this.m_clinicType; }
            set { this.m_clinicType = value; }
        }
        public string ClinicName
        {
            get { return this.m_clinicName; }
            set { this.m_clinicName = value; }
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
        public string BedNo
        {
            get { return this.m_bedNo; }
            set { this.m_bedNo = value; }
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
    }
}
