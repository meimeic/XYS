using XYS.Common;
namespace XYS.Model
{
    public enum ReportElementTag : int
    {
        ReportElement = 1,
        InfoElement,
        ItemElement,
        GraphElement,
        CustomElement,
        KVElement,
        NoneElement
    }
    public interface IReportElement
    {
        ReportElementTag ElementTag { get; }
       // ReportKey ReporterKey { get; set; }
    }
}
