#region Licence

// Description: SymuBiz - SymuOrgModTests
// Website: https://symu.org
// Copyright: (c) 2020 laurent morisseau
// License : the program is distributed under the terms of the GNU General Public License

#endregion

#region using directives

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Symu.Common.Interfaces;
using Symu.OrgMod.Edges;
using Symu.OrgMod.GraphNetworks;

#endregion

namespace SymuOrgModTests.Edges
{
    [TestClass]
    public class ActorRoleTests
    {
        private readonly GraphMetaNetwork _metaNetwork = new GraphMetaNetwork();
        private readonly IAgentId _actorId = new AgentId(1, 1);
        private readonly IAgentId _organizationId = new AgentId(3, 3);
        private readonly IAgentId _organizationId1 = new AgentId(4, 3);
        private readonly IAgentId _roleId = new AgentId(2, 2);

        private ActorRole _edge;

        [TestInitialize]
        public void Initialize()
        {
            _edge = new ActorRole(_metaNetwork.ActorRole, _actorId, _roleId, _organizationId);
        }

        [TestMethod]
        public void IsOrganizationTest()
        {
            Assert.IsTrue(_edge.IsOrganization(_organizationId));
            Assert.IsFalse(_edge.IsOrganization(_organizationId1));
        }

        [TestMethod]
        public void IsOrganizationTest1()
        {
            Assert.IsTrue(_edge.IsOrganization(_organizationId.ClassId));
            Assert.IsTrue(_edge.IsOrganization(_organizationId1.ClassId));
        }

        [TestMethod]
        public void HasRoleInOrganizationTest()
        {
            Assert.IsTrue(_edge.HasRoleInOrganization(_roleId, _organizationId));
            Assert.IsFalse(_edge.HasRoleInOrganization(_roleId, _organizationId1));
        }

        [TestMethod]
        public void HasRoleInOrganizationTest1()
        {
            Assert.IsTrue(_edge.HasRoleInOrganization(_actorId, _roleId, _organizationId));
            Assert.IsFalse(_edge.HasRoleInOrganization(_actorId, _roleId, _organizationId1));
        }

        [TestMethod]
        public void HasRolesInOrganizationTest()
        {
            Assert.IsTrue(_edge.HasRolesInOrganization(_actorId, _organizationId));
            Assert.IsFalse(_edge.HasRolesInOrganization(_actorId, _organizationId1));
        }

        [TestMethod]
        public void IsActorOfOrganizationsTest()
        {
            Assert.IsTrue(_edge.IsActorOfOrganizations(_actorId, _organizationId.ClassId));
            Assert.IsFalse(_edge.IsActorOfOrganizations(_actorId, _roleId.ClassId));
        }

        [TestMethod]
        public void CloneTest()
        {
            var clone = _edge.Clone();
            Assert.AreEqual(_edge, clone);
        }
    }
}