using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XYS.Report.HandleQueue
{
   public abstract class ReportHandleQueueItem<TR>
    {
       public string Key { get; set; }
       public Action<TR> Action { get; set; }
       public TR Report { get; set; }

       public void Operate()
       {
           if (Action != null)
           {
               Action(Report);
           }
       }

       public ReportHandleQueueItem(string key,TR report,Action<TR> action)
       {
           Key = key;
           Report = report;
           Action = action;
       }
    }
}
