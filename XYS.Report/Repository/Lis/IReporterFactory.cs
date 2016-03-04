using System;

using XYS.Report.Core;
namespace XYS.Report.Repository.Lis
{
   public interface IReporterFactory
    {
       LisReporter CreateReporter(IReporterRepository repository,ReporterKey key);
    }
}
