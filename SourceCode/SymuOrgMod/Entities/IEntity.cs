#region Licence

// Description: SymuBiz - SymuOrgMod
// Website: https://symu.org
// Copyright: (c) 2020 laurent morisseau
// License : the program is distributed under the terms of the GNU General Public License

#endregion

#region using directives

using System;
using Symu.Common.Interfaces;
using Symu.OrgMod.GraphNetworks;

#endregion

namespace Symu.OrgMod.Entities
{
    /// <summary>
    ///     Base interface for entities
    ///     Entities are used in One Mode Networks
    /// </summary>
    public interface IEntity : ICloneable
    {
        IAgentId EntityId { get; set; }
        string Name { get; set; }

        /// <summary>
        ///     Set the metaNetwork embedded in the Entity and the OneModeNetwork
        /// </summary>
        /// <param name="metaNetwork"></param>
        /// <param name="network"></param>
        void SetMetaNetwork(GraphMetaNetwork metaNetwork, OneModeNetwork network);

        /// <summary>
        ///     Triggered when the entity is removed
        ///     At least must clean the MetaNetwork
        /// </summary>
        void Remove();
    }
}