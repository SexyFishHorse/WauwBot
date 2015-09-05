namespace SexyFishHorse.WauwBot.DataAccess.Stores
{
    using System.Threading.Tasks;
    using SexyFishHorse.WauwBot.Model;

    public interface IWriteStore
    {
        Task Save<T>(T mongoObject) where T : MongoBase;

        Task Delete<T>(T mongoObject) where T : MongoBase;
    }
}
