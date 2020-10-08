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
    ///     Interface to define who is member of a group and how
    ///     By default how is characterized by an allocation of capacity to define part-time membership
    /// </summary>
    public class OrganizationResource : Edge<IOrganizationResource>, IOrganizationResource
    {
        private readonly OrganizationResourceNetwork _network;
        /// <summary>
        /// Factory
        /// </summary>
        /// <param name="network"></param>
        /// <param name="organizationId"></param>
        /// <param name="resourceId"></param>
        /// <param name="usage"></param>
        /// <param name="weight"></param>
        /// <returns></returns>
        public static OrganizationResource CreateInstance(OrganizationResourceNetwork network, IAgentId organizationId, IAgentId resourceId, IResourceUsage usage,
            float weight = 100)
        {
            return new OrganizationResource(network, organizationId, resourceId, usage, weight);
        }
        public OrganizationResource(OrganizationResourceNetwork network, IAgentId organizationId, IAgentId resourceId, IResourceUsage usage,
            float weight = 100): base(organizationId, resourceId, weight)
        {
            Usage = usage;
            // Intentionally before network
            _network = network ?? throw new ArgumentNullException(nameof(network));
            _network.Add(this);
        }

        #region IOrganizationResource Members

        public IResourceUsage Usage { get; }

        public bool Equals(IResourceUsage resourceUsage)
        {
            return Usage.Equals(resourceUsage);
        }

        #endregion


        public override object Clone()
        {
            return new OrganizationResource(_network, Source, Target, Usage, Weight);
        }


        public override bool Equals(object obj)
        {
            return obj is OrganizationResource organizationResource &&
                   Target.Equals(organizationResource.Target) &&
                   Source.Equals(organizationResource.Source) &&
                   Usage.Equals(organizationResource.Usage);
        }
    }
}