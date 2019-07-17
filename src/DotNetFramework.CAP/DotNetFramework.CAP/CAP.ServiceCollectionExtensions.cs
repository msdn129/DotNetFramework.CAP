// Copyright (c) .NET Core Community. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using DotNetFramework.CAP;
using DotNetFramework.CAP.Abstractions;
using DotNetFramework.CAP.Internal;
using DotNetFramework.CAP.Processor;
using DotNetFramework.CAP.Processor.States;
using DotNetFramework.CAP.Core.Ioc;
using DotNetFramework.CAP.Core.Logger;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// Contains extension methods to <see cref="IServiceCollection" /> for configuring consistence services.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        internal static IServiceCollection ServiceCollection;

        /// <summary>
        /// Adds and configures the consistence services for the consistency.
        /// </summary>
        /// <param name="services">The services available in the application.</param>
        /// <param name="setupAction">An action to configure the <see cref="CapOptions" />.</param>
        /// <returns>An <see cref="CapBuilder" /> for application services.</returns>
        public static CapBuilder AddCap(this IServiceCollection services, Action<CapOptions> setupAction)
        {
            if (setupAction == null)
            {
                throw new ArgumentNullException(nameof(setupAction));
            }

            ServiceCollection = services;

            services.AddSingleton(typeof(ILogger<>), typeof(Logger<>));
            services.TryAddSingleton<ILogger, Logger>();
            services.TryAddSingleton<ILoggerFactory, LoggerFactory>();

            services.TryAddSingleton<CapMarkerService>();

            //Serializer and model binder
            services.TryAddSingleton<IContentSerializer, JsonContentSerializer>();
            services.TryAddSingleton<IMessagePacker, DefaultMessagePacker>();
            services.TryAddSingleton<IConsumerServiceSelector, DefaultConsumerServiceSelector>();
            services.TryAddSingleton<IModelBinderFactory, ModelBinderFactory>();

            services.TryAddSingleton<ICallbackMessageSender, CallbackMessageSender>();
            services.TryAddSingleton<IConsumerInvokerFactory, ConsumerInvokerFactory>();
            services.TryAddSingleton<MethodMatcherCache>();

            //Processors
            services.TryAddSingleton<IConsumerRegister, ConsumerRegister>();

            services.TryAddEnumerable(ServiceDescriptor.Singleton<IProcessingServer, CapProcessingServer>());
            services.TryAddEnumerable(ServiceDescriptor.Singleton<IProcessingServer, ConsumerRegister>());
            services.TryAddSingleton<IStateChanger, StateChanger>();

            //Queue's message processor
            services.TryAddSingleton<NeedRetryMessageProcessor>();
            services.TryAddSingleton<TransportCheckProcessor>();

            //Sender and Executors   
            services.TryAddSingleton<IDispatcher, Dispatcher>();
            // Warning: IPublishMessageSender need to inject at extension project. 
            services.TryAddSingleton<ISubscriberExecutor, DefaultSubscriberExecutor>();

            //Options and extension service
            var options = new CapOptions();
            setupAction(options);
            foreach (var serviceExtension in options.Extensions)
            {
                serviceExtension.AddServices(services);
            }
            services.AddSingleton(options);

            //Startup and Middleware
            //heng
            services.AddTransient<IBootstrapper, DefaultBootstrapper>();

            //heng
            //services.AddTransient<IStartupFilter, CapStartupFilter>();

            return new CapBuilder(services);
        }
    }
}