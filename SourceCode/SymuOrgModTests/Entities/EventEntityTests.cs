#region Licence

// Description: SymuBiz - SymuOrgModTests
// Website: https://symu.org
// Copyright: (c) 2020 laurent morisseau
// License : the program is distributed under the terms of the GNU General Public License

#endregion

#region using directives

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Symu.OrgMod.Entities;
using Symu.OrgMod.GraphNetworks;

#endregion

namespace SymuOrgModTests.Entities
{
    [TestClass]
    public class EventEntityTests
    {
        private readonly GraphMetaNetwork _metaNetwork = new GraphMetaNetwork();
        private IEvent _entity;

        [TestInitialize]
        public void Initialize()
        {
            _entity = new EventEntity(_metaNetwork);
        }

        [TestMethod]
        public void RemoveTest()
        {
            _entity.Remove();

            Assert.IsFalse(_metaNetwork.Event.Any());
        }
    }
}