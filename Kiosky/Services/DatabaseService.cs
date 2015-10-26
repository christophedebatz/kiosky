using Kiosky.Models;
using Kiosky.Services.Internal;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kiosky.Services
{
    public class DatabaseService
    {
        private static readonly object _padlock = new object();

        private static DatabaseService _instance;

        public IMongoDatabase Database { get; private set; }

        private DatabaseService()
        {
            RegisterSerializableClasses();

            string connectionString = ConfiguratorRegistrar.Config.GetAsString("DatabaseConnectionString");
            string databaseName = ConfiguratorRegistrar.Config.GetAsString("DetabaseName");

            var client = new MongoClient(connectionString);
            this.Database = client.GetDatabase(databaseName);
        }

        public static DatabaseService Instance
        {
            get 
            {
                lock (_padlock) // simple thread-safety
                {
                    if (_instance == null) 
                    {
                        _instance = new DatabaseService();
                    }

                    return _instance;
                }
            }
        }

        private static void RegisterSerializableClasses()
        {
            BsonClassMap.RegisterClassMap<User>();
        }
    }
}