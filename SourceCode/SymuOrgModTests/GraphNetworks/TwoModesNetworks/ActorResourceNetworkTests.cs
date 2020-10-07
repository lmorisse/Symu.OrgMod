#region Licence

// Description: SymuBiz - SymuOrgModTests
// Website: https://symu.org
// Copyright: (c) 2020 laurent morisseau
// License : the program is distributed under the terms of the GNU General Public License

#endregion

#region using directives

using System;
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
    public class ActorResourceNetworkTests
    {
        private readonly IAgentId _actorId = new AgentId(3, 3);
        private readonly ActorResourceNetwork _network = new ActorResourceNetwork();
        private readonly IAgentId _resourceId = new AgentId(2, 2);
        private readonly IResourceUsage _usage = new ResourceUsage(1);
        
        [TestInitialize]
        public void Initialize()
        {
        }

        /// <summary>
        ///     Don't exists
        /// </summary>
        [TestMethod]
        public void AddTest()
        {
            Assert.IsFalse(_network.Any());
            _ = new ActorResource(_network, _actorId, _resourceId, _usage);
            Assert.IsTrue(_network.Any());
        }

        /// <summary>
        ///     Exists + updateAllocations
        /// </summary>
        [TestMethod]
        public void AddTest1()
        {
            var edge = new ActorResource(_network, _actorId, _resourceId, _usage, 0);
            Assert.AreEqual(0, _network.Weight(edge));
        }

        [TestMethod]
        public void GetWeightTest()
        {
            Assert.AreEqual(0, _network.GetWeight(_actorId, _resourceId, _usage));
            _ = new ActorResource(_network, _actorId, _resourceId, _usage);
            Assert.AreEqual(100, _network.GetWeight(_actorId, _resourceId, _usage));
        }

        [TestMethod]
        public void GetActorResourceTest()
        {
            Assert.IsNull(_network.GetActorResource(_actorId, _resourceId, _usage));
            _ = new ActorResource(_network, _actorId, _resourceId, _usage);
            Assert.IsNotNull(_network.GetActorResource(_actorId, _resourceId, _usage));
        }

        [TestMethod]
        public void HasResourceTest()
        {
            Assert.IsFalse(_network.HasResource(_actorId, _usage));
            _ = new ActorResource(_network, _actorId, _resourceId, _usage);
            Assert.IsTrue(_network.HasResource(_actorId, _usage));
        }

        [TestMethod]
        public void HasResourceTest1()
        {
            Assert.IsFalse(_network.HasResource(_actorId, _resourceId, _usage));
            _ = new ActorResource(_network, _actorId, _resourceId, _usage);
            Assert.IsTrue(_network.HasResource(_actorId, _resourceId, _usage));
        }

        [TestMethod]
        public void GetResourceIdsTest()
        {
            Assert.AreEqual(0, _network.GetResourceIds(_actorId, _usage).Count());
            _ = new ActorResource(_network, _actorId, _resourceId, _usage);
            Assert.AreEqual(1, _network.GetResourceIds(_actorId, _usage).Count());
        }

        [TestMethod]
        public void GetResourceIdsTest1()
        {
            Assert.AreEqual(0, _network.GetResourceIds(_actorId, _usage, _resourceId.ClassId).Count());
            _ = new ActorResource(_network, _actorId, _resourceId, _usage);
            Assert.AreEqual(1, _network.GetResourceIds(_actorId, _usage, _resourceId.ClassId).Count());
        }

        [TestMethod]
        public void GetActorIdsTest()
        {
            Assert.AreEqual(0, _network.GetActorIds(_resourceId, _usage, _actorId.ClassId).Count());
            _ = new ActorResource(_network, _actorId, _resourceId, _usage);
            Assert.AreEqual(1, _network.GetActorIds(_resourceId, _usage, _actorId.ClassId).Count());
        }

        /// <summary>
        ///     Non passing test
        /// </summary>
        [TestMethod]
        public void UpdateAllocationTest()
        {
            Assert.ThrowsException<NullReferenceException>(() =>
                _network.UpdateWeight(_actorId, _resourceId, _usage, 0, 0));
        }

        [TestMethod]
        public void UpdateAllocationTest1()
        {
            // Test increment
            _ = new ActorResource(_network, _actorId, _resourceId, _usage, 50);
            _network.UpdateWeight(_actorId, _resourceId, _usage, 20, 0);
            Assert.AreEqual(70, _network.Weight(_actorId, _resourceId));
            // Test decrement with a threshold
            _network.UpdateWeight(_actorId, _resourceId, _usage, -70, 10);
            Assert.AreEqual(10, _network.Weight(_actorId, _resourceId));
        }

        /// <summary>
        ///     Full allocation true
        /// </summary>
        [TestMethod]
        public void UpdateAllocationsTest()
        {
            _ = new ActorResource(_network, _actorId, _resourceId, _usage, 50);
            _network.UpdateWeights(_actorId, _resourceId.ClassId, true);
            Assert.AreEqual(100, _network.Weight(_actorId, _resourceId));
        }

        /// <summary>
        ///     Full allocation false
        /// </summary>
        [TestMethod]
        public void UpdateAllocationsTest1()
        {
            _ = new ActorResource(_network, _actorId, _resourceId, _usage, 50);
            _network.UpdateWeights(_actorId, _resourceId.ClassId, false);
            Assert.AreEqual(50, _network.Weight(_actorId, _resourceId));
        }

        [TestMethod]
        public void UpdateAllocationsTest2()
        {
            Assert.ThrowsException<ArgumentOutOfRangeException>(() =>
                _network.UpdateWeights(_actorId, _resourceId.ClassId, true));
        }

        /// <summary>
        ///     Default organization
        /// </summary>
        [TestMethod]
        public void GetMainOrganizationOrDefaultTest()
        {
            _network.Clear();
            Assert.IsNull(_network.GetMainResourceOrDefault(_actorId, _resourceId.ClassId));
            Assert.AreNotEqual(_resourceId,
                _network.GetMainResourceOrDefault(_actorId, _resourceId.ClassId));
        }

        /// <summary>
        ///     First organization
        /// </summary>
        [TestMethod]
        public void GetMainOrganizationOrDefaultTest1()
        {
            _ = new ActorResource(_network, _actorId, _resourceId, _usage);
            var teamId1 = new AgentId(2, _resourceId.ClassId);
            _ = new ActorResource(_network, _actorId, teamId1, new ResourceUsage(0));
            Assert.AreEqual(_resourceId, _network.GetMainResourceOrDefault(_actorId, _resourceId.ClassId));
        }
    }
}