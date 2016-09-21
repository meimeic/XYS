using XYS.Report.Entities;

namespace XYS.Report.ReportHandler
{
    public interface IReportHandler<TDB, TTS>
        where TDB : IDBReport
        where TTS : ITSReport
    {
        bool FillItem(TDB tdb);
        bool Convert(TDB tdb,TTS ts);
    }
    public abstract class ReportHandler<TR>
    {

    }
}
