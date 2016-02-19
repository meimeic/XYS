namespace XYS.Lis.Core
{
    public enum ReportElementTag : int
    {
        Report = 1,
        Exam,
        Patient,
        Item,
        Graph,
        Custom,
        Temp
    }
    public interface IReportElement
    {
        ReportElementTag ElementTag { get; }
    }
}
