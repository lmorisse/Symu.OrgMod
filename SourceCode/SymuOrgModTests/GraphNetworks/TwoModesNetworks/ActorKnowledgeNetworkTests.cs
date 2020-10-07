#region Licence

// Description: SymuBiz - SymuOrgModTests
// Website: https://symu.org
// Copyright: (c) 2020 laurent morisseau
// License : the program is distributed under the terms of the GNU General Public License

#endregion

#region using directives

using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Symu.Common.Interfaces;
using Symu.OrgMod.Edges;
using Symu.OrgMod.GraphNetworks.TwoModesNetworks;

#endregion

namespace SymuOrgModTests.GraphNetworks.TwoModesNetworks
{
    [TestClass]
    public class ActorKnowledgeNetworkTests
    {
        private readonly IAgentId _actorId = new AgentId(1, 1);

        private readonly ActorKnowledgeNetwork _actorKnowledgeNetwork = new ActorKnowledgeNetwork();

        private readonly IAgentId _knowledgeId = new AgentId(2, 1);
        private IEntityKnowledge _edge;

        [TestInitialize]
        public void Initialize()
        {
            _edge = new EntityKnowledge(_actorId, _knowledgeId);
        }

        /// <summary>
        ///     Non passing test
        /// </summary>
        [TestMethod]
        public void FilterActorsWithKnowledgeTest()
        {
            var agentIds = new List<IAgentId>
            {
                _actorId
            };
            var filteredAgents = _actorKnowledgeNetwork.FilterActorsWithKnowledge(agentIds, new AgentId(0, 0));
            Assert.AreEqual(0, filteredAgents.Count());
            filteredAgents = _actorKnowledgeNetwork.FilterActorsWithKnowledge(agentIds, _knowledgeId);
            Assert.AreEqual(0, filteredAgents.Count());
        }

        /// <summary>
        ///     Passing test
        /// </summary>
        [TestMethod]
        public void FilterActorsWithKnowledgeTest1()
        {
            var agentIds = new List<IAgentId>
            {
                _actorId
            };
            _actorKnowledgeNetwork.Add(_edge);
            var filteredAgents = _actorKnowledgeNetwork.FilterActorsWithKnowledge(agentIds, _knowledgeId);
            Assert.AreEqual(1, filteredAgents.Count());
        }
    }
}