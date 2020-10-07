#region Licence

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
    ///     Task * Knowledge network
    ///     What knowledge is necessary for what Task
    ///     Source : Task
    ///     Target : Knowledge
    /// </summary>
    public class TaskKnowledgeNetwork : TwoModesNetwork<IEntityKnowledge>
    {
    }
}