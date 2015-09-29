using XYS.Model;
namespace XYS.Lis.Core
{
    public interface ILisReportElement:IReportElement
    {
        string SearchSQL { get; }
        //数据填充后操作
        void AfterFill();
    }
}
