using System;
using XYS.Lis.Core;
using XYS.Lis.Appender;
namespace XYS.Lis.Repository
{
    public interface IReportRepository
    {
        string RepositoryName { get; set; }
        ILisReportElement Exists(string name);
        ILisReportElement[] GetCurrentReports();
        ILisReportElement GetReport(string name);
        IAppender[] GetAppenders();
    }
}
