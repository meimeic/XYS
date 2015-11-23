using System;
using System.Collections.Generic;

using XYS.Model;
using XYS.Lis.Core;

namespace XYS.Lis.Model.Export
{
    public class ReportInfo : ILisExportElement
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
        private int m_age;
        private int m_genderName;
        private string m_pid;
        private int m_clinicName;
        private int m_visitTimes;
        private string m_deptName;
        private string m_doctor;
        private string m_bedNo;
        private string m_clinicalDiagnosis;
        private string m_explanation;
        public ReportElementTag ElementTag
        {
            get { throw new NotImplementedException(); }
        }
    }
}
