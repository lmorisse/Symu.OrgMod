﻿#region Licence

// Description: SymuBiz - SymuDNATests
// Website: https://symu.org
// Copyright: (c) 2020 laurent morisseau
// License : the program is distributed under the terms of the GNU General Public License

#endregion

#region using directives

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Symu.OrgMod.Edges;
using Symu.OrgMod.Entities;
using Symu.OrgMod.GraphNetworks;
using Symu.OrgMod.GraphNetworks.TwoModesNetworks.Sphere;

#endregion

namespace SymuOrgModTests.GraphNetworks
{
    [TestClass]
    public class MetaNetworkTests
    {
        private const byte Usage = 1;

        private IActor _actor;

        private IActorActor _actorActor;
        private IActorBelief _actorBelief;
        private IEntityKnowledge _actorKnowledge;
        private IActorOrganization _actorOrganization;
        private IActorResource _actorResource;
        private IActorRole _actorRole;
        private IActorTask _actorTask;
        private IBelief _belief;
        private IEvent _event;
        private IKnowledge _knowledge;

        private GraphMetaNetwork _network;
        private IOrganization _organization;
        private IOrganizationResource _organizationResource;
        private IResource _resource;
        private IResourceResource _resourceResource;
        private IResourceTask _resourceTask;
        private IEntityKnowledge _resourceKnowledge;
        private IRole _role;
        private ITask _task;
        private IEntityKnowledge _taskKnowledge;


        [TestInitialize]
        public void Initialize()
        {
            var interactionSphereModel = new InteractionSphereModel();
            interactionSphereModel.SetInteractionPatterns(InteractionStrategy.SocialDemographics);
            _network = new GraphMetaNetwork(interactionSphereModel);
            _actor = new ActorEntity(_network);
            _role = new RoleEntity(_network);
            _belief = new BeliefEntity(_network);
            _task = new TaskEntity(_network);
            _event = new EventEntity(_network);
            _organization = new OrganizationEntity(_network);
            _resource = new ResourceEntity(_network);
            _knowledge = new KnowledgeEntity(_network);
            _actorRole = new ActorRole(_actor.EntityId, _role.EntityId, _organization.EntityId);
            _actorResource = new ActorResource(_actor.EntityId, _resource.EntityId, new ResourceUsage(Usage));
            _actorOrganization = new ActorOrganization(_actor.EntityId, _organization.EntityId);
            _actorKnowledge = new EntityKnowledge(_actor.EntityId, _knowledge.EntityId);
            _actorTask = new ActorTask(_actor.EntityId, _task.EntityId);
            var actor1 = new ActorEntity(_network);
            _actorActor = new ActorActor(_actor.EntityId, actor1.EntityId);
            _actorBelief = new ActorBelief(_actor.EntityId, _belief.EntityId);
            _resourceTask = new ResourceTask(_resource.EntityId, _task.EntityId);
            _resourceKnowledge = new EntityKnowledge(_resource.EntityId, _knowledge.EntityId);
            _taskKnowledge = new EntityKnowledge(_task.EntityId, _knowledge.EntityId);
            _organizationResource = new OrganizationResource(_organization.EntityId, _resource.EntityId,
                new ResourceUsage(Usage));
            _resourceResource = new ResourceResource(_resource.EntityId, _resource.EntityId, new ResourceUsage(Usage));

            _network.ActorActor.Add(_actorActor);
            _network.ActorOrganization.Add(_actorOrganization);
            _network.ActorKnowledge.Add(_actorKnowledge);
            _network.ActorTask.Add(_actorTask);
            _network.ActorBelief.Add(_actorBelief);
            _network.ResourceTask.Add(_resourceTask);
            _network.TaskKnowledge.Add(_taskKnowledge);
            _network.ActorResource.Add(_actorResource);
            _network.ActorRole.Add(_actorRole);
            _network.OrganizationResource.Add(_organizationResource);
            _network.ResourceResource.Add(_resourceResource);
            _network.ResourceKnowledge.Add(_resourceKnowledge);
        }


        [TestMethod]
        public void InitializeNetworkTest()
        {
            foreach (var oneModeNetwork in _network.OneModeNetworks)
            {
                Assert.IsTrue(oneModeNetwork.Any());
            }

            Assert.AreEqual(1, _network.ActorActor.Count);
            Assert.AreEqual(1, _network.ActorOrganization.Count);
            Assert.AreEqual(1, _network.ActorKnowledge.Count);
            Assert.AreEqual(1, _network.ActorTask.Count);
            Assert.AreEqual(1, _network.ActorBelief.Count);
            Assert.AreEqual(1, _network.ResourceTask.Count);
            Assert.AreEqual(1, _network.TaskKnowledge.Count);
            Assert.AreEqual(1, _network.ActorResource.Count);
            Assert.AreEqual(1, _network.ActorRole.Count);
            Assert.AreEqual(1, _network.OrganizationResource.Count);
            Assert.AreEqual(1, _network.ResourceResource.Count);
            Assert.AreEqual(1, _network.ResourceKnowledge.Count);
        }

        [TestMethod]
        public void ClearTest()
        {
            _network.Clear();

            foreach (var oneModeNetwork in _network.OneModeNetworks)
            {
                Assert.IsFalse(oneModeNetwork.Any());
            }

            Assert.IsFalse(_network.ActorActor.Any());
            Assert.IsFalse(_network.ActorOrganization.Any());
            Assert.IsFalse(_network.ActorKnowledge.Any());
            Assert.IsFalse(_network.ActorTask.Any());
            Assert.IsFalse(_network.ActorBelief.Any());
            Assert.IsFalse(_network.ActorTask.Any());
            Assert.IsFalse(_network.ResourceTask.Any());
            Assert.IsFalse(_network.TaskKnowledge.Any());
            Assert.IsFalse(_network.ActorResource.Any());
            Assert.IsFalse(_network.ActorRole.Any());
            Assert.IsFalse(_network.OrganizationResource.Any());
            Assert.IsFalse(_network.ResourceResource.Any());
            Assert.IsFalse(_network.ResourceKnowledge.Any());
        }

        [TestMethod]
        public void CloneTest()
        {
            var copy = _network.Clone();
            //test that Entity are pointing to copy MetaNetwork
            _network.Organization.Add(new OrganizationEntity(_network));
            Assert.AreEqual(2, _network.Organization.List.Count);
            Assert.AreEqual(1, copy.Organization.List.Count);

            foreach (var oneModeNetwork in copy.OneModeNetworks)
            {
                Assert.IsTrue(oneModeNetwork.Any());
            }

            Assert.IsTrue(copy.ActorActor.Any());
            Assert.IsTrue(copy.ActorOrganization.Any());
            Assert.IsTrue(copy.ActorKnowledge.Any());
            Assert.IsTrue(copy.ActorTask.Any());
            Assert.IsTrue(copy.ActorBelief.Any());
            Assert.IsTrue(copy.ActorTask.Any());
            Assert.IsTrue(copy.ResourceTask.Any());
            Assert.IsTrue(copy.TaskKnowledge.Any());
            Assert.IsTrue(copy.ActorResource.Any());
            Assert.IsTrue(copy.ActorRole.Any());
            Assert.IsTrue(copy.OrganizationResource.Any());
            Assert.IsTrue(copy.ResourceResource.Any());
            Assert.IsTrue(copy.ResourceKnowledge.Any());
            Assert.AreEqual(0, _network.InteractionSphere.Model.RelativeActivityWeight);
            Assert.AreEqual(1, _network.InteractionSphere.Model.SocialDemographicWeight);
        }

        [TestMethod]
        public void ToMatrixTest()
        {
            var clone = _network.Clone();
            Assert.IsTrue(clone.Organization.Any());
            Assert.IsTrue(clone.Actor.Any());
            Assert.IsTrue(clone.Belief.Any());
            Assert.IsTrue(clone.Knowledge.Any());
            Assert.IsTrue(clone.Task.Any());
            Assert.IsTrue(clone.Event.Any());
            Assert.IsTrue(clone.Resource.Any());
            Assert.IsTrue(clone.Role.Any());
            Assert.IsTrue(clone.ActorActor.Any());
            Assert.IsTrue(clone.ActorOrganization.Any());
            Assert.IsTrue(clone.ActorKnowledge.Any());
            Assert.IsTrue(clone.ActorTask.Any());
            Assert.IsTrue(clone.ActorBelief.Any());
            Assert.IsTrue(clone.ActorTask.Any());
            Assert.IsTrue(clone.ResourceTask.Any());
            Assert.IsTrue(clone.TaskKnowledge.Any());
            Assert.IsTrue(clone.ActorResource.Any());
            Assert.IsTrue(clone.ActorRole.Any());
            Assert.IsTrue(clone.OrganizationResource.Any());
            Assert.IsTrue(clone.ResourceResource.Any());
            Assert.IsTrue(clone.ResourceKnowledge.Any());
        }
    }
}