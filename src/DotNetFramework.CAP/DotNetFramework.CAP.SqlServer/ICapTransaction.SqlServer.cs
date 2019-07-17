// Copyright (c) .NET Core Community. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using DotNetFramework.CAP.Internal;
using DotNetFramework.CAP.Models;
using DotNetFramework.CAP.SqlServer.Diagnostics;

using Microsoft.Extensions.DependencyInjection;
using IServiceProvider = DotNetFramework.CAP.Core.Ioc.IServiceProvider;

// ReSharper disable once CheckNamespace
namespace DotNetFramework.CAP
{
    public class SqlServerCapTransaction : CapTransactionBase
    {
        private readonly DbContext _dbContext;
        private readonly DiagnosticProcessorObserver _diagnosticProcessor;

        public SqlServerCapTransaction(
            IDispatcher dispatcher,
            SqlServerOptions sqlServerOptions,
            IServiceProvider serviceProvider) : base(dispatcher)
        {
            if (sqlServerOptions.DbContextType != null)
            {
                _dbContext = serviceProvider.GetService(sqlServerOptions.DbContextType) as DbContext;
            }

            _diagnosticProcessor = serviceProvider.GetRequiredService<DiagnosticProcessorObserver>();
        }

        protected override void AddToSent(CapPublishedMessage msg)
        {
            if (DbTransaction is NoopTransaction)
            {
                base.AddToSent(msg);
                return;
            }

            base.AddToSent(msg);
            return;
            //heng
            //var dbTransaction = DbTransaction as IDbTransaction;
            //if (dbTransaction == null)
            //{
            //    if (DbTransaction is IDbContextTransaction dbContextTransaction)
            //    {
            //        dbTransaction = dbContextTransaction.GetDbTransaction();
            //    }

            //    if (dbTransaction == null)
            //    {
            //        throw new ArgumentNullException(nameof(DbTransaction));
            //    }
            //}

            //var transactionKey = ((SqlConnection) dbTransaction.Connection).ClientConnectionId;
            //if (_diagnosticProcessor.BufferList.TryGetValue(transactionKey, out var list))
            //{
            //    list.Add(msg);
            //}
            //else
            //{
            //    var msgList = new List<CapPublishedMessage>(1) {msg};
            //    _diagnosticProcessor.BufferList.TryAdd(transactionKey, msgList);
            //}
        }

        public override void Commit()
        {
            switch (DbTransaction)
            {
                case NoopTransaction _:
                    Flush();
                    break;
                case IDbTransaction dbTransaction:
                    dbTransaction.Commit();
                    Flush();
                    break;
                case DbContextTransaction dbContextTransaction:
                    _dbContext?.SaveChanges();
                    dbContextTransaction.Commit();
                    Flush();
                    break;
            }
        }

        public override void Rollback()
        {
            switch (DbTransaction)
            {
                case IDbTransaction dbTransaction:
                    dbTransaction.Rollback();
                    break;
                case DbContextTransaction dbContextTransaction:
                    dbContextTransaction.Rollback();
                    break;
            }
        }

        public override void Dispose()
        {
            switch (DbTransaction)
            {
                case IDbTransaction dbTransaction:
                    dbTransaction.Dispose();
                    break;
                case DbContextTransaction dbContextTransaction:
                    dbContextTransaction.Dispose();
                    break;
            }
        }
    }

    public static class CapTransactionExtensions
    {
        public static ICapTransaction Begin(this ICapTransaction transaction,
            IDbTransaction dbTransaction, bool autoCommit = false)
        {
            transaction.DbTransaction = dbTransaction;
            transaction.AutoCommit = autoCommit;

            return transaction;
        }

        public static ICapTransaction Begin(this ICapTransaction transaction,
            DbContextTransaction dbTransaction, bool autoCommit = false)
        {
            transaction.DbTransaction = dbTransaction;
            transaction.AutoCommit = autoCommit;

            return transaction;
        }

        /// <summary>
        /// Start the CAP transaction
        /// </summary>
        /// <param name="dbConnection">The <see cref="IDbConnection" />.</param>
        /// <param name="publisher">The <see cref="ICapPublisher" />.</param>
        /// <param name="autoCommit">Whether the transaction is automatically committed when the message is published</param>
        /// <returns>The <see cref="ICapTransaction" /> object.</returns>
        public static IDbTransaction BeginTransaction(this IDbConnection dbConnection,
            ICapPublisher publisher, bool autoCommit = false)
        {
            if (dbConnection.State == ConnectionState.Closed)
            {
                dbConnection.Open();
            }

            var dbTransaction = dbConnection.BeginTransaction();
            var capTransaction = publisher.Transaction.Begin(dbTransaction, autoCommit);
            return (IDbTransaction) capTransaction.DbTransaction;
        }

        /// <summary>
        /// Start the CAP transaction
        /// </summary>
        /// <param name="database">The <see cref="DatabaseFacade" />.</param>
        /// <param name="publisher">The <see cref="ICapPublisher" />.</param>
        /// <param name="autoCommit">Whether the transaction is automatically committed when the message is published</param>
        /// <returns>The <see cref="IDbContextTransaction" /> of EF dbcontext transaction object.</returns>
        public static DbContextTransaction BeginTransaction(this Database database,
            ICapPublisher publisher, bool autoCommit = false)
        {
            var trans = database.BeginTransaction();
            var capTrans = publisher.Transaction.Begin(trans, autoCommit);
            return trans;
        }
    }
}