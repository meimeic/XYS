using System;
using System.Collections;

using XYS.Util;
using XYS.Report.Lis.Core;
namespace XYS.Report.Lis.Repository
{
    public delegate void ReporterRepositoryConfigurationChangedEventHandler(object sender, EventArgs e);

    public interface IReporterRepository
    {
        #region
        Hashtable FillerMap { get; }
        Hashtable HandlerMap { get; }
        Hashtable StrategyMap { get; }

        bool Configured { get; set; }
        string RepositoryName { get; set; }
        ICollection ConfigurationMessages { get; set; }
        PropertiesDictionary Properties { get; }
        #endregion

        #region reporter相关方法
        ILisReporter Exists(ReporterKey key);
        ILisReporter GetReporter(ReporterKey key);
        #endregion
    }
}
