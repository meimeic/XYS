using XYS.Common;
namespace XYS.Model
{
    public enum ReportElementType
    {
        ExamElement = 1,
        PatientElement,
        ItemElement,
        GraphElement,
        GeneralElement,
        ReportElement,
        CustomElement
    }
    public interface IReportElement
    {
        ReportElementType ElementType { get; }
        ReportPK ReporterPK { get; set; }
    }
}
