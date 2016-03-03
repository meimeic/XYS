using System;

namespace XYS.Report.Core
{
    public interface IReporterWrapper
    {
        IReporter Reporter { get; }
    }
}
