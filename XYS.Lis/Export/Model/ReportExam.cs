using System;

using XYS.Common;
using XYS.Lis.Core;
namespace XYS.Lis.Export.Model
{
    [FRExport()]
   public class ReportExam:IExportElement
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
        public string SampleTypeName
        {
            get { return m_sampleTypeName; }
            set { m_sampleTypeName = value; }
        }
        public string ParItemName
        {
            get { return m_parItemName; }
            set { m_parItemName = value; }
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
    }
}
