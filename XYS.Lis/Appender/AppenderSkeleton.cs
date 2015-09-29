using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XYS.Lis.Appender
{
   public abstract class AppenderSkeleton:IAppender
    {

        public string AppenderName
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }
    }
}
