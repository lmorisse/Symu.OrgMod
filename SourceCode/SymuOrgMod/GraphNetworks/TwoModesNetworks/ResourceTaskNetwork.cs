#region Licence

// Description: SymuBiz - SymuDNA
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
    ///     Resource * task network
    ///     What resources can do what task
    ///     Source : Resource
    ///     Target : Task
    /// </summary>
    public class ResourceTaskNetwork : TwoModesNetwork<IResourceTask>
    {
    }
}