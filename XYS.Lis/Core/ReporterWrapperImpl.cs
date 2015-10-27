using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XYS.Lis.Core
{
   public abstract class ReporterWrapperImpl:IReporterWrapper
    {
       private readonly ILisReporter m_reporter;
       protected ReporterWrapperImpl(ILisReporter reporter) 
		{
            this.m_reporter = reporter;
		}
        public virtual ILisReporter Reporter
        {
            get { return this.m_reporter; }
        }
       
    }
}
