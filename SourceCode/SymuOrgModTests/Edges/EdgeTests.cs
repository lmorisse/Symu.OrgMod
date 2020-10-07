using Microsoft.VisualStudio.TestTools.UnitTesting;
using Symu.Common.Interfaces;
using Symu.OrgMod.Edges;

namespace SymuOrgModTests.Edges
{
    [TestClass()]
    public class EdgeTests
    {
        private readonly IAgentId _source = new AgentId(1,1);
        private readonly IAgentId _target = new AgentId(2, 2);
        private Edge<IActorBelief> _edge;

        [TestInitialize]
        public void Initialize()
        {
            _edge = new Edge<IActorBelief>(_source, _target,1);
        }

        [TestMethod()]
        public void EdgeTest()
        {
            Assert.AreEqual(_source, _edge.Source);
            Assert.AreEqual(_target, _edge.Target);
            Assert.AreEqual(1, _edge.Weight);
        }

        [TestMethod()]
        public void EqualsTest()
        {
            var edge = new Edge<IActorBelief>(_source, _target, 1);
            Assert.IsTrue(_edge.Equals(edge));
            var edge1 = new Edge<IActorRole>(_source, _target, 1);
            Assert.IsFalse(_edge.Equals(edge1));
        }

        [TestMethod()]
        public void EqualsSourceTest()
        {
            Assert.IsTrue(_edge.EqualsSource(_source));
            Assert.IsFalse(_edge.EqualsSource(_target));
        }

        [TestMethod()]
        public void EqualsTargetTest()
        {
            Assert.IsFalse(_edge.EqualsTarget(_source));
            Assert.IsTrue(_edge.EqualsTarget(_target));
        }

        [TestMethod()]
        public void CloneTest()
        {
            var clone = _edge.Clone();
            Assert.AreEqual(_edge, clone);
        }
    }
}