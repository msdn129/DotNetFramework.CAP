using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNetFramework.CAP.Core.Logger
{
    public interface ILoggerFactory
    {
       ILogger<T> CreateLogger<T>();
    }

    public class LoggerFactory: ILoggerFactory
    {
       public ILogger<T> CreateLogger<T>()
        {
            return new Logger<T>();
        }
    }
}
