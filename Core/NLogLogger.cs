using System;
using NLog;
using ILogger = Core.Logging.ILogger;

namespace Infrastructure.Logging
{
    public class NLogLogger : ILogger
    {
        private static readonly Lazy<NLogLogger> LazyLogger = new Lazy<NLogLogger>(() => new NLogLogger());
        private static readonly Lazy<Logger> LazyNLogger = new Lazy<Logger>(LogManager.GetCurrentClassLogger);

        private NLogLogger()
        {
        }

        public static ILogger Instance
        {
            get { return LazyLogger.Value; }
        }

        public void Log(string message)
        {
            LazyNLogger.Value.Info(message);
        }

        public void Log(Exception ex)
        {
            LazyNLogger.Value.Error(ex);
        }

        public T Log<T>(Func<T> t)
        {
            try
            {
                return t.Invoke();
            }
            catch (Exception ex)
            {
                LazyNLogger.Value.Error(ex);
            }
            return default(T);
        }
    }
}