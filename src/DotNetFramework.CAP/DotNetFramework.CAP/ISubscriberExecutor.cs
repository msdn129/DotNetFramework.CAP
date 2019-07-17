// Copyright (c) .NET Core Community. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System.Threading.Tasks;
using DotNetFramework.CAP.Models;

namespace DotNetFramework.CAP
{
    /// <summary>
    /// Consumer execotor
    /// </summary>
    public interface ISubscriberExecutor
    {
        Task<OperateResult> ExecuteAsync(CapReceivedMessage message);
    }
}