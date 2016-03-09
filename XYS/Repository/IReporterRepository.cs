using System;
using System.Collections;

using XYS.Util;
using XYS.Common;
namespace XYS.Repository
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
        IReporter Exists(ReporterKey key);
        IReporter GetReporter(ReporterKey key);
        #endregion
    }
}
