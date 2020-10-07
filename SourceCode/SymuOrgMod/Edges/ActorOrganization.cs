﻿#region Licence

// Description: SymuBiz - SymuOrgMod
// Website: https://symu.org
// Copyright: (c) 2020 laurent morisseau
// License : the program is distributed under the terms of the GNU General Public License

#endregion

#region using directives

using System;
using Symu.Common.Interfaces;

#endregion

namespace Symu.OrgMod.Edges
{
    /// <summary>
    ///     Interface to define who is member of a group and how
    ///     By default how is characterized by an allocation of capacity to define part-time membership
    /// </summary>
    public class ActorOrganization : IActorOrganization
    {
        public ActorOrganization(IAgentId actorId, IAgentId organizationId, float weight = 100)
        {
            Source = actorId;
            Target = organizationId;
            Weight = weight;
        }

        public override bool Equals(object obj)
        {
            return obj is ActorOrganization actorOrganization &&
                   Target.Equals(actorOrganization.Target) &&
                   Source.Equals(actorOrganization.Source);
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

        public object Clone()
        {
            return new ActorOrganization(Source, Target, Weight);
        }

        #endregion
    }
}