using Autofac;
using Autofac.Core;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNetFramework.CAP.Core.Ioc
{
    public interface IServiceProvider
    {
        ILifetimeScope Scope { get; set; }

        IServiceScope CreateScope();

        T GetRequiredService<T>();

        T GetService<T>();

        IEnumerable<object> GetServices(Type serviceType);

        IEnumerable<object> GetServices<T>();

        object GetService(Type serviceType);

        IContainer Container { get; set; }

        IServiceCollection ServiceCollection { get; set; }
    }

    public class ServiceProvider : IServiceProvider
    {
        public IServiceCollection ServiceCollection { get; set; }

        public ILifetimeScope Scope
        {
            get
            {
                return _root;
            }
            set
            {
                _root = value;
            }
        }
        private IServiceScope Root { set; get; }
        public ILifetimeScope _root;
        private IContainer _container;
        public IContainer Container
        {
            set
            {
                _container = value;
                var scope = _container.BeginLifetimeScope();
                _root = scope;
                Root = new ServiceScope(scope);
                Root.ServiceProvider = this;
            }
            get
            {
                return _container;
            }
        }

        public ServiceProvider()
        {

        }

        public IServiceScope CreateScope()
        {
            var _scope = _root.BeginLifetimeScope();
            var _serviceScope = new ServiceScope(_scope);
            _serviceScope.ServiceProvider = new ServiceProvider();
            _serviceScope.ServiceProvider.Container = this._container;
            _serviceScope.ServiceProvider.Scope = _scope;
            _serviceScope.ServiceProvider.ServiceCollection = this.ServiceCollection;
            return _serviceScope;
        }

        public T GetRequiredService<T>()
        {
            return _root.Resolve<T>();
        }

        public T GetService<T>()
        {
            try
            {
                return _root.Resolve<T>();
            }
            catch { return default(T); }
        }

        public object GetService(Type serviceType)
        {
            try
            {
                return _root.Resolve(serviceType);
            }
            catch {
                Log.Warning(serviceType.FullName);
                return null;
            }
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            foreach (var item in ServiceCollection)
            {
                if (item.ServiceType == serviceType)
                {
                    var impl = GetServiceByName(item.ImplementationType.Name,serviceType);
                    if (impl != null)
                        yield return impl;
                }
         
            }
        }

        private object GetServiceByName(string Name, Type serviceType)
        {
            try
            {
                return _root.ResolveNamed(Name, serviceType);
            } 
            catch
            {
                Log.Warning(serviceType.FullName);
                return null;
            }
        }


        public IEnumerable<object> GetServices<T>()
        {
            return this.GetServices(typeof(T));
        }
    }


}
