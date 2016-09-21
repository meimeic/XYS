using System;
using System.Runtime.Serialization;

namespace XYS.Common
{
    [Serializable]
    public class ReportException : ApplicationException
    {
        public ReportException()
        {
        }
        public ReportException(String message)
            : base(message)
        {
        }
        public ReportException(String message, Exception innerException)
            : base(message, innerException)
        {
        }
        protected ReportException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
