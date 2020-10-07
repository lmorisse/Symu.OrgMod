#region Licence

// Description: SymuBiz - SymuOrgMod
// Website: https://symu.org
// Copyright: (c) 2020 laurent morisseau
// License : the program is distributed under the terms of the GNU General Public License

#endregion

#region using directives

using System.Collections.Generic;
using Symu.Common.Interfaces;
using Symu.OrgMod.Edges;

#endregion

namespace Symu.OrgMod.Entities
{
    /// <summary>
    ///     A Task is a well defined procedures or goals of an organization, scheduled or planned activities
    /// </summary>
    public interface ITask : IEntity
    {
        //todo refactor with a TaskKnowledgeNetwork
        /// <summary>
        ///     List of knowledges required to work on this activity
        /// </summary>
        List<IKnowledge> Knowledge { get; }

        /// <summary>
        ///     Add knowledge to an activity
        /// </summary>
        /// <param name="taskKnowledge"></param>
        void AddKnowledge(IEntityKnowledge taskKnowledge);
    }
}