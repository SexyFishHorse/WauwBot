namespace SexyFishHorse.WauwBot.DataAccess.Stores
{
    using System;
    using System.Reflection;
    using System.Threading.Tasks;
    using MongoDB.Bson;
    using MongoDB.Driver;
    using SexyFishHorse.WauwBot.DataAccess;
    using SexyFishHorse.WauwBot.Model;

    public class WriteStore : IWriteStore
    {
        private readonly IMongoClient client;

        public WriteStore(IMongoClient client)
        {
            this.client = client;
        }

        public async Task Save<TObject>(TObject mongoObject) where TObject : MongoBase
        {
            var collection = GetCollection(mongoObject);

            if (mongoObject.Id == ObjectId.Empty)
            {
                mongoObject.Id = ObjectId.GenerateNewId();

                await collection.InsertOneAsync(mongoObject);
            }
            else
            {
                await collection.FindOneAndReplaceAsync(x => x.Id == mongoObject.Id, mongoObject);
            }
        }

        public async Task Delete<TObject>(TObject mongoObject) where TObject : MongoBase
        {
            if (mongoObject.Id == ObjectId.Empty)
            {
                throw new ArgumentException("Cannot delete object without an id");
            }

            var collection = GetCollection(mongoObject);

            await collection.FindOneAndDeleteAsync(x => x.Id == mongoObject.Id);
        }

        private IMongoCollection<TCollection> GetCollection<TCollection>(TCollection mongoObject) where TCollection : MongoBase
        {
            var attr = (StoredInAttribute)mongoObject.GetType().GetCustomAttribute(typeof(StoredInAttribute));

            if (attr == null)
            {
                throw new ArgumentException("Object does not contain the StoredIn attribute");
            }

            var db = client.GetDatabase(attr.DatabaseName);

            return db.GetCollection<TCollection>(mongoObject.GetType().Name);
        }
    }
}
