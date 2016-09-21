using System;

namespace XYS.Report.Entities
{
    public interface ITSReport : ITSEntity
    {
    }

    [Serializable]
    public abstract class TSReportBase : ITSReport
    {
    }
}
