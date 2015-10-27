using XYS.Common;
namespace XYS.Model
{
    public enum ReportElementTag : int
    {
        ReportElement = 1,
        ExamElement,
        PatientElement,
        ItemElement,
        GraphElement,
        CustomElement,
        NoneElement
    }
    public interface IReportElement
    {
        ReportElementTag ElementTag { get; }
       // ReportKey ReporterKey { get; set; }
    }
}
