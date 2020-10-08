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
    public class ActorTask : Edge<IActorTask>, IActorTask
    {
        private readonly ActorTaskNetwork _network;
        public static ActorTask CreateInstance(ActorTaskNetwork network, IAgentId actorId, IAgentId taskId)
        {
            return new ActorTask(network, actorId, taskId);
        }
        public ActorTask(ActorTaskNetwork network, IAgentId actorId, IAgentId taskId): base(actorId, taskId, 1)
        {
            _network = network ?? throw new ArgumentNullException(nameof(network));
            _network.Add(this);
        }
        public override object Clone()
        {
            return new ActorTask(_network, Source, Target);
        }
    }
}