#region Licence

// Description: SymuBiz - SymuOrgModTests
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

        private IBelief _belief;
        private IKnowledge _knowledge;

        private GraphMetaNetwork _network;
        private IOrganization _organization;
        private IResource _resource;
        private IRole _role;
        private ITask _task;


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
            _ = new EventEntity(_network);
            _organization = new OrganizationEntity(_network);
            _resource = new ResourceEntity(_network);
            _knowledge = new KnowledgeEntity(_network);
            _ = new ActorRole(_network.ActorRole, _actor.EntityId, _role.EntityId, _organization.EntityId);
            _ = new ActorResource(_network.ActorResource, _actor.EntityId, _resource.EntityId, new ResourceUsage(Usage));
            _ = new ActorOrganization(_network.ActorOrganization, _actor.EntityId, _organization.EntityId);
            _ = new ActorTask(_network.ActorTask, _actor.EntityId, _task.EntityId);
            var actor1 = new ActorEntity(_network);
            _ = new ActorActor(_network.ActorActor, _actor.EntityId, actor1.EntityId);
            _ = new ActorBelief(_network.ActorBelief, _actor.EntityId, _belief.EntityId);
            _ = new ResourceTask(_network.ResourceTask, _resource.EntityId, _task.EntityId);
            _ = new OrganizationResource(_network.OrganizationResource, _organization.EntityId, _resource.EntityId,
                new ResourceUsage(Usage));
            _ = new ResourceResource(_network.ResourceResource, _resource.EntityId, _resource.EntityId, new ResourceUsage(Usage));
            _ = new EntityKnowledge(_network.ResourceKnowledge, _resource.EntityId, _knowledge.EntityId);
            _ = new EntityKnowledge(_network.ActorKnowledge, _actor.EntityId, _knowledge.EntityId);
            _ = new EntityKnowledge(_network.TaskKnowledge, _task.EntityId, _knowledge.EntityId);
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
            _organization =new OrganizationEntity(_network);
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