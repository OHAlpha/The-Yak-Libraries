using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Logging
{
    public class LoggingClass
    {

        public delegate void LogMessage(string message);

        private bool logging;

        public LogMessage logger;

        public void Log(string message)
        {
            logger(message);
        }

        public bool isLogging()
        {
            return logging;
        }

        public void setLogging(bool logging)
        {
            this.logging = logging;
        }

    }
}
