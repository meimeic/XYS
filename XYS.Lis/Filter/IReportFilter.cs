using XYS.Lis.Core;

namespace XYS.Lis.Filter
{
   public interface IReportFilter:IReportOptionHandler
    {
       IReportFilter Next { get; set; }
    }
}
