using System;
using System.Collections;

using XYS.Util;
namespace XYS.Report.Lis
{
    public class ReportManager
    {
        private static readonly Hashtable ReportMap = new Hashtable(10);

        public static IReport GetReport(Type type)
        {
            IReport impl = ReportMap[type] as IReport;
            if (impl == null)
            {
                DefaultReporter reporter = new DefaultReporter();
                impl = new ReportImpl(reporter);
                ReportMap[type] = impl;
            }
            return impl;
        }
    }
}
