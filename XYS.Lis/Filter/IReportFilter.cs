using XYS.Lis.Core;

namespace XYS.Lis.Filter
{
   public interface IReportFilter
    {
       IReportFilter Next { get; set; }
    }
}
