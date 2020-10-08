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
    ///     Interface to define who is member of a group and how
    ///     By default how is characterized by an allocation of capacity to define part-time membership
    /// </summary>
    public class ActorOrganization : Edge<IActorOrganization>, IActorOrganization
    {
        private readonly ActorOrganizationNetwork _network;
        /// <summary>
        /// Factory
        /// </summary>
        /// <param name="network"></param>
        /// <param name="actorId"></param>
        /// <param name="organizationId"></param>
        /// <param name="weight"></param>
        /// <returns></returns>
        public static ActorOrganization CreateInstance(ActorOrganizationNetwork network, IAgentId actorId, IAgentId organizationId, float weight = 100)
        {
            return new ActorOrganization(network, actorId, organizationId, weight);
        }
        public ActorOrganization(ActorOrganizationNetwork network, IAgentId actorId, IAgentId organizationId, float weight = 100): base(actorId, organizationId, weight)
        {
            _network = network ?? throw new ArgumentNullException(nameof(network));
            _network.Add(this);
        }

        public override object Clone()
        {
            return new ActorOrganization(_network, Source, Target, Weight);
        }
    }
}