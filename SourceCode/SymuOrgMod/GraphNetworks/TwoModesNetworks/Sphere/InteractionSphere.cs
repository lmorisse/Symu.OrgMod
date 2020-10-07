﻿#region Licence

// Description: SymuBiz - SymuOrgMod
// Website: https://symu.org
// Copyright: (c) 2020 laurent morisseau
// License : the program is distributed under the terms of the GNU General Public License

#endregion

#region using directives

using System;
using System.Collections.Generic;
using System.Linq;
using Symu.Common;
using Symu.Common.Interfaces;
using Symu.Common.Math.ProbabilityDistributions;

#endregion

namespace Symu.OrgMod.GraphNetworks.TwoModesNetworks.Sphere
{
    /// <summary>
    ///     Computed Agent * Agent of derived parameters from others networks.
    ///     Those parameters are relative parameters of an agent from another agent,
    ///     used indirectly to change agent behavior.
    /// </summary>
    public class InteractionSphere
    {
        private DerivedParameter _lastAverage;
        private VectorNetwork _vector;

        public InteractionSphere(InteractionSphereModel model)
        {
            Model = model ?? throw new ArgumentNullException(nameof(model));
        }

        public InteractionSphereModel Model { get; }

        /// <summary>
        ///     List of all agentId and their enculturation information
        /// </summary>
        public DerivedParameter[,] Sphere { get; private set; }

        public void SetSphere(bool initialization, List<IAgentId> agentIds, GraphMetaNetwork network)
        {
            if (agentIds == null)
            {
                throw new ArgumentNullException(nameof(agentIds));
            }

            if (network == null)
            {
                throw new ArgumentNullException(nameof(network));
            }

            var modelOn = Model.On;
            var sphereChange = initialization || Model.SphereUpdateOverTime;
            if (!modelOn || !sphereChange)
            {
                return;
            }

            //todo use => _vector = new VectorNetwork(network.Actor.ToVector());
            network.NormalizeWeights();
            if (Model.RandomlyGeneratedSphere)
            {
                if (initialization)
                {
                    SetSphereRandomly(agentIds);
                }
                else
                {
                    UpdateSphereRandomly(agentIds, network);
                }
            }
            else
            {
                SetSphereWithSimilarityMatching(agentIds, network);
            }

            _lastAverage = InteractionMatrix.GetAverageInteractionMatrix(Sphere);
        }

        /// <summary>
        ///     Clone sphere randomly
        /// </summary>
        /// <param name="agentIds"></param>
        public void SetSphereRandomly(IReadOnlyList<IAgentId> agentIds)
        {
            if (agentIds == null)
            {
                throw new ArgumentNullException(nameof(agentIds));
            }

            var count = agentIds.Count;
            _vector = new VectorNetwork(agentIds);
            Sphere = new DerivedParameter[count, count];
            // for the moment the matrix is symmetrical
            for (var i = 0; i < count; i++)
            {
                var agentI = agentIds[i];
                _vector.ItemIndex[agentI] = i;
                _vector.IndexItem[i] = agentI;

                var density = ContinuousUniform.Sample(Model.MinSphereDensity, Model.MaxSphereDensity);
                for (var j = i + 1; j < count; j++)
                {
                    var value = Bernoulli.Sample(density) ? ContinuousUniform.Sample(0F, 1F) : 0F;
                    Sphere[i, j] =
                        new DerivedParameter(Model, value, value, value, value);
                    Sphere[j, i] = Sphere[i, j];
                }
            }
        }

        /// <summary>
        ///     Update sphere randomly with new agent
        /// </summary>
        /// <param name="agentIds"></param>
        /// <param name="network"></param>
        public void UpdateSphereRandomly(IReadOnlyList<IAgentId> agentIds, GraphMetaNetwork network)
        {
            if (agentIds == null)
            {
                throw new ArgumentNullException(nameof(agentIds));
            }

            if (network == null)
            {
                throw new ArgumentNullException(nameof(network));
            }

            var count = agentIds.Count;
            var tempVector = new VectorNetwork(agentIds);
            var tempSphere = new DerivedParameter[count, count];
            for (var i = 0; i < count; i++)
            {
                var agentI = agentIds[i];
                if (_vector.ItemIndex.ContainsKey(agentI))
                {
                    var oldIndexI = _vector.ItemIndex[agentI];
                    for (var j = i + 1; j < count; j++)
                    {
                        var agentJ = agentIds[j];
                        if (_vector.ItemIndex.ContainsKey(agentJ))
                        {
                            var oldIndexJ = _vector.ItemIndex[agentJ];

                            var tempDerivedParameter = SetDerivedParameter(network, agentI, agentJ);
                            var socialProximity = Math.Max(tempDerivedParameter.SocialDemographic,
                                Sphere[oldIndexI, oldIndexJ].SocialDemographic);
                            var relativeBelief = Math.Max(tempDerivedParameter.RelativeBelief,
                                Sphere[oldIndexI, oldIndexJ].RelativeBelief);
                            var relativeKnowledge = Math.Max(tempDerivedParameter.RelativeKnowledge,
                                Sphere[oldIndexI, oldIndexJ].RelativeKnowledge);
                            var relativeActivity = Math.Max(tempDerivedParameter.RelativeActivity,
                                Sphere[oldIndexI, oldIndexJ].RelativeActivity);

                            tempSphere[i, j] = new DerivedParameter(socialProximity, relativeBelief, relativeKnowledge,
                                relativeActivity);
                        }
                        else
                        {
                            //new agent
                            var density = ContinuousUniform.Sample(Model.MinSphereDensity, Model.MaxSphereDensity);
                            var value = Bernoulli.Sample(density) ? ContinuousUniform.Sample(0F, 1F) : 0F;
                            tempSphere[i, j] =
                                new DerivedParameter(Model, value, value, value, value);
                        }

                        tempSphere[j, i] = tempSphere[i, j];
                    }
                }
                else
                {
                    // new agent
                    var density = ContinuousUniform.Sample(Model.MinSphereDensity, Model.MaxSphereDensity);
                    for (var j = i + 1; j < count; j++)
                    {
                        var value = Bernoulli.Sample(density) ? ContinuousUniform.Sample(0F, 1F) : 0F;
                        tempSphere[i, j] =
                            new DerivedParameter(Model, value, value,
                                value, value);
                        tempSphere[j, i] = tempSphere[i, j];
                    }
                }
            }

            _vector = tempVector;
            Sphere = tempSphere;
        }

        private void SetSphereWithSimilarityMatching(IReadOnlyList<IAgentId> agentIds, GraphMetaNetwork network)
        {
            var count = agentIds.Count;
            _vector = new VectorNetwork(agentIds);
            Sphere = new DerivedParameter[count, count];
            var orderedAgents = agentIds.OrderBy(x => x.Id).ToList();
            // for the moment it is a symmetrical matrix
            for (var i = 0; i < count; i++)
            {
                var agentI = orderedAgents[i];
                _vector.ItemIndex[agentI] = i;
                _vector.IndexItem[i] = agentI;
                for (var j = i + 1; j < count; j++)
                {
                    var agentJ = orderedAgents[j];
                    Sphere[i, j] = SetDerivedParameter(network, agentI, agentJ);
                    Sphere[j, i] = Sphere[i, j];
                }
            }
        }

        private DerivedParameter SetDerivedParameter(GraphMetaNetwork network, IAgentId agentI, IAgentId agentJ)
        {
            var socialProximity = Model.SocialDemographicWeight > Constants.Tolerance
                ? SetSocialProximity(agentI, agentJ, network.ActorActor)
                : 0;

            var relativeBelief = Model.RelativeBeliefWeight > Constants.Tolerance
                ? SetRelativeBelief(agentI, agentJ, network.ActorBelief)
                : 0;
            var relativeKnowledge = Model.RelativeKnowledgeWeight > Constants.Tolerance
                ? SetRelativeKnowledge(agentI, agentJ, network.ActorKnowledge)
                : 0;
            var relativeActivity = Model.RelativeActivityWeight > Constants.Tolerance
                ? SetRelativeActivity(agentI, agentJ, network.ActorTask)
                : 0;

            var derivedParameter =
                new DerivedParameter(Model, socialProximity, relativeBelief, relativeKnowledge, relativeActivity);
            return derivedParameter;
        }

        /// <summary>
        ///     The closer two agents are in the belief area, the more likely they will be to interact.
        /// </summary>
        public static float SetRelativeBelief(IAgentId agentId1, IAgentId agentId2, ActorBeliefNetwork beliefNetwork)
        {
            if (beliefNetwork == null)
            {
                throw new ArgumentNullException(nameof(beliefNetwork));
            }

            if (!beliefNetwork.ExistsSource(agentId1) || !beliefNetwork.ExistsSource(agentId2))
            {
                return 0;
            }

            var relativeBelief = 0F;
            var beliefIds = beliefNetwork.TargetsFilteredBySource(agentId1).ToList();
            foreach (var beliefId in beliefIds)
            {
                if (!beliefNetwork.Exists(agentId1, beliefId) || !beliefNetwork.Exists(agentId2, beliefId))
                {
                    continue;
                }

                var agentBelief1 = beliefNetwork.Edge(agentId1, beliefId);
                var agentBelief2 = beliefNetwork.Edge(agentId2, beliefId);
                relativeBelief += agentBelief1.CompareTo(agentBelief2);
            }

            return beliefIds.Any() ? relativeBelief / beliefIds.Count : 0;
        }

        /// <summary>
        ///     The closer two agents are in the knowledge area, the more likely they will be to interact.
        /// </summary>
        public static float SetRelativeKnowledge(IAgentId agentId1, IAgentId agentId2,
            ActorKnowledgeNetwork knowledgeNetwork)
        {
            if (knowledgeNetwork == null)
            {
                throw new ArgumentNullException(nameof(knowledgeNetwork));
            }

            if (!knowledgeNetwork.ExistsSource(agentId1) || !knowledgeNetwork.ExistsSource(agentId2))
            {
                return 0;
            }

            var relativeExpertise = 0F;
            var knowledgeIds = knowledgeNetwork.TargetsFilteredBySource(agentId1).ToList();
            foreach (var knowledgeId in knowledgeIds)
            {
                if (!knowledgeNetwork.Exists(agentId1, knowledgeId) || !knowledgeNetwork.Exists(agentId2, knowledgeId))
                {
                    continue;
                }

                var agentKnowledge1 = knowledgeNetwork.Edge(agentId1, knowledgeId);
                var agentKnowledge2 = knowledgeNetwork.Edge(agentId2, knowledgeId);
                relativeExpertise += agentKnowledge1.CompareTo(agentKnowledge2);
            }

            return knowledgeIds.Any() ? relativeExpertise / knowledgeIds.Count : 0;
        }

        /// <summary>
        ///     The closer two agents are, the more likely they will be to interact.
        /// </summary>
        /// <param name="agentId1"></param>
        /// <param name="agentId2"></param>
        /// <param name="actorActorNetwork"></param>
        /// <returns></returns>
        public static float SetSocialProximity(IAgentId agentId1, IAgentId agentId2,
            ActorActorNetwork actorActorNetwork)
        {
            //todo graph : number of nodes between agentId1 and agentId2
            if (actorActorNetwork == null)
            {
                throw new ArgumentNullException(nameof(actorActorNetwork));
            }

            return
                actorActorNetwork.NormalizedWeight(agentId1,
                    agentId2); //actorActorNetwork.NormalizedCountLinks(agentId1, agentId2);
        }

        /// <summary>
        ///     The closer two agents are on this area, the more likely they will be to interact.
        /// </summary>
        /// <param name="agentId1"></param>
        /// <param name="agentId2"></param>
        /// <param name="actorTaskNetwork"></param>
        /// <returns></returns>
        public static float SetRelativeActivity(IAgentId agentId1, IAgentId agentId2,
            ActorTaskNetwork actorTaskNetwork)
        {
            if (actorTaskNetwork == null)
            {
                throw new ArgumentNullException(nameof(actorTaskNetwork));
            }

            var task1 = actorTaskNetwork.TargetsFilteredBySource(agentId1);
            if (task1 == null)
            {
                return 0;
            }

            var task2 = actorTaskNetwork.TargetsFilteredBySource(agentId2);
            if (task2 == null)
            {
                return 0;
            }

            var taskList1 = task1.ToList();
            var relativeActivity = taskList1.Count(activity => task2.Contains(activity));
            return taskList1.Any() ? relativeActivity / taskList1.Count : 0;
        }

        /// <summary>
        ///     List of AgentId for interactions, interactions that are below above interactions (difference with
        ///     GetAgentIdsForNewInteractions)
        ///     based on the interaction strategy of the interaction patterns :
        ///     Filtered with interactionStrategy and limit with number of new interactions
        /// </summary>
        /// <param name="agentId"></param>
        /// <param name="interactionStrategy"></param>
        /// <returns></returns>
        public IEnumerable<IAgentId> GetAgentIdsForInteractions(IAgentId agentId,
            InteractionStrategy interactionStrategy)
        {
            if (!Model.On || _vector.ItemIndex is null || !_vector.ItemIndex.ContainsKey(agentId))
            {
                return new List<IAgentId>();
            }

            var agentIdDerivedParameters = new Dictionary<IAgentId, DerivedParameter>();
            var agentIndex = _vector.ItemIndex[agentId];
            for (var i = 0; i < _vector.ItemIndex.Count; i++)
            {
                if (i == agentIndex)
                {
                    continue;
                }

                switch (interactionStrategy)
                {
                    case InteractionStrategy.Homophily:
                        if (Sphere[agentIndex, i].Homophily < _lastAverage.Homophily - Constants.Tolerance ||
                            Math.Abs(Sphere[agentIndex, i].Homophily) < Constants.Tolerance)
                        {
                            continue;
                        }

                        break;
                    case InteractionStrategy.Knowledge:
                        if (Sphere[agentIndex, i].RelativeKnowledge <
                            _lastAverage.RelativeKnowledge - Constants.Tolerance ||
                            Math.Abs(Sphere[agentIndex, i].RelativeKnowledge) < Constants.Tolerance)
                        {
                            continue;
                        }

                        break;
                    case InteractionStrategy.Activities:
                        if (Sphere[agentIndex, i].RelativeActivity <
                            _lastAverage.RelativeActivity - Constants.Tolerance ||
                            Math.Abs(Sphere[agentIndex, i].RelativeActivity) < Constants.Tolerance)
                        {
                            continue;
                        }

                        break;
                    case InteractionStrategy.Beliefs:
                        if (Sphere[agentIndex, i].RelativeBelief < _lastAverage.RelativeBelief - Constants.Tolerance ||
                            Math.Abs(Sphere[agentIndex, i].RelativeBelief) < Constants.Tolerance)
                        {
                            continue;
                        }

                        break;
                    case InteractionStrategy.SocialDemographics:
                        if (Sphere[agentIndex, i].SocialDemographic <
                            _lastAverage.SocialDemographic - Constants.Tolerance ||
                            Math.Abs(Sphere[agentIndex, i].SocialDemographic) < Constants.Tolerance)
                        {
                            continue;
                        }

                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(interactionStrategy), interactionStrategy, null);
                }

                agentIdDerivedParameters.Add(_vector.IndexItem[i], Sphere[agentIndex, i]);
            }

            return OrderAgentIdsForInteractions(interactionStrategy, agentIdDerivedParameters);
        }

        private static IEnumerable<IAgentId> OrderAgentIdsForInteractions(InteractionStrategy interactionStrategy,
            Dictionary<IAgentId, DerivedParameter> agentIdDerivedParameters)
        {
            List<IAgentId> orderedAgentIds;
            switch (interactionStrategy)
            {
                case InteractionStrategy.Homophily:
                    orderedAgentIds = agentIdDerivedParameters.OrderByDescending(x => x.Value.Homophily)
                        .Select(x => x.Key)
                        .ToList();
                    break;
                case InteractionStrategy.Knowledge:
                    orderedAgentIds = agentIdDerivedParameters.OrderByDescending(x => x.Value.RelativeKnowledge)
                        .Select(x => x.Key).ToList();
                    break;
                case InteractionStrategy.Activities:
                    orderedAgentIds = agentIdDerivedParameters.OrderByDescending(x => x.Value.RelativeActivity)
                        .Select(x => x.Key).ToList();
                    break;
                case InteractionStrategy.Beliefs:
                    orderedAgentIds = agentIdDerivedParameters.OrderByDescending(x => x.Value.RelativeBelief)
                        .Select(x => x.Key)
                        .ToList();
                    break;
                case InteractionStrategy.SocialDemographics:
                    orderedAgentIds = agentIdDerivedParameters.OrderByDescending(x => x.Value.SocialDemographic)
                        .Select(x => x.Key).ToList();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(interactionStrategy), interactionStrategy, null);
            }

            return orderedAgentIds;
        }

        /// <summary>
        ///     List of AgentId for new interactions, interactions that are below average interactions (difference with
        ///     GetAgentIdsForInteractions)
        ///     based on the interaction strategy of the interaction patterns :
        ///     Filtered with interactionStrategy and limit with number of new interactions
        /// </summary>
        /// <param name="agentId"></param>
        /// <param name="interactionStrategy"></param>
        /// <returns></returns>
        public IEnumerable<IAgentId> GetAgentIdsForNewInteractions(IAgentId agentId,
            InteractionStrategy interactionStrategy)
        {
            if (_vector.ItemIndex is null)
            {
                throw new NullReferenceException("Sphere is not Setted");
            }

            if (!Model.On || !Model.SphereUpdateOverTime || !_vector.ItemIndex.ContainsKey(agentId))
            {
                return new List<IAgentId>();
            }

            var agentIdDerivedParameters = new Dictionary<IAgentId, DerivedParameter>();
            var agentIndex = _vector.ItemIndex[agentId];
            for (var i = 0; i < _vector.ItemIndex.Count; i++)
            {
                if (i == agentIndex)
                {
                    continue;
                }

                switch (interactionStrategy)
                {
                    case InteractionStrategy.Homophily:
                        if (Sphere[agentIndex, i].Homophily > _lastAverage.Homophily + Constants.Tolerance ||
                            Math.Abs(Sphere[agentIndex, i].Homophily - 1) < Constants.Tolerance)
                        {
                            continue;
                        }

                        break;
                    case InteractionStrategy.Knowledge:
                        if (Sphere[agentIndex, i].RelativeKnowledge >
                            _lastAverage.RelativeKnowledge + Constants.Tolerance ||
                            Math.Abs(Sphere[agentIndex, i].RelativeKnowledge - 1) < Constants.Tolerance)
                        {
                            continue;
                        }

                        break;
                    case InteractionStrategy.Activities:
                        if (Sphere[agentIndex, i].RelativeActivity >
                            _lastAverage.RelativeActivity + Constants.Tolerance ||
                            Math.Abs(Sphere[agentIndex, i].RelativeActivity - 1) < Constants.Tolerance)
                        {
                            continue;
                        }

                        break;
                    case InteractionStrategy.Beliefs:
                        if (Sphere[agentIndex, i].RelativeBelief > _lastAverage.RelativeBelief + Constants.Tolerance ||
                            Math.Abs(Sphere[agentIndex, i].RelativeBelief - 1) < Constants.Tolerance)
                        {
                            continue;
                        }

                        break;
                    case InteractionStrategy.SocialDemographics:
                        if (Sphere[agentIndex, i].SocialDemographic >
                            _lastAverage.SocialDemographic + Constants.Tolerance ||
                            Math.Abs(Sphere[agentIndex, i].SocialDemographic - 1) < Constants.Tolerance)
                        {
                            continue;
                        }

                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(interactionStrategy), interactionStrategy, null);
                }

                agentIdDerivedParameters.Add(_vector.IndexItem[i], Sphere[agentIndex, i]);
            }

            return OrderAgentIdsForInteractions(interactionStrategy, agentIdDerivedParameters);
        }

        /// <summary>
        ///     Get the homophily value of the AgentId2 for the AgentId1
        /// </summary>
        /// <param name="agentId1"></param>
        /// <param name="agentId2"></param>
        /// <returns>
        ///     0 if one of the agent is not yet present in the network
        ///     The homophily value if both exist.
        /// </returns>
        /// <remarks>
        ///     An agent that acts via homophily attempts to ﬁnd an _model partner that shares its characteristics.
        ///     When searching for suitable partners, the agent will stress agents who have similar socio-demographic parameters,
        ///     similar knowledge, and similar beliefs.
        ///     This process utilizes the derived parameters
        /// </remarks>
        public float GetHomophily(IAgentId agentId1, IAgentId agentId2)
        {
            if (_vector.ItemIndex == null)
            {
                throw new NullReferenceException(nameof(_vector.ItemIndex));
            }

            if (!Model.On || !_vector.ItemIndex.ContainsKey(agentId1) || !_vector.ItemIndex.ContainsKey(agentId2))
            {
                return 0;
            }

            var index1 = _vector.ItemIndex[agentId1];
            var index2 = _vector.ItemIndex[agentId2];
            return Sphere[index1, index2].Homophily;
        }

        public float GetSphereWeight()
        {
            if (!Model.On)
            {
                return 0;
            }

            // for the moment it is a symmetrical matrix
            var weight = 0F;
            for (var i = 0; i < Sphere.GetLength(0); i++)
            {
                for (var j = i + 1; j < Sphere.GetLength(1); j++)
                {
                    weight += Sphere[i, j].Homophily;
                }
            }

            return weight * 2;
        }

        public float GetMaxSphereWeight()
        {
            if (!Model.On)
            {
                return 0;
            }

            return (Model.SocialDemographicWeight + Model.RelativeBeliefWeight + Model.RelativeKnowledgeWeight +
                    Model.RelativeActivityWeight) * Sphere.GetLength(0) * (Sphere.GetLength(0) - 1);
        }
    }
}