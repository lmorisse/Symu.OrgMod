﻿#region Licence

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
    public class ActorRoleNetworkTests
    {
        private readonly IAgentId _actorId = new AgentId(3, ActorEntity.ClassId);
        private readonly IClassId _classId0 = new ClassId(0);
        private readonly ActorRoleNetwork _network = new ActorRoleNetwork();
        private readonly IAgentId _organizationId = new AgentId(1, OrganizationEntity.ClassId);
        private readonly IAgentId _organizationId1 = new AgentId(2, OrganizationEntity.ClassId);
        private readonly IAgentId _roleId = new AgentId(4, RoleEntity.ClassId);
        
        [TestInitialize]
        public void Initialize()
        {
        }

        [TestMethod]
        public void RemoveSourceTest()
        {
            ActorRole.CreateInstance(_network, _actorId, _roleId, _organizationId);
            ActorRole.CreateInstance(_network, _actorId, _roleId, _organizationId1);
            _network.RemoveSource(_actorId, _organizationId);
            Assert.IsFalse(_network.HasARoleIn(_actorId, _organizationId));
            Assert.IsTrue(_network.HasARoleIn(_actorId, _organizationId1));
        }

        [TestMethod]
        public void IsActorOfOrganizationIdsTest()
        {
            Assert.AreEqual(0, _network.IsActorOfOrganizationIds(_actorId, _classId0).Count());
            ActorRole.CreateInstance(_network, _actorId, _roleId, _organizationId);
            Assert.AreEqual(0, _network.IsActorOfOrganizationIds(_actorId, _classId0).Count());
            Assert.AreEqual(1, _network.IsActorOfOrganizationIds(_actorId, _organizationId.ClassId).Count());
            Assert.AreEqual(_organizationId.ClassId,
                _network.IsActorOfOrganizationIds(_actorId, _organizationId.ClassId).Select(x => x.ClassId).First());
            ActorRole.CreateInstance(_network, _actorId, _roleId, _organizationId1);
            Assert.AreEqual(0, _network.IsActorOfOrganizationIds(_actorId, _classId0).Count());
            Assert.AreEqual(2, _network.IsActorOfOrganizationIds(_actorId, _organizationId.ClassId).Count());
        }

        [TestMethod]
        public void IsActorOfTest()
        {
            Assert.IsFalse(_network.IsActorOf(_actorId, _organizationId.ClassId));
            ActorRole.CreateInstance(_network, _actorId, _roleId, _organizationId);
            Assert.IsTrue(_network.IsActorOf(_actorId, _organizationId.ClassId));
        }

        [TestMethod]
        public void ExistActorForRoleTypeTest()
        {
            Assert.IsFalse(_network.ExistActorForRoleType(_roleId, _organizationId));
            ActorRole.CreateInstance(_network, _actorId, _roleId, _organizationId);
            Assert.IsTrue(_network.ExistActorForRoleType(_roleId, _organizationId));
        }

        [TestMethod]
        public void GetActorIdForRoleTypeTest()
        {
            Assert.IsNull(_network.GetActorIdForRoleType(_roleId, _organizationId));
            ActorRole.CreateInstance(_network, _actorId, _roleId, _organizationId);
            Assert.AreEqual(_actorId, _network.GetActorIdForRoleType(_roleId, _organizationId));
        }

        [TestMethod]
        public void HasARoleInTest()
        {
            Assert.IsFalse(_network.HasARoleIn(_actorId, _organizationId));
            Assert.IsFalse(_network.HasARoleIn(_actorId, _organizationId1));
            ActorRole.CreateInstance(_network, _actorId, _roleId, _organizationId);
            ActorRole.CreateInstance(_network, _actorId, _roleId, _organizationId1);
            Assert.IsTrue(_network.HasARoleIn(_actorId, _organizationId));
            Assert.IsTrue(_network.HasARoleIn(_actorId, _organizationId1));
        }

        [TestMethod]
        public void HasARoleInTest1()
        {
            Assert.IsFalse(_network.HasARoleIn(_actorId, _roleId, _organizationId));
            ActorRole.CreateInstance(_network, _actorId, _roleId, _organizationId);
            Assert.IsTrue(_network.HasARoleIn(_actorId, _roleId, _organizationId));
        }

        [TestMethod]
        public void GetRolesInTest()
        {
            var getRoles = _network.GetRolesIn(_actorId, _organizationId);
            Assert.IsFalse(getRoles.Any());
            ActorRole.CreateInstance(_network, _actorId, _roleId, _organizationId);
            getRoles = _network.GetRolesIn(_actorId, _organizationId);
            Assert.IsTrue(getRoles.Any());
        }

        [TestMethod]
        public void GetRolesTest()
        {
            var getRoles = _network.GetRoles(_organizationId);
            Assert.IsFalse(getRoles.Any());
            ActorRole.CreateInstance(_network, _actorId, _roleId, _organizationId);
            getRoles = _network.GetRoles(_organizationId);
            Assert.IsTrue(getRoles.Any());
        }

        [TestMethod]
        public void TransferToTest()
        {
            ActorRole.CreateInstance(_network, _actorId, _roleId, _organizationId);
            _network.TransferTo(_actorId, _organizationId, _organizationId1);
            Assert.IsFalse(_network.HasARoleIn(_actorId, _organizationId));
            Assert.IsTrue(_network.HasARoleIn(_actorId, _organizationId1));
        }


        [TestMethod]
        public void RemoveMembersByRoleTypeFromGroupTest()
        {
            ActorRole.CreateInstance(_network, _actorId, _roleId, _organizationId);
            _network.RemoveActorsByRoleFromOrganization(_roleId, _organizationId);
            Assert.IsFalse(_network.Any());
        }
    }
}