﻿#region Licence

// Description: SymuBiz - SymuOrgMod
// Website: https://symu.org
// Copyright: (c) 2020 laurent morisseau
// License : the program is distributed under the terms of the GNU General Public License

#endregion

#region using directives

using System.Collections.Generic;
using System.Linq;
using Symu.Common.Interfaces;
using Symu.OrgMod.GraphNetworks;

#endregion

namespace Symu.OrgMod.Entities
{
    /// <summary>
    ///     Organization is a group of people with a same goal or mission
    ///     Default implementation of IOrganization
    /// </summary>
    /// <example>team, community of practices, ...</example>
    public class OrganizationEntity : Entity<OrganizationEntity>, IOrganization
    {
        public const byte Class = ClassIdCollection.Organization;
        public static IClassId ClassId => new ClassId(Class);

        public OrganizationEntity()
        {
        }

        public OrganizationEntity(GraphMetaNetwork metaNetwork) : base(metaNetwork, metaNetwork?.Organization, Class)
        {
        }

        public OrganizationEntity(GraphMetaNetwork metaNetwork, byte classId) : base(metaNetwork,
            metaNetwork?.Organization, classId)
        {
        }

        public OrganizationEntity(GraphMetaNetwork metaNetwork, byte classId, string name) : base(metaNetwork,
            metaNetwork?.Organization, classId, name)
        {
        }
        public static OrganizationEntity CreateInstance(GraphMetaNetwork metaNetwork)
        {
            return new OrganizationEntity(metaNetwork);
        }

        public static OrganizationEntity CreateInstance(GraphMetaNetwork metaNetwork, byte classId)
        {
            return new OrganizationEntity(metaNetwork, classId);
        }

        public static OrganizationEntity CreateInstance(GraphMetaNetwork metaNetwork, byte classId, string name)
        {
            return new OrganizationEntity(metaNetwork, classId, name);
        }

        #region IOrganization Members

        public override void Remove()
        {
            base.Remove();
            MetaNetwork.ActorOrganization.RemoveTarget(EntityId);
            MetaNetwork.OrganizationResource.RemoveSource(EntityId);
        }

        #endregion

        /// <summary>
        ///     Copy the metaNetwork, the two modes networks where the entity exists
        /// </summary>
        /// <param name="entityId"></param>
        public override void CopyMetaNetworkTo(IAgentId entityId)
        {
            MetaNetwork.ActorOrganization.CopyToFromTarget(EntityId, entityId);
            MetaNetwork.OrganizationResource.CopyToFromSource(EntityId, entityId);
        }

        #region Actor Management

        /// <summary>
        ///     Count of the actorsIds of the organization
        /// </summary>
        public byte ActorsCount =>
            (byte) MetaNetwork.ActorOrganization.SourcesFilteredByTargetAndSourceClassIdCount(EntityId,
                ActorEntity.ClassId);

        /// <summary>
        ///     List of the actorsIds of the organization
        /// </summary>
        public IEnumerable<IAgentId> ActorIds =>
            MetaNetwork.ActorOrganization.SourcesFilteredByTargetAndSourceClassId(EntityId, ActorEntity.ClassId);

        /// <summary>
        ///     List of the actors of the organization
        /// </summary>
        public IEnumerable<IActor> Actors =>
            ActorIds.Select(actorId => MetaNetwork.Actor.GetEntity<ActorEntity>(actorId));

        /// <summary>
        ///     Get the first actor fo the group
        /// </summary>
        /// <returns></returns>
        public IAgentId GetFirstActorId => ActorsCount == 0 ? new AgentId() : ActorIds.First();

        #endregion
    }
}