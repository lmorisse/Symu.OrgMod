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
    ///     A knowledge is cognitive capabilities and skills
    ///     Default implementation of IKnowledge
    /// </summary>
    public class KnowledgeEntity : Entity<KnowledgeEntity>, IKnowledge
    {
        public const byte Class = ClassIdCollection.Knowledge;
        public static IClassId ClassId => new ClassId(Class);

        public KnowledgeEntity()
        {
        }

        public KnowledgeEntity(GraphMetaNetwork metaNetwork) : base(metaNetwork, metaNetwork?.Knowledge, ClassId)
        {
        }

        public KnowledgeEntity(GraphMetaNetwork metaNetwork, string name) : base(metaNetwork, metaNetwork?.Knowledge,
            ClassId, name)
        {
        }

        public static KnowledgeEntity CreateInstance(GraphMetaNetwork metaNetwork)
        {
            return new KnowledgeEntity(metaNetwork);
        }

        public static KnowledgeEntity CreateInstance(GraphMetaNetwork metaNetwork, string name)
        {
            return new KnowledgeEntity(metaNetwork, name);
        }

        #region IKnowledge Members

        public override void Remove()
        {
            base.Remove();
            MetaNetwork.ActorKnowledge.RemoveTarget(EntityId);
            MetaNetwork.TaskKnowledge.RemoveTarget(EntityId);
            MetaNetwork.ResourceKnowledge.RemoveTarget(EntityId);
        }

        #endregion

        /// <summary>
        ///     Copy the metaNetwork, the two modes networks where the entity exists
        /// </summary>
        /// <param name="entityId"></param>
        public override void CopyMetaNetworkTo(IAgentId entityId)
        {
            MetaNetwork.ActorKnowledge.CopyToFromTarget(EntityId, entityId);
            MetaNetwork.TaskKnowledge.CopyToFromTarget(EntityId, entityId);
            MetaNetwork.ResourceKnowledge.CopyToFromTarget(EntityId, entityId);
        }
    }
}