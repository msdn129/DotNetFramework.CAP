﻿// Copyright (c) .NET Core Community. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System;

namespace DotNetFramework.CAP.Diagnostics
{
    public interface IErrorEventData
    {
        Exception Exception { get; }
    }
}