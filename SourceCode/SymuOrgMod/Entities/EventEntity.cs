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
    ///     An Event is occurrences or phenomena that happen
    ///     Default implementation of IEvent
    /// </summary>
    public class EventEntity : Entity<EventEntity>, IEvent
    {
        public const byte Class = ClassIdCollection.Event;
        public static IClassId ClassId => new ClassId(Class);

        public EventEntity()
        {
        }

        public EventEntity(GraphMetaNetwork metaNetwork) : base(metaNetwork, metaNetwork?.Event, ClassId)
        {
        }

        public EventEntity(GraphMetaNetwork metaNetwork, string name) : base(metaNetwork, metaNetwork?.Event, ClassId,
            name)
        {
        }


        public static EventEntity CreateInstance(GraphMetaNetwork metaNetwork)
        {
            return new EventEntity(metaNetwork);
        }

        public static EventEntity CreateInstance(GraphMetaNetwork metaNetwork, string name)
        {
            return new EventEntity(metaNetwork, name);
        }
    }
}