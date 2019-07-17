// Copyright (c) .NET Core Community. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System;
using System.Data.Entity;
using DotNetFramework.CAP.Core.Ioc;
using DotNetFramework.CAP.Processor;
using DotNetFramework.CAP.SqlServer;
using DotNetFramework.CAP.SqlServer.Diagnostics;
using Microsoft.Extensions.DependencyInjection;

// ReSharper disable once CheckNamespace
namespace DotNetFramework.CAP
{
    internal class SqlServerCapOptionsExtension : ICapOptionsExtension
    {
        private readonly Action<SqlServerOptions> _configure;

        public SqlServerCapOptionsExtension(Action<SqlServerOptions> configure)
        {
            _configure = configure;
        }

        public void AddServices(IServiceCollection services)
        {
            services.AddSingleton<CapStorageMarkerService>();
            services.AddSingleton<DiagnosticProcessorObserver>();
            services.AddSingleton<IStorage, SqlServerStorage>();
            services.AddSingleton<IStorageConnection, SqlServerStorageConnection>();

            services.AddScoped<ICapPublisher, SqlServerPublisher>();
            services.AddScoped<ICallbackPublisher, SqlServerPublisher>();

            services.AddTransient<ICollectProcessor, SqlServerCollectProcessor>();
            services.AddTransient<CapTransactionBase, SqlServerCapTransaction>();

            AddSqlServerOptions(services);
        }

        private void AddSqlServerOptions(IServiceCollection services)
        {
            var sqlServerOptions = new SqlServerOptions();

            _configure(sqlServerOptions);

            if (sqlServerOptions.DbContextType != null)
            {
                services.AddSingleton(x =>
                {
                    using (var scope = x.CreateScope())
                    {
                        var provider = scope.ServiceProvider;
                        var dbContext = (DbContext) provider.GetService(sqlServerOptions.DbContextType);
                        sqlServerOptions.ConnectionString = dbContext.Database.Connection.ConnectionString;
                        return sqlServerOptions;
                    }
                });
            }
            else
            {
                services.AddSingleton(sqlServerOptions);
            }
        }
    }
}