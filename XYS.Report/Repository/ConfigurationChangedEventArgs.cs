using System;
using System.Collections;

namespace XYS.Report.Repository
{
    public class ConfigurationChangedEventArgs : EventArgs
    {
        private readonly ICollection configurationMessages;
        public ConfigurationChangedEventArgs(ICollection configurationMessages)
        {
            this.configurationMessages = configurationMessages;
        }
        public ICollection ConfigurationMessages
        {
            get { return configurationMessages; }
        }
    }
}