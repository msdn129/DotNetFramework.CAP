using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNetFramework.CAP.Core.Ioc
{
    public interface IServiceScope:IDisposable
    {
        IServiceProvider ServiceProvider { get; set; }

    }

    public class ServiceScope : IServiceScope
    {
        public IServiceProvider ServiceProvider { get; set; }

        private ILifetimeScope _scope;
        public ServiceScope(ILifetimeScope scope)
        {
        
            _scope = scope;
        }


        public void Dispose()
        {
            _scope.Dispose();
        }
    }
}
