namespace SexyFishHorse.WauwBot.DataAccess.Stores
{
    using System.Threading.Tasks;
    using MongoDB.Bson;
    using SexyFishHorse.WauwBot.Model;

    public interface IReadStore
    {
        Task<TResult> Get<TResult>(ObjectId id) where TResult : MongoBase;
    }
}
