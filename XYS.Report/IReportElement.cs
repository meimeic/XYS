using System;
namespace XYS.Report
{
    public interface IReportElement
    {
        IReportKey PK { get; }
        HandleResult HandleResult { get; }
    }
}