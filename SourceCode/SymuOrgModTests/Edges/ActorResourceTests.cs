﻿#region Licence

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

namespace SymuOrgModTests.Edges
{
    [TestClass]
    public class ActorResourceTests
    {
        private readonly GraphMetaNetwork _metaNetwork = new GraphMetaNetwork();
        private readonly IAgentId _actorId = new AgentId(1, 1);
        private readonly IAgentId _resourceId = new AgentId(2, 2);
        private readonly IResourceUsage _usage = new ResourceUsage(1);
        private readonly IResourceUsage _usage1 = new ResourceUsage(2);

        private IActorResource _edge;
        private IActorResource _edge1;

        [TestInitialize]
        public void Initialize()
        {
            _edge = new ActorResource(_metaNetwork.ActorResource, _actorId, _resourceId, _usage, 1);
            _edge1 = new ActorResource(_metaNetwork.ActorResource, _actorId, _resourceId, _usage1, 1);
        }

        [TestMethod]
        public void EqualsTest()
        {
            Assert.IsFalse(_edge.Equals(_edge1.Usage));
            Assert.IsTrue(_edge.Equals(_edge.Usage));
        }

        [TestMethod]
        public void CloneTest()
        {
            var clone = _edge.Clone();
            Assert.AreEqual(_edge, clone);
        }
    }
}