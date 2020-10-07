﻿#region Licence

// Description: SymuBiz - SymuDNA
// Website: https://symu.org
// Copyright: (c) 2020 laurent morisseau
// License : the program is distributed under the terms of the GNU General Public License

#endregion

#region using directives

using Symu.Common.Interfaces;

#endregion

namespace Symu.OrgMod.Edges
{
    public class ActorTask : IActorTask
    {
        public ActorTask(IAgentId actorId, IAgentId taskId)
        {
            Source = actorId;
            Target = taskId;
            Weight = 1;
        }

        public override bool Equals(object obj)
        {
            return obj is ActorTask actorTask &&
                   Target.Equals(actorTask.Target) &&
                   Source.Equals(actorTask.Source);
        }

        #region Interface IEdge

        /// <summary>
        ///     Number of interactions between the two agents
        /// </summary>
        public float Weight { get; set; }
        /// <summary>
        ///     Normalized weight computed by the network via the NormalizeWeights method
        /// </summary>
        public float NormalizedWeight { get; set; }

        public bool EqualsSource(IAgentId source)
        {
            return source.Equals(Source);
        }

        public bool EqualsTarget(IAgentId target)
        {
            return target.Equals(Target);
        }

        /// <summary>
        ///     Unique key of the agent with the smallest key
        /// </summary>
        public IAgentId Source { get; set; }

        /// <summary>
        ///     Unique key of the agent with the highest key
        /// </summary>
        public IAgentId Target { get; set; }
        public object Clone()
        {
            return new ActorTask(Source, Target);
        }
        #endregion
    }
}