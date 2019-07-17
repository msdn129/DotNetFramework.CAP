using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNetFramework.CAP.Core.Ioc
{
    public static class ActivatorUtilities
    {
        public static object GetServiceOrCreateInstance(IServiceProvider provider, Type type)
        {
            var instance = provider.GetService(type);
            if (instance == null)
                instance = Activator.CreateInstance(type);
            return instance;
        }
    }
}
