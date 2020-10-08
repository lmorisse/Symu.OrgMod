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
    ///     Default implementation of IActorRole
    ///     An actor has a role in a organization
    /// </summary>
    public class ActorRole : Edge<IActorRole>, IActorRole
    {
        private readonly ActorRoleNetwork _network;
        public static ActorRole CreateInstance(ActorRoleNetwork network, IAgentId actorId, IAgentId roleId, IAgentId organizationId)
        {
            return new ActorRole(network, actorId, roleId, organizationId);
        }
        public ActorRole(ActorRoleNetwork network, IAgentId actorId, IAgentId roleId, IAgentId organizationId): base(actorId, roleId,1)
        {
            OrganizationId = organizationId;
            // Intentionally before network
            _network = network ?? throw new ArgumentNullException(nameof(network));
            _network.Add(this);
        }

        #region IActorRole Members

        /// <summary>
        ///     Unique key of the group
        /// </summary>
        public IAgentId OrganizationId { get; set; }


        public bool IsOrganization(IAgentId organizationId)
        {
            return OrganizationId.Equals(organizationId);
        }

        public bool IsOrganization(IClassId organizationClassId)
        {
            return OrganizationId.ClassId.Equals(organizationClassId);
        }

        #endregion

        public override bool Equals(object obj)
        {
            return obj is ActorRole actorRole &&
                   Target.Equals(actorRole.Target) &&
                   Source.Equals(actorRole.Source) &&
                   OrganizationId.Equals(actorRole.OrganizationId);
        }

        /// <summary>
        ///     CHeck that there is a role of roleType for that groupId
        /// </summary>
        /// <param name="roleId"></param>
        /// <param name="organizationId"></param>
        /// <returns></returns>
        public bool HasRoleInOrganization(IAgentId roleId, IAgentId organizationId)
        {
            return EqualsTarget(roleId) && IsOrganization(organizationId);
        }

        public bool HasRoleInOrganization(IAgentId actorId, IAgentId roleId, IAgentId organizationId)
        {
            return EqualsSource(actorId) && HasRoleInOrganization(roleId, organizationId);
        }

        public bool HasRolesInOrganization(IAgentId actorId, IAgentId organizationId)
        {
            return EqualsSource(actorId) && IsOrganization(organizationId);
        }

        public bool IsActorOfOrganizations(IAgentId actorId, IClassId organizationClassId)
        {
            return OrganizationId.Equals(organizationClassId) && EqualsSource(actorId);
        }

        public override object Clone()
        {
            return new ActorRole(_network, Source, Target, OrganizationId);
        }
    }
}