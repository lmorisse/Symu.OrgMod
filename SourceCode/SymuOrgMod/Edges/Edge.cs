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
using static Symu.Common.Constants;

#endregion

namespace Symu.OrgMod.Edges
{
    /// <summary>
    ///     Base class for IEdge
    /// </summary>
    public class Edge<TEdge> : IEdge where TEdge : IEdge
    {
        public Edge(){}
        public Edge(IAgentId source, IAgentId target, float weight)
        {
            Source = source ?? throw new ArgumentNullException(nameof(source));
            Target = target ?? throw new ArgumentNullException(nameof(target));
            Weight = weight;
            NormalizedWeight = weight;
        }
        public override bool Equals(object obj)
        {
            return obj is Edge<TEdge> edge &&
                   Target.Equals(edge.Target) &&
                   Source.Equals(edge.Source);
        }

        #region Interface IEdge

        /// <summary>
        ///     Number of interactions between the two agents
        /// </summary>
        public virtual float Weight { get; set; }

        /// <summary>
        ///     Normalized weight computed by the network via the NormalizeWeights method
        /// </summary>
        public float NormalizedWeight { get; set; }

        public bool Equals(IAgentId source, IAgentId target)
        {
            return EqualsTarget(target) && EqualsSource(source);
        }

        public bool EqualsSource(IAgentId source)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            return source.Equals(Source);
        }

        public bool EqualsTarget(IAgentId target)
        {
            if (target == null)
            {
                throw new ArgumentNullException(nameof(target));
            }

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

        public virtual object Clone()
        {
            return new Edge<TEdge>(Source, Target, Weight);
        }

        #endregion
    }
}