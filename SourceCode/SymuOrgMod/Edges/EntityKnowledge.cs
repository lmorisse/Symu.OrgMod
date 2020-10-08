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
    ///     Describe an area of knowledge
    /// </summary>
    public class EntityKnowledge : Edge<IEntityKnowledge>, IEntityKnowledge
    {
        private readonly TwoModesNetwork<IEntityKnowledge> _network;
        /// <summary>
        /// Constructor that doesn't store the instance in the network
        /// </summary>
        /// <param name="actorId"></param>
        /// <param name="knowledgeId"></param>
        /// <param name="weight"></param>
        public EntityKnowledge(IAgentId actorId, IAgentId knowledgeId, float weight = 1) : base(actorId, knowledgeId, weight)
        {
        }
        /// <summary>
        /// Factory
        /// </summary>
        /// <param name="network"></param>
        /// <param name="actorId"></param>
        /// <param name="knowledgeId"></param>
        /// <param name="weight"></param>
        /// <returns></returns>
        public static EntityKnowledge CreateInstance(TwoModesNetwork<IEntityKnowledge> network, IAgentId actorId, IAgentId knowledgeId, float weight = 1)
        {
            return new EntityKnowledge(network, actorId, knowledgeId, weight);
        }
        /// <summary>
        /// Constructor that store the instance in the network
        /// </summary>
        /// <param name="network"></param>
        /// <param name="actorId"></param>
        /// <param name="knowledgeId"></param>
        /// <param name="weight"></param>
        public EntityKnowledge(TwoModesNetwork<IEntityKnowledge> network, IAgentId actorId, IAgentId knowledgeId, float weight = 1): base (actorId, knowledgeId, weight)
        {
            _network = network ?? throw new ArgumentNullException(nameof(network));
            _network.Add(this);
        }

        #region IEntityKnowledge Members

        public float CompareTo(IEntityKnowledge other)
        {
            if (other is EntityKnowledge test)
            {
                return NormalizedWeight * test.NormalizedWeight;
            }

            return 0;
        }

        #endregion
        public override object Clone()
        {
            return new EntityKnowledge(_network, Source, Target, Weight);
        }
    }
}