﻿#region Licence

// Description: SymuBiz - SymuOrgMod
// Website: https://symu.org
// Copyright: (c) 2020 laurent morisseau
// License : the program is distributed under the terms of the GNU General Public License

#endregion

#region using directives

using Symu.OrgMod.Edges;

#endregion

namespace Symu.OrgMod.GraphNetworks.TwoModesNetworks
{
    /// <summary>
    ///     Actor * belief network
    ///     Who (Actor) believes what (Belief)
    ///     Source : Actor
    ///     Target : Belief
    /// </summary>
    public class ActorBeliefNetwork : TwoModesNetwork<IActorBelief>
    {
        ///// <summary>
        /////     Get actor's beliefs
        ///// </summary>
        ///// <param name="actorId"></param>
        ///// <returns>null if actorId don't Exists, ActorBelief otherwise</returns>
        //public ActorBeliefs GetActorBeliefs(IAgentId actorId)
        //{
        //    if (!ExistsSource(actorId))
        //    {
        //        return new ActorBeliefs();
        //    }

        //    var edges = EdgesFilteredBySource(actorId);
        //    return ActorBeliefs.CreateInstance(edges);
        //}
    }
}