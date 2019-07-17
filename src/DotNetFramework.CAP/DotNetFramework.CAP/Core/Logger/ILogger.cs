using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNetFramework.CAP.Core.Logger
{
    public interface ILogger
    {
        void LogDebug(string message, params object[] args);

        void LogWarning(string message, params object[] args);

        void LogInformation(string message, params object[] args);

        void LogError(string message, params object[] args);

        void LogCritical(string message, params object[] args);

        void LogError(Exception e, string message, params object[] args);

        void LogWarning(Exception e, string message, params object[] args);

        void LogWarning(int num, Exception e, string message, params object[] args);

        void LogTrace(string message, params object[] args);

    }
    public interface ILogger<T> : ILogger
    {
    }

    public class Logger : ILogger
    {

        public Logger()
        {

        }

        public void LogCritical(string message, params object[] args)
        {
            Log.Fatal(message, args);
        }

        public void LogDebug(string message, params object[] args)
        {
            Log.Debug(message, args);
        }

        public void LogError(string message, params object[] args)
        {
            Log.Error(message, args);
        }

        public void LogError(Exception e, string message, params object[] args)
        {
            Log.Error(e, message, args);
        }

        public void LogInformation(string message, params object[] args)
        {
            Log.Information(message, args);
        }

        public void LogTrace(string message, params object[] args)
        {
            Log.Information(message, args);
        }

        public void LogWarning(string message, params object[] args)
        {
            Log.Warning(message, args);
        }

        public void LogWarning(Exception e, string message, params object[] args)
        {
            Log.Warning(e, message, args);
        }

        public void LogWarning(int num, Exception e, string message, params object[] args)
        {
            Log.Warning(e, message, args);
        }
    }

    public class Logger<T> : ILogger<T>
    {
        public void LogCritical(string message, params object[] args)
        {
            Log.Fatal(message, args);
        }

        public void LogDebug(string message, params object[] args)
        {
            Log.Debug(message, args);
        }

        public void LogError(string message, params object[] args)
        {
            Log.Error(message, args);
        }

        public void LogError(Exception e, string message, params object[] args)
        {
            Log.Error(e, message, args);
        }

        public void LogInformation(string message, params object[] args)
        {
            Log.Information(message, args);
        }

        public void LogTrace(string message, params object[] args)
        {
            Log.Information(message, args);
        }

        public void LogWarning(string message, params object[] args)
        {
            Log.Warning(message, args);
        }

        public void LogWarning(Exception e, string message, params object[] args)
        {
            Log.Warning(e, message, args);
        }

        public void LogWarning(int num, Exception e, string message, params object[] args)
        {
            Log.Warning(e, message, args);
        }
    }
}
