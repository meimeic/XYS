using System;

using XYS.Common;
using XYS.Lis.Core;
namespace XYS.Lis.Export.Model
{
    [FRExport()]
    public class ReportPatient:IExportElement
    {
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

        public ReportPatient()
        { }

        public string PatientName
        {
            get { return this.m_patientName; }
            set { this.m_patientName = value; }
        }
        public string GenderName
        {
            get { return this.m_genderName; }
            set { this.m_genderName = value; }
        }
        public string AgeStr
        {
            get { return this.m_ageStr; }
            set { this.m_ageStr = value; }
        }
        public string CID
        {
            get { return this.m_cid; }
            set { this.m_cid = value; }
        }
        public string PID
        {
            get { return this.m_pid; }
            set { this.m_pid = value; }
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