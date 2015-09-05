namespace SexyFishHorse.WauwBot.DataAccess
{
    using System;

    public class StoredInAttribute : Attribute
    {
        public StoredInAttribute(string databaseName)
        {
            DatabaseName = databaseName;
        }

        public string DatabaseName { get; set; }
    }
}
