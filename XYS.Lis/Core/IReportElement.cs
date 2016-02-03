namespace XYS.Lis.Core
{
    public enum ReportElementTag : int
    {
        Report = 1,
        Filler,
        Inner
    }
    public interface IReportElement
    {
        ReportElementTag ElementTag { get; }
        void After();
    }
}
