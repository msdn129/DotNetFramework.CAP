// Copyright (c) .NET Core Community. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using DotNetFramework.CAP.Models;

namespace DotNetFramework.CAP.Processor.States
{
    public interface IStateChanger
    {
        void ChangeState(CapPublishedMessage message, IState state, IStorageTransaction transaction);

        void ChangeState(CapReceivedMessage message, IState state, IStorageTransaction transaction);
    }
}