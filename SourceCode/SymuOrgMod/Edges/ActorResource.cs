#region Licence

// Description: SymuBiz - SymuOrgMod
// Website: https://symu.org
// Copyright: (c) 2020 laurent morisseau
// License : the program is distributed under the terms of the GNU General Public License

#endregion

#region using directives

using System;
using Symu.Common.Interfaces;
using Symu.OrgMod.Entities;
using Symu.OrgMod.GraphNetworks.TwoModesNetworks;

#endregion

namespace Symu.OrgMod.Edges
{
    /// <summary>
    ///     Class for tests
    /// </summary>
    public class ActorResource : Edge<IActorResource>, IActorResource
    {
        private readonly ActorResourceNetwork _network;
        public ActorResource(IAgentId actorId, IAgentId resourceId, IResourceUsage resourceUsage,
            float weight = 100) : base(actorId, resourceId, weight)
        {
            Usage = resourceUsage;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="network"></param>
        /// <param name="actorId"></param>
        /// <param name="resourceId"></param>
        /// <param name="resourceUsage"></param>
        /// <param name="weight">Allocation of capacity per resource - Ranging from [0; 100]</param>
        public ActorResource(ActorResourceNetwork network, IAgentId actorId, IAgentId resourceId, IResourceUsage resourceUsage,
            float weight = 100) : base(actorId, resourceId, weight)
        {
            Usage = resourceUsage;
            // intentionally before network
            _network = network ?? throw new ArgumentNullException(nameof(network));
            _network.Add(this);
        }

        #region IActorResource Members

        /// <summary>
        ///     Define how the AgentId is using the resource
        /// </summary>
        public IResourceUsage Usage { get; }

        public bool Equals(IResourceUsage resourceUsage)
        {
            return Usage.Equals(resourceUsage);
        }

        #endregion

        public override bool Equals(object obj)
        {
            return obj is ActorResource agentResource &&
                   Target.Equals(agentResource.Target) &&
                   Source.Equals(agentResource.Source) &&
                   Usage.Equals(agentResource.Usage);
        }

        public override object Clone()
        {
            return new ActorResource(_network, Source, Target, Usage, Weight);
        }
    }
}