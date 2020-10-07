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
using Symu.OrgMod.Entities;
using Symu.OrgMod.GraphNetworks;
using Symu.OrgMod.GraphNetworks.TwoModesNetworks.Sphere;

#endregion

namespace SymuOrgModTests.GraphNetworks.TwoModesNetworks.Sphere
{
    [TestClass]
    public class InteractionSphereTests
    {
        private readonly IAgentId _actorId1 = new AgentId(1, 1);
        private readonly IAgentId _actorId2 = new AgentId(2, 1);
        private readonly List<IAgentId> _agents = new List<IAgentId>();
        private IBelief _belief;
        private InteractionSphereModel _interactionSphereModel;
        private IKnowledge _knowledge;
        private GraphMetaNetwork _network = new GraphMetaNetwork();
        private ITask _task;
        private InteractionSphere InteractionSphere => _network.InteractionSphere;

        [TestInitialize]
        public void Initialize()
        {
            _task = new TaskEntity(_network);
            _belief = new BeliefEntity(_network);
            _knowledge = new KnowledgeEntity(_network);
            _interactionSphereModel = new InteractionSphereModel {On = true};
            _network = new GraphMetaNetwork(_interactionSphereModel);
            _agents.Add(_actorId1);
            _agents.Add(_actorId2);
        }

        /// <summary>
        ///     No interaction
        /// </summary>
        [TestMethod]
        public void GeAgentIdsForInteractionsTest()
        {
            InteractionSphere.SetSphere(true, _agents, _network);
            Assert.AreEqual(0,
                InteractionSphere.GetAgentIdsForInteractions(_actorId1, InteractionStrategy.Homophily).Count());
        }

        /// <summary>
        ///     Without link & !_model.SphereUpdateOverTime
        /// </summary>
        [TestMethod]
        public void GeAgentIdsForNewInteractionsTest()
        {
            InteractionSphere.SetSphere(true, _agents, _network);
            Assert.AreEqual(0,
                InteractionSphere.GetAgentIdsForNewInteractions(_actorId1, InteractionStrategy.Homophily).Count());
        }

        /// <summary>
        ///     Without link & _model.SphereUpdateOverTime
        /// </summary>
        [TestMethod]
        public void GeAgentIdsForNewInteractionsTest1()
        {
            _interactionSphereModel.SphereUpdateOverTime = true;
            InteractionSphere.SetSphere(true, _agents, _network);
            Assert.AreEqual(1,
                InteractionSphere.GetAgentIdsForNewInteractions(_actorId1, InteractionStrategy.Homophily).Count());
        }

        /// <summary>
        ///     With link
        /// </summary>
        [TestMethod]
        public void GeAgentIdsForNewInteractionsTest2()
        {
            _interactionSphereModel.SphereUpdateOverTime = true;
            AddLink();
            InteractionSphere.SetSphere(true, _agents, _network);
            Assert.AreEqual(0,
                InteractionSphere.GetAgentIdsForNewInteractions(_actorId1, InteractionStrategy.Homophily).Count());
        }

        /// <summary>
        ///     With no interaction
        /// </summary>
        [TestMethod]
        public void GetSphereWeightTest()
        {
            InteractionSphere.SetSphere(true, _agents, _network);
            Assert.AreEqual(0, InteractionSphere.GetSphereWeight());
        }

        /// <summary>
        ///     With full interaction
        /// </summary>
        [TestMethod]
        public void GetSphereWeightTest1()
        {
            AddBelief(_actorId1, 1);
            AddBelief(_actorId2, 1);
            AddKnowledge(_actorId1, 1);
            AddKnowledge(_actorId2, 1);
            AddLink();
            AddActivity(_actorId1);
            AddActivity(_actorId2);
            InteractionSphere.SetSphere(true, _agents, _network);
            Assert.AreEqual(InteractionSphere.GetMaxSphereWeight(), InteractionSphere.GetSphereWeight());
        }

        [TestMethod]
        public void GetMaxSphereWeightTest()
        {
            InteractionSphere.SetSphere(true, _agents, _network);
            Assert.AreEqual(8, InteractionSphere.GetMaxSphereWeight());
        }

        #region common

        private void AddLink()
        {
            _ = new ActorActor(_network.ActorActor, _actorId1, _actorId2);
        }

        private void AddKnowledge(IAgentId actorId, float knowledgeValue)
        {
            _ = new EntityKnowledge(_network.ActorKnowledge, actorId, _knowledge.EntityId, knowledgeValue);
            _network.NormalizeWeights();
        }

        private void AddActivity(IAgentId actorId)
        {
            _ = new ActorTask(_network.ActorTask, actorId, _task.EntityId);
        }

        private void AddBelief(IAgentId actorId, float beliefValue)
        {
            var agentBelief = new ActorBelief(_network.ActorBelief, actorId, _belief.EntityId);
            _network.ActorBelief.Add(agentBelief);
            agentBelief.Weight = beliefValue;
        }

        #endregion

        #region Homophily

        /// <summary>
        ///     Empty network
        /// </summary>
        [TestMethod]
        public void GetHomophilyTest()
        {
            InteractionSphere.SetSphere(true, _agents, _network);
            Assert.AreEqual(0, InteractionSphere.GetHomophily(_actorId1, _actorId2));
        }

        /// <summary>
        ///     Linked agents
        /// </summary>
        [TestMethod]
        public void GetHomophilyTest1()
        {
            AddLink();
            _interactionSphereModel.SetInteractionPatterns(InteractionStrategy.SocialDemographics);
            InteractionSphere.SetSphere(true, _agents, _network);
            Assert.AreEqual(1, InteractionSphere.GetHomophily(_actorId1, _actorId2));
        }

        /// <summary>
        ///     Knowledge
        /// </summary>
        [TestMethod]
        public void GetHomophilyTest2()
        {
            AddKnowledge(_actorId1, 1);
            AddKnowledge(_actorId2, 1);
            InteractionSphere.SetSphere(true, _agents, _network);
            Assert.AreEqual(1, InteractionSphere.GetHomophily(_actorId1, _actorId2));
        }

        /// <summary>
        ///     Belief
        /// </summary>
        [TestMethod]
        public void GetHomophilyTest3()
        {
            AddBelief(_actorId1, 1);
            AddBelief(_actorId2, 1);
            InteractionSphere.SetSphere(true, _agents, _network);
            Assert.AreEqual(1, InteractionSphere.GetHomophily(_actorId1, _actorId2));
        }

        #endregion

        #region Belief

        /// <summary>
        ///     Without belief
        /// </summary>
        [TestMethod]
        public void SetRelativeBeliefTest()
        {
            Assert.AreEqual(0, InteractionSphere.SetRelativeBelief(_actorId1, _actorId2, _network.ActorBelief));
        }

        /// <summary>
        ///     With same belief 1
        /// </summary>
        [TestMethod]
        public void SetRelativeBeliefTest1()
        {
            AddBelief(_actorId1, 1);
            AddBelief(_actorId2, 1);
            Assert.AreEqual(1, InteractionSphere.SetRelativeBelief(_actorId1, _actorId2, _network.ActorBelief));
        }

        /// <summary>
        ///     With same belief -1
        /// </summary>
        [TestMethod]
        public void SetRelativeBeliefTest2()
        {
            AddBelief(_actorId1, -1);
            AddBelief(_actorId2, -1);
            Assert.AreEqual(1, InteractionSphere.SetRelativeBelief(_actorId1, _actorId2, _network.ActorBelief));
        }

        /// <summary>
        ///     With same belief 0
        /// </summary>
        [TestMethod]
        public void SetRelativeBeliefTest3()
        {
            AddBelief(_actorId1, 0);
            AddBelief(_actorId2, 0);
            Assert.AreEqual(0, InteractionSphere.SetRelativeBelief(_actorId1, _actorId2, _network.ActorBelief));
        }

        /// <summary>
        ///     With different belief
        /// </summary>
        [TestMethod]
        public void SetRelativeBeliefTest4()
        {
            AddBelief(_actorId1, -1);
            AddBelief(_actorId2, 1);
            Assert.AreEqual(-1, InteractionSphere.SetRelativeBelief(_actorId1, _actorId2, _network.ActorBelief));
        }

        #endregion

        #region Knowledge

        /// <summary>
        ///     Without knowledge
        /// </summary>
        [TestMethod]
        public void SetRelativeExpertiseTest()
        {
            Assert.AreEqual(0,
                InteractionSphere.SetRelativeKnowledge(_actorId1, _actorId2, _network.ActorKnowledge));
        }

        /// <summary>
        ///     With different knowledge level
        /// </summary>
        [TestMethod]
        public void SetRelativeExpertiseTest1()
        {
            AddKnowledge(_actorId1, 1);
            AddKnowledge(_actorId2, 0);
            Assert.AreEqual(0,
                InteractionSphere.SetRelativeKnowledge(_actorId1, _actorId2, _network.ActorKnowledge));
        }

        /// <summary>
        ///     With same knowledge level
        /// </summary>
        [TestMethod]
        public void SetRelativeExpertiseTest2()
        {
            AddKnowledge(_actorId1, 1);
            AddKnowledge(_actorId2, 1);
            Assert.AreEqual(1,
                InteractionSphere.SetRelativeKnowledge(_actorId1, _actorId2, _network.ActorKnowledge));
        }

        #endregion

        #region SocialProximity

        /// <summary>
        ///     Without link
        /// </summary>
        [TestMethod]
        public void SetSocialProximityTest()
        {
            _network.ActorActor.NormalizeWeights();
            Assert.AreEqual(0, InteractionSphere.SetSocialProximity(_actorId1, _actorId2, _network.ActorActor));
        }

        /// <summary>
        ///     With active link
        /// </summary>
        [TestMethod]
        public void SetSocialProximityTest1()
        {
            AddLink();
            _network.ActorActor.NormalizeWeights();
            Assert.AreEqual(1, InteractionSphere.SetSocialProximity(_actorId1, _actorId2, _network.ActorActor));
        }

        /// <summary>
        ///     With passive link
        /// </summary>
        [TestMethod]
        public void SetSocialProximityTest2()
        {
            var networkLink = new ActorActor(_network.ActorActor, _actorId1, _actorId2);
            networkLink.DecreaseWeight();
            Assert.AreEqual(0F, InteractionSphere.SetSocialProximity(_actorId1, _actorId2, _network.ActorActor));
        }

        #endregion

        #region Activities

        /// <summary>
        ///     Without Activity
        /// </summary>
        [TestMethod]
        public void SetRelativeActivityTest()
        {
            Assert.AreEqual(0, InteractionSphere.SetRelativeActivity(_actorId1, _actorId2, _network.ActorTask));
        }

        /// <summary>
        ///     With different activities level
        /// </summary>
        [TestMethod]
        public void SetRelativeActivityTest1()
        {
            AddActivity(_actorId1);
            AddActivity(_actorId2);
            Assert.AreEqual(1, InteractionSphere.SetRelativeActivity(_actorId1, _actorId2, _network.ActorTask));
        }

        #endregion
    }
}