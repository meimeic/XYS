namespace XYS.Lis.Core
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
        string SearchSQL { get; }
        //数据填充后操作
        void AfterFill();
    }
}
