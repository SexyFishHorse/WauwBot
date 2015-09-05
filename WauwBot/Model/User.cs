namespace SexyFishHorse.WauwBot.Model
{
    using System;
    using SexyFishHorse.WauwBot.DataAccess;

    [StoredIn("WauwBot")]
    public class User : MongoBase
    {
        public string Username { get; set; }

        public string Alias { get; set; }

        public Provider Provider { get; set; }

        public DateTime FirstSeen { get; set; }

        public DateTime LastSeen { get; set; }

        public long AccumulatedMinutes { get; set; }

        public bool Regular { get; set; }
    }
}
