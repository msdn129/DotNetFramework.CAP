// Copyright (c) .NET Core Community. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using RabbitMQ.Client;

namespace DotNetFramework.CAP.RabbitMQ
{
    public interface IConnectionChannelPool
    {
        string HostAddress { get; }

        string Exchange { get; }

        IConnection GetConnection();

        IModel Rent();

        bool Return(IModel context);
    }
}