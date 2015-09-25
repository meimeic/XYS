using XYS.Model;
namespace XYS.Lis.Core
{
    public interface ILisReportElement:IReportElement
    {
        //数据填充后操作
        void AfterFill();
    }
}
