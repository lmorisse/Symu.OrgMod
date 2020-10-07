using Microsoft.VisualStudio.TestTools.UnitTesting;
using Symu.Common.Interfaces;
using Symu.OrgMod.Entities;
using Symu.OrgMod.GraphNetworks;

namespace SymuOrgModTests.Entities
{
    [TestClass()]
    public class EntityTests
    {
        private readonly GraphMetaNetwork _metaNetwork = new GraphMetaNetwork();
        private readonly IAgentId _uid0 = new AgentId(0,0);
        private ActorEntity _entity;

        [TestInitialize]
        public void Initialize()
        {
            _entity = new ActorEntity(_metaNetwork);
        }

        [TestMethod]
        public void EntityTest()
        {
            Assert.AreEqual(_uid0, _entity.EntityId);
        }

        [TestMethod]
        public void CloneTest()
        {
            var clone = _entity.Clone() as IEntity;
            Assert.IsNotNull(clone);
            Assert.AreEqual(_entity.EntityId, clone.EntityId);
            Assert.AreEqual(_entity.Name, clone.Name);
        }

        [TestMethod]
        public void DuplicateTest()
        {
            var clone = _entity.Duplicate< ActorEntity>() ;
            Assert.IsNotNull(clone);
            Assert.IsNotNull(clone.EntityId);
            Assert.IsNotNull(_metaNetwork.Actor.GetEntity(clone.EntityId));
            Assert.AreNotEqual(_entity.EntityId, clone.EntityId);
            Assert.AreEqual(_entity.EntityId.ClassId, clone.EntityId.ClassId);
            Assert.AreEqual(_entity.Name, clone.Name);
        }

        [TestMethod]
        public void EqualsTest()
        {
            var clone = _entity.Clone();// as Entity;
            Assert.IsNotNull(clone);
            Assert.AreEqual(_entity, clone);
        }

        //[TestMethod()]
        //public void ToStringTest()
        //{
        //   // Assert.AreEqual("1", _entity.ToString());
        //}
    }
}