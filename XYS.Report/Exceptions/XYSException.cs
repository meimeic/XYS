using System;
using System.Runtime.Serialization;

namespace XYS.Common
{
    [Serializable]
    public class XYSException : ApplicationException
    {
        public XYSException()
        {
        }
        public XYSException(String message)
            : base(message)
        {
        }
        public XYSException(String message, Exception innerException)
            : base(message, innerException)
        {
        }
        protected XYSException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
