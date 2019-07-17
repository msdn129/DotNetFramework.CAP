// Copyright (c) .NET Core Community. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using DotNetFramework.CAP.Models;

namespace DotNetFramework.CAP
{
    public interface IDispatcher
    {
        void EnqueueToPublish(CapPublishedMessage message);

        void EnqueueToExecute(CapReceivedMessage message);
    }
}