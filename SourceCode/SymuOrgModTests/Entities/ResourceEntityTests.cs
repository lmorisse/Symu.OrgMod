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
using Symu.OrgMod.GraphNetworks;

#endregion

namespace SymuOrgModTests.Entities
{
    [TestClass]
    public class ResourceEntityTests
    {
        private readonly IAgentId _agentId = new AgentId(2, ActorEntity.ClassId);
        private readonly IAgentId _agentId1 = new AgentId(3, ActorEntity.ClassId);
        private readonly GraphMetaNetwork _metaNetwork = new GraphMetaNetwork();
        private readonly ResourceUsage _usage = new ResourceUsage(1);
        private ResourceEntity _entity;

        [TestInitialize]
        public void Initialize()
        {
            _entity = new ResourceEntity(_metaNetwork);
        }

        private void TestMetaNetwork(IEntity entity)
        {
            Assert.AreEqual(1, _metaNetwork.ResourceResource.EdgesFilteredBySourceCount(entity.EntityId));
            Assert.AreEqual(1, _metaNetwork.ResourceResource.EdgesFilteredByTargetCount(entity.EntityId));
            Assert.AreEqual(1, _metaNetwork.ResourceTask.EdgesFilteredBySourceCount(entity.EntityId));
            Assert.AreEqual(1, _metaNetwork.ActorResource.EdgesFilteredByTargetCount(entity.EntityId));
            Assert.AreEqual(1, _metaNetwork.OrganizationResource.EdgesFilteredByTargetCount(entity.EntityId));
            Assert.AreEqual(1, _metaNetwork.ResourceKnowledge.EdgesFilteredBySourceCount(entity.EntityId));
        }

        private void SetMetaNetwork()
        {

            ResourceResource.CreateInstance(_metaNetwork.ResourceResource, _entity.EntityId, _agentId, _usage, 1);
            ResourceResource.CreateInstance(_metaNetwork.ResourceResource, _agentId, _entity.EntityId, _usage, 1);
            ResourceTask.CreateInstance(_metaNetwork.ResourceTask, _entity.EntityId, _agentId);
            EntityKnowledge.CreateInstance(_metaNetwork.ResourceKnowledge, _entity.EntityId, _agentId);
            ActorResource.CreateInstance(_metaNetwork.ActorResource, _agentId, _entity.EntityId, _usage, 1);
            OrganizationResource.CreateInstance(_metaNetwork.OrganizationResource, _agentId, _entity.EntityId, _usage, 1);
        }

        [TestMethod]
        public void CloneTest()
        {
            SetMetaNetwork();
            var clone = _entity.Clone() as ResourceEntity;
            Assert.IsNotNull(clone);
            Assert.AreEqual(_entity.EntityId, clone.EntityId);
            Assert.AreEqual(_entity.Name, clone.Name);
            TestMetaNetwork(clone);
        }

        [TestMethod]
        public void DuplicateTest()
        {
            SetMetaNetwork();
            var clone = _entity.Duplicate<ResourceEntity>();
            Assert.IsNotNull(clone);
            Assert.IsNotNull(_metaNetwork.Resource.GetEntity(clone.EntityId));
            Assert.AreNotEqual(_entity.EntityId, clone.EntityId);
            Assert.AreEqual(_entity.Name, clone.Name);
            TestMetaNetwork(clone);
        }

        [TestMethod]
        public void CopyMetaNetworkToTest()
        {
            SetMetaNetwork();
            var resource1 = new ResourceEntity(_metaNetwork);
            _entity.CopyMetaNetworkTo(resource1.EntityId);
            TestMetaNetwork(resource1);
        }

        [TestMethod]
        public void RemoveTest()
        {
            SetMetaNetwork();
            _entity.Remove();
            Assert.IsFalse(_metaNetwork.ResourceResource.Any());
            Assert.IsFalse(_metaNetwork.ResourceTask.Any());
            Assert.IsFalse(_metaNetwork.OrganizationResource.Any());
            Assert.IsFalse(_metaNetwork.ActorResource.Any());
            Assert.IsFalse(_metaNetwork.ResourceKnowledge.Any());
            Assert.IsFalse(_metaNetwork.Resource.Any());
        }

        [TestMethod]
        public void AddTest()
        {
            _entity.AddActor(_agentId, _usage, 1);
            Assert.AreEqual(1, _metaNetwork.ActorResource.Count);
        }

        [TestMethod]
        public void AddTest1()
        {
            _entity.AddOrganization(_agentId, _usage, 1);
            Assert.AreEqual(1, _metaNetwork.OrganizationResource.Count);
        }

        [TestMethod]
        public void AddTest2()
        {
            _entity.AddResource(_agentId, _usage, 1);
            Assert.AreEqual(1, _metaNetwork.ResourceResource.Count);
        }

        [TestMethod]
        public void AddTest4()
        {
            _entity.AddKnowledge(_agentId);
            Assert.AreEqual(1, _metaNetwork.ResourceKnowledge.Count);
        }

        [TestMethod]
        public void GetActorWeightTest()
        {
            Assert.AreEqual(0, _entity.GetActorWeight(_agentId, _usage));
            _entity.AddActor(_agentId, _usage, 1);
            Assert.AreEqual(1, _entity.GetActorWeight(_agentId, _usage));
        }

        [TestMethod]
        public void GetSumWeightTest()
        {
            Assert.AreEqual(0, _entity.GetSumWeight);
            _entity.AddResource(_agentId, _usage, 1);
            Assert.AreEqual(0, _entity.GetSumWeight);
            _entity.AddActor(_agentId, _usage, 1);
            _entity.AddActor(_agentId1, _usage, 1);
            Assert.AreEqual(2, _entity.GetSumWeight);
        }

        [TestMethod]
        public void GetActorsCountTest()
        {
            Assert.AreEqual(0, _entity.ActorsCount);
            _entity.AddActor(_agentId, _usage, 1);
            Assert.AreEqual(1, _entity.ActorsCount);
        }

        [TestMethod]
        public void GetOrganizationWeightTest()
        {
            Assert.AreEqual(0, _entity.GetOrganizationWeight(_agentId, _usage));
            _entity.AddOrganization(_agentId, _usage, 1);
            Assert.AreEqual(1, _entity.GetOrganizationWeight(_agentId, _usage));
        }

        [TestMethod]
        public void SetOrganizationWeightTest()
        {
            _entity.AddOrganization(_agentId, _usage, 1);
            Assert.AreEqual(1, _entity.GetOrganizationWeight(_agentId, _usage));
            _entity.SetOrganizationWeight(_agentId, _usage, 2);
            Assert.AreEqual(2, _entity.GetOrganizationWeight(_agentId, _usage));
        }


        [TestMethod]
        public void GetActorResourcesTest()
        {
            Assert.AreEqual(0, _entity.GetActorResources(_agentId).Count());
            _entity.AddActor(_agentId, _usage, 1);
            Assert.AreEqual(1, _entity.GetActorResources(_agentId).Count());
        }


        [TestMethod]
        public void GetOrganizationResourcesTest()
        {
            Assert.AreEqual(0, _entity.GetOrganizationResources(_agentId).Count());
            _entity.AddOrganization(_agentId, _usage, 1);
            Assert.AreEqual(1, _entity.GetOrganizationResources(_agentId).Count());
        }

        [TestMethod]
        public void GetResourceResourcesTest()
        {
            Assert.AreEqual(0, _entity.GetResourceResources(_agentId).Count());
            _entity.AddResource(_agentId, _usage, 1);
            Assert.AreEqual(1, _entity.GetResourceResources(_agentId).Count());
        }

        [TestMethod]
        public void ExistsKnowledgeTest()
        {
            Assert.IsFalse(_entity.ExistsKnowledge(_agentId));
            _entity.AddKnowledge(_agentId);
            Assert.IsTrue(_entity.ExistsKnowledge(_agentId));
        }

        [TestMethod]
        public void GetResourceWeightTest()
        {
            Assert.AreEqual(0, _entity.GetResourceWeight(_agentId, _usage));
            _entity.AddResource(_agentId, _usage, 1);
            Assert.AreEqual(1, _entity.GetResourceWeight(_agentId, _usage));
        }
    }
}