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
    public class ActorBelief : Edge<IActorBelief>, IActorBelief
    {
        private readonly ActorBeliefNetwork _network;
        public ActorBelief(IAgentId actorId, IAgentId beliefId, float weight = 1) : base(actorId, beliefId, weight)
        {
        }
        /// <summary>
        /// Factory
        /// </summary>
        /// <param name="network"></param>
        /// <param name="actorId"></param>
        /// <param name="beliefId"></param>
        /// <param name="weight"></param>
        /// <returns></returns>
        public static ActorBelief CreateInstance(ActorBeliefNetwork network, IAgentId actorId, IAgentId beliefId, float weight = 1)
        {
            return new ActorBelief(network, actorId, beliefId, weight);
        }
        public ActorBelief(ActorBeliefNetwork network, IAgentId actorId, IAgentId beliefId, float weight = 1): base(actorId, beliefId, weight)
        {
            _network = network ?? throw new ArgumentNullException(nameof(network));
            _network.Add(this);
        }

        #region IActorBelief Members

        public virtual float CompareTo(IActorBelief other)
        {
            if (other is ActorBelief test)
            {
                return Weight * test.Weight;
            }

            return 0;
        }

        #endregion

        public override object Clone()
        {
            return new ActorBelief(_network, Source, Target, Weight);
        }
    }
}