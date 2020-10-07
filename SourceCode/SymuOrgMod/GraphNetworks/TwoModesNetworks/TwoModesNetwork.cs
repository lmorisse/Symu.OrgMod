#region Licence

// Description: SymuBiz - SymuOrgMod
// Website: https://symu.org
// Copyright: (c) 2020 laurent morisseau
// License : the program is distributed under the terms of the GNU General Public License

#endregion

#region using directives

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Symu.Common.Interfaces;
using Symu.OrgMod.Edges;
using static Symu.Common.Constants;

#endregion

namespace Symu.OrgMod.GraphNetworks.TwoModesNetworks
{
    /// <summary>
    ///     Abstract class for two modes network
    /// </summary>
    /// <remarks>
    ///     todo should be replaced by QuickGraph
    ///     TVertex = IAgentId
    /// </remarks>
    public class TwoModesNetwork<TEdge> where TEdge : class, IEdge
    {
        /// <summary>
        ///     List of all edges of the graph
        /// </summary>
        protected List<TEdge> List { get; }= new List<TEdge>();

        /// <summary>
        ///     Gets or sets the element at the specified index
        /// </summary>
        /// <param name="index">0 based</param>
        /// <returns></returns>
        public TEdge this[int index]
        {
            get => List[index];
            set => List[index] = value;
        }

        public int Count => List.Count;


        public bool Any()
        {
            return List.Any();
        }

        public void Clear()
        {
            List.Clear();
        }

        /// <summary>
        ///     Check that the source exists in the repository
        /// </summary>
        /// <param name="sourceId"></param>
        /// <returns></returns>
        public bool ExistsSource(IAgentId sourceId)
        {
            return List.Exists(x => x.Source.Equals(sourceId));
        }

        /// <summary>
        ///     Check that the source exists in the repository
        /// </summary>
        /// <param name="targetId"></param>
        /// <returns></returns>
        public bool ExistsTarget(IAgentId targetId)
        {
            return List.Exists(x => x.Target.Equals(targetId));
        }

        public bool Exists(TEdge edge)
        {
            return List.Exists(x => x.Equals(edge));
        }

        public virtual bool Exists(IAgentId sourceId, IAgentId targetId)
        {
            return List.Exists(x => x.Source.Equals(sourceId) && x.Target.Equals(targetId));
        }

        public virtual void Add(TEdge edge)
        {
            if (edge == null)
            {
                throw new ArgumentNullException(nameof(edge));
            }

            if (Exists(edge))
            {
                return;
            }

            List.Add(edge);
        }

        public void Add(IEnumerable<TEdge> edges)
        {
            if (edges == null)
            {
                throw new ArgumentNullException(nameof(edges));
            }

            foreach (var edge in edges)
            {
                Add(edge);
            }
        }

        /// <summary>
        ///     Make a copy of of the network
        /// </summary>
        /// <param name="network"></param>
        public void CopyTo(TwoModesNetwork<TEdge> network)
        {
            if (network is null)
            {
                throw new ArgumentNullException(nameof(network));
            }

            foreach (var edge in List)
            {
                network.Add(edge);
            }
        }

        public void Remove(TEdge edge)
        {
            List.RemoveAll(x => x.Equals(edge));
        }

        public void Remove(IAgentId sourceId, IAgentId targetId)
        {
            List.RemoveAll(x => x.Source.Equals(sourceId) && x.Target.Equals(targetId));
        }

        public void RemoveSource(IAgentId sourceId)
        {
            List.RemoveAll(x => x.Source.Equals(sourceId));
        }

        public void RemoveTarget(IAgentId targetId)
        {
            List.RemoveAll(x => x.Target.Equals(targetId));
        }

        /// <summary>
        ///     Copy all edges of a sourceId * targetFromId to another targetToId
        /// </summary>
        /// <param name="sourceId"></param>
        /// <param name="targetFromId"></param>
        /// <param name="targetToId"></param>
        public void CopyTo(IAgentId sourceId, IAgentId targetFromId, IAgentId targetToId)
        {
            foreach (var copy in Edges(sourceId, targetFromId).ToImmutableList().Select(edge => (TEdge) edge.Clone()))
            {
                copy.Target = targetToId;
                Add(copy);
            }
        }

        /// <summary>
        ///     Copy all edges of a targetFromId to another targetToId
        /// </summary>
        /// <param name="targetFromId"></param>
        /// <param name="targetToId"></param>
        public void CopyToFromTarget(IAgentId targetFromId, IAgentId targetToId)
        {
            foreach (var copy in EdgesFilteredByTarget(targetFromId).ToImmutableList().Select(edge => (TEdge) edge.Clone()))
            {
                copy.Target = targetToId;
                Add(copy);
            }
        }

        /// <summary>
        ///     Copy all edges of a sourceFromId to another sourceToId
        /// </summary>
        /// <param name="sourceFromId"></param>
        /// <param name="sourceToId"></param>
        public void CopyToFromSource(IAgentId sourceFromId, IAgentId sourceToId)
        {
            foreach (var copy in EdgesFilteredBySource(sourceFromId).ToImmutableList().Select(edge => (TEdge) edge.Clone()))
            {
                copy.Source = sourceToId;
                Add(copy);
            }
        }

        public TwoModesNetwork<IEdge> Clone()
        {
            var clone = new TwoModesNetwork<IEdge>();
            foreach (var edge in List)
            {
                clone.Add(edge);
            }

            return clone;
        }

        #region Edge

        public virtual TEdge Edge(IAgentId sourceId, IAgentId targetId)
        {
            return List.FirstOrDefault(x => x.Source.Equals(sourceId) && x.Target.Equals(targetId));
        }

        public virtual TTEdge Edge<TTEdge>(IAgentId sourceId, IAgentId targetId) where TTEdge : TEdge
        {
            return (TTEdge) List.FirstOrDefault(x => x.Source.Equals(sourceId) && x.Target.Equals(targetId));
        }

        public TEdge Edge(TEdge edge)
        {
            if (edge == null)
            {
                throw new ArgumentNullException(nameof(edge));
            }

            return Edge(edge.Source, edge.Target);
        }

        public IEnumerable<TEdge> Edges()
        {
            return List;
        }

        public virtual IEnumerable<TEdge> Edges(IAgentId sourceId, IAgentId targetId)
        {
            return List.Where(x => x.Source.Equals(sourceId) && x.Target.Equals(targetId));
        }

        public IEnumerable<TEdge> EdgesFilteredBySourceAndTargetClassId(IAgentId sourceId, IClassId targetClassId)
        {
            return List.Where(x => x.Source.Equals(sourceId) && x.Target.ClassId.Equals(targetClassId));
        }

        public IEnumerable<TEdge> EdgesFilteredByTargetAndSourceClassId(IAgentId targetId, IClassId sourceClassId)
        {
            return List.Where(x => x.Target.Equals(targetId) && x.Source.ClassId.Equals(sourceClassId));
        }

        public IEnumerable<TEdge> EdgesFilteredBySource(IAgentId sourceId)
        {
            return List.FindAll(x => x.Source.Equals(sourceId));
        }

        public IEnumerable<TTEdge> EdgesFilteredBySource<TTEdge>(IAgentId sourceId) where TTEdge : TEdge
        {
            return EdgesFilteredBySource(sourceId).Cast<TTEdge>();
        }

        public IEnumerable<TEdge> EdgesFilteredByTarget(IAgentId targetId)
        {
            return List.FindAll(x => x.Target.Equals(targetId));
        }

        public ushort EdgesFilteredBySourceCount(IAgentId sourceId)
        {
            return (ushort) List.Count(x => x.Source.Equals(sourceId));
        }

        public ushort EdgesFilteredByTargetCount(IAgentId targetId)
        {
            return (ushort) List.Count(x => x.Target.Equals(targetId));
        }

        #endregion

        #region Target

        public IEnumerable<IAgentId> Targets()
        {
            return List.Select(x => x.Target).Distinct();
        }

        public IEnumerable<IAgentId> TargetsFilteredBySource(IAgentId sourceId)
        {
            return List.FindAll(x => x.Source.Equals(sourceId)).Select(x => x.Target).Distinct();
        }

        public IEnumerable<IAgentId> TargetsFilteredByTargetClassId(IClassId targetClassId)
        {
            return List.FindAll(x => x.Target.ClassId.Equals(targetClassId)).Select(x => x.Target).Distinct();
        }

        public IEnumerable<IAgentId> TargetsFilteredBySourceAndTargetClassId(IAgentId sourceId, IClassId targetClassId)
        {
            return EdgesFilteredBySourceAndTargetClassId(sourceId, targetClassId).Select(x => x.Target).Distinct();
        }

        #endregion

        #region Source

        public IEnumerable<IAgentId> Sources()
        {
            return List.Select(x => x.Source).Distinct();
        }

        public IEnumerable<IAgentId> SourcesFilteredByTarget(IAgentId targetId)
        {
            return List.FindAll(x => x.Target.Equals(targetId)).Select(x => x.Source).Distinct();
        }

        public IEnumerable<IAgentId> SourcesFilteredByTargetAndSourceClassId(IAgentId targetId, IClassId sourceClassId)
        {
            return EdgesFilteredByTargetAndSourceClassId(targetId, sourceClassId).Select(x => x.Source).Distinct();
        }

        public ushort SourcesFilteredByTargetAndSourceClassIdCount(IAgentId targetId, IClassId sourceClassId)
        {
            return (ushort) EdgesFilteredByTargetAndSourceClassId(targetId, sourceClassId).Count();
        }

        #endregion

        #region Weight

        public float Weight(IEdge edge)
        {
            if (edge == null)
            {
                throw new ArgumentNullException(nameof(edge));
            }

            return Weight(edge.Source, edge.Target);
        }

        public float Weight(IAgentId sourceId, IAgentId targetId)
        {
            return Edges(sourceId, targetId).Sum(x => x.Weight);
        }

        public float WeightFilteredBySource(IAgentId sourceId)
        {
            return EdgesFilteredBySource(sourceId).Sum(x => x.Weight);
        }

        public float WeightFilteredByTarget(IAgentId targetId)
        {
            return EdgesFilteredByTarget(targetId).Sum(x => x.Weight);
        }

        private float _maxWeight;

        public float NormalizedWeight(IAgentId actorId1, IAgentId actorId2)
        {
            return _maxWeight < Tolerance ? 0 : Weight(actorId1, actorId2) / _maxWeight;
        }

        /// <summary>
        ///     Normalize weights of the network
        ///     Call this method before calling NormalizedWeight method
        ///     This method compute the edge.NormalizedWeight by dividing the edge.Weight by the maximum weight found in the
        ///     network
        /// </summary>
        public void NormalizeWeights()
        {
            _maxWeight = Any() ? List.Max(x => x.Weight) : 0;
            foreach (var edge in List)
            {
                edge.NormalizedWeight = edge.Weight / _maxWeight;
            }
        }

        #endregion
    }
}