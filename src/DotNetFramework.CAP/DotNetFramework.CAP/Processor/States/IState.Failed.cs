﻿// Copyright (c) .NET Core Community. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System;
using DotNetFramework.CAP.Models;

namespace DotNetFramework.CAP.Processor.States
{
    public class FailedState : IState
    {
        public const string StateName = "Failed";

        public TimeSpan? ExpiresAfter => TimeSpan.FromDays(15);

        public string Name => StateName;

        public void Apply(CapPublishedMessage message, IStorageTransaction transaction)
        {
        }

        public void Apply(CapReceivedMessage message, IStorageTransaction transaction)
        {
        }
    }
}