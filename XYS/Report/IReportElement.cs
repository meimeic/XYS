using System;
namespace XYS.Report
{
    public interface IReportElement
    {
        IReportKey RK { get; }
        HandleResult HandleResult { get; }
    }
}