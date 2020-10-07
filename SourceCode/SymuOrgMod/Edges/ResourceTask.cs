#region Licence

// Description: SymuBiz - SymuOrgMod
// Website: https://symu.org
// Copyright: (c) 2020 laurent morisseau
// License : the program is distributed under the terms of the GNU General Public License

#endregion

#region using directives

using System;
using Symu.Common.Interfaces;
using Symu.OrgMod.GraphNetworks.TwoModesNetworks;

#endregion

namespace Symu.OrgMod.Edges
{
    /// <summary>
    ///     Resource * Task
    ///     Default implementation of IResourceTask
    ///     Source : Resource
    ///     Target : Task
    /// </summary>
    public class ResourceTask : Edge<IResourceTask>, IResourceTask
    {
        private readonly ResourceTaskNetwork _network;
        public ResourceTask(ResourceTaskNetwork network, IAgentId resourceId, IAgentId taskId) : base(resourceId, taskId, 1)
        {
            _network = network ?? throw new ArgumentNullException(nameof(network));
            _network.Add(this);
        }
        public override object Clone()
        {
            return new ResourceTask(_network, Source, Target);
        }
    }
}