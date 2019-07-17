using Autofac;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNetFramework.CAP.Core.Ioc
{
    public interface IServiceCollection : IEnumerable<ServiceDescriptor>
    {
        IServiceProvider ServiceProvider { get; set; }

        IServiceCollection AddSingleton<TService>(TService implementationInstance) where TService : class;

        IServiceCollection AddSingleton<TService, TImplementation>()
        where TService : class
        where TImplementation : class, TService;

        void TryAddSingleton<TService, TImplementation>()
        where TService : class
        where TImplementation : class, TService;

        void TryAddSingleton<TService>() where TService : class;

        void TryAddEnumerable(ServiceDescriptor descriptor);

        IServiceCollection AddTransient<TService, TImplementation>();

        IServiceCollection AddScoped(Type serviceType, Type implementationType);

        IServiceCollection AddSingleton(Type serviceType, Type implementationType);

        IServiceCollection AddSingleton<TService>(Func<IServiceProvider, TService> implementationFactory) where TService : class;

        IServiceCollection AddSingleton<TService>() where TService : class;

        IServiceCollection AddScoped<TService, TImplementation>();

        IServiceCollection AddInstance<TService>(TService instance) where TService : class;

        IServiceCollection AddScopedMuti<TService, TImplementation>();


        void BeginRegister();

    }

    public class ServiceCollection : IServiceCollection
    {
        private List<ServiceDescriptor> ListServiceDescriptor;

        private ContainerBuilder _builder;
        public IContainer Container { get; set; }
        public IServiceProvider ServiceProvider { get; set; }

        public ServiceCollection()
        {
            _builder = new ContainerBuilder();
          
            ListServiceDescriptor = new List<ServiceDescriptor>();
        }

        public void BeginRegister()
        {
            ServiceProvider = new ServiceProvider();
            this.AddInstance(ServiceProvider);
            Container = _builder.Build();
            ServiceProvider.Container = Container;
            ServiceProvider.ServiceCollection = this;

        }

        public IServiceCollection AddInstance<TService>(TService instance) where TService : class
        {
            ListServiceDescriptor.Add(new ServiceDescriptor(instance.GetType(), typeof(TService)));
            _builder.RegisterInstance(instance).As<TService>();
            return this;
        }

        public IServiceCollection AddScoped(Type serviceType, Type implementationType)
        {
            ListServiceDescriptor.Add(new ServiceDescriptor(implementationType, serviceType));
            _builder.RegisterGeneric(implementationType).As(serviceType);
            return this;
        }

        public IServiceCollection AddScoped<TService, TImplementation>()
        {
            ListServiceDescriptor.Add(new ServiceDescriptor(typeof(TImplementation), typeof(TService)));
            _builder.RegisterType<TImplementation>().As<TService>();
            return this;
        }

        public IServiceCollection AddSingleton<TService>(TService implementationInstance) where TService : class
        {
            ListServiceDescriptor.Add(new ServiceDescriptor(implementationInstance.GetType(), typeof(TService)));
            _builder.RegisterInstance(implementationInstance).As<TService>().SingleInstance();
            return this;
        }

        public IServiceCollection AddSingleton(Type serviceType, Type implementationType)
        {
            ListServiceDescriptor.Add(new ServiceDescriptor(implementationType, serviceType));
            _builder.RegisterGeneric(implementationType).As(serviceType).SingleInstance();
            return this;
        }

        public IServiceCollection AddSingleton<TService>() where TService : class
        {
            ListServiceDescriptor.Add(new ServiceDescriptor(typeof(TService), typeof(TService)));
            _builder.RegisterType<TService>().SingleInstance();
            return this;
        }

        public IServiceCollection AddTransient<TService, TImplementation>()
        {
            ListServiceDescriptor.Add(new ServiceDescriptor(typeof(TImplementation), typeof(TService)));
            _builder.RegisterType<TImplementation>().As<TService>();
            return this;
        }

        public void TryAddSingleton<TService>() where TService : class
        {
            ListServiceDescriptor.Add(new ServiceDescriptor(typeof(TService), typeof(TService)));
            _builder.RegisterType<TService>().SingleInstance();
        }

        void IServiceCollection.TryAddSingleton<TService, TImplementation>()
        {

            ListServiceDescriptor.Add(new ServiceDescriptor(typeof(TImplementation), typeof(TService)));
            _builder.RegisterType<TImplementation>().As<TService>().SingleInstance();
        }

        IServiceCollection IServiceCollection.AddSingleton<TService, TImplementation>()
        {

            ListServiceDescriptor.Add(new ServiceDescriptor(typeof(TImplementation), typeof(TService)));
            _builder.RegisterType<TImplementation>().As<TService>().SingleInstance();
            return this;
        }

        public IServiceCollection AddScopedMuti<TService, TImplementation>()
        {
            ListServiceDescriptor.Add(new ServiceDescriptor(typeof(TImplementation), typeof(TService)));
            _builder.RegisterType<TImplementation>().Named<TService>(typeof(TImplementation).Name);
            return this;
        }


        public IServiceCollection AddSingleton<TService>(Func<IServiceProvider, TService> implementationFactory) where TService : class
        {
            implementationFactory?.Invoke(ServiceProvider);
            return this;
        }

        public IEnumerator<ServiceDescriptor> GetEnumerator()
        {
            foreach (var item in ListServiceDescriptor)
            {
                yield return item;
            }
        }

        public void TryAddEnumerable(ServiceDescriptor descriptor)
        {
            _builder.RegisterType(descriptor.ImplementationType).As(descriptor.ServiceType);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            foreach (var item in ListServiceDescriptor)
            {
                yield return item;
            }
        }
    }

}
