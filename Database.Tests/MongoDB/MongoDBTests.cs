using Database.Interfaces;
using Database.Interfaces.Map;
using Database.Interfaces.User;
using Database.Interfaces.Workstation;
using Database.MongoDB;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

namespace Database.Tests.MongoDB
{
    [TestFixture]
    class MongoDBTests
    {
        private IDatabase Database { get; set; }
        private IUsers Users => Database.Users;
        private IWorkstations Workstations => Database.Workstations;
        private IMaps Maps => Database.Maps;


        [OneTimeSetUp]
        public void SetUp()
        {
            Database = new MongoDatabase(new DBSettings());
            if (Database.CreateIfNeeded())
                SeedDatabase();
        }

        private void SeedDatabase()
        {
            Users.AddUser("l1", "p1");
            Users.AddUser("l2", "p2");
        }

        [Test]
        public void FindFirst()
        {
            var login = "l1";
            var found = Users.FindFirst(x => x.Login.Equals(login));
            Assert.IsNotNull(found);
            Assert.AreEqual(login, found.Login);

            login = "notexistinglogin";
            found = Users.FindFirst(x => x.Login.Equals(login));
            Assert.IsNull(found);
        }

        [Test]
        public void FindAll()
        {
            var login = "l1";
            var found = Users.FindAll(x => x.Login.Equals(login));
            Assert.IsNotNull(found);
            Assert.IsTrue(found.Count == 1);
            Assert.IsNotNull(found[0]);
            Assert.AreEqual(login, found[0].Login);

            login = "notexistinglogin";
            found = Users.FindAll(x => x.Login.Equals(login));
            Assert.IsNotNull(found);
            Assert.IsTrue(found.Count == 0);
        }

        [Test]
        public void GetAll()
        {
            var found = Users.GetAll();
            Assert.IsNotNull(found);
            Assert.IsTrue(found.Count == 2);
            foreach (var item in found)
                Assert.IsNotNull(item);
        }

        [Test]
        public void AddRemove()
        {
            var added = Workstations.AddWorkstation("n1");
            Assert.IsNotNull(added);
            var id = added.GetId();
            Assert.IsTrue(Workstations.GetAll().Count == 1);
            var removed = Workstations.Remove(id);
            Assert.IsTrue(removed);
            Assert.IsTrue(Workstations.GetAll().Count == 0);
        }

        [Test]
        public void AddOrReplace()
        {
            var currentMaps = Maps.GetAll();
            Assert.IsTrue(currentMaps.Count == 0);
            var map = Maps.CreateMap("map1");
            var id = map.GetId();
            var item = Maps.CreateImageItem(0, 0, 1, 1, 0, new byte[] { });
            map.AddItem(item);
            var isAdded = Maps.AddOrReplace(map);
            Assert.IsTrue(isAdded);

            currentMaps = Maps.GetAll();
            Assert.IsTrue(currentMaps.Count == 1);
            Assert.AreEqual("map1", currentMaps[0].Name);
            Assert.IsTrue(currentMaps[0].Items.Count() == 1);

            var changedMap = Maps.CreateMap("map2", id);
            var item1 = Maps.CreateImageItem(0, 0, 1, 1, 0, new byte[] { });
            var item2 = Maps.CreateImageItem(0, 0, 1, 1, 0, new byte[] { });
            changedMap.AddItem(item1);
            changedMap.AddItem(item2);
            var isReplaced = Maps.AddOrReplace(changedMap);
            Assert.IsTrue(isReplaced);

            currentMaps = Maps.GetAll();
            Assert.IsTrue(currentMaps.Count == 1);
            Assert.AreEqual("map2", currentMaps[0].Name);
            Assert.IsTrue(currentMaps[0].Items.Count() == 2);

            var removed = Maps.Remove(id);
            Assert.IsTrue(removed);
            Assert.IsTrue(Maps.GetAll().Count == 0);
        }

        [Test]
        public void ReplaceCollection()
        {
            var currentMaps = Maps.GetAll();
            Assert.IsTrue(currentMaps.Count == 0);
            var map1 = Maps.CreateMap("map1");
            var map2 = Maps.CreateMap("map2");
            Maps.ReplaceCollection(new List<IMap>() { map1, map2 });

            currentMaps = Maps.GetAll();
            Assert.IsTrue(currentMaps.Count == 2);

            var map1changed = Maps.CreateMap("mapChanged", map1.GetId());
            var newMap3 = Maps.CreateMap("newMap3");
            Maps.ReplaceCollection(new List<IMap>() { map1changed, newMap3 });

            currentMaps = Maps.GetAll();
            Assert.IsTrue(currentMaps.Count == 2);
            Assert.AreEqual("mapChanged", currentMaps[0].Name);
            Assert.AreEqual("newMap3", currentMaps[1].Name);

            Maps.ReplaceCollection(new List<IMap>());
            currentMaps = Maps.GetAll();
            Assert.IsTrue(currentMaps.Count == 0);
        }

        [Test]
        public void GetOne()
        {
            var users = Users.GetAll();
            var user = users.First();
            var id = user.GetId();

            var foundUser = Users.GetOne(id);
            Assert.IsNotNull(foundUser);

            foundUser = Users.GetOne("5ef2222f7ad75a01e0224bf0");
            Assert.IsNull(foundUser);
        }

    }
}
