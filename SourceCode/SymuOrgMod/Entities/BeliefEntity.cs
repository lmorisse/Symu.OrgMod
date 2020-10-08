#region Licence

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
    ///     Beliefs are any form of religion or other persuasion.
    ///     Default implementation of IBelief
    /// </summary>
    public class BeliefEntity : Entity<BeliefEntity>, IBelief
    {
        public const byte Class = ClassIdCollection.Belief;
        public static IClassId ClassId => new ClassId(Class);

        public BeliefEntity()
        {
        }

        public BeliefEntity(GraphMetaNetwork metaNetwork) : base(metaNetwork, metaNetwork?.Belief, ClassId)
        {
        }

        public BeliefEntity(GraphMetaNetwork metaNetwork, string name) : base(metaNetwork, metaNetwork?.Belief, ClassId,
            name)
        {
        }

        public static BeliefEntity CreateInstance(GraphMetaNetwork metaNetwork)
        {
            return new BeliefEntity(metaNetwork);
        }

        public static BeliefEntity CreateInstance(GraphMetaNetwork metaNetwork, string name)
        {
            return new BeliefEntity(metaNetwork, name);
        }

        #region IBelief Members

        public override void Remove()
        {
            base.Remove();
            MetaNetwork.ActorBelief.RemoveTarget(EntityId);
        }

        #endregion

        /// <summary>
        ///     Copy the metaNetwork, the two modes networks where the entity exists
        /// </summary>
        /// <param name="entityId"></param>
        public override void CopyMetaNetworkTo(IAgentId entityId)
        {
            MetaNetwork.ActorBelief.CopyToFromTarget(EntityId, entityId);
        }
    }
}