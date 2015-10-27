using System;
using XYS.Lis.Appender;

namespace XYS.Lis.Core
{
    public interface IAppenderAttachable
    {
        void AddAppender(IAppender appender);
        AppenderCollection Appenders { get; }
        IAppender GetAppender(string name);
        IAppender RemoveAppender(IAppender appender);
        IAppender RemoveAppender(string name);
    }
}
