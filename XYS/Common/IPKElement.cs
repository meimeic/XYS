using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XYS.Common
{
   public interface IPKElement
    {
        string PKElementName { get; }
        long PKElementValue { get; }
        Type PKType { get; }
    }
}
