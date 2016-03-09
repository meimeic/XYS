using System;

using XYS.Common;
using XYS.Repository;
namespace XYS.Report.Lis.Repository
{
   public interface IReporterFactory
    {
       Reporter CreateReporter(IReporterRepository repository,ReporterKey key);
    }
}
