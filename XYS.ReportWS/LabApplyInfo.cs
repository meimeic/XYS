using System;
using System.Xml;
using System.Xml.Serialization;
using System.Collections.Generic;

namespace XYS.ReportWS
{
    [XmlRoot("root")]
    public class LabApplyInfo
    {
        public LabApplyInfo()
        {
        }

        [XmlElement("visit_type")]
        public int ClinicType { get; set; }

        [XmlElement("patient_id")]
        public string PatientID { get; set; }

        [XmlElement("visit_no")]
        public int VisitTimes { get; set; }

        [XmlElement("operator")]
        public string Operator { get; set; }

        [XmlArray("applys")]
        [XmlArrayItem("apply")]
        public List<ApplyItem> ApplyCollection { get; set; }
    }

    [XmlRoot("apply")]
    public class ApplyItem
    {
        public ApplyItem()
        { }
        [XmlElement("apply_no")]
        public string ApplyNo { get; set; }

        [XmlElement("apply_status")]
        public int ApplyStatus { get; set; }

        [XmlElement("operator")]
        public string Operator { get; set; }
    }
}