using System;
namespace XYS.Report
{
    public interface IReportKey
    {
        bool Configured { get; set; }
        string ID { get; }
        string KeyWhere();
    }
}
