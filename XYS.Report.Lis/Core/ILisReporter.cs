using System;
using XYS.Model;
using XYS.Common;
namespace XYS.Report.Lis.Core
{
    public interface ILisReporter : IReporter
    {
        bool IsLisReport(IReportElement report);
    }
}