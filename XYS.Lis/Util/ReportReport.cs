using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using XYS.Lis.Util.TypeConverters;

namespace XYS.Lis.Util
{
    public delegate void ReportReceivedEventHandler(object sender, ReportReceivedEventArgs e);
    public class ReportReceivedEventArgs : EventArgs
    {
        private ReportReport m_report;

        public ReportReceivedEventArgs(ReportReport rr)
        {
            this.m_report = rr;
        }

        public ReportReport ReportReport
        {
            get { return this.m_report; }
        }

    }
    //处理dll加载过程中的错误
    public class ReportReport
    {
        #region
        private static bool s_debugEnabled = false;
        private static bool s_quietMode = false;
        private static bool s_emitInternalMessages = true;
        private static readonly string PREFIX = "lis-report: ";
        private static readonly string ERR_PREFIX = "lis-report:ERROR ";
        private static readonly string WARN_PREFIX = "lis-report:WARN ";
        #endregion

        #region
        private readonly Type source;
        private readonly DateTime timeStamp;
        private readonly string prefix;
        private readonly string message;
        private readonly Exception exception;
        #endregion

        #region
        public static event ReportReceivedEventHandler ReportReceived;
        #endregion

        #region
        public ReportReport(Type source, string prefix, string message, Exception exception)
        {
            timeStamp = DateTime.Now;
            this.source = source;
            this.prefix = prefix;
            this.message = message;
            this.exception = exception;
        }
        #endregion

        #region
        static ReportReport()
        {
            try
            {
                InternalDebugging = OptionConverter.ToBoolean(SystemInfo.GetAppSetting("lis-report.Internal.Debug"), false);
                QuietMode = OptionConverter.ToBoolean(SystemInfo.GetAppSetting("lis-report.Internal.Quiet"), false);
                EmitInternalMessages = OptionConverter.ToBoolean(SystemInfo.GetAppSetting("lis-report.Internal.Emit"), true);
            }
            catch (Exception ex)
            {
                // If an exception is thrown here then it looks like the config file does not
                // parse correctly.
                //
                // We will leave debug OFF and print an Error message
                Error(typeof(ReportReport), "Exception while reading ConfigurationSettings. Check your .config file is well formed XML.", ex);
            }
        }
        #endregion
        #region
        public Type Source
        {
            get { return source; }
        }

        public DateTime TimeStamp
        {
            get { return timeStamp; }
        }

        public string Prefix
        {
            get { return prefix; }
        }

        public string Message
        {
            get { return message; }
        }

        public Exception Exception
        {
            get { return exception; }
        }

        public override string ToString()
        {
            return Prefix + Source.Name + ": " + Message;
        }
        #endregion

        #region
        public static bool QuietMode
        {
            get { return s_quietMode; }
            set { s_quietMode = value; }
        }
        
        public static bool InternalDebugging
        {
            get { return s_debugEnabled; }
            set { s_debugEnabled = value; }
        }
        
        public static bool EmitInternalMessages
        {
            get { return s_emitInternalMessages; }
            set { s_emitInternalMessages = value; }
        }
        
        public static bool IsDebugEnabled
        {
            get { return s_debugEnabled && !s_quietMode; }
        }
        
        public static bool IsWarnEnabled
        {
            get { return !s_quietMode; }
        }
        
        public static bool IsErrorEnabled
        {
            get { return !s_quietMode; }
        }
        #region

        #endregion
        public static void Debug(Type source, string message)
        {
            if (IsDebugEnabled)
            {
                if (EmitInternalMessages)
                {
                    EmitOutLine(PREFIX + message);
                }
                OnReportReceived(source, PREFIX, message, null);
            }
        }
        public static void Debug(Type source, string message, Exception exception)
        {
            if (IsDebugEnabled)
            {
                if (EmitInternalMessages)
                {
                    EmitOutLine(PREFIX + message);
                    if (exception != null)
                    {
                        EmitOutLine(exception.ToString());
                    }
                }
                OnReportReceived(source, PREFIX, message, exception);
            }
        }
        public static void Warn(Type source, string message)
        {
            if (IsWarnEnabled)
            {
                if (EmitInternalMessages)
                {
                    EmitErrorLine(WARN_PREFIX + message);
                }
                OnReportReceived(source, WARN_PREFIX, message, null);
            }
        }
        public static void Warn(Type source, string message, Exception exception)
        {
            if (IsWarnEnabled)
            {
                if (EmitInternalMessages)
                {
                    EmitErrorLine(WARN_PREFIX + message);
                    if (exception != null)
                    {
                        EmitErrorLine(exception.ToString());
                    }
                }
                OnReportReceived(source, WARN_PREFIX, message, exception);
            }
        }
        public static void Error(Type source, string message)
        {
            if (IsErrorEnabled)
            {
                if (EmitInternalMessages)
                {
                    EmitErrorLine(ERR_PREFIX + message);
                }

                OnReportReceived(source, ERR_PREFIX, message, null);
            }
        }
        public static void Error(Type source, string message, Exception exception)
        {
            if (IsErrorEnabled)
            {
                if (EmitInternalMessages)
                {
                    EmitErrorLine(ERR_PREFIX + message);
                    if (exception != null)
                    {
                        EmitErrorLine(exception.ToString());
                    }
                }
                OnReportReceived(source, ERR_PREFIX, message, exception);
            }
        }
        public static void OnReportReceived(Type source, string prefix, string message, Exception exception)
        {
            if (ReportReceived != null)
            {
                ReportReceived(null, new ReportReceivedEventArgs(new ReportReport(source, prefix, message, exception)));
            }
        }
        #endregion

        #region
        private static void EmitOutLine(string message)
        {
            try
            {
#if NETCF
				Console.WriteLine(message);
				//System.Diagnostics.Debug.WriteLine(message);
#else
                Console.Out.WriteLine(message);
                //Trace.WriteLine(message);
#endif
            }
            catch
            {
                // Ignore exception, what else can we do? Not really a good idea to propagate back to the caller
            }
        }
        private static void EmitErrorLine(string message)
        {
            try
            {
#if NETCF
				Console.WriteLine(message);
				//System.Diagnostics.Debug.WriteLine(message);
#else
                Console.Error.WriteLine(message);
                //Trace.WriteLine(message);
#endif
            }
            catch
            {
                // Ignore exception, what else can we do? Not really a good idea to propagate back to the caller
            }
        }
        #endregion

        #region
        public class ReportReceivedAdapter : IDisposable
        {
            private readonly IList items;
            private readonly ReportReceivedEventHandler handler;

            public ReportReceivedAdapter(IList items)
            {
                this.items = items;
                handler = new ReportReceivedEventHandler(ReportReport_ReportReceived);
                ReportReceived += handler;
            }

            void ReportReport_ReportReceived(object source, ReportReceivedEventArgs e)
            {
                items.Add(e.ReportReport);
            }

            public IList Items
            {
                get { return items; }
            }

            public void Dispose()
            {
                ReportReceived -= handler;
            }
        }
        #endregion
    }
}
