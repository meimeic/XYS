using System;
using System.Collections;
using System.Collections.Generic;
namespace XYS.Report.Entities
{
    public interface IDBReport : IEntityBase
    {
        IReportPK RK { get; set; }
        List<Type> FillTypes { get; }
        Hashtable ItemCollection { get; }
    }
    public abstract class DBReportBase : IDBReport
    {
        public IReportPK RK { get; set; }
        public List<Type> FillTypes { get; }
        public Hashtable ItemCollection { get; }
    }
}
