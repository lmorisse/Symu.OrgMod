#region Licence

// Description: SymuBiz - SymuOrgModTests
// Website: https://symu.org
// Copyright: (c) 2020 laurent morisseau
// License : the program is distributed under the terms of the GNU General Public License

#endregion

#region using directives

using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Symu.Common.Interfaces;
using Symu.OrgMod.Edges;
using Symu.OrgMod.Entities;
using Symu.OrgMod.GraphNetworks.TwoModesNetworks;

#endregion

namespace SymuOrgModTests.GraphNetworks.TwoModesNetworks
{
    [TestClass]
    public class ResourceResourceNetworkTests
    {
        private readonly ResourceResourceNetwork _network = new ResourceResourceNetwork();
        private readonly IAgentId _resourceId = new AgentId(1, 1);
        private readonly IAgentId _resourceId1 = new AgentId(2, 1);
        private readonly IResourceUsage _usage = new ResourceUsage(1);


        [TestInitialize]
        public void Initialize()
        {
        }

        [TestMethod]
        public void GetWeightTest()
        {
            Assert.AreEqual(0, _network.GetWeight(_resourceId, _resourceId1, _usage));
            ResourceResource.CreateInstance(_network, _resourceId, _resourceId1, _usage);
            Assert.AreEqual(100, _network.GetWeight(_resourceId, _resourceId1, _usage));
        }

        [TestMethod]
        public void EdgeTest()
        {
            Assert.IsNull(_network.Edge(_resourceId, _resourceId1, _usage));
            var edge = new ResourceResource(_network, _resourceId, _resourceId1, _usage);
            Assert.AreEqual(edge, _network.Edge(_resourceId, _resourceId1, _usage));
        }

        [TestMethod]
        public void HasResourceTest()
        {
            Assert.IsFalse(_network.HasResource(_resourceId, _resourceId1, _usage));
            ResourceResource.CreateInstance(_network, _resourceId, _resourceId1, _usage);
            Assert.IsTrue(_network.HasResource(_resourceId, _resourceId1, _usage));
        }

        [TestMethod]
        public void TargetsFromSourceTest()
        {
            Assert.AreEqual(0, _network.TargetsFromSource(_resourceId, _usage).Count());
            ResourceResource.CreateInstance(_network, _resourceId, _resourceId1, _usage);
            Assert.AreEqual(1, _network.TargetsFromSource(_resourceId, _usage).Count());
        }

        [TestMethod]
        public void RemoveResourceTest()
        {
            //Remove empty network
            _network.RemoveResource(_resourceId);
            ResourceResource.CreateInstance(_network, _resourceId, _resourceId1, _usage);
            _network.RemoveResource(_resourceId);
            Assert.IsFalse(_network.Any());
        }
    }
}