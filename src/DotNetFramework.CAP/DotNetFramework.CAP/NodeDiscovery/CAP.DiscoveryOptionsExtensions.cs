﻿// Copyright (c) .NET Core Community. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System;
using DotNetFramework.CAP;
using DotNetFramework.CAP.Core.Ioc;
using DotNetFramework.CAP.NodeDiscovery;
using Microsoft.Extensions.DependencyInjection;

namespace DotNetFramework.CAP
{
    internal sealed class DiscoveryOptionsExtension : ICapOptionsExtension
    {
        private readonly Action<DiscoveryOptions> _options;

        public DiscoveryOptionsExtension(Action<DiscoveryOptions> option)
        {
            _options = option;
        }

        public void AddServices(IServiceCollection services)
        {
            var discoveryOptions = new DiscoveryOptions();

            _options?.Invoke(discoveryOptions);
            services.AddSingleton(discoveryOptions);

            services.AddSingleton<IDiscoveryProviderFactory, DiscoveryProviderFactory>();
            services.AddSingleton<IProcessingServer, ConsulProcessingNodeServer>();
            services.AddSingleton(x =>
            {
                var configOptions = x.GetService<DiscoveryOptions>();
                var factory = x.GetService<IDiscoveryProviderFactory>();
                return factory.Create(configOptions);
            });
        }
    }
}

namespace Microsoft.Extensions.DependencyInjection
{
    public static class CapDiscoveryOptionsExtensions
    {
        public static CapOptions UseDiscovery(this CapOptions capOptions)
        {
            return capOptions.UseDiscovery(opt => { });
        }

        public static CapOptions UseDiscovery(this CapOptions capOptions, Action<DiscoveryOptions> options)
        {
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            capOptions.RegisterExtension(new DiscoveryOptionsExtension(options));

            return capOptions;
        }
    }
}