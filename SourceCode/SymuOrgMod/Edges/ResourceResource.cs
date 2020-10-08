#region Licence

// Description: SymuBiz - SymuOrgMod
// Website: https://symu.org
// Copyright: (c) 2020 laurent morisseau
// License : the program is distributed under the terms of the GNU General Public License

#endregion

#region using directives

using System;
using Symu.Common.Interfaces;
using Symu.OrgMod.Entities;
using Symu.OrgMod.GraphNetworks.TwoModesNetworks;

#endregion

namespace Symu.OrgMod.Edges
{
    /// <summary>
    ///     Resource * Resource
    ///     Default implementation of IResourceResource
    ///     Source : Resource
    ///     Target : Resource
    /// </summary>
    public class ResourceResource : Edge<IResourceResource>, IResourceResource
    {
        private readonly ResourceResourceNetwork _network;
        public static ResourceResource CreateInstance(ResourceResourceNetwork network, IAgentId source, IAgentId target, IResourceUsage usage, float weight = 100)
        {
            return new ResourceResource(network, source, target, usage, weight);
        }
        public ResourceResource(ResourceResourceNetwork network, IAgentId source, IAgentId target, IResourceUsage usage, float weight = 100): base(source, target, weight)
        {
            Usage = usage;
            // Intentionally before network
            _network = network ?? throw new ArgumentNullException(nameof(network));
            _network.Add(this);
        }

        #region IResourceResource Members

        /// <summary>
        ///     Define how the AgentId is using the resource
        /// </summary>
        public IResourceUsage Usage { get; }


        public bool Equals(IResourceUsage resourceUsage)
        {
            return Usage.Equals(resourceUsage);
        }

        #endregion

        public override bool Equals(object obj)
        {
            return obj is ResourceResource resourceResource &&
                   Target.Equals(resourceResource.Target) &&
                   Source.Equals(resourceResource.Source) &&
                   Usage.Equals(resourceResource.Usage);
        }
        public override object Clone()
        {
            return new ResourceResource(_network, Source, Target, Usage, Weight);
        }
    }
}