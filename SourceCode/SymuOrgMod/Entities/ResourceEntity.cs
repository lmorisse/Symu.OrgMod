#region Licence

// Description: SymuBiz - SymuOrgMod
// Website: https://symu.org
// Copyright: (c) 2020 laurent morisseau
// License : the program is distributed under the terms of the GNU General Public License

#endregion

#region using directives

using System;
using System.Collections.Generic;
using Symu.Common.Interfaces;
using Symu.OrgMod.Edges;
using Symu.OrgMod.GraphNetworks;

#endregion

namespace Symu.OrgMod.Entities
{
    /// <summary>
    ///     Resources are products, materials, or goods that are necessary to perform Tasks and Events
    ///     Default implementation of IResource
    /// </summary>
    /// <example>database, products, routines, processes, ...</example>
    public class ResourceEntity : Entity<ResourceEntity>, IResource
    {
        public const byte Class = ClassIdCollection.Resource;
        public static IClassId ClassId => new ClassId(Class);

        public ResourceEntity()
        {
        }

        public ResourceEntity(GraphMetaNetwork metaNetwork) : base(metaNetwork, metaNetwork?.Resource, ClassId)
        {
        }

        public ResourceEntity(GraphMetaNetwork metaNetwork, string name) : base(metaNetwork, metaNetwork?.Resource,
            ClassId, name)
        {
        }

        public ResourceEntity(GraphMetaNetwork metaNetwork, IClassId classId) : base(metaNetwork, metaNetwork?.Resource,
            classId)
        {
        }

        public ResourceEntity(GraphMetaNetwork metaNetwork, IClassId classId, string name) : base(metaNetwork,
            metaNetwork?.Resource, classId, name)
        {
        }

        public static ResourceEntity CreateInstance(GraphMetaNetwork metaNetwork)
        {
            return new ResourceEntity(metaNetwork);
        }

        public static ResourceEntity CreateInstance(GraphMetaNetwork metaNetwork, string name)
        {
            return new ResourceEntity(metaNetwork, name);
        }

        #region IResource Members

        public override void Remove()
        {
            base.Remove();
            MetaNetwork.ResourceResource.RemoveResource(EntityId);
            MetaNetwork.ResourceTask.RemoveSource(EntityId);
            MetaNetwork.OrganizationResource.RemoveTarget(EntityId);
            MetaNetwork.ActorResource.RemoveTarget(EntityId);
            MetaNetwork.ResourceKnowledge.RemoveSource(EntityId);
        }

        #endregion

        /// <summary>
        ///     Copy the metaNetwork, the two modes networks where the entity exists
        /// </summary>
        /// <param name="entityId"></param>
        public override void CopyMetaNetworkTo(IAgentId entityId)
        {
            MetaNetwork.ResourceResource.CopyToFromSource(EntityId, entityId);
            MetaNetwork.ResourceResource.CopyToFromTarget(EntityId, entityId);
            MetaNetwork.ResourceTask.CopyToFromSource(EntityId, entityId);
            MetaNetwork.ResourceKnowledge.CopyToFromSource(EntityId, entityId);
            MetaNetwork.OrganizationResource.CopyToFromTarget(EntityId, entityId);
            MetaNetwork.ActorResource.CopyToFromTarget(EntityId, entityId);
        }

        public float GetWeight(IEntity entity, IResourceUsage resourceUsage)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            switch (entity)
            {
                case IResource _:
                    return GetResourceWeight(entity.EntityId, resourceUsage);
                case IOrganization _:
                    return GetOrganizationWeight(entity.EntityId, resourceUsage);
                case IActor _:
                    return GetActorWeight(entity.EntityId, resourceUsage);
                default:
                    return 0;
            }
        }

        #region Actor * Resource management

        public void AddActor(IAgentId actorId, IResourceUsage resourceUsage, float weight = 100)
        {
            ActorResource.CreateInstance(MetaNetwork.ActorResource, actorId, EntityId, resourceUsage, weight);
        }

        public float GetActorWeight(IAgentId actorId, IResourceUsage resourceUsage)
        {
            return MetaNetwork.ActorResource.GetWeight(actorId, EntityId, resourceUsage);
        }

        public IEnumerable<IActorResource> GetActorResources(IAgentId actorId)
        {
            return MetaNetwork.ActorResource.Edges(actorId, EntityId);
        }

        /// <summary>
        ///     Get the number of actors having this resource
        /// </summary>
        public byte ActorsCount =>
            (byte) MetaNetwork.ActorResource.SourcesFilteredByTargetAndSourceClassIdCount(EntityId,
                ActorEntity.ClassId);

        /// <summary>
        ///     Get the number of actors having this resource
        /// </summary>
        public IEnumerable<IAgentId> ActorIds =>
            MetaNetwork.ActorResource.SourcesFilteredByTargetAndSourceClassId(EntityId, ActorEntity.ClassId);

        public float GetSumWeight => MetaNetwork.ActorResource.WeightFilteredByTarget(EntityId);

        #endregion

        #region Organization * Resource management

        public void AddOrganization(IAgentId organizationId, IResourceUsage resourceUsage, float weight = 100)
        {
            OrganizationResource.CreateInstance(MetaNetwork.OrganizationResource, organizationId, EntityId, resourceUsage, weight);
        }

        public float GetOrganizationWeight(IAgentId organizationId, IResourceUsage resourceUsage)
        {
            return MetaNetwork.OrganizationResource.GetWeight(organizationId, EntityId, resourceUsage);
        }

        public void SetOrganizationWeight(IAgentId organizationId, IResourceUsage resourceUsage, float weight)
        {
            MetaNetwork.OrganizationResource.SetWeight(organizationId, EntityId, resourceUsage, weight);
        }

        public IEnumerable<IOrganizationResource> GetOrganizationResources(IAgentId organizationId)
        {
            return MetaNetwork.OrganizationResource.Edges(organizationId, EntityId);
        }

        #endregion

        #region Resource * Resource management

        public void AddResource(IAgentId target, IResourceUsage resourceUsage, float weight = 100)
        {
            ResourceResource.CreateInstance(MetaNetwork.ResourceResource, EntityId, target, resourceUsage, weight);
        }

        public IEnumerable<IResourceResource> GetResourceResources(IAgentId resourceId)
        {
            return MetaNetwork.ResourceResource.Edges(EntityId, resourceId);
        }

        public float GetResourceWeight(IAgentId resourceId, IResourceUsage resourceUsage)
        {
            return MetaNetwork.ResourceResource.GetWeight(EntityId, resourceId, resourceUsage);
        }

        #endregion

        #region Resource * Knowledge management

        public EntityKnowledge AddKnowledge(IAgentId knowledgeId)
        {
            return new EntityKnowledge(MetaNetwork.ResourceKnowledge, EntityId, knowledgeId);
        }

        public bool ExistsKnowledge(IAgentId knowledgeId)
        {
            return MetaNetwork.ResourceKnowledge.ExistsTarget(knowledgeId);
        }

        #endregion

        #region Resource* Task management

        /// <summary>
        ///     Get all the tasks (activities) that a resource can do
        /// </summary>
        public IEnumerable<IAgentId> TaskIds => MetaNetwork.ResourceTask.TargetsFilteredBySource(EntityId);
        /// <summary>
        ///     Add a list of tasks a resource can perform
        /// </summary>
        /// <param name="tasks"></param>
        public void AddResourceTasks(IEnumerable<ITask> tasks)
        {
            if (tasks == null)
            {
                throw new ArgumentNullException(nameof(tasks));
            }

            foreach (var task in tasks)
            {
                ResourceTask.CreateInstance(MetaNetwork.ResourceTask, EntityId, task.EntityId);
            }
        }
        /// <summary>
        ///     Get the all the knowledges for all the tasks of an agent
        /// </summary>
        /// <returns></returns>
        public IDictionary<ITask, IEnumerable<IKnowledge>> Knowledge
        {
            get
            {
                var knowledge = new Dictionary<ITask, IEnumerable<IKnowledge>>();
                foreach (var taskId in TaskIds)
                {
                    var task = MetaNetwork.Task.GetEntity<ITask>(taskId);
                    knowledge.Add(task, task.Knowledge);
                }

                return knowledge;
            }
        }

        #endregion
    }
}