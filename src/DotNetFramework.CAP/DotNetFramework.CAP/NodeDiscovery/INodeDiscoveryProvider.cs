// Copyright (c) .NET Core Community. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Threading.Tasks;

namespace DotNetFramework.CAP.NodeDiscovery
{
    public interface INodeDiscoveryProvider
    {
        Task<IList<Node>> GetNodes();

        Task RegisterNode();
    }
}