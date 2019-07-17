// Copyright (c) .NET Core Community. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System.Threading.Tasks;

namespace DotNetFramework.CAP.Internal
{
    internal interface ICallbackMessageSender
    {
        Task SendAsync(string messageId, string topicName, object bodyObj);
    }
}