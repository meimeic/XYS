using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XYS.Report.Entities
{
    public interface IReportPK
    {
        bool Configured { get; set; }
        string ID { get; }
        string KeyWhere();
    }
    public abstract class ReportPKBase : IReportPK
    {
        public bool Configured { get; set; }

        public virtual string ID { get; }

        public abstract string KeyWhere();
    }
}
