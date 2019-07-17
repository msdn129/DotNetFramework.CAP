// Copyright (c) .NET Core Community. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using DotNetFramework.CAP.Core.Ioc;
using Microsoft.Extensions.DependencyInjection;

namespace DotNetFramework.CAP
{
    /// <summary>
    /// Cap options extension
    /// </summary>
    public interface ICapOptionsExtension
    {
        /// <summary>
        /// Registered child service.
        /// </summary>
        /// <param name="services">add service to the <see cref="IServiceCollection" /></param>
        void AddServices(IServiceCollection services);
    }
}