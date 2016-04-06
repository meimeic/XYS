namespace XYS.Report
{
    public class HandlerResult
    {
        private int m_statusCode;
        public string m_message;
        public HandlerResult()
        {
            this.m_statusCode = 1;
            this.m_message = "OK";
        }
        public HandlerResult(int code, string message)
        {
            this.m_statusCode = code;
            this.m_message = message;
        }

        public int StatusCode
        {
            get { return this.m_statusCode; }
            set { this.m_statusCode = value; }
        }
        public string Message
        {
            get { return this.m_message; }
            set { this.m_message = value; }
        }
    }
}