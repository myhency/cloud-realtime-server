using NLog;
using NLog.Targets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudRealtime.util
{
    [Target("control")]
    public class NLogCustomTarget : TargetWithLayout
    {
        public delegate void LogEventDelegate(string message);
        public LogEventDelegate LogEventListener;

        public NLogCustomTarget() { }

        protected override void Write(LogEventInfo logEvent)
        {
            string logMessage = this.Layout.Render(logEvent);
            LogEventListener?.Invoke(logMessage);
        }
    }
}
