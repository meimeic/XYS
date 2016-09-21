using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XYS.Report.HandleQueue
{
    public abstract class ReportHandleQueue<TR>
    {
        private readonly Dictionary<string, ReportHandleQueueItem<TR>> MessageQueueDictionary = new Dictionary<string, SenparcMessageQueueItem>(StringComparer.OrdinalIgnoreCase);

        public static string GenerateKey(string name, string identityKey, string handleName)
        {
            var key = string.Format("Name@{0}||Key@{2}||HandleName@{3}", name, identityKey, handleName);
            return key;
        }

    }
}
