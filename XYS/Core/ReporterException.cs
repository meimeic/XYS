using System;
using System.Runtime.Serialization;
namespace XYS.Core
{
    [Serializable]
    public class ReporterException : ApplicationException
    {
        public ReporterException()
        {
        }
        public ReporterException(String message)
            : base(message)
        {
        }
        public ReporterException(String message, Exception innerException)
            : base(message, innerException)
        {
        }
        protected ReporterException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
