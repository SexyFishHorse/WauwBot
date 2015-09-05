namespace SexyFishHorse.WauwBot.DataAccess.Stores
{
    using System;
    using System.Reflection;
    using System.Threading.Tasks;
    using MongoDB.Bson;
    using MongoDB.Driver;
    using SexyFishHorse.WauwBot.DataAccess;
    using SexyFishHorse.WauwBot.Model;

    public class ReadStore : IReadStore
    {
        private readonly IMongoClient client;

        public ReadStore(IMongoClient client)
        {
            this.client = client;
        }

        public async Task<TResult> Get<TResult>(ObjectId id) where TResult : MongoBase
        {
            var collection = GetCollection<TResult>();

            return await collection.Find(x => x.Id == id).FirstOrDefaultAsync();
        }

        private IMongoCollection<TCollection> GetCollection<TCollection>() where TCollection : MongoBase
        {
            var collectionType = typeof(TCollection);
            var attr = (StoredInAttribute)collectionType.GetCustomAttribute(typeof(StoredInAttribute));

            if (attr == null)
            {
                throw new ArgumentException("Object does not contain the StoredIn attribute");
            }

            var db = client.GetDatabase(attr.DatabaseName);

            return db.GetCollection<TCollection>(collectionType.Name);
        }
    }
}
