using System;

using XYS.Lis.Core;
using XYS.Lis.Repository;
namespace XYS.Lis.Repository.Hierarchy
{
   public interface IReporterFactory
    {
       Reporter CreateReporter(IReporterRepository repository,ReporterKey key);
    }
}
