using log4net;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;

namespace ICSLicenseMaintV2
{
    public class RequestDurationLoggerModule : IHttpModule
    {
        private const string ContextItemKey = "stopwatchContextItemKey";
        private static log4net.ILog Log = LogManager.GetLogger(typeof(RequestDurationLoggerModule));
        private HttpApplication application;
        public void Init(HttpApplication application)
        {
            this.application = application;
            if(Log.IsDebugEnabled && application != null)
            {
                application.BeginRequest += Application_BeginRequest;
                application.EndRequest += Application_EndRequest;
            }
        }

        private void Application_BeginRequest(object sender, EventArgs e)
        {
            if (application != null)
            {
                application.Context.Items[ContextItemKey] = Stopwatch.StartNew();
            }
        }

        private void Application_EndRequest(object sender, EventArgs e)
        {
            if(application != null)
            {
                var stopwatch = (Stopwatch)application.Context.Items[ContextItemKey];
                stopwatch.Stop();

                Log.Debug(string.Format("{0} -> [{1} ms]", application.Context.Request.RawUrl, stopwatch.ElapsedMilliseconds));
            }
        }

        public void Dispose()
        {
            if(application != null)
            {
                application.BeginRequest -= Application_BeginRequest;
                application.EndRequest -= Application_EndRequest;
            }
        }
    }
}