using System;
using System.Collections;
using System.Collections.Generic;
namespace XYS.Report.Entities
{
    public interface IDBReport : IEntityBase
    {
        IReportPK RK { get; set; }
        List<Type> FillTypes { get; }
        List<IDBReportItem> ItemCollection(Type type);
    }
    public abstract class DBReportBase : IDBReport
    {
        public IReportPK RK { get; set; }
        public virtual List<Type> FillTypes { get; }
        public abstract List<IDBReportItem> ItemCollection(Type type);
    }
}
