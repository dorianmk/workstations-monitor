using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Database.Interfaces.Common;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Database.MongoDB.Common
{
    internal abstract class DbCollectionBase<TEntity, TImplementation> : IDbCollection<TEntity>
        where TEntity : IEntity
        where TImplementation : class, TEntity, IMongoEntity
    {
        private IMongoCollection<TImplementation> Collection { get; }

        protected DbCollectionBase(IMongoDatabase db, string collectionName)
        {
            Collection = db.GetCollection<TImplementation>(collectionName);
        }

        protected void Add(TImplementation item) => Collection.InsertOne(item);

        public List<TEntity> GetAll() => Collection.AsQueryable().ToList<TEntity>();

        public List<TEntity> FindAll(Expression<Func<TEntity, bool>> predicate) => Collection.AsQueryable().Where(predicate).ToList();

        public TEntity FindFirst(Expression<Func<TEntity, bool>> predicate) => Collection.AsQueryable().FirstOrDefault(predicate);

        public TEntity GetOne(string id) => Collection.Find(x => x.Id == new ObjectId(id)).SingleOrDefault();

        public bool Remove(string id) => Collection.DeleteOne(x => x.Id == new ObjectId(id)).IsAcknowledged;

        public bool AddOrReplace(TEntity entity)
        {
            TImplementation item = entity as TImplementation;
            var updateOptions = new UpdateOptions() { IsUpsert = true };
            return Collection.ReplaceOne(x => x.Id == item.Id, item, updateOptions).IsAcknowledged;
        }

        public void ReplaceCollection(IEnumerable<TEntity> items)
        {
            foreach (var item in items)
                AddOrReplace(item);

            var existingIds = items.Select(x => new ObjectId(x.GetId())).ToArray();
            Collection.DeleteMany(x => !existingIds.Contains(x.Id));
        }
    }
}
