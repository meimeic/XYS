using XYS.Common;

using XYS.Report.Lis.Core;
namespace XYS.Report.Lis.Repository
{
   public interface IReporterFactory
    {
       Reporter CreateReporter(IReporterRepository repository,ReporterKey key);
    }
}
