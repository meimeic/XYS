using System;

using XYS.Lis.Fill;
using XYS.Lis.Export;
using XYS.Lis.Core;

namespace XYS.Lis.Repository.Hierarchy
{
    public class DefaultReporterFactory:IReporterFactory
    {
        #region
       public DefaultReporterFactory()
        { 
       }
        #endregion

       #region 实现IReporterFactory接口
       public Reporter CreateReporter(IReporterRepository repository, ReporterKey key)
       {
           if (key == null)
           {
               return new DefaultReporter();
           }
           return new ReporterImpl(key.ReporterName, key.StrategyName);
       }
       #endregion

       #region 内部类
       public sealed class ReporterImpl : Reporter
       {
           public ReporterImpl(string name, string strategyName)
               : base(name, strategyName)
           {
           }
       }
        #endregion
    }
}
