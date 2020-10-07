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
using Symu.OrgMod.Entities;
using Symu.OrgMod.GraphNetworks;

#endregion

namespace SymuOrgModTests.Entities
{
    [TestClass]
    public class BeliefEntityTests
    {
        private readonly IAgentId _agentId = new AgentId(2, 2);
        private readonly GraphMetaNetwork _metaNetwork = new GraphMetaNetwork();
        private BeliefEntity _entity;

        [TestInitialize]
        public void Initialize()
        {
            _entity = new BeliefEntity(_metaNetwork);
        }

        private void TestMetaNetwork(IEntity entity)
        {
            Assert.AreEqual(1, _metaNetwork.ActorBelief.EdgesFilteredByTargetCount(entity.EntityId));
        }

        private void SetMetaNetwork()
        {
            // _metaNetwork.ActorBelief.Add(new ActorBelief(_agentId, _entity.EntityId));
            _ = new ActorBelief(_metaNetwork.ActorBelief, _agentId, _entity.EntityId);
        }

        [TestMethod]
        public void CloneTest()
        {
            SetMetaNetwork();
            var clone = _entity.Clone() as BeliefEntity;
            Assert.IsNotNull(clone);
            Assert.AreEqual(_entity.EntityId, clone.EntityId);
            Assert.AreEqual(_entity.Name, clone.Name);
            TestMetaNetwork(clone);
        }

        [TestMethod]
        public void DuplicateTest()
        {
            SetMetaNetwork();
            var clone = _entity.Duplicate<BeliefEntity>();
            Assert.IsNotNull(clone);
            Assert.IsNotNull(_metaNetwork.Belief.GetEntity(clone.EntityId));
            Assert.AreNotEqual(_entity.EntityId, clone.EntityId);
            Assert.AreEqual(_entity.Name, clone.Name);
            TestMetaNetwork(clone);
        }

        [TestMethod]
        public void CopyMetaNetworkToTest()
        {
            SetMetaNetwork();
            var belief1 = new BeliefEntity(_metaNetwork);
            _entity.CopyMetaNetworkTo(belief1.EntityId);
            TestMetaNetwork(belief1);
        }

        [TestMethod]
        public void RemoveTest()
        {
            SetMetaNetwork();
            _entity.Remove();

            Assert.IsFalse(_metaNetwork.ActorBelief.Any());
            Assert.IsFalse(_metaNetwork.Belief.Any());
        }
    }
}