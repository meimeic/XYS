using System;

using XYS.Common;
using XYS.FRReport.Model;
namespace XYS.FRReport.Model.Lis
{
    [FRExport()]
    public class ReportExam : IFRExportElement
    {
        private string m_serialNo;
        private string m_sampleNo;
        private string m_sampleTypeName;

        private string m_parItemName;

        private string m_formMemo;
        private string m_formComment;
        private string m_formComment2;

        private string m_technician;
        private string m_checker;

        public ReportExam()
        { }
        [FRExport()]
        public string SerialNo
        {
            get { return m_serialNo; }
            set { m_serialNo = value; }
        }
        [FRExport()]
        public string SampleNo
        {
            get { return m_sampleNo; }
            set { m_sampleNo = value; }
        }
        [FRExport()]
        public string SampleTypeName
        {
            get { return m_sampleTypeName; }
            set { m_sampleTypeName = value; }
        }
        [FRExport()]
        public string ParItemName
        {
            get { return m_parItemName; }
            set { m_parItemName = value; }
        }
        [FRExport()]
        public string FormMemo
        {
            get { return m_formMemo; }
            set { m_formMemo = value; }
        }
        [FRExport()]
        public string FormComment
        {
            get { return m_formComment; }
            set { m_formComment = value; }
        }
        [FRExport()]
        public string FormComment2
        {
            get { return m_formComment2; }
            set { m_formComment2 = value; }
        }
        [FRExport()]
        public string Technician
        {
            get { return m_technician; }
            set { m_technician = value; }
        }
        [FRExport()]
        public string Checker
        {
            get { return m_checker; }
            set { m_checker = value; }
        }
    }
}
