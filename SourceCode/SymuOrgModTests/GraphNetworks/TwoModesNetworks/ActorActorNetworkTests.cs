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
using Symu.OrgMod.GraphNetworks.TwoModesNetworks;

#endregion

namespace SymuOrgModTests.GraphNetworks.TwoModesNetworks
{
    [TestClass]
    public class ActorActorNetworkTests
    {
        private readonly IAgentId _agentId1 = new AgentId(2, 2);
        private readonly IAgentId _agentId2 = new AgentId(3, 2);
        private readonly IAgentId _agentId3 = new AgentId(4, 2);
        private readonly ActorActorNetwork _network = new ActorActorNetwork();

        [TestInitialize]
        public void Initialize()
        {
        }

        [TestMethod]
        public void RemoveActorTest()
        {
            ActorActor.CreateInstance(_network, _agentId1, _agentId2);
            ActorActor.CreateInstance(_network, _agentId3, _agentId1);
            _network.RemoveActor(_agentId1);
            Assert.IsFalse(_network.Any());
        }

        [TestMethod]
        public void RemoveActorTest1()
        {
            ActorActor.CreateInstance(_network, _agentId1, _agentId2);
            ActorActor.CreateInstance(_network, _agentId3, _agentId2);
            _network.RemoveActor(_agentId1);
            Assert.AreEqual(1, _network.Count);
        }

        /// <summary>
        ///     Direct test
        /// </summary>
        [TestMethod]
        public void DecreaseInteractionTest()
        {
            ActorActor.CreateInstance(_network, _agentId1, _agentId2);
            var link = _network[0];
            // Active link
            Assert.IsTrue(link.IsActive);
            // Deactivate
            _network.DecreaseInteraction(_agentId1, _agentId2);
            Assert.IsFalse(link.IsActive);
            Assert.IsTrue(link.IsPassive);
        }

        /// <summary>
        ///     indirect test
        /// </summary>
        [TestMethod]
        public void DecreaseInteractionTest1()
        {
            ActorActor.CreateInstance(_network, _agentId1, _agentId2);
            var link = _network[0];
            // Active link
            Assert.IsTrue(link.IsActive);
            // Deactivate
            _network.DecreaseInteraction(_agentId2, _agentId1);
            Assert.IsFalse(link.IsActive);
            Assert.IsTrue(link.IsPassive);
        }

        /// <summary>
        ///     Direct test
        /// </summary>
        [TestMethod]
        public void HasActiveInteractionTest()
        {
            ActorActor.CreateInstance(_network, _agentId1, _agentId2);
            var link = _network[0];
            Assert.IsTrue(link.HasActiveInteraction(_agentId1, _agentId2));
            Assert.IsFalse(link.HasActiveInteraction(_agentId1, _agentId3));
        }

        /// <summary>
        ///     Indirect test
        /// </summary>
        [TestMethod]
        public void HasActiveInteractionTest1()
        {
            ActorActor.CreateInstance(_network, _agentId1, _agentId2);
            var link = _network[0];
            Assert.IsTrue(link.HasActiveInteraction(_agentId2, _agentId1));
            Assert.IsFalse(link.HasActiveInteraction(_agentId1, _agentId3));
        }

        /// <summary>
        ///     Direct test
        /// </summary>
        [TestMethod]
        public void HasPassiveInteractionTest()
        {
            ActorActor.CreateInstance(_network, _agentId1, _agentId2);
            var link = _network[0];
            link.DecreaseWeight();
            Assert.IsTrue(link.HasPassiveInteraction(_agentId1, _agentId2));
            Assert.IsFalse(link.HasPassiveInteraction(_agentId1, _agentId3));
        }

        /// <summary>
        ///     Indirect test
        /// </summary>
        [TestMethod]
        public void HasPassiveInteractionTest1()
        {
            ActorActor.CreateInstance(_network, _agentId1, _agentId2);
            var link = _network[0];
            link.DecreaseWeight();
            Assert.IsTrue(link.HasPassiveInteraction(_agentId2, _agentId1));
            Assert.IsFalse(link.HasPassiveInteraction(_agentId1, _agentId3));
        }

        [TestMethod]
        public void GetActiveInteractionsTest()
        {
            Assert.AreEqual(0, _network.ActiveInteractionCount(_agentId1));
            ActorActor.CreateInstance(_network, _agentId1, _agentId2);
            ActorActor.CreateInstance(_network, _agentId3, _agentId2);
            var teammateId4 = new AgentId(5, 2);
            ActorActor.CreateInstance(_network, _agentId1, teammateId4);
            Assert.AreEqual(2, _network.ActiveInteractionCount(_agentId1));

            // Distinct test
            ActorActor.CreateInstance(_network, _agentId1, _agentId2);
            Assert.AreEqual(2, _network.ActiveInteractionCount(_agentId1));
        }

        [TestMethod]
        public void ExistsTest()
        {
            ActorActor.CreateInstance(_network, _agentId1, _agentId2);
            Assert.IsTrue(_network.Exists(_agentId1, _agentId2));
            Assert.IsTrue(_network.Exists(_agentId2, _agentId1));
        }


        [TestMethod]
        public void EdgeTest()
        {
            var edge = new ActorActor(_network, _agentId1, _agentId2);
            Assert.AreEqual(edge, _network.Edge(_agentId1, _agentId2));
            Assert.AreEqual(edge, _network.Edge(_agentId2, _agentId1));
        }

        [TestMethod]
        public void AddTest()
        {
            var link = new ActorActor(_network, _agentId1, _agentId2);
            Assert.IsTrue(_network.Exists(link));
            // Deactivate test
            link.DecreaseWeight();
            ActorActor.CreateInstance(_network, _agentId1, _agentId2);
            Assert.AreEqual(1, _network.Count);
            Assert.IsTrue(_network[0].IsActive);
        }

        [TestMethod]
        public void WeightTest()
        {
            Assert.AreEqual(0, _network.Weight(_agentId1, _agentId2));
            ActorActor.CreateInstance(_network, _agentId1, _agentId2);
            Assert.AreEqual(1, _network.Weight(_agentId1, _agentId2));
            Assert.AreEqual(1, _network.Weight(_agentId2, _agentId1));
            ActorActor.CreateInstance(_network, _agentId1, _agentId2);
            Assert.AreEqual(2, _network.Weight(_agentId1, _agentId2));
        }

        [TestMethod]
        public void NormalizedCountLinksTest()
        {
            Assert.AreEqual(0, _network.NormalizedWeight(_agentId1, _agentId2));
            ActorActor.CreateInstance(_network, _agentId1, _agentId2);
            _network.NormalizeWeights();
            Assert.AreEqual(1, _network.NormalizedWeight(_agentId1, _agentId2));
            ActorActor.CreateInstance(_network, _agentId2, _agentId2);
            _network.NormalizeWeights();
            Assert.AreEqual(1, _network.NormalizedWeight(_agentId1, _agentId2));
        }
    }
}