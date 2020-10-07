﻿#region Licence

// Description: SymuBiz - SymuOrgMod
// Website: https://symu.org
// Copyright: (c) 2020 laurent morisseau
// License : the program is distributed under the terms of the GNU General Public License

#endregion

#region using directives

using Symu.Common.Interfaces;
using Symu.OrgMod.GraphNetworks;

#endregion

namespace Symu.OrgMod.Entities
{
    /// <summary>
    ///     A role describe functions of actors
    ///     Default implementation of IRole
    /// </summary>
    public class RoleEntity : Entity<RoleEntity>, IRole
    {
        public const byte Class = ClassIdCollection.Role;

        public RoleEntity()
        {
        }

        public RoleEntity(GraphMetaNetwork metaNetwork) : base(metaNetwork, metaNetwork?.Role, Class)
        {
        }

        public RoleEntity(GraphMetaNetwork metaNetwork, string name) : base(metaNetwork, metaNetwork?.Role, Class, name)
        {
        }

        public static IClassId ClassId => new ClassId(Class);

        #region IRole Members

        public override void Remove()
        {
            base.Remove();
            MetaNetwork.ActorRole.RemoveTarget(EntityId);
        }

        #endregion

        /// <summary>
        ///     Copy the metaNetwork, the two modes networks where the entity exists
        /// </summary>
        /// <param name="entityId"></param>
        public override void CopyMetaNetworkTo(IAgentId entityId)
        {
            MetaNetwork.ActorRole.CopyToFromTarget(EntityId, entityId);
        }
    }
}