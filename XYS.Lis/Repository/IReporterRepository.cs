using System;
using System.Collections;
using System.Collections.Generic;

using XYS.Model;
using XYS.Common;
using XYS.Lis.Core;
using XYS.Lis.Model;
using XYS.Lis.Util;
namespace XYS.Lis.Repository
{
    public delegate void ReporterRepositoryConfigurationChangedEventHandler(object sender, EventArgs e);

    public interface IReporterRepository
    {
      
        #region
        string RepositoryName { get; set; }
        Hashtable FillerMap { get; }
        Hashtable HandlerMap { get; }
        Hashtable ExportMap { get; }
        Hashtable StrategyMap { get; }
        bool Configured { get; set; }
        ICollection ConfigurationMessages { get; set; }
        PropertiesDictionary Properties { get; }
        #endregion
        
        #region
        ILisReporter Exists(ReporterKey key);
        ILisReporter[] GetCurrentReporters();
        ILisReporter GetReporter(ReporterKey key);
        #endregion
        
        #region
        event ReporterRepositoryConfigurationChangedEventHandler ConfigurationChanged;
        #endregion
    }
}
